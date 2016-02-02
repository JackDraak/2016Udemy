using UnityEngine;
using System.Collections;

public class Boundary : MonoBehaviour {

	public LevelManager levelManager;

void OnTriggerEnter2D (Collider2D trigger) {
	Debug.Log (this + " trigger " + trigger);
	levelManager.LoadLevel("Win");
}

void OnCollisionEnter2D (Collision2D collision) {
	Debug.Log (this + " collision " + collision);
}

}
