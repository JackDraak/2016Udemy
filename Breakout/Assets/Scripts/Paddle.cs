using UnityEngine;
using System.Collections;

public class Paddle : MonoBehaviour {

	private Ball ball;
	private bool bWindowDrift;
	private float windowDrift;
	private LevelManager levelManager;

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
		float mouseXPos = (Input.mousePosition.x / Screen.width * 16 - 8); // in game units, 16 grid units wide
		float paddleXPos = PaddleClamp(mouseXPos);
		Vector3 paddlePosition = new Vector3 (paddleXPos, this.transform.position.y, 0f);
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

	float PaddleClamp (float xPos) {
		return Mathf.Clamp (xPos, -7.2f, 7.2f);
	}
	
	void TestDriftDirection (float span) {
		if (span > 0.52f || span < -0.52f ) bWindowDrift = !bWindowDrift;
	}

	void AutoMove (float jitter) {
		float ballXPosition = ball.transform.position.x;
		float window = ballXPosition + jitter; 
		float paddleXPosition = PaddleClamp(window);
		Vector3 paddlePosition = new Vector3 (paddleXPosition, this.transform.position.y, 0f);
		this.transform.position = paddlePosition;
	}
}
