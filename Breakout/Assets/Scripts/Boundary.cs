using UnityEngine;
using System.Collections;

public class Boundary : MonoBehaviour {

	private LevelManager levelManager;
	private Ball ball;

	void Start () { 
		levelManager = GameObject.FindObjectOfType<LevelManager>(); 
		ball = GameObject.FindObjectOfType<Ball>();
	}

	void OnTriggerEnter2D (Collider2D trigger) {
		levelManager.BallsMinus();
		if (levelManager.BallsReturn() <= 0) { levelManager.LoadLevel("Lose"); } 
		else { ball.ResetBall(); }
	}
}
