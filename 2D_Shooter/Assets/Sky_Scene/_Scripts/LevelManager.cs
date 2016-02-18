using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class LevelManager : MonoBehaviour {
	static LevelManager instance = null;
	public static int enemiesRemaining;
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

	private bool bCredit = false;
	private FormationController formation;
	private float playerHitPoints;
	private float playerMaxHealth;
	private int playerShipCount;
	private Text scoreboard, waveboard;

	public float GetPlayerHealth () { return playerHitPoints; }
	public float GetPlayerMaxHealth () { return playerMaxHealth; }
	public int GetPlayerShips () { return playerShipCount; }
	public void PlayerChangeHealth (float pips) { playerHitPoints += pips; }
	public void PlayerDown () { playerShipCount--; }
	public void PlayerUp () { playerShipCount++; }

	public void EnemyUp () { enemiesRemaining++; }
	public void EnemyDown () { enemiesRemaining--; }
	public int GetEnemies () { return enemiesRemaining; }
	public void ZeroEnemies () { enemiesRemaining = 0; }

	void Start () {	
		if (instance != null && instance != this) { Destroy (gameObject); } 
		else { instance = this; GameObject.DontDestroyOnLoad(gameObject); }

		if (!formation) formation = enemyFormation.GetComponent<FormationController>();
			if (!formation) Debug.Log ("formation 2 pickup error");
		scoreboard = GameObject.FindWithTag("Scoreboard").GetComponent<Text>();
			if (!scoreboard) Debug.LogError("FAIL tag Scoreboard");
		waveboard = GameObject.FindWithTag("Waveboard").GetComponent<Text>();
			if (!waveboard) Debug.LogError("FAIL tag Waveboard");

		playerMaxHealth = 710f;
		playerHitPoints = playerMaxHealth;
		playerShipCount = playerMaxShips;
		creditButton.gameObject.SetActive(true);
		creditMessage.gameObject.SetActive(false);
		loseMessage.gameObject.SetActive(false);
		quitButton.gameObject.SetActive(true);
		startButton.gameObject.SetActive(true);
		startMessage.gameObject.SetActive(true);
		startOverButton.gameObject.SetActive(false);
		waveboard.gameObject.SetActive(false);
		winMessage.gameObject.SetActive(false);
		ShowMyShips();
	}

	void Update () { 
		ShowMyShips();
		scoreboard.text = ("Score: " + score); 
	}

	public void ShowWave () { 
		waveboard.gameObject.SetActive(true);
		int waveNumber = formation.GetWaveNumber();
		waveboard.text = waveNumber.ToString() + "/10";
	}

	public void HideWave () { waveboard.gameObject.SetActive(false); }
	public void ChangeScore (float scoreDelta) { score += scoreDelta; }
	public void EndOfLine() { Application.Quit(); }
	public float GetScore () { return score; }
	public void PlayerResetHitpoints () { playerHitPoints = playerMaxHealth; }
	
	void ConfigureAnyLevel () { Cursor.visible = true; }
	void ConfigureSkyGame () {	Cursor.visible = false;	}

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
		formation.Despawner();
		formation.TriggerRespawn();
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
		waveboard.gameObject.SetActive(false);
		quitButton.gameObject.SetActive(true);
		startOverButton.gameObject.SetActive(true);
		winMessage.gameObject.SetActive(true);

		formation.Despawner();
		enemyFormation.gameObject.SetActive(false);
	}

	public void LoadLevel(string name){
		StoreHighs();
		ConfigureAnyLevel();
		if (name == "SkyGame") ConfigureSkyGame (); 
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
