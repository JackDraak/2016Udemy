using UnityEngine;
using System.Collections;

public class FormationController : MonoBehaviour {

	// adjust/set in inspector!
	public GameObject enemyPrefab;
	public GameObject resetButton;
	public LevelManager levelManager;
	public float spawnDelay = 0.8f;

	[SerializeField]
	private float baseAcceleration, maxSpeed, padding, speed, timeWaveboard, xMax, xMin;
	[SerializeField]
	private bool afterMatch, rePaddedA, rePaddedB, rePaddedC, respawn, right, showWave;

	private ArrayList enemies;
	private int checkWave, finalWave, flash, thisWave;

	// public function(s)
	public void Despawner () {
		foreach (GameObject enemy in enemies) { Destroy (enemy, 0.001f); }
		levelManager.ZeroEnemies();
	}

	public void EnemyAdd (GameObject enemy) { enemies.Add (enemy); }

	public void TriggerRespawn () {
		// difficulty tuning
		thisWave = levelManager.GetWaveNumber();
		baseAcceleration = 0.10f + (thisWave * 0.02f); 

		if (!respawn) Invoke ("Respawn", spawnDelay);
		respawn = true;
	}

	void Start () {
		baseAcceleration = 0.10f;
		thisWave = 1;
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
		// set Waveboard visability
		if (timeWaveboard + 0.5f < Time.time) showWave = false;
		if (!showWave) HideWaveboard();

		// test boundary, set direction
		if (transform.position.x >= xMax) { right = !right; speed = -0.33f; maxSpeed = Mathf.Clamp(Random.Range(4f + (thisWave * 0.2f), 6f + (thisWave * 1.15f)), 4f, (thisWave/3)+6);}
		else if (transform.position.x <= xMin) { right = !right; speed = 0.35f; maxSpeed = Mathf.Clamp(Random.Range(4f + (thisWave * 0.2f), 6f + (thisWave * 1.15f)), 4f, (thisWave/3)+6);}

		// set position
		transform.position += new Vector3 (speed * Time.deltaTime, 0f, 0f);
		
		// set acceleration
		if (right && speed < maxSpeed) speed += baseAcceleration;
		else if (!right && speed > -maxSpeed) speed -= baseAcceleration;

		// set padding
		if (thisWave > 29 && !rePaddedA) { padding = 5.1f; rePaddedA = true; SetMinMaxX(); }
		if (thisWave > 49 && !rePaddedB) { padding = 5.8f; rePaddedB = true; SetMinMaxX(); }
		if (thisWave > 69 && !rePaddedC) { padding = 6.4f; rePaddedC = true; SetMinMaxX(); }

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
			flash = 0; 
			checkWave = thisWave;
			thisWave = levelManager.GetWaveNumber();
			if (checkWave != thisWave) {
				Debug.Log("Full Formation " + checkWave  + " @ " + Time.time.ToString());
			}
		}

		// formation !respawn debugging
		if (Input.GetKeyDown(KeyCode.R) && FormationIsEmpty()) Respawn();
		if (FormationIsEmpty() && !respawn && !afterMatch) { TriggerRespawn(); }
//		if (FormationIsEmpty() && !afterMatch) Debug.LogError("formation is empty && !afterMatch"); 
//		if (FormationIsEmpty() && levelManager.GetEnemies() != 0) Debug.LogError("GetEnemies != 0 but FormationEmpty");
//		if (!FormationIsEmpty() && levelManager.GetEnemies() == 0) Debug.LogError("GetEnemies = 0 but Formation!Empty");
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

	void HideWaveboard () { levelManager.HideWave(); }

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
		if (flash < 5) {
			showWave = true;
			timeWaveboard = Time.time;
			levelManager.ShowWave();
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
}
