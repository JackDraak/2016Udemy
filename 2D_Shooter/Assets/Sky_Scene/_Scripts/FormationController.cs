using UnityEngine;
using System.Collections;

public class FormationController : MonoBehaviour {
	public GameObject enemyPrefab;
	public float reverseBuffer = -2.12f;
	public float reverseSquelch = 1.12f;
	public float speedFactor = 61.3f;

	private float acceleration;
	private float baseAcceleration;
	private bool decelerate, right, shoot;
	private float lateralVelocity;
	private float maxAcceleration;
	private float maxSpeed;
	private float padding = 3.4f;
	private Vector3 tempPos;
	private float xMax, xMin;
	private LevelManager levelManager;

	void Start () {
		levelManager = GameObject.FindObjectOfType<LevelManager>(); if (!levelManager) Debug.LogError ("LEVEL_MANAGER_FAIL");
		acceleration = 0f;
		baseAcceleration = 0.00010f;
		decelerate = true;
		lateralVelocity = 0f;
		maxAcceleration = 0.003f;
		maxSpeed = 0.19f;
		right = true;
		SetMinMaxX();
		foreach (Transform child in transform) {
			GameObject enemy = Instantiate(enemyPrefab, child.transform.position, Quaternion.identity) as GameObject;
			EnemyAdd(enemy);
			levelManager.EnemyUp();
			enemy.transform.parent = child;
		}
	}

	// TODO this is not working as advertised.... the used game objects linger in the effects "folder" game object **some scenes are okay?
	private ArrayList enemies = new ArrayList();
	public void EnemyAdd (GameObject enemy) { enemies.Add (enemy); }

	void ExpungeDeadEnemies () {
		foreach (GameObject enemy in enemies) {
			if (enemy && !enemy.gameObject.activeSelf) {
			Destroy (enemy, 0.000001f);
			}
			//enemy.BroadcastMessage("Disarm");
		}
	}

	void OnDrawGizmos () { Gizmos.DrawWireCube(transform.position, new Vector3 (8,8,1)); }

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
	
	float SetXClamps (float position) { return Mathf.Clamp(position, xMin, xMax); }
	
	void Update () {
		SetNextPos();
	//	Debug.Log(enemies.Count + " enemies.Count");
	//	ExpungeDeadEnemies();
		if (levelManager.GetEnemies() <= 0) levelManager.WinBattle();

	}
}
