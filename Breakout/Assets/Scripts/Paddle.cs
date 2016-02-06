using UnityEngine;
using System.Collections;

public class Paddle : MonoBehaviour {

	private LevelManager levelManager;
	private Ball ball;
	private float windowDrift;
	private bool bWindowDrift;

	void Start () {
		ball = GameObject.FindObjectOfType<Ball>();
		levelManager = GameObject.FindObjectOfType<LevelManager>();
	}

	void Update () {
		if (Input.GetKeyDown(KeyCode.A)) levelManager.AutoplayToggle();
		if (levelManager.AutoplayReturn()) ComputerPaddle();
		else MousePaddle();
	}

	void MousePaddle () {
		float mouseXPos = (Input.mousePosition.x / Screen.width * 16); // in game units, 16 grid units wide
		float paddleXPos = Mathf.Clamp (mouseXPos, 0.8f, 15.2f);
		Vector3 paddlePosition = new Vector3 (paddleXPos -8, this.transform.position.y, 0f);
		this.transform.position = paddlePosition;
	}

	void ComputerPaddle() {
		if (bWindowDrift) {
			windowDrift = windowDrift + .01f;
			TestDriftDirection(windowDrift);
			AutoMove(windowDrift);
		} else {
			windowDrift = windowDrift - .01f;
			TestDriftDirection(windowDrift);
			AutoMove(windowDrift);
		}
	}

	void TestDriftDirection (float span) {
		if (span > 0.5f || span < -0.5f ) bWindowDrift = !bWindowDrift;
	}

	void AutoMove (float jitter) {
		float ballXPosition = ball.transform.position.x;
		float window = ballXPosition + jitter; 
		float paddleXPosition = Mathf.Clamp (window, -7.2f, 7.2f);
		Vector3 paddlePosition = new Vector3 (paddleXPosition, this.transform.position.y, 0f);
		this.transform.position = paddlePosition;
	}
}
