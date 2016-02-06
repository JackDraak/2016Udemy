using UnityEngine;
using System.Collections;

public class Boundary : MonoBehaviour {

	private LevelManager levelManager;

	void Start () {
		levelManager = GameObject.FindObjectOfType<LevelManager>();
	}

	void OnTriggerEnter2D (Collider2D trigger) {
		levelManager.BallsMinus();
		if (levelManager.BallsReturn() <= 0) {
			levelManager.LoadLevel("Lose");
		} else {
			levelManager.LaunchedUnset();
		}
	}
}
