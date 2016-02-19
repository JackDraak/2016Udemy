using UnityEngine;
using System.Collections;
public class SmokeMachine : MonoBehaviour {
	void Start () {
		ParticleSystem smoke = GetComponent<ParticleSystem>();
		smoke.Emit(10);
		Destroy(gameObject, 1.5f);
	}
}
