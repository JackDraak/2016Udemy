using UnityEngine;
using System.Collections;

public class Ball : MonoBehaviour {

	public AudioClip ball;

	private float currentVelocityX;
	private float currentVelocityY;
	private LevelManager levelManager;
	private float maxVelocityX = 16f;
	private float maxVelocityY = 21f;
	private Paddle paddle;
	private Vector3 paddleToBallVector;
	private Vector2 preClampVelocity;

	void Start () {
		levelManager = GameObject.FindObjectOfType<LevelManager>();
		paddle = GameObject.FindObjectOfType<Paddle>();
		paddleToBallVector = this.transform.position - paddle.transform.position;
		if (levelManager.AutoplayReturn()) { LaunchBall(); }
	}

	void Update () {
		if (!levelManager.LaunchedReturn() && Input.GetMouseButtonDown(0)) { LaunchBall(); }
		else if (!levelManager.LaunchedReturn() && !levelManager.AutoplayReturn()) { ResetBall(); }
		else if (!levelManager.LaunchedReturn() && levelManager.AutoplayReturn()) { LaunchBall(); }
	}

	public void ResetBall () { 
		this.transform.position = paddle.transform.position + paddleToBallVector; 
		levelManager.LaunchedUnset();
	}
	
	void LaunchBall () {
		levelManager.LaunchedSet();
		this.GetComponent<Rigidbody2D>().velocity = new Vector2 (Random.Range(-2f, 2f), Random.Range(8f, 12f));
	}

	void CueAudio () {
		AudioSource.PlayClipAtPoint (ball, transform.position); // optional 3rd float value for volume
	}

		// this is here (mostly) for two purposes. 1: clamp velocity. 2: prevent bounce-looping with some random bounce jarring
	void OnCollisionEnter2D(Collision2D collision) {
		Vector2 jitter = new Vector2 (Random.Range(-0.14f, 0.14f), Random.Range(-0.26f, 0.26f));
		if (levelManager.LaunchedReturn()) {
			CueAudio(); // the "other" thing that happens here
			preClampVelocity = (GetComponent<Rigidbody2D>().velocity += jitter);
			currentVelocityX = Mathf.Clamp (preClampVelocity.x, -maxVelocityX, maxVelocityX);
			currentVelocityY = Mathf.Clamp (preClampVelocity.y, -maxVelocityY, maxVelocityY);
			GetComponent<Rigidbody2D>().velocity = new Vector2 (currentVelocityX, currentVelocityY);
		}
	}
}
