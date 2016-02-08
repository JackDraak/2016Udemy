using UnityEngine;
using System.Collections;

public class LoseCollider : MonoBehaviour {

	public AudioClip loser;
	private LevelManager levelManager;
//	private Ball ball;
	
	void Start () {
	//	ball = GameObject.FindObjectOfType<Ball>();
		levelManager = GameObject.FindObjectOfType<LevelManager>();
	}
	
	void OnTriggerEnter2D (Collider2D trigger) { 
		AudioSource.PlayClipAtPoint (loser, transform.position);
		levelManager.HasStartedFalse();
		levelManager.BallDown();
	}
}
