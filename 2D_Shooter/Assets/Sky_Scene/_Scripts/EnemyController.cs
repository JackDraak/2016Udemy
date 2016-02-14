using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour {
	private LevelManager levelManager;
	private float hitPoints;

	void Start () {
		levelManager = GameObject.FindObjectOfType<LevelManager>(); if (!levelManager) Debug.LogError ("LEVEL_MANAGER_FAIL");
		hitPoints = 101f;
	}
	
	void OnTriggerEnter2D (Collider2D collider) {
		if (collider.tag == "PlayerProjectile") {
			TakeDamage();
			Destroy (collider.gameObject);
		}
	}

	void TakeDamage () {
		hitPoints = hitPoints / 3 - 10;
		Debug.Log (hitPoints);
		if (hitPoints <= 0) ScoreAndDestroy();
	}

	void ScoreAndDestroy () {
		levelManager.ChangeScore(100);
		Destroy(this.gameObject, 0.001f);
	}
}
