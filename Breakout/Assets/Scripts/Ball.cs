using UnityEngine;
using System.Collections;

public class Ball : MonoBehaviour {

	public AudioClip ball;

	private float currentVelocityX;
	private float currentVelocityY;
	private LevelManager levelManager;
	private float maxVelocityX = 12f;
	private float maxVelocityY = 19f;
	private Paddle paddle;
	private Vector3 PaddleToBallVector;
	private Vector2 preClampVelocity;

	void Start () {
		levelManager = GameObject.FindObjectOfType<LevelManager>();
		paddle = GameObject.FindObjectOfType<Paddle>();
		PaddleToBallVector = this.transform.position - paddle.transform.position;
	}

	void Update () {
		if (Input.GetMouseButtonDown(0)) {
			levelManager.SetInPlay();
			this.GetComponent<Rigidbody2D>().velocity = new Vector2 (2f, 10f);
		}
		else if (!levelManager.ReturnInPlay()) this.transform.position = paddle.transform.position + PaddleToBallVector;
	}

	void CueAudio () {
		AudioSource.PlayClipAtPoint (ball, transform.position); // optional 3rd float value for volume
	}

		// this is here (mostly) for two purposes. 1: clamp velocity. 2: prevent bounce-looping with some random bounce jarring
	void OnCollisionEnter2D(Collision2D collision) {
		Vector2 tweak = new Vector2 (Random.Range(-0.15f, 0.15f), Random.Range(-0.25f, 0.25f));
		if (levelManager.ReturnInPlay()) {
			CueAudio(); // the "other" thing that happens here
			preClampVelocity = (GetComponent<Rigidbody2D>().velocity += tweak);
			currentVelocityX = Mathf.Clamp (preClampVelocity.x, -maxVelocityX, maxVelocityX);
			currentVelocityY = Mathf.Clamp (preClampVelocity.y, -maxVelocityY, maxVelocityY);
			GetComponent<Rigidbody2D>().velocity = new Vector2 (currentVelocityX, currentVelocityY);
		}
	}
}
