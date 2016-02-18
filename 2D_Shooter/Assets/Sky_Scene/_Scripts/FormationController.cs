﻿using UnityEngine;
using System.Collections;

public class FormationController : MonoBehaviour {
	// adjust/set in inspector!
	public GameObject enemyPrefab;
	public float reverseBuffer = -2.12f;
	public float reverseSquelch = 1.12f;
	public float spawnDelay = 0.8f;
	public GameObject resetButton;

	private float acceleration, baseAcceleration, lateralVelocity, maxAcceleration, maxSpeed, padding, spawnTime, xMax, xMin;
	private bool decelerate, gameStarted, respawn, right, shoot;
	private ArrayList enemies;
	private int waveNumber;
	private LevelManager levelManager;
	private Vector3 tempPos;

	public void EnemyAdd (GameObject enemy) { enemies.Add (enemy); }
	public int GetWaveNumber () { return waveNumber; }
	public void ResetWaveNumber () { waveNumber = 1; }

	void OnDrawGizmos () { Gizmos.DrawWireCube(transform.position, new Vector3 (8,8,1)); }
	float SetXClamps (float position) { return Mathf.Clamp(position, xMin, xMax); }

	void Start () {
		levelManager = GameObject.FindObjectOfType<LevelManager>();
			if (!levelManager) Debug.LogError ("LEVEL_MANAGER_FAIL_Start");

		acceleration = 0f;
		baseAcceleration = 0.00010f;
		decelerate = true;
		enemies = new ArrayList();
		waveNumber = 1;
		lateralVelocity = 0f;
		maxAcceleration = 0.003f;
		maxSpeed = 0.19f;
		padding = 3.4f;
		right = true;
		SetMinMaxX();
	}

	public void TriggerRespawn () {
		respawn = true;
		gameStarted = true;
		Invoke ("Respawn", spawnDelay);
	}

	void FixedUpdate () {
		SetNextPos();
		// TODO come up with a *good* win condition
		if (levelManager.GetScore() > 300f) {
			Despawner();
			levelManager.WinBattle();
		}
		bool afterMatch = resetButton.activeSelf;

		if (FormationIsFull()) { respawn = false; levelManager.HideWave(); }
		if (FormationIsEmpty() && !respawn && !afterMatch) { TriggerRespawn(); }
		if (levelManager.GetEnemies() == 0 && !respawn && gameStarted && !afterMatch) { TriggerRespawn(); }
	}

	bool FormationIsEmpty () {
		foreach(Transform childPositionGameObject in transform) {
			if (childPositionGameObject.childCount > 0) return false;
		} return true;
	}

	Transform NextFreePosition () {
		foreach(Transform childPosition in transform) {
			if (childPosition.childCount == 0) return childPosition;
		} return null;
	}

	void Respawn () {
		levelManager.ShowWave();
		Transform freePos = NextFreePosition();
		if (freePos) FillPosition(freePos);
		if (NextFreePosition()) Invoke("Respawn", spawnDelay);
		else if (FormationIsFull()) waveNumber++;
	}

	bool FormationIsFull () {
		foreach(Transform childPosition in transform) {
			if (childPosition.childCount == 0) return false;
		} return true;
	}

	void FillPosition (Transform pos) {
		GameObject enemy = Instantiate(enemyPrefab, pos.transform.position, Quaternion.identity) as GameObject;
		EnemyAdd(enemy);
		enemy.transform.parent = pos;
		levelManager.EnemyUp();
	}

	public void Despawner () {
		foreach (GameObject enemy in enemies) {
			Destroy (enemy, 0.001f);
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
