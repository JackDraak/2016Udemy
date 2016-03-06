using UnityEngine;
using System.Collections;

public class FormationController : MonoBehaviour {
	// adjust/set in inspector!
	public GameObject enemyPrefab;
	public GameObject resetButton;
	public LevelManager levelManager;
	public float reverseBuffer = -2.12f;
	public float reverseSquelch = 1.12f;
	public float spawnDelay = 0.8f;
	[SerializeField]
	private float baseAcceleration, maxSpeed, padding, speed, xMax, xMin;
	[SerializeField]
	private bool afterMatch, decelerate, gameStarted, rePaddedA, rePaddedB, respawn, right;
	private ArrayList enemies;
	private int finalWave, myWave, flash;

	public void EnemyAdd (GameObject enemy) { enemies.Add (enemy); }

	void OnDrawGizmos () { Gizmos.DrawWireCube(transform.position, new Vector3 (9,9,1)); }
	float SetXClamps (float position) { return Mathf.Clamp(position, xMin, xMax); }

	void Start () {
		myWave = levelManager.GetWaveNumber();
		baseAcceleration = 0.10f;
		decelerate = true;
		enemies = new ArrayList();
		finalWave = 100;
		flash = 0;
		maxSpeed = Random.Range(5f, 6f);
		padding = 4.6f;
		right = true;
		speed = baseAcceleration;
		SetMinMaxX();
	}

	void Update () {
		// test boundary and flip if needed
		if (transform.position.x >= xMax) { right = !right; speed = -0.33f; decelerate = false; maxSpeed = Random.Range(5f + (myWave * 0.2f), 6f + (myWave * 1.15f));}
		else if (transform.position.x <= xMin) { right = !right; speed = 0.35f; decelerate = false; maxSpeed = Random.Range(5f + (myWave * 0.2f), 6f + (myWave * 1.15f));}

		// set position
		transform.position += new Vector3 (speed * Time.deltaTime, 0f, 0f);
		
		// accelerator
		if (right && speed < maxSpeed) speed += baseAcceleration;
		else if (!right && speed > -maxSpeed) speed -= baseAcceleration;

		if (speed >= maxSpeed) decelerate = true;

		if (myWave > 29 && !rePaddedA) { padding = 4.9f; rePaddedA = true; SetMinMaxX(); }
		if (myWave > 74 && !rePaddedB) { padding = 5.1f; rePaddedB = true; SetMinMaxX(); }

	/*	// TODO finish deceleration 
		if (decelerate) {
			if (transform.position.x < xMin - reverseBuffer || transform.position.x > xMax + reverseBuffer)  {
				Debug.Log ("squelch");
				if (speed > 0.1f) speed = speed / reverseSquelch;
				if (maxSpeed > 1f) maxSpeed = maxSpeed / reverseSquelch;
			}
		} */

		// TODO come up with a *good* win condition, furthermore let levelmanager control wins and losses?
		if (levelManager.GetScore() > 5000000f) {
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
		if (levelManager.GetEnemies() == 0 && !respawn && gameStarted && !afterMatch && FormationIsEmpty()) TriggerRespawn();
	}

	void StopWarn () {
		if (decelerate) return;
	}

	public void TriggerRespawn () {
		gameStarted = true;
		respawn = true;

		// difficulty tuning
		myWave = levelManager.GetWaveNumber();
		baseAcceleration = 0.10f + (myWave * 0.02f); 

		Invoke ("Respawn", spawnDelay);
	}

	bool FormationIsEmpty () {
		foreach(Transform childPositionGameObject in transform) {
			if (childPositionGameObject.childCount > 0) return false;
		} return true;
	}

	Transform RandomFreePosition () {
		Transform[] myEmptyChildren = new Transform[transform.childCount];
		int inCount = 0;
		foreach(Transform childPosition in transform) {
			if (childPosition.childCount == 0) {
				myEmptyChildren[inCount] = childPosition;
				inCount++;
			}
		}
		if (inCount > 0) return myEmptyChildren[Random.Range(0, inCount)];
		else return null;
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
		Transform freePos = RandomFreePosition();
		if (Random.Range(0,100) > 30) {
			if (freePos) FillPosition(freePos);
		}
		if (RandomFreePosition()) Invoke("Respawn", spawnDelay);
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
