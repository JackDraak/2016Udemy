using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {

	public void LoadLevel(string LevelName){
	// depreciated in 5.3: Application.LoadLevel(LevelName);
		SceneManager.LoadScene(LevelName); // req.: using UnityEngine.SceneManagement;
		Debug.Log("LOG : Level Manager : Load Level : " + LevelName );
	}

	public void LoadNextLevel () {
	//	Scene thisScene = SceneManager.GetActiveScene();
	//	SceneManager.LoadScene(Scene.buildIndex +1);
		Application.LoadLevel(Application.loadedLevel +1);
	}
}

// pickup at lecture 82
