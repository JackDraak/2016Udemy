using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour {
	public AudioClip damage;
	public AudioClip scuttle;

	private LevelManager levelManager;
	private float hitPoints;

	void Start () {
		levelManager = GameObject.FindObjectOfType<LevelManager>(); if (!levelManager) Debug.LogError ("LEVEL_MANAGER_FAIL");
		hitPoints = 251f;
	}
	
	void OnTriggerEnter2D (Collider2D collider) {
		if (collider.tag == "PlayerProjectile") {
			TakeDamage();
			Destroy (collider.gameObject);
		}
	}

	void TakeDamage () {
		hitPoints = (hitPoints * 0.8f) - 5f;
		AudioSource.PlayClipAtPoint (damage, transform.position);
		Debug.Log ("HitPoints: " + hitPoints);
		if (hitPoints <= 0f) ScoreAndDestroy();
	}

	void ScoreAndDestroy () {
		levelManager.ChangeScore(100f);
		AudioSource.PlayClipAtPoint (scuttle, transform.position);
		Destroy(this.gameObject, 0.001f);
	}
}
