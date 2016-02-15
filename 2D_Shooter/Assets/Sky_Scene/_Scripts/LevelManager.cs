using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class LevelManager : MonoBehaviour {
	static LevelManager instance = null;
	public static float score;
	public static int enemiesRemaining;

	public Text winMessage, loseMessage, startMessage, creditMessage;
	public Button startButton, startOverButton, creditButton;
	public GameObject enemyFormation;

	private bool bCredit = false;

	public void EnemyUp () { enemiesRemaining++; }
	public void EnemyDown () { enemiesRemaining--; }
	public int GetEnemies () { return enemiesRemaining; }

	// TODO this is not working as advertised.... the used game objects linger in the effects "folder" game object **some scenes are okay?
	private ArrayList deadEffects = new ArrayList();
	public void EffectAdd (GameObject preDE) { deadEffects.Add (preDE); }
	void ExpungeDeadEffects () {
		foreach (GameObject de in deadEffects) { // more stuff for REE
			if (de && !de.GetComponent<ParticleSystem>().IsAlive()) {
				Destroy (de);
			}
		}
	}

	public void StartGameButton () {
		score = 0;
		startMessage.gameObject.SetActive(false);
		startButton.gameObject.SetActive(false);
		creditButton.gameObject.SetActive(false);
		creditMessage.gameObject.SetActive(false);
		loseMessage.gameObject.SetActive(false);
		startOverButton.gameObject.SetActive(false);
		winMessage.gameObject.SetActive(false);

		enemyFormation.gameObject.SetActive(true);
	}

	public void RestartButton () {
		score = 0;
		// tell player to reset to full hitpoints / lives or manage it here
		startMessage.gameObject.SetActive(false);
		startButton.gameObject.SetActive(false);
		creditButton.gameObject.SetActive(false);
		creditMessage.gameObject.SetActive(false);
		loseMessage.gameObject.SetActive(false);
		startOverButton.gameObject.SetActive(false);
		winMessage.gameObject.SetActive(false);

		// do some more things so it really restarts
		enemyFormation.gameObject.SetActive(true);
	}

	public void CreditButton () {
		bCredit = !bCredit;
		startMessage.gameObject.SetActive(!bCredit);
		creditMessage.gameObject.SetActive(bCredit);
	}

	public void LoseBattle () {
		loseMessage.gameObject.SetActive(true);
		startOverButton.gameObject.SetActive(true);

		enemyFormation.gameObject.SetActive(false);
		// broadcast disarm to children/enemies
	}

	public void WinBattle () {
		winMessage.gameObject.SetActive(true);
		startOverButton.gameObject.SetActive(true);

		enemyFormation.gameObject.SetActive(false);
	}

	void Start () {	
		if (instance != null && instance != this) { Destroy (gameObject); } 
		else { instance = this; GameObject.DontDestroyOnLoad(gameObject); }
	}

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
