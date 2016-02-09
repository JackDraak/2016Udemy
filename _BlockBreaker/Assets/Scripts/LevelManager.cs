using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class LevelManager : MonoBehaviour {

	static LevelManager instance = null;

	public static int ballCount = 2;
	public static int brickCount;
	public static bool hasStarted;
	public static int sceneIndex = 1;
	public static float score;
	public static float scoreFactor;

	private SpriteRenderer ball1, ball2, ball3, ball4, ball5;
	// TODO working on structure to expunge relic effects REE
	private ArrayList deadEffects = new ArrayList();
//	private Text hintBoard; // for bug-testing purposes
	private Color offColor = new Color (0f, 0f, 0f, 0f), onColor = new Color (1f, 1f, 1f, 0.667f);
	private Text scoreBoard;

	public void BrickCountMinus ()		{ brickCount--; BrickDestroyed(); }
	public void BrickCountPlus ()		{	brickCount++; }
	public void BrickDestroyed()		{ if (brickCount <= 0) LoadNextLevel(); }
	public int BrickGetNumRemaining ()	{ return brickCount; } 
	public int GetSceneIndex ()			{ return sceneIndex; }
	public void HasStartedFalse()		{ hasStarted = false; }
	public bool HasStartedReturn ()		{ return hasStarted; }
	public void HasStartedToggle()		{ hasStarted = !hasStarted; }
	public void HasStartedTrue()		{ hasStarted = true; }


	void Start () {	
		if (instance != null && instance != this) { Destroy (gameObject); } 
		else { instance = this; GameObject.DontDestroyOnLoad(gameObject); }
//		hintBoard = GameObject.Find ("HintBoard").GetComponent<Text>(); // remaining bricks counter in-scene
		scoreBoard = GameObject.Find ("ScoreBoard").GetComponent<Text>();
		ShowMyBalls ();
	}

	void Update () {
		// TODO this is not working as advertised.... the used game objects linger in the effects "folder" game object **some scenes are okay?
		foreach (GameObject de in deadEffects) { // more stuff for REE
			if (de && !de.GetComponent<ParticleSystem>().IsAlive()) {
				Destroy (de);
			}
		}

		if (!scoreBoard) scoreBoard = GameObject.Find ("ScoreBoard").GetComponent<Text>();
		if (scoreBoard) scoreBoard.text = ("High: " + score + "  -  [Highest: " + PlayerPrefsManager.GetTopscore() + "]");
		else Debug.LogError ("Levelmanager.cs Update() Unable to update Scoreboard");

//		if (!hintBoard) hintBoard = GameObject.Find ("HintBoard").GetComponent<Text>();
//		if (hintBoard) hintBoard.text = ("Breakable: [" + brickCount + "]");
//		else Debug.LogError ("Levelmanager.cs Update() Unable to update Hintboard");
	}

	public void  CalculateScoreFactor () {
		if (PlayerPrefsManager.GetTrails()) scoreFactor = 1.25f;
		if (PlayerPrefsManager.GetFireBalls()) scoreFactor = 1.3f;
		if (PlayerPrefsManager.GetFireBalls() && PlayerPrefsManager.GetTrails()) scoreFactor = 2.0f;
		if (PlayerPrefsManager.GetEasy()) scoreFactor = (scoreFactor * .7f);
		if (PlayerPrefsManager.GetAutoplay()) scoreFactor = (scoreFactor * 0.2f);
	}

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
		if (GameObject.FindGameObjectWithTag ("ball5")) {
			ball5 = GameObject.FindGameObjectWithTag ("ball5").GetComponent<SpriteRenderer>();
			if (ballCount > 4) ball5.color = onColor;
			if (ballCount < 5) ball5.color = offColor;
		}
	}

	public void BallDown() {
		if (ballCount-- <= 0) {
			brickCount = 0;
			if (PlayerPrefsManager.GetTopscore () < score) PlayerPrefsManager.SetTopscore (score);
			LoadLevel("Game Over");
		}
		ShowMyBalls ();
	}

	public void FreeBallin () { // set reward levels where free plays are granted
//		Debug.Log(this + " score: " + score + " and Award: " + PlayerPrefsManager.GetAward());
		if (PlayerPrefsManager.GetAward() == 0) {
			if (score > 2000 && score < 10000) { // TODO tweak these score thresholds for release
				ballCount++;
				ShowMyBalls();
				PlayerPrefsManager.SetAward(1);
			}
		}
		else if (PlayerPrefsManager.GetAward() == 1) {
			if (score > 10000 && score < 25000) {
				ballCount++;
				ShowMyBalls();
				PlayerPrefsManager.SetAward(2);
			}
		}
		else if (PlayerPrefsManager.GetAward() == 2) {
			if (score > 25000) {
				ballCount++;
				ShowMyBalls();
				PlayerPrefsManager.SetAward(3);
			}
		}
	}

	public void LoadLevel(string name){
		Cursor.visible = true;
		if (name != "Level_01") { Cursor.visible = true; }
		if (name == "Level_01") {
			Cursor.visible = false;
			ballCount = 2;
			score = 0;
			sceneIndex = 1;
			PlayerPrefsManager.SetAward(0);
		}
		brickCount = 0;
		hasStarted = false;
		SceneManager.LoadScene(name);
	}
	
	void LoadNextLevel() {
		if (PlayerPrefsManager.GetTopscore () < score) PlayerPrefsManager.SetTopscore (score);
		sceneIndex++;
		hasStarted = false;
		brickCount = 0; // overkill? didn't seem nec. but does seem like where it ought to go
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex +1);
	}
}
