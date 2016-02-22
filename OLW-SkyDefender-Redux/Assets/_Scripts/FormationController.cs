using UnityEngine;
using System.Collections;

public class FormationController : MonoBehaviour {
	// adjust/set in inspector!
	public GameObject enemyPrefab;
	public float reverseBuffer = -2.12f;
	public float reverseSquelch = 1.12f;
	public float spawnDelay = 0.8f;
	public GameObject resetButton;

	private float baseAcceleration, direction, maxSpeed, padding, spawnTime, speed, xMax, xMin;
	private bool afterMatch, decelerate, gameStarted, passGo, respawn, right, shoot;
	private ArrayList enemies;
	private int finalWave, flash;
	private LevelManager levelManager;

	public void EnemyAdd (GameObject enemy) { enemies.Add (enemy); }

	void OnDrawGizmos () { Gizmos.DrawWireCube(transform.position, new Vector3 (9,9,1)); }
	float SetXClamps (float position) { return Mathf.Clamp(position, xMin, xMax); }

	void Start () {
		levelManager = GameObject.FindObjectOfType<LevelManager>();
			if (!levelManager) Debug.LogError ("LEVEL_MANAGER_FAIL_Start");

		baseAcceleration = 0.10f;
		decelerate = true;
		enemies = new ArrayList();
		finalWave = 42;
		flash = 0;
		maxSpeed = 9f;
		padding = 4.6f;
		right = true;
		speed = baseAcceleration;
		SetMinMaxX();
	}

	void Update () {
		// set position
		transform.position += new Vector3 (speed * Time.deltaTime, 0f, 0f);

		// test and flip TODO fix this
		if (transform.position.x >= xMax) { right = !right; speed = -0.33f; decelerate = false; }
		else if (transform.position.x <= xMin) { right = !right; speed = 0.35f; decelerate = false; }

		// accelerator
		if (right && speed < maxSpeed) speed += baseAcceleration;
		else if (!right && speed > -maxSpeed) speed -= baseAcceleration;

		if (speed == maxSpeed) decelerate = true;

	//	passGo = right;
		// decelerator
		if (decelerate) { // TODO finish deceleration 
			if (transform.position.x < xMin - reverseBuffer || transform.position.x > xMax + reverseBuffer)  {
				Debug.Log ("squelch");
				speed = speed / reverseSquelch;
			} //else if (transform.position.x <= xMin || transform.position.x >= xMax ) { right = !right; decelerate = false; }
		}
	}

	public void TriggerRespawn () {
		respawn = true;
		gameStarted = true;
		Invoke ("Respawn", spawnDelay);
	}

	void FixedUpdate () {
		// TODO come up with a *good* win condition, furthermore let levelmanager control wins and losses
		if (levelManager.GetScore() > 50000f) {
			Despawner();
			levelManager.WinBattle();
		}

		// win after X waves?
		if (levelManager.GetWaveNumber() == finalWave && FormationIsEmpty()) {
			Despawner();
			levelManager.WinBattle();
		}

		// formation spawn control
		afterMatch = resetButton.activeSelf;
		if (FormationIsFull()) { respawn = false; flash = 0; }
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

	Transform RandomFreePosition () {
	//	Transform trans_1 = NextFreePosition();
		// then what, smart guy?
		// suggestion : replace NextFreePosition with FreePosition[] the array, then you can easily select any random member to fill
		return null;
	}

	void HideWave () {
		levelManager.HideWave();
	}

	void Respawn () {
		if (flash < 6) {
			levelManager.ShowWave();
			Invoke ("HideWave", 2);
			flash++;
		}
		Transform freePos = NextFreePosition();
		if (Random.Range(0,100) > 30) {
			Debug.Log (Random.Range(0,100));
			if (freePos) FillPosition(freePos);
		}
		if (NextFreePosition()) Invoke("Respawn", spawnDelay);
		else if (FormationIsFull() && levelManager.GetWaveNumber() < finalWave) levelManager.IncrementWaveNumber();
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
}