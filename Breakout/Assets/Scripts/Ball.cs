using UnityEngine;
using System.Collections;

public class Ball : MonoBehaviour {
	private Paddle paddle;
	private Vector3 PaddleToBallVector;
	private bool bInPlay = false;

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
}
