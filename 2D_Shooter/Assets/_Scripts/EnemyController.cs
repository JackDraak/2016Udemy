using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour {

	private bool decelerate, right, shoot;
	private float xMax, xMin;
	private float padding = 3.2f;
	private float acceleration;
	private float baseAcceleration;
	private float lateralVelocity;
	private float maxAcceleration;
	private float maxSpeed;
	private Vector3 tempPos;

	public float reverseBuffer =1f;
	public float reverseSquelch = 1f;

	void Start () { 
		acceleration = 0f;
		baseAcceleration = 0.00010f;
		lateralVelocity = 0f;
		maxAcceleration = 0.003f;
		maxSpeed = 0.21f;
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
		BoundaryTestAndFlip ();
		SetVelocity();
		if (right) transform.position = new Vector3(SetXClamps(tempPos.x + lateralVelocity), tempPos.y, tempPos.z);
		else  transform.position = new Vector3(SetXClamps(tempPos.x - lateralVelocity), tempPos.y, tempPos.z);
	}

	void BoundaryTestAndFlip () {
		tempPos = transform.position;
		if (tempPos.x <= xMin || tempPos.x >= xMax) {
			right = !right;
			lateralVelocity = 0.00010f;
			acceleration = baseAcceleration;
		}
	}
	
	void SetVelocity () {
		if (acceleration < maxAcceleration) acceleration = acceleration + baseAcceleration;
		if (lateralVelocity < maxSpeed) lateralVelocity = lateralVelocity + acceleration;
		if (tempPos.x < xMin - reverseBuffer || tempPos.x > xMax + reverseBuffer)  lateralVelocity = lateralVelocity / reverseSquelch;
		Debug.Log (lateralVelocity);
	}
	
	float SetXClamps (float position) {
		return Mathf.Clamp(position, xMin, xMax); //  0.55f, 15.41f);
	}
	
	void Update () {
		SetNextPos();
	}

	void ShootToggle () {
		shoot = !shoot;
		if (shoot) Debug.Log ("pew pew pew"); // invoke repeating
		else Debug.Log ("squelch");
	}
}
