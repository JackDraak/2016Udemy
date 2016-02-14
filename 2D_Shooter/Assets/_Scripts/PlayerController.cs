﻿using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public GameObject zappyBolt;

	private float bulletSpeed = 220f;
	private GameObject playerGun;
	private float acceleration;
	private float baseAcceleration;
	private float lateralVelocity;
	private float maxAcceleration = 0.4f;
	private float maxSpeed= 3f;
	private float padding = 0.6f;
	private bool right, shoot;
	private Vector3 tempPos;
	private float xMax, xMin;
	private float fireTime;
	private float fireDelay = 0.3f;

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
		return Mathf.Clamp(position, xMin, xMax);
	}
	
	void Start () {
		playerGun = GameObject.FindGameObjectWithTag("PlayerGun"); if (!playerGun) Debug.LogError (this + " cant attach to PlayerGun. ERROR");
		float distance = transform.position.z - Camera.main.transform.position.z;
		Vector3 leftBoundary = Camera.main.ViewportToWorldPoint(new Vector3(0,0,distance));
		Vector3 rightBoundary = Camera.main.ViewportToWorldPoint(new Vector3(1,0,distance));
		xMin = leftBoundary.x + padding;
		xMax = rightBoundary.x - padding;
		acceleration = 0f;
		baseAcceleration = 0.025f;
		lateralVelocity = 0f;
		maxAcceleration = 0.7f;
		maxSpeed = 5f;
		fireTime = Time.time;
	}

	void Update () {
		if (acceleration > 0.001f) acceleration =- 0.01f; 
		if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) SetLeftward();
		if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) SetRightward();
		if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W)) ShootToggle();
		else if (Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.W)) ShootToggle();
	}

	void FireBlaster () {
		if (fireTime + fireDelay <= Time.time) {
			GameObject discharge = Instantiate(zappyBolt, playerGun.transform.position, Quaternion.identity) as GameObject;
			discharge.GetComponent<Rigidbody2D>().velocity += Vector2.up * bulletSpeed * Time.deltaTime;
			fireTime = Time.time;
		}
	}

	void ShootToggle () {
		shoot = !shoot;
		if (shoot) InvokeRepeating ("FireBlaster", 0.0000001f, 0.3f);
		else CancelInvoke();
	}
}

// lecture 106