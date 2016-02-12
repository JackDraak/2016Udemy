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
	private Vector3 playerPos;


	void SetNextPos () {
		playerPos = player.transform.position;
		if (right) nextPos = new Vector3(SetXClamps(playerPos.x + lateralVelocity), playerPos.y, playerPos.z);
		else  nextPos = new Vector3(SetXClamps(playerPos.x - lateralVelocity), playerPos.y, playerPos.z);
		player.transform.position = nextPos;
		Debug.Log ("Accel:" + acceleration + ", LVelocity:" + lateralVelocity);
	}

	float SetXClamps(float position) {
		return Mathf.Clamp(position, 0.55f, 15.41f);
	}

	void SetVelocity () {
		if (acceleration < maxAcceleration) acceleration = acceleration + baseAcceleration;
		if (lateralVelocity < maxSpeed) lateralVelocity = lateralVelocity + acceleration;
	}

	void SetLeftward () {
		if (right) { right = !right; lateralVelocity = 0f; acceleration = 0f;}
		SetVelocity();
		SetNextPos();
	}

	void SetRightward() {
		if (!right) { right = !right; lateralVelocity = 0f; acceleration = 0f;}
		SetVelocity();
		SetNextPos();
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
		if (Input.GetKey(KeyCode.RightArrow)) SetRightward();
	}
}
