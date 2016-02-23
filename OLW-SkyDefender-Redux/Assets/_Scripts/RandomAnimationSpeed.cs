using UnityEngine;
using System.Collections;

public class RandomAnimationSpeed : MonoBehaviour {

	public float high = 2f;
	public float low = 1f;
	public bool slowDown;
	public float squelch = 0.01f;
	public float minSpeed = 0.3f;

	private Animator animator;

	void Start () {
		animator = transform.GetComponent<Animator>();
		animator.speed = Random.Range(low, high);
	}

	void Update () {
		if (slowDown && animator.speed > minSpeed) animator.speed -= squelch;
	}
}
