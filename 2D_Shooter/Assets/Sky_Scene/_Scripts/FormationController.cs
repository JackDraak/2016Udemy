﻿using UnityEngine;
using System.Collections;

public class FormationController : MonoBehaviour {
	// adjust/set in inspector!
	public GameObject enemyPrefab;
	public float reverseBuffer = -2.12f;
	public float reverseSquelch = 1.12f;

	private float acceleration;
	private float baseAcceleration;
	private bool decelerate, right, shoot;
	private float lateralVelocity;
	private LevelManager levelManager;
	private float maxAcceleration;
	public float maxSpeed;
	private float padding = 3.4f;
	private Vector3 tempPos;
	private float xMax, xMin;

	void OnDrawGizmos () { Gizmos.DrawWireCube(transform.position, new Vector3 (8,8,1)); }
	float SetXClamps (float position) { return Mathf.Clamp(position, xMin, xMax); }

	void Start () {
		levelManager = GameObject.FindObjectOfType<LevelManager>();
			if (!levelManager) Debug.LogError ("LEVEL_MANAGER_FAIL");
		acceleration = 0f;
		baseAcceleration = 0.00010f;
		decelerate = true;
		lateralVelocity = 0f;
		maxAcceleration = 0.003f;
		maxSpeed = 0.19f;
		right = true;
		SetMinMaxX();
	}

	void FixedUpdate () {
		SetNextPos();
	//	if (levelManager.GetEnemies() <= 0) levelManager.WinBattle();
		if (FormationIsEmpty()) {
			 Debug.Log ("Formation is empty @ " + Time.time);
			 SpawnEnemies();
		}
	}

	bool FormationIsEmpty () {
		foreach(Transform childPositionGameObject in transform) {
			if (childPositionGameObject.childCount > 0 ) return false;
		} return true;
	}

	public void SpawnEnemies () {
		levelManager = GameObject.FindObjectOfType<LevelManager>();
			if (!levelManager) Debug.LogError ("LEVEL_MANAGER_FAIL_Stage2");
		foreach (Transform child in transform) {
			GameObject enemy = Instantiate(enemyPrefab, child.transform.position, Quaternion.identity) as GameObject;
			EnemyAdd(enemy);
			levelManager.EnemyUp();
			enemy.transform.parent = child;
		}
	}

	// TODO this is not working as advertised.... 
	// the used game objects linger in the effects "folder" game object **some scenes are okay?
	private ArrayList enemies = new ArrayList();
	public void EnemyAdd (GameObject enemy) { enemies.Add (enemy); }

	void ExpungeDeadEnemies () { // TODO clean this up / get rid of it
		foreach (GameObject enemy in enemies) {
			if (enemy && !enemy.gameObject.activeSelf) {
			Destroy (enemy, 0.000001f);
			}
		}
	}

	public void Despawner () {
		foreach (GameObject enemy in enemies) {
			Destroy (enemy, 0.000001f);
		}
		levelManager.ZeroEnemies();
	}

	void SetMinMaxX () {
		float distance = transform.position.z - Camera.main.transform.position.z;
		Vector3 leftBoundary = Camera.main.ViewportToWorldPoint(new Vector3(0,0,distance));
		Vector3 rightBoundary = Camera.main.ViewportToWorldPoint(new Vector3(1,0,distance));
		xMax = rightBoundary.x - padding;
		xMin = leftBoundary.x + padding;
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
			acceleration = baseAcceleration;
			decelerate = false;
			lateralVelocity = 0.00010f;
		}
	}
	
	void SetVelocity () {
		if (acceleration < maxAcceleration) acceleration = acceleration + baseAcceleration;
		if (lateralVelocity < maxSpeed) lateralVelocity = lateralVelocity + acceleration;
		else decelerate = true;

		if (decelerate) {
			if (tempPos.x < xMin - reverseBuffer || tempPos.x > xMax + reverseBuffer)  {
				lateralVelocity = lateralVelocity / reverseSquelch;
			}
		}
	}
}
