using UnityEngine;
using System.Collections;

public class Ball : MonoBehaviour {

	public AudioClip ball;

	private Paddle paddle;
	private Vector3 PaddleToBallVector;
	private bool bInPlay = false;
	private float maxVelocityX = 13f;
	private float currentVelocityX;
	private float maxVelocityY = 19f;
	private float currentVelocityY;
	private Vector2 preClampVelocity;

	void Start () {
		paddle = GameObject.FindObjectOfType<Paddle>();
		PaddleToBallVector = this.transform.position - paddle.transform.position;
	}

	void Update () {
		if (Input.GetMouseButtonDown(0)) {
			bInPlay = true;
			this.GetComponent<Rigidbody2D>().velocity = new Vector2 (2f, 10f);
		}
		else if (!bInPlay) this.transform.position = paddle.transform.position + PaddleToBallVector;
	}

		// this is here for two purposes. 1: clamp velocity. 2: prevent bounce-looping with some random bounce jarring
	void OnCollisionEnter2D(Collision2D collision) {
		Vector2 tweak = new Vector2 (Random.Range(-0.25f, 0.25f), Random.Range(-0.15f, 0.15f));
		if (bInPlay) {
			AudioSource.PlayClipAtPoint (ball, transform.position); // optional 3rd float value for volume
			preClampVelocity = (rigidbody2D.velocity += tweak);
			currentVelocityX = Mathf.Clamp (preClampVelocity.x, -maxVelocityX, maxVelocityX);
			currentVelocityY = Mathf.Clamp (preClampVelocity.y, -maxVelocityY, maxVelocityY);
			rigidbody2D.velocity = new Vector2 (currentVelocityX, currentVelocityY);
		}
	}

}
