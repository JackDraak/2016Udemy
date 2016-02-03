using UnityEngine;
using System.Collections;

public class Boundary : MonoBehaviour {

	private LevelManager levelManager;

void Start () {
	levelManager = GameObject.FindObjectOfType<LevelManager>();
}

void OnTriggerEnter2D (Collider2D trigger) {
//	Debug.Log (this + " trigger " + trigger);
	levelManager.LoadLevel("Lose");
	}

void OnCollisionEnter2D (Collision2D collision) {
//	Debug.Log (this + " collision " + collision);
	}
	
}
