using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {

	public static bool bAutoPlay = false;
	public static bool bLaunched = false;
	public static int cBallsRemaining = 3;
	public static int cBricksRemaining = 0;

	public bool AutoplayReturn()	{ return bAutoPlay; }
	public void AutoplayToggle()	{ bAutoPlay = !bAutoPlay; }
	public void AutoplayUnset()		{ bAutoPlay = false; }
	public void BallsMinus ()		{ cBallsRemaining--; }
	public int BallsReturn ()		{ return cBallsRemaining; }
	public void BricksPlus ()		{ cBricksRemaining++; }
	public bool LaunchedReturn()	{ return bLaunched; }
	public void LaunchedSet()		{ bLaunched = true; }
	public void LaunchedUnset()		{ bLaunched = false; }
	
	void Update () {
		if (Input.GetKeyDown(KeyCode.A)) bAutoPlay = !bAutoPlay;
	}
	
	public void BricksMinus () { 
		cBricksRemaining--;
		if (cBricksRemaining <= 0) { LoadNextLevel(); }
	}

	public void LoadLevel(string LevelName){
		SceneManager.LoadScene(LevelName);
	}

	public void LoadNextLevel () {
		cBricksRemaining = 0;
		if (!bAutoPlay) { bLaunched = false; }

		// TODO depreciated in Unity 5.3, find alternative
		Application.LoadLevel(Application.loadedLevel +1);
	}

	public void CreateNewGame () {
		cBallsRemaining = 3;
		LoadNextLevel();
	}
}
