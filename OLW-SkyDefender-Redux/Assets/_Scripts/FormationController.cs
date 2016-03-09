using UnityEngine;
using System.Collections;

public class FormationController : MonoBehaviour {

	// adjust/set in inspector!
	public GameObject enemyPrefab;
	public GameObject resetButton;
	public LevelManager levelManager;
	public float reverseBuffer = -2.12f; // TODO someday, fix or get rid of this
	public float reverseSquelch = 1.12f; // TODO someday, fix or get rid of this
	public float spawnDelay = 0.8f;

	[SerializeField]
	private float baseAcceleration, maxSpeed, padding, speed, xMax, xMin;
	[SerializeField]
	private bool afterMatch, decelerate, gameStarted, rePaddedA, rePaddedB, rePaddedC, respawn, right, stRespawn;

	private ArrayList enemies;
	private int checkWave, finalWave, flash, thisWave, myWave;

	// public function(s)
	public void Despawner () {
		foreach (GameObject enemy in enemies) { Destroy (enemy, 0.001f); }
		levelManager.ZeroEnemies();
	}

	public void EnemyAdd (GameObject enemy) { enemies.Add (enemy); }

	public void TriggerRespawn () {
		// difficulty tuning
		myWave = levelManager.GetWaveNumber();
		baseAcceleration = 0.10f + (myWave * 0.02f); 

		gameStarted = true;
		respawn = true;
		Invoke ("Respawn", spawnDelay);
	}

	void Start () {
		baseAcceleration = 0.10f;
//		checkWave = 1;
		thisWave = 1;
		decelerate = true;
		enemies = new ArrayList();
		finalWave = 100;
		flash = 0;
		maxSpeed = Random.Range(5f, 6f);
		myWave = levelManager.GetWaveNumber();
		padding = 4.6f;
		right = true;
		speed = baseAcceleration;
		SetMinMaxX();
	}

	void Update () {
		// test boundary, set direction
		if (transform.position.x >= xMax) { right = !right; speed = -0.33f; decelerate = false; maxSpeed = Mathf.Clamp(Random.Range(4f + (myWave * 0.2f), 6f + (myWave * 1.15f)), 4f, (myWave/3)+6);}
		else if (transform.position.x <= xMin) { right = !right; speed = 0.35f; decelerate = false; maxSpeed = Mathf.Clamp(Random.Range(4f + (myWave * 0.2f), 6f + (myWave * 1.15f)), 4f, (myWave/3)+6);}

		// set position
		transform.position += new Vector3 (speed * Time.deltaTime, 0f, 0f);
		
		// set acceleration
		if (right && speed < maxSpeed) speed += baseAcceleration;
		else if (!right && speed > -maxSpeed) speed -= baseAcceleration;

		// set speed
		if (speed >= maxSpeed) decelerate = true;

		// set padding
		if (myWave > 29 && !rePaddedA) { padding = 5.1f; rePaddedA = true; SetMinMaxX(); }
		if (myWave > 49 && !rePaddedB) { padding = 5.8f; rePaddedB = true; SetMinMaxX(); }
		if (myWave > 69 && !rePaddedC) { padding = 6.4f; rePaddedC = true; SetMinMaxX(); }

		/*	// TODO finish deceleration 
		if (decelerate) {
			if (transform.position.x < xMin - reverseBuffer || transform.position.x > xMax + reverseBuffer)  {
				Debug.Log ("squelch");
				if (speed > 0.1f) speed = speed / reverseSquelch;
				if (maxSpeed > 1f) maxSpeed = maxSpeed / reverseSquelch;
			}
		} */

		// win condition: ludicrous score
		if (levelManager.GetScore() > 25000000f) {
			Despawner();
			levelManager.WinBattle();
		}

		// win condition: x waves to copmplete
		if (levelManager.GetWaveNumber() == finalWave && FormationIsEmpty()) {
			Despawner();
			levelManager.WinBattle();
		}

		// formation spawn control: should activate respawns as required.... ~1/400 wave completions doesn't, however....
		afterMatch = resetButton.activeSelf;
		if (FormationIsFull()) { 
			respawn = false; 
			stRespawn = false; 
			flash = 0; 
			checkWave = thisWave;
			thisWave = levelManager.GetWaveNumber();
			if (checkWave != thisWave) Debug.Log("Full Formation " + checkWave  + " @ " + Time.time.ToString());
		}

		if (FormationIsEmpty() && !respawn && !afterMatch) { TriggerRespawn(); }
		if (!respawn && !afterMatch && gameStarted && FormationIsEmpty()) TriggerRespawn();

		// formation !respawn debugging
		if (FormationIsEmpty() && !afterMatch) SlowTriggerRespawn(); // this ought to fix it....? saw `respawn` was true in inspector during a hang 

//		if (FormationIsEmpty() && levelManager.GetEnemies() != 0) Debug.LogError("GetEnemies != 0 but FormationEmpty");
//		if (!FormationIsEmpty() && levelManager.GetEnemies() == 0) Debug.LogError("GetEnemies = 0 but Formation!Empty");
		if (Input.GetKeyDown(KeyCode.R) && FormationIsEmpty()) SlowTriggerRespawn();
	}

	void SlowTriggerRespawn () {
		Debug.Log("STR_Phase_1 ENTER");
		if (FormationIsEmpty() && !stRespawn) {
			stRespawn = true;
			Invoke("STR", 2f);
		}
	}

	// have added wave-logging to debug output for monitoring purposes; seems to keep the spawner in the proper grove now....
	void STR () {
		Debug.Log("STR_Phase_2 ENTER");
		if (FormationIsEmpty()) { 
			Debug.Log("STR_Phase_2 EXIT"); 
			levelManager.DecrementWaveNumber();
			TriggerRespawn();
		}
	}
	
	void FillPosition (Transform pos) {
		GameObject enemy = Instantiate(enemyPrefab, pos.transform.position, Quaternion.identity) as GameObject;
		EnemyAdd(enemy);
		enemy.transform.parent = pos;
		levelManager.EnemyUp();
	}
	
	bool FormationIsEmpty () {
		foreach(Transform childPositionGameObject in transform) {
			if (childPositionGameObject.childCount > 0) return false;
		} return true;
	}

	bool FormationIsFull () {
		foreach(Transform childPosition in transform) {
			if (childPosition.childCount == 0) return false;
		} return true;
	}

	void HideWave () { levelManager.HideWave(); }

	void OnDrawGizmos () { Gizmos.DrawWireCube(transform.position, new Vector3 (9,9,1)); }
	
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
		else if (FormationIsFull() && levelManager.GetWaveNumber() <= finalWave) levelManager.IncrementWaveNumber(); // can this formationcheck be leading to the zombie waves? !RandomFreePos then...
	}
	
	float SetXClamps (float position) { return Mathf.Clamp(position, xMin, xMax); }

	void SetMinMaxX () {
		float distance = transform.position.z - Camera.main.transform.position.z;
		Vector3 leftBoundary = Camera.main.ViewportToWorldPoint(new Vector3(0,0,distance));
		Vector3 rightBoundary = Camera.main.ViewportToWorldPoint(new Vector3(1,0,distance));
		xMax = rightBoundary.x - padding;
		xMin = leftBoundary.x + padding;
	}

	void StopWarn () { if (decelerate) return; }
}
