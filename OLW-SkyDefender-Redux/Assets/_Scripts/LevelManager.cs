using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class LevelManager : MonoBehaviour {
	static LevelManager instance = null;
	public static int enemiesRemaining;
	public static int waveNumber;
	public static float score;

	//[HideInInspector]
	public bool insane;

	// adjust/set in inspector!
	public Button creditButton;
	public Button quitButton;
	public Button startButton;
	public Button startInsaneButton;
	public Button startOverButton;
	public GameObject enemyFormation;
	public GameObject extra_01;
	public GameObject extra_02;
	public GameObject extra_03;
	public GameObject extra_04;
	public ProceduralMusic music_Menu;
	public ProceduralMusic music_Game;
	public GameObject playerShip;
	public int playerMaxShips;
	public Text creditMessage;
	public Text loseMessage;
	public Text scoreboard;
	public Text startMessage;
	public Text winMessage;

	private bool bCredit, showFramerate;
	private FormationController formation;
	private float deltaTime, fps, fpsAverage, priorScore, totalFrameTime;
	private Text frameboard, waveboard;
	private int playerShipCount, priorShipCount, totalFrames;

	public int GetPlayerShips () { return playerShipCount; } 
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
	
	void ConfigureAnyLevel () { Cursor.visible = true; }
	void ConfigureSkyGame () {	Cursor.visible = false;	}

	void Start () {	
		if (instance != null && instance != this) { Destroy (gameObject); } 
		else { instance = this; GameObject.DontDestroyOnLoad(gameObject); }

		insane = false;
		bCredit = false;

		Connections();
		SharedStart();
		ShowMyShips();

		creditButton.gameObject.SetActive(true);
		enemyFormation.gameObject.SetActive(false);
		quitButton.gameObject.SetActive(true);
		startButton.gameObject.SetActive(true);
		startInsaneButton.gameObject.SetActive(true);
		startMessage.gameObject.SetActive(true);
		waveboard.gameObject.SetActive(false);
		music_Menu.Begin();
		music_Game.End();

		fps = 0.0f;
		showFramerate = true; // TODO turn off for final relase
		totalFrames = 0;
		totalFrameTime = 0f;
	}

	void SharedStart () {
		waveNumber = 1;
		score = 0;
		playerShipCount = playerMaxShips;
		playerShip.SetActive(true);
		creditButton.gameObject.SetActive(false);
		creditMessage.gameObject.SetActive(false);
		loseMessage.gameObject.SetActive(false);
		startOverButton.gameObject.SetActive(false);
		winMessage.gameObject.SetActive(false);
		music_Menu.End();
		music_Game.Begin();
	}

	void InitGame () {
		SharedStart();
		Cursor.visible = false;
		enemyFormation.gameObject.SetActive(true);
		quitButton.gameObject.SetActive(false);
		startButton.gameObject.SetActive(false);
		startInsaneButton.gameObject.SetActive(false);
		startMessage.gameObject.SetActive(false);
		formation.TriggerRespawn();
	}

	public void InsaneMode () { insane = true; InitGame(); }

	void Update () { 
		// update extra ships
		if (priorShipCount != playerShipCount) ShowMyShips();
		priorShipCount = playerShipCount;

		//update scoreboard
		if (score == 0) scoreboard.text = ("Zip");
		if (priorScore != score) scoreboard.text = (score.ToString()); 
		priorScore = score;

		// frame rate calulator
		if (Input.GetKeyDown(KeyCode.F)) showFramerate = !showFramerate;
		if (showFramerate) {
			float myDelta = Time.deltaTime;
			fps = 1 / myDelta;
			totalFrameTime += myDelta;
			totalFrames++;
			fpsAverage = totalFrames/totalFrameTime;
			float myFPS = Mathf.Round(fps);
			float myMean = Mathf.Round(fpsAverage);
			frameboard.text = myFPS.ToString() + "/" + myMean.ToString() + " (fps/mean)\n[F to hide/show]";
		} else frameboard.text = "";
		if (totalFrameTime >= 3.0f) {
			totalFrames = 0;
			totalFrameTime = 0;
		}
	}

	void Connections() {
		if (!waveboard) { waveboard = GameObject.FindWithTag("Waveboard").GetComponent<Text>(); }
			if (!waveboard) Debug.LogError("FAIL tag Waveboard");
		if (!formation) formation = enemyFormation.GetComponent<FormationController>();
			if (!formation) Debug.Log ("formation 2 pickup error");
		if (!frameboard) frameboard = GameObject.FindWithTag("Frameboard").GetComponent<Text>();
			if (!frameboard) Debug.LogError("FAIL tag Frameboard");
		if (!scoreboard) scoreboard = GameObject.FindWithTag("Scoreboard").GetComponent<Text>();
			if (!scoreboard) Debug.LogError("FAIL tag Scoreboard");
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

	public void StartGameButton () { InitGame(); }
	public void RestartButton () { StoreHighs(); InitGame(); } 

	public void CreditButton () {
		bCredit = creditMessage.gameObject.activeInHierarchy;
		bCredit = !bCredit;
		startMessage.gameObject.SetActive(!bCredit);
		creditMessage.gameObject.SetActive(bCredit);
	}

	public void LoseBattle () {
		loseMessage.gameObject.SetActive(true);
		EndBattle();
	}

	public void WinBattle () {
		winMessage.gameObject.SetActive(true);
		EndBattle();
	}

	void EndBattle () {
		Cursor.visible = true;
		quitButton.gameObject.SetActive(true);
		startOverButton.gameObject.SetActive(true);
		enemyFormation.gameObject.SetActive(false);
		music_Menu.Begin();
		music_Game.End();
		formation.Despawner();
	}

	public void LoadLevel(string name){
		StoreHighs();
		ConfigureAnyLevel();
		if (name == "SkyDefender") ConfigureSkyGame (); 
		SceneManager.LoadScene(name);
	}
	
	void LoadNextLevel() {
		StoreHighs();
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex +1);
	}
	
	void StoreHighs () {
		if (PlayerPrefsManager.GetTopscore () < score) PlayerPrefsManager.SetTopscore (score);
	}
}
