using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public GameObject player;

	private float acceleration;
	private float baseAcceleration;
	private bool right;
	private float lateralVelocity;
	private float maxAcceleration;
	private float maxSpeed;
	private Vector3 nextPos;

	void SetLeftward () {
		if (right) { right = !right; lateralVelocity = 0f; acceleration = 0f;}
//		transform.rotation.y = 17f;
		SetVelocity();
		SetNextPos();
	}

	void SetRightward() {
		if (!right) { right = !right; lateralVelocity = 0f; acceleration = 0f;}
//		transform.rotation.y = -17f;
		SetVelocity();
		SetNextPos();
	}

	void SetNextPos () {
		if (right) transform.position = new Vector3(SetXClamps(transform.position.x + lateralVelocity), transform.position.y, transform.position.z);
		else  transform.position = new Vector3(SetXClamps(transform.position.x - lateralVelocity), transform.position.y, transform.position.z);
		//		Debug.Log ("Accel:" + acceleration + ", LVelocity:" + lateralVelocity);
	}
	
	void SetVelocity () {
		if (acceleration < maxAcceleration) acceleration = acceleration + baseAcceleration;
		if (lateralVelocity < maxSpeed) lateralVelocity = lateralVelocity + acceleration;
	}
	
	float SetXClamps(float position) {
		return Mathf.Clamp(position, 0.55f, 15.41f);
	}
	
	void Start () {
		acceleration = 0f;
		baseAcceleration = 0.02f;
		lateralVelocity = 0f;
		maxAcceleration = 0.5f;
		maxSpeed = 1f;
	}
	
	void Update () {
		if (acceleration > 0.001f) acceleration =- 0.01f; 
		if (Input.GetKey(KeyCode.LeftArrow)) SetLeftward();
		else if (Input.GetKey(KeyCode.RightArrow)) SetRightward();
	//	else transform.rotation.y = 0f;
	}
}
