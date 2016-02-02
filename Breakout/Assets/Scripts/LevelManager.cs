using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {
	public void LoadLevel(string LevelName){
	print ("Level Manager : Load Level : " + LevelName );
	// depreciated in 5.3 Application.LoadLevel(LevelName);
	SceneManager.LoadScene(LevelName);
	}
}
