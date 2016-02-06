using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {
	public static bool bLaunched = false;
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
	public bool LaunchedReturn()	{ return bLaunched; }
	public void LaunchedSet()		{ bLaunched = true; }
	public void LaunchedUnset()		{ bLaunched = false; }
	public void LaunchedToggle()	{ bLaunched = !bLaunched; }

	public void LoadNextLevel () {
		cBricksRemaining = 0;
		if (!bAutoPlay) { bLaunched = false; } // TODO something buggin trying to implement full autoplay (from level to level)
	//	bLaunched = false;

		// depreciated in Unity 5.3... some failed attempts to replace it below:
		Application.LoadLevel(Application.loadedLevel +1);
		//	SceneManager.LoadScene(Scene.buildIndex +1);
		//	Scene thisScene = SceneManager.GetActiveScene();
	}
}

// pickup at lecture 86
