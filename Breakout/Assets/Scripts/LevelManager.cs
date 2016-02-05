using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {
	public static bool bInPlay = false;
	public static bool bAutoPlay = false;
	public static int cBallsRemaining = 3;
	public int cBricksRemaining = 0;

	void Update () {
		if (cBricksRemaining <= 0) {
			LoadNextLevel();
		}
	}

	public void LoadLevel(string LevelName){
		SceneManager.LoadScene(LevelName);
		Debug.Log("LOG : Level Manager : Load Level : " + LevelName );
	}

	public bool AutoplayReturn()	{ return bAutoPlay; }
	public void AutoplayToggle()	{ bAutoPlay = !bAutoPlay; }
	public void BallsMinus ()		{ cBallsRemaining--; }
	public void BallsPlus ()		{ cBallsRemaining++; }
	public int BallsReturn ()		{ return cBallsRemaining; }
	public bool ReturnInPlay ()		{ return bInPlay; }
	public void SetInPlay ()		{ bInPlay = true; }
	public void UnsetInPlay ()		{ bInPlay = false; }

	public void LoadNextLevel () {
		cBricksRemaining = 0;
		if (!bAutoPlay) { bInPlay = false; }

		// depreciated in Unity 5.3... some failed attempts to replace it below:
		Application.LoadLevel(Application.loadedLevel +1);
		//	SceneManager.LoadScene(Scene.buildIndex +1);
		//	Scene thisScene = SceneManager.GetActiveScene();
	}
}

// pickup at lecture 86
