using UnityEngine;
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
		// TODO come up with a *good* win condition
		if (levelManager.GetScore() > 1500f) {
			levelManager.WinBattle();
			Despawner();
		}
	}

	void Update () {
		if (FormationIsEmpty()) {
			Debug.Log ("Formation is empty @ " + Time.time);
			SpawnFullFormation();
		}
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

	void SpawnFullFormation () {
		while (!FormationIsFull()) {
			Transform freePos = NextFreePosition();
			if (freePos) FillPosition(freePos);
		}
	}

	bool FormationIsFull () {
		foreach(Transform childPosition in transform) {
			if (childPosition.childCount == 0) return false;
		} return true;
	}

	public void SpawnFormation () {
		levelManager = GameObject.FindObjectOfType<LevelManager>();
			if (!levelManager) Debug.LogError ("LEVEL_MANAGER_FAIL_Stage2"); // again... why the heck?? what'up LevelManager? You're drunk!
		foreach (Transform child in transform) {
			FillPosition(child);
		}
	}

	void FillPosition (Transform pos) {
		GameObject enemy = Instantiate(enemyPrefab, pos.transform.position, Quaternion.identity) as GameObject;
		EnemyAdd(enemy);
		enemy.transform.parent = pos;
		levelManager.EnemyUp();
	}

	// TODO this is not working as advertised.... 
	// the used game objects linger in the effects "folder" game object **some scenes are okay?
	private ArrayList enemies = new ArrayList();
	public void EnemyAdd (GameObject enemy) { enemies.Add (enemy); }

	void ExpungeDeadEnemies () { // TODO clean this up / get rid of it
		foreach (GameObject enemy in enemies) {
			if (enemy && !enemy.gameObject.activeSelf) {
			Destroy (enemy, 0.001f);
			}
		}
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
