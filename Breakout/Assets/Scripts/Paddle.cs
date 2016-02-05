using UnityEngine;
using System.Collections;

public class Paddle : MonoBehaviour {

	private LevelManager levelManager;
	private Ball ball;

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
		float MouseXPos = (Input.mousePosition.x / Screen.width * 16); // in game units, 16 grid units wide
		float PaddleXPos = Mathf.Clamp (MouseXPos, 0.8f, 15.2f);
		Vector3 PaddlePosition = new Vector3 (PaddleXPos -8, this.transform.position.y, 0f);
		this.transform.position = PaddlePosition;
	}

	void ComputerPaddle() {
		float BallXPos = Mathf.Clamp (ball.transform.position.x, -7.2f, 7.2f);
		Vector3 PaddlePosition = new Vector3 (BallXPos, this.transform.position.y, 0f);
		this.transform.position = PaddlePosition;
	}
}
