using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LevelManager : MonoBehaviour {

	static LevelManager instance = null;
	
	public static int ballCount = 2;
	public static int brickCount;
	public static float score;
	public static float scoreFactor;
	
	private Text scoreBoard;
	private SpriteRenderer ball1, ball2, ball3, ball4;
	private Color onColor = new Color (1f, 1f, 1f, 0.667f), offColor = new Color (0f, 0f, 0f, 0f);
	
	// working on structure to expunge relic effects REE
	private ArrayList deadEffects = new ArrayList();
	public void EffectAdd (GameObject preDE) {
		deadEffects.Add (preDE);
	}

	public void ShowMyBalls () {
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
	}
	
	void Start () {	
		if (instance != null && instance != this) {
			Destroy (gameObject);
		} else {
			instance = this;
			GameObject.DontDestroyOnLoad(gameObject);
		}

		ShowMyBalls ();

		scoreBoard = GameObject.Find ("ScoreBoard").GetComponent<Text>();
		// adjust scoring relative to difficulty at the begining of each level here
		if (PlayerPrefsManager.GetTrails()) scoreFactor = 1.25f;
		if (PlayerPrefsManager.GetFireBalls()) scoreFactor = 1.3f;
		if (PlayerPrefsManager.GetFireBalls() && PlayerPrefsManager.GetTrails()) scoreFactor = 2.0f;
		if (PlayerPrefsManager.GetEasy()) scoreFactor = (scoreFactor * .7f);
		if (PlayerPrefsManager.GetAutoplay()) scoreFactor = (scoreFactor * 0.1f);
	}

	void Update () {
		foreach (GameObject de in deadEffects) { // more stuff for REE
			if (de && !de.GetComponent<ParticleSystem>().IsAlive()) {
				Destroy (de);
			}
		}
		if (!scoreBoard) scoreBoard = GameObject.Find ("ScoreBoard").GetComponent<Text>();
		if (scoreBoard) scoreBoard.text = ("High: " + score + "  -  [Highest: " + PlayerPrefsManager.GetTopscore() + "]");
		else Debug.LogError ("Levelmanager.cs Update() Unable to update Scoreboard");
	}

	public void BallDown() {
		if (ballCount-- <= 0) {
			Brick.breakableCount = 0;
			if (PlayerPrefsManager.GetTopscore () < LevelManager.score) PlayerPrefsManager.SetTopscore (LevelManager.score);
			LoadLevel("Game Over");
		}
		ShowMyBalls (); // call after decrement in IF of BallDown
	}
	
	public void LoadNextLevel() {
		if (PlayerPrefsManager.GetTopscore () < LevelManager.score) {
			PlayerPrefsManager.SetTopscore (LevelManager.score);
		}
		Application.LoadLevel(Application.loadedLevel + 1);
	}
	
	public void BrickDestroyed() {
		if (Brick.breakableCount <= 0) {
			Invoke ("LoadNextLevel()", .5f);
		}
	}
	
	public void LoadLevel(string name){
		Cursor.visible = true;
		if (name == "_Start Menu" || name == "Level_01") {
			ballCount = 2;
			score = 0;
		}
		Brick.breakableCount = 0;
		Application.LoadLevel(name);
	}
	
	public void QuitRequest() {
		Application.Quit();
	}
}