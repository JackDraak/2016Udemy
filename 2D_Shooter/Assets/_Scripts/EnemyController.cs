using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour {

	private bool right, shoot;
	private float xMax, xMin;
	private float padding = 0.6f;
	private float acceleration;
	private float baseAcceleration;
	private float lateralVelocity;
	private float maxAcceleration;
	private float maxSpeed;
	private Vector3 tempPos;

	void Start () { 
		acceleration = 0f;
		baseAcceleration = 0.025f;
		lateralVelocity = 0f;
		maxAcceleration = 0.7f;
		maxSpeed = 1.4f;
		right = true;
		shoot = false;
		SetMinMaxX();
	}

	void SetMinMaxX () {
		float distance = transform.position.z - Camera.main.transform.position.z;
		Vector3 leftBoundary = Camera.main.ViewportToWorldPoint(new Vector3(0,0,distance));
		Vector3 rightBoundary = Camera.main.ViewportToWorldPoint(new Vector3(1,0,distance));
		xMin = leftBoundary.x + padding;
		xMax = rightBoundary.x - padding;
	}

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
	
	void Update () {
		// test direction
		if (right) {
		// move right (test boundary, toggle direction if needed, move)
		} else {
			// move left (test boundary, toggle direction if needed, move)
		}
	}

	void ShootToggle () {
		shoot = !shoot;
		if (shoot) Debug.Log ("pew pew pew"); // invoke repeating
		else Debug.Log ("squelch");
	}
}
