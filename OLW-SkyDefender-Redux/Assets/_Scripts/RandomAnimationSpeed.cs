using UnityEngine;
using System.Collections;

public class RandomAnimationSpeed : MonoBehaviour {

	public float high = 2f;
	public float low = 1f;
	public bool slowDown;

	private Animator animator;

	void Start () {
		animator = transform.GetComponent<Animator>();
		animator.speed = Random.Range(low, high);
	}

	void Update () {
		if (slowDown && animator.speed > 0.02) animator.speed -= 0.01f;
	}
}
