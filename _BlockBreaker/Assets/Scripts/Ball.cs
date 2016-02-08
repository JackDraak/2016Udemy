using UnityEngine;
using System.Collections;

public class Ball : MonoBehaviour {

	public AudioClip ball;
	
	private bool fire, trails;
	private Paddle paddle;
	private Vector3 paddleToBallVector;
	private float maxVelocityX = 13f;
	private float currentVelocityX;
	private float maxVelocityY = 19f;
	private float currentVelocityY;
	private Vector2 preClampVelocity;
	private GameObject fireBalls, trippyTrails, sphere;
	private GameObject startNote;
	private LevelManager levelManager;

	void Start () {
		levelManager = GameObject.FindObjectOfType<LevelManager>();
		if (!levelManager) Debug.LogError (this + ": unable to attach to LevelManager");
		paddle = GameObject.FindObjectOfType<Paddle>();
		paddleToBallVector = this.transform.position - paddle.transform.position;
		maxVelocityY = (1.18f * (PlayerPrefsManager.GetSpeed ()) * maxVelocityY);
		maxVelocityX = (0.7f * (PlayerPrefsManager.GetSpeed ()) * maxVelocityX); // less variance than in Y
		if (GameObject.FindGameObjectWithTag ("StartNote")) startNote = GameObject.FindGameObjectWithTag ("StartNote");
		
		fire = PlayerPrefsManager.GetFireBalls ();
		trails = PlayerPrefsManager.GetTrails ();
		fireBalls = GameObject.FindGameObjectWithTag ("fireball");
		trippyTrails = GameObject.FindGameObjectWithTag ("trail");
		sphere = GameObject.FindGameObjectWithTag ("sphere");
		fireBalls.SetActive (fire);
		sphere.SetActive (!fire);
		trippyTrails.SetActive (trails);
	}
	
	void Update () {
		// lock ball (relative) to paddle if game !started || ballDropped
		if (!levelManager.HasStartedTest()) {
			this.transform.position = paddle.transform.position + paddleToBallVector;
			
			// launch the ball and begin play on mouse-click
			if (Input.GetMouseButtonDown(0)) {
				if (startNote) startNote.SetActive (false);
				levelManager.HasStartedSet();
				this.GetComponent<Rigidbody2D>().velocity = new Vector2 (Random.Range(-12f, 12f), Random.Range(8f, 10f));
			}
		}
	}

	// this is here for two purposes. 1: clamp velocity. 2: prevent bounce-looping with some random bounce jarring
	void OnCollisionEnter2D(Collision2D collision) {
		Vector2 tweak = new Vector2 (Random.Range(-0.25f, 0.25f), Random.Range(-0.15f, 0.15f));
		if (levelManager.HasStartedTest()) {
			AudioSource.PlayClipAtPoint (ball, transform.position); // optional 3rd float value for volume
			preClampVelocity = (GetComponent<Rigidbody2D>().velocity += tweak);
			currentVelocityX = Mathf.Clamp (preClampVelocity.x, -maxVelocityX, maxVelocityX);
			currentVelocityY = Mathf.Clamp (preClampVelocity.y, -maxVelocityY, maxVelocityY);
			GetComponent<Rigidbody2D>().velocity = new Vector2 (currentVelocityX, currentVelocityY);
		}
	}
}