using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {

	public static bool bAutoPlay = false;
	public static bool bLaunched = false;
	public static int cBallsRemaining = 3;
	public static int cBricksRemaining = 0;

	public bool AutoplayReturn()	{ return bAutoPlay; }			// users: Ball, Paddle
	public void AutoplayToggle()	{ bAutoPlay = !bAutoPlay; }		// users: Paddle
	public void AutoplayUnset()		{ bAutoPlay = false; }			// users: LevelManager.inspector
	public void BallsMinus ()		{ cBallsRemaining--; }			// users: Boundary
	public int BallsReturn ()		{ return cBallsRemaining; }		// users: Boundary
	public void BricksPlus ()		{ cBricksRemaining++; }			// users: Brick
	public bool LaunchedReturn()	{ return bLaunched; }			// users: Ball
	public void LaunchedSet()		{ bLaunched = true; }			// users: Ball
	public void LaunchedUnset()		{ bLaunched = false; }			// users: Boundary
	
	public void BricksMinus () { 
		cBricksRemaining--;
		if (cBricksRemaining <= 0) {
			LoadNextLevel();
		}
	}

	public void LoadLevel(string LevelName){
		SceneManager.LoadScene(LevelName);
	}

	public void LoadNextLevel () {
		cBricksRemaining = 0;
		if (!bAutoPlay) { bLaunched = false; }

		// TODO depreciated in Unity 5.3
		Application.LoadLevel(Application.loadedLevel +1);
	}
}
// pickup at lecture 86
