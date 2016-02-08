using UnityEngine;
using System.Collections;

public class LoseCollider : MonoBehaviour {
	public AudioClip loser;
	private LevelManager levelManager;
	
	void Start () {
		levelManager = GameObject.FindObjectOfType<LevelManager>();
		if (!levelManager) Debug.LogError (this + ": unable to attach to LevelManager");
	}
	
	void OnTriggerEnter2D (Collider2D trigger) { 
		AudioSource.PlayClipAtPoint (loser, transform.position);
		levelManager.HasStartedFalse();
		levelManager.BallDown();
	}
}
