using UnityEngine;
using System.Collections;

public class FormationController : MonoBehaviour {
	// adjust/set in inspector!
	public GameObject enemyPrefab;
	public float reverseBuffer = -2.12f;
	public float reverseSquelch = 1.12f;
	public float spawnDelay = 0.8f;
	public GameObject resetButton;

	private float acceleration, baseAcceleration, direction, lateralVelocity, maxAcceleration, maxSpeed, padding, spawnTime, speed, xMax, xMin;
	private bool afterMatch, decelerate, gameStarted, respawn, right, shoot;
	private ArrayList enemies;
	private int finalWave, waveNumber;
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
		baseAcceleration = 0.10f;
		decelerate = true;
		direction = 0f;
		enemies = new ArrayList();
		finalWave = 42;
		waveNumber = 1;
		lateralVelocity = 0f;
		maxAcceleration = 0.003f;
		maxSpeed = 9f;
		padding = 3.4f;
		right = true;
		SetMinMaxX();

		speed = baseAcceleration;
	}

	void Update () {
		// formation motion
//		SetNextPos();

		// set position
		transform.position += new Vector3 (speed * Time.deltaTime, 0f, 0f);

		// do debugging
		string bugString = right + " X: " + transform.position.x;

		// test and flip TODO fix this
		if (transform.position.x >= xMax) { right = !right; speed = -0.1f; }
		else if (transform.position.x <= xMin) { right = !right; speed = 0.1f; }

		// do debugging
		bugString += right + " -- Speed: " + speed;

		if (right && speed < maxSpeed) speed += baseAcceleration;
		else if (!right && speed > -maxSpeed) speed -= baseAcceleration;

		// do debugging
		bugString += " : " + speed;
		Debug.Log (bugString);
	}

	public void TriggerRespawn () {
		respawn = true;
		gameStarted = true;
		Invoke ("Respawn", spawnDelay);
	}

	void FixedUpdate () {
		// TODO come up with a *good* win condition
		if (levelManager.GetScore() > 50000f) {
			Despawner();
			levelManager.WinBattle();
		}

		// win after X waves?
		if (waveNumber == finalWave && FormationIsEmpty()) {
			Despawner();
			levelManager.WinBattle();
		}

		// formation spawn control
		afterMatch = resetButton.activeSelf;
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
		else if (FormationIsFull() && waveNumber < finalWave) waveNumber++;
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
//		SetVelocity();
		// old broken motion system... looked nice at first, but closer inspection reveals odd sprite slicing on some frames
//		if (right) transform.position = new Vector3(SetXClamps(tempPos.x + lateralVelocity), tempPos.y, tempPos.z);
//		else  transform.position = new Vector3(SetXClamps(tempPos.x - lateralVelocity), tempPos.y, tempPos.z);

		// formation is kinematic, wont respond to force, I'm quite sure...
		// how to move it in delta time, while giving it a smoother motion than the binary control 
		// proposed in the udemy course....
		// direction should walk from -1 to +1 in small increments to accelerate after direction change
		// then to decelerate it needs to know when it's getting close to the edge

		transform.position += new Vector3(direction * speed * Time.deltaTime,0,0);

	}

	void BoundaryTestAndFlip () {
		// flipper
		tempPos = transform.position;
		if (tempPos.x + padding <= xMin) {
			tempPos.x = xMin + 0.001f;
			right = !right;
			direction = 0f; 
		}
		if (tempPos.x - padding >= xMax) {
			tempPos.x = xMax - 0.001f;
			right = !right;
			direction = 0f; 
		}


		// accelerator
		if (right && direction <= 1.0f) direction += baseAcceleration;
		if (!right && direction >= -1.0f) direction -= baseAcceleration;
		Debug.Log (right + " direction: " + direction + "speed: " + speed);
//			acceleration = baseAcceleration;
//			decelerate = false;
//			lateralVelocity = 0.00010f;
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
