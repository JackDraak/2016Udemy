using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	private float acceleration;
	private float baseAcceleration;
	private float lateralVelocity;
	private float maxAcceleration;
	private float maxSpeed;
	private float padding = 0.6f;
	private bool right, shoot;
	private Vector3 tempPos;
	private float xMax, xMin;

	void SetLeftward () {
		if (right) { right = !right; lateralVelocity = 0f; acceleration = 0f;}
		SetVelocity();
		SetNextPos();
	}

	void SetRightward () {
		if (!right) { right = !right; lateralVelocity = 0f; acceleration = 0f;}
		SetVelocity();
		SetNextPos();
	}

	void SetNextPos () {
		tempPos = transform.position;
		if (right) transform.position = new Vector3(SetXClamps(tempPos.x + lateralVelocity), tempPos.y, tempPos.z);
		else  transform.position = new Vector3(SetXClamps(tempPos.x - lateralVelocity), tempPos.y, tempPos.z);
	}
	
	void SetVelocity () {
		if (acceleration < maxAcceleration) acceleration = acceleration + baseAcceleration;
		if (lateralVelocity < maxSpeed) lateralVelocity = lateralVelocity + acceleration;
	}
	
	float SetXClamps (float position) {
		return Mathf.Clamp(position, xMin, xMax); //  0.55f, 15.41f);
	}
	
	void Start () {
		float distance = transform.position.z - Camera.main.transform.position.z;
		Vector3 leftBoundary = Camera.main.ViewportToWorldPoint(new Vector3(0,0,distance));
		Vector3 rightBoundary = Camera.main.ViewportToWorldPoint(new Vector3(1,0,distance));
		xMin = leftBoundary.x + padding;
		xMax = rightBoundary.x - padding;
		acceleration = 0f;
		baseAcceleration = 0.025f;
		lateralVelocity = 0f;
		maxAcceleration = 0.7f;
		maxSpeed = 1.4f;
	}
	
	void Update () {
		if (acceleration > 0.001f) acceleration =- 0.01f; 
		if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) SetLeftward();
		if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) SetRightward();
		if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W)) ShootToggle();
		else if (Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.W)) ShootToggle();
	}

	void ShootToggle () {
		shoot = !shoot;
		if (shoot) Debug.Log ("pew pew pew"); // invoke repeating
		else Debug.Log ("squelch");
	}
}

// lecture 106