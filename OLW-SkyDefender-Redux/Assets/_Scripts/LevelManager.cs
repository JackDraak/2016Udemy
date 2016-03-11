using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class LevelManager : MonoBehaviour {

	static LevelManager instance = null;
	public static int enemiesRemaining;
	public static int waveNumber;
	public static float score;

	[HideInInspector]
	public bool insane;

	// adjust/set in inspector!
	public Button creditButton;
	public Button insaneButton;
	public Button quitButton;
	public Button startButton;
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

	private bool bCredit, inPlay, showFramerate;
	private int bonusShipCount, playerShipCount, priorShipCount, totalFrames;
	private float deltaTime, fps, fpsAverage, priorScore, totalFrameTime;
	private FormationController formation;
	private Text frameboard, waveboard;

	// public function(s)
	public void ChangeScore (float scoreDelta) { score += scoreDelta; }
	public void EndOfLine() { Application.Quit(); }
	public float GetScore () { return score; }
	public void HideWave () { waveboard.gameObject.SetActive(false); }

	public void EnemyUp () { enemiesRemaining++; }
	public void EnemyDown () { enemiesRemaining--; }
	public int GetEnemies () { return enemiesRemaining; }
	public void ZeroEnemies () { enemiesRemaining = 0; }
	
	public int GetPlayerShips () { return playerShipCount; } 
	public bool GetPlayState () { return inPlay; }
	public void PlayerDown () { playerShipCount--; }
	public void PlayerUp () { playerShipCount++; }

	public int GetWaveNumber () { return waveNumber; }
	public void DecrementWaveNumber () { waveNumber--; }
	public void IncrementWaveNumber () { waveNumber++; }

	public void CreditButton () {
		bCredit = creditMessage.gameObject.activeInHierarchy;
		bCredit = !bCredit;
		startMessage.gameObject.SetActive(!bCredit);
		creditMessage.gameObject.SetActive(bCredit);
	}

	public void InsaneButton () { insane = true; InitGame(); }
	
	public void LoadLevel(string name){
		StoreHighs();
		ConfigureAnyLevel();
		if (name == "SkyDefender") ConfigureSkyGame(); 
		SceneManager.LoadScene(name);
	}

	public void LoseBattle () {
		loseMessage.gameObject.SetActive(true);
		EndBattle();
	}
	
	public void RestartButton () { 
		StoreHighs(); 
		HeadStart(); 
		winMessage.gameObject.SetActive(false);
		loseMessage.gameObject.SetActive(false);
		startOverButton.gameObject.SetActive(false);
	}

	public void ShowWave () { 
		waveboard.gameObject.SetActive(true);
		waveboard.text = waveNumber.ToString();
	}
	
	public void StartGameButton () { insane = false; InitGame(); }

	public void WinBattle () {
		winMessage.gameObject.SetActive(true);
		if (playerShip) playerShip.GetComponent<PlayerController>().Debuff();
		else Debug.LogError("ERROR - null reference to Player, can't debuff");
		EndBattle();
	}

	void Start () {	
		if (instance != null && instance != this) { Destroy (gameObject); } 
		else { instance = this; GameObject.DontDestroyOnLoad(gameObject); }
		insane = false;
		showFramerate = true; // TODO turn off for final relase
		Connections();
		SharedStart();
		HeadStart();
	}

	void Update () { 
		// grant bonus ships at: 25,000 | 100,000 | 500,000 | 2,500,000
		if (score > 25000 && bonusShipCount == 0) {
			playerShipCount++;
			bonusShipCount++;
		}
		if (score > 100000 && bonusShipCount == 1) {
			playerShipCount++;
			bonusShipCount++;
		}
		if (score > 500000 && bonusShipCount == 2) {
			playerShipCount++;
			bonusShipCount++;
		}
		if (score > 2500000 && bonusShipCount == 3) {
			playerShipCount++;
			bonusShipCount++;
		}

		// copnform extra ships display
		if (priorShipCount != playerShipCount) ShowMyShips();
		priorShipCount = playerShipCount;
		

		// conform scoreboard display
		if (score == 0) scoreboard.text = ("Zilch");
		if (priorScore != score) scoreboard.text = (score.ToString()); 
		priorScore = score;

		// process: frame rate calulator
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

	void ConfigureAnyLevel () { Cursor.visible = true; }

	void ConfigureSkyGame () {	Cursor.visible = false;	}

	void Connections() {
		if (!waveboard) { waveboard = GameObject.FindWithTag("Waveboard").GetComponent<Text>(); }
			if (!waveboard) Debug.LogError("Level Manager Connections !waveboard");
		if (!formation) formation = enemyFormation.GetComponent<FormationController>();
			if (!formation) Debug.Log ("Level Manager Connections !formation");
		if (!frameboard) frameboard = GameObject.FindWithTag("Frameboard").GetComponent<Text>();
			if (!frameboard) Debug.LogError("Level Manager Connections !frameboard");
		if (!scoreboard) scoreboard = GameObject.FindWithTag("Scoreboard").GetComponent<Text>();
			if (!scoreboard) Debug.LogError("Level Manager Connections !scoreboard");
	}
	
	void EndBattle () {
		Cursor.visible = true;
		enemyFormation.gameObject.SetActive(false);
		quitButton.gameObject.SetActive(true);
		startOverButton.gameObject.SetActive(true);
		music_Menu.Begin();
		music_Game.End();
		inPlay = false;
		formation.Despawner();
	}
	
	void HeadStart () {
		ShowMyShips();
		bCredit = false;
		creditButton.gameObject.SetActive(true);
		enemyFormation.gameObject.SetActive(false);
		insaneButton.gameObject.SetActive(true);
		quitButton.gameObject.SetActive(true);
		startButton.gameObject.SetActive(true);
		startMessage.gameObject.SetActive(true);
		waveboard.gameObject.SetActive(false);
		music_Menu.Begin();
		music_Game.End();
		fps = 0.0f;
		totalFrames = 0;
		totalFrameTime = 0f;
	}

	void InitGame () {
		SharedStart();
		Cursor.visible = false;
		enemyFormation.gameObject.SetActive(true);
		insaneButton.gameObject.SetActive(false);
		quitButton.gameObject.SetActive(false);
		startButton.gameObject.SetActive(false);
		startMessage.gameObject.SetActive(false);
		inPlay = true;
		formation.TriggerRespawn();
	}

	void LoadNextLevel() {
		StoreHighs();
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex +1);
	}

	void SharedStart () {
		waveNumber = 1;
		bonusShipCount = 0;
		score = 0;
		playerShipCount = playerMaxShips;
		creditButton.gameObject.SetActive(false);
		creditMessage.gameObject.SetActive(false);
		loseMessage.gameObject.SetActive(false);
		playerShip.SetActive(true);
		startOverButton.gameObject.SetActive(false);
		winMessage.gameObject.SetActive(false);
		music_Menu.End();
		music_Game.Begin();
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

	void StoreHighs () {
		if (PlayerPrefsManager.GetTopscore () < score) PlayerPrefsManager.SetTopscore (score);
	}
}
