using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour {
	static GameManager instance = null;
	public static int enemiesRemaining;
	public static int waveNumber;
	public static float score;

	// adjust/set in inspector!
	public Button creditButton;
	public Text creditMessage;
	public GameObject enemyFormation;
	public GameObject extra_01;
	public GameObject extra_02;
	public GameObject extra_03;
	public GameObject extra_04;
	public Text loseMessage;
	public int playerMaxShips;
	public GameObject playerShip;
	public Button quitButton;
	public Button startButton;
	public Text startMessage;
	public Button startOverButton;
	public Text winMessage;

	private bool bCredit, showFramerate;
	private FormationController formation;
	private float playerHitPoints;
	private float playerMaxHealth;
	private int playerShipCount, priorShipCount, totalFrames;
	private Text frameboard, scoreboard, waveboard;
	private float deltaTime, fps, fpsAverage, totalFrameTime;

	public float GetPlayerHealth () { return playerHitPoints; }
	public float GetPlayerMaxHealth () { return playerMaxHealth; }
	public int GetPlayerShips () { return playerShipCount; }
	public void PlayerChangeHealth (float pips) { playerHitPoints += pips; }
	public void PlayerDown () { playerShipCount--; }
	public void PlayerUp () { playerShipCount++; }

	public int GetWaveNumber () { return waveNumber; }
	public void ResetWaveNumber () { waveNumber = 1; }
	public void IncrementWaveNumber () { waveNumber++; }

	public void EnemyUp () { enemiesRemaining++; }
	public void EnemyDown () { enemiesRemaining--; }
	public int GetEnemies () { return enemiesRemaining; }
	public void ZeroEnemies () { enemiesRemaining = 0; }

	public void ChangeScore (float scoreDelta) { score += scoreDelta; }
	public void EndOfLine() { Application.Quit(); }
	public float GetScore () { return score; }
	public void HideWave () { waveboard.gameObject.SetActive(false); }
	public void PlayerResetHitpoints () { playerHitPoints = playerMaxHealth; }

	void Start () {	
		if (instance != null && instance != this) { Destroy (gameObject); } 
		else { instance = this; GameObject.DontDestroyOnLoad(gameObject); }

		if (!formation) formation = enemyFormation.GetComponent<FormationController>();
			if (!formation) Debug.Log ("formation 2 pickup error");
		frameboard = GameObject.FindWithTag("Frameboard").GetComponent<Text>();
			if (!frameboard) Debug.LogError("FAIL tag Frameboard");
		scoreboard = GameObject.FindWithTag("Scoreboard").GetComponent<Text>();
			if (!scoreboard) Debug.LogError("FAIL tag Scoreboard");
		waveboard = GameObject.FindWithTag("Waveboard").GetComponent<Text>();
			if (!waveboard) Debug.LogError("FAIL tag Waveboard");

		playerMaxHealth = 420f;
		playerHitPoints = playerMaxHealth;
		playerShipCount = playerMaxShips;
		bCredit = false;
		creditButton.gameObject.SetActive(true);
		creditMessage.gameObject.SetActive(false);
		enemyFormation.gameObject.SetActive(false);
		loseMessage.gameObject.SetActive(false);
		quitButton.gameObject.SetActive(true);
		startButton.gameObject.SetActive(true);
		startMessage.gameObject.SetActive(true);
		startOverButton.gameObject.SetActive(false);
		waveboard.gameObject.SetActive(false);
		winMessage.gameObject.SetActive(false);
		ShowMyShips();
		priorShipCount = playerShipCount;
		fps = 0.0f;
		showFramerate = true; // TODO turn off for relase
		totalFrames = 0;
		totalFrameTime = 0f;
		waveNumber = 1;
	}

	void Update () { 
		if (priorShipCount != playerShipCount) ShowMyShips();
		scoreboard.text = ("Score: " + score); 

		// frame rate calulator
		deltaTime += Time.deltaTime;
		deltaTime /= 2.0f;
		fps = 1.0f/deltaTime;
		totalFrameTime += deltaTime;
		totalFrames++;
		fpsAverage = totalFrames/totalFrameTime;
		if (Input.GetKeyDown(KeyCode.F)) showFramerate = !showFramerate;
		if (showFramerate) {
			float myFPS = Mathf.Round(fps);
			float myMean = Mathf.Round(fpsAverage);
			frameboard.text = myFPS.ToString() + "/" + myMean.ToString() + " (fps/mean)\n[F to hide/show]";
		} else frameboard.text = "";
		if (totalFrameTime >= 3.0f) {
			totalFrames = 0;
			totalFrameTime = 0;
		}
		priorShipCount = playerShipCount;
	}

	public void ShowWave () { 
		waveboard.gameObject.SetActive(true);
		waveboard.text = waveNumber.ToString();
	}

	void ShowMyShips() {
		if (playerShipCount > 1) extra_01.gameObject.SetActive(true);
		else extra_01.gameObject.SetActive(false);
		if (playerShipCount > 2) extra_02.gameObject.SetActive(true);
		else  extra_02.gameObject.SetActive(false);
		if (playerShipCount > 3) extra_03.gameObject.SetActive(true);
		else extra_03.gameObject.SetActive(false);
		if (playerShipCount > 4) extra_04.gameObject.SetActive(true);
		else extra_04.gameObject.SetActive(false);
	}

	public void StartGameButton () {
		score = 0;
		creditButton.gameObject.SetActive(false);
		creditMessage.gameObject.SetActive(false);
		enemyFormation.gameObject.SetActive(true);
		loseMessage.gameObject.SetActive(false);
		quitButton.gameObject.SetActive(false);
		startButton.gameObject.SetActive(false);
		startMessage.gameObject.SetActive(false);
		startOverButton.gameObject.SetActive(false);
		winMessage.gameObject.SetActive(false);
		formation.TriggerRespawn();
		waveNumber = 1;

	}

	public void RestartButton () {
		Debug.Log (enemiesRemaining + " enemies.");
		score = 0;
		playerHitPoints = playerMaxHealth;
		playerShipCount = playerMaxShips;
		creditButton.gameObject.SetActive(false);
		creditMessage.gameObject.SetActive(false);
		enemyFormation.gameObject.SetActive(true);
		loseMessage.gameObject.SetActive(false);
		playerShip.SetActive(true);
		quitButton.gameObject.SetActive(false);
		startButton.gameObject.SetActive(false);
		startMessage.gameObject.SetActive(false);
		startOverButton.gameObject.SetActive(false);
		winMessage.gameObject.SetActive(false);
		formation.TriggerRespawn();
		waveNumber = 1;
	}

	public void CreditButton () {
		bCredit = !bCredit;
		startMessage.gameObject.SetActive(!bCredit);
		creditMessage.gameObject.SetActive(bCredit);
	}

	public void LoseBattle () {
		loseMessage.gameObject.SetActive(true);
		quitButton.gameObject.SetActive(true);
		startOverButton.gameObject.SetActive(true);

		formation.Despawner();
		enemyFormation.gameObject.SetActive(false);
	}

	public void WinBattle () {
		quitButton.gameObject.SetActive(true);
		startOverButton.gameObject.SetActive(true);
		winMessage.gameObject.SetActive(true);

		formation.Despawner();
		enemyFormation.gameObject.SetActive(false);
	}
}
