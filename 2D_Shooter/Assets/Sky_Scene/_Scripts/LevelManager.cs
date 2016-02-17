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
	public Button quitButton;
	public Button startButton;
	public Text startMessage;
	public Button startOverButton;
	public Text winMessage;

	private bool bCredit = false;
	private FormationController formation;
	private float playerHitPoints;
	private float playerMaxHealth = 710f;
	private int playerShipCount;

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
		creditButton.gameObject.SetActive(true);
		creditMessage.gameObject.SetActive(false);
		loseMessage.gameObject.SetActive(false);
		playerHitPoints = playerMaxHealth;
		playerShipCount = playerMaxShips;
		quitButton.gameObject.SetActive(true);
		startButton.gameObject.SetActive(true);
		startMessage.gameObject.SetActive(true);
		startOverButton.gameObject.SetActive(false);
		winMessage.gameObject.SetActive(false);
		ShowMyShips();
	}

	void Update () { ShowMyShips(); }

	void ShowMyShips() {
		if (playerShipCount > 1) extra_01.gameObject.SetActive(true);
		else extra_01.gameObject.SetActive(false);
		if (playerShipCount > 2) extra_02.gameObject.SetActive(true);
		else  extra_02.gameObject.SetActive(false);
		if (playerShipCount > 3) extra_03.gameObject.SetActive(true);
		else extra_03.gameObject.SetActive(false);
		if (playerShipCount > 4) extra_04.gameObject.SetActive(true);
		else extra_04.gameObject.SetActive(false);

		Debug.Log (Time.time + ": " + playerShipCount);
	}

	// TODO this is not working as advertised.... 
	// the used game objects linger in the effects "folder" game object **some scenes are okay?
	private ArrayList deadEffects = new ArrayList();
	public void EffectAdd (GameObject preDE) { deadEffects.Add (preDE); }
	void ExpungeDeadEffects () {
		foreach (GameObject de in deadEffects) { // more stuff for REE
			if (de && !de.GetComponent<ParticleSystem>().IsAlive()) {
				Destroy (de);
			}
		}
	}

	public void EndOfLine() {
		Application.Quit();
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
		formation.SpawnFormation();
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
		quitButton.gameObject.SetActive(false);
		startButton.gameObject.SetActive(false);
		startMessage.gameObject.SetActive(false);
		startOverButton.gameObject.SetActive(false);
		winMessage.gameObject.SetActive(false);
		formation.SpawnFormation();
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

	public void PlayerResetHitpoints () { playerHitPoints = playerMaxHealth; }

	void ConfigureAnyLevel () { Cursor.visible = true; }
	void ConfigureSkyGame () {	Cursor.visible = false;	}
	public float GetScore () { return score; }
	public void ChangeScore (float scoreDelta) { score += scoreDelta; }

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

/*	public void ShowMyBalls () {
		if (GameObject.FindGameObjectWithTag ("ball1")) {
			ball1 = GameObject.FindGameObjectWithTag ("ball1").GetComponent<SpriteRenderer>();
			if (ballCount > 0) ball1.color = onColor;
			if (ballCount < 1) ball1.color = offColor;
		}
		if (GameObject.FindGameObjectWithTag ("ball2")) {
			ball2 = GameObject.FindGameObjectWithTag ("ball2").GetComponent<SpriteRenderer>();
			if (ballCount > 1) ball2.color = onColor;
			if (ballCount < 2) ball2.color = offColor;
		}
		if (GameObject.FindGameObjectWithTag ("ball3")) {
			ball3 = GameObject.FindGameObjectWithTag ("ball3").GetComponent<SpriteRenderer>();
			if (ballCount > 2) ball3.color = onColor;
			if (ballCount < 3) ball3.color = offColor;
		}
		if (GameObject.FindGameObjectWithTag ("ball4")) {
			ball4 = GameObject.FindGameObjectWithTag ("ball4").GetComponent<SpriteRenderer>();
			if (ballCount > 3) ball4.color = onColor;
			if (ballCount < 4) ball4.color = offColor;
		}
		if (GameObject.FindGameObjectWithTag ("ball5")) {
			ball5 = GameObject.FindGameObjectWithTag ("ball5").GetComponent<SpriteRenderer>();
			if (ballCount > 4) ball5.color = onColor;
			if (ballCount < 5) ball5.color = offColor;
		}
	} */
}
