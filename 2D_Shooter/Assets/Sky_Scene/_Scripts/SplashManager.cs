using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SplashManager : MonoBehaviour {

	public float autoLoadNextLevelDelay = 2;

	//private OptionsController optionsController;
	
	void Start () {
	//	Cursor.visible = false;
	//	optionsController = GameObject.FindObjectOfType<OptionsController>(); if (!optionsController) Debug.LogError (this + ":ERROR: unable to attach to OptionsController to reset options to defaults");
	//	optionsController.SetDefaults();
	//	optionsController.Save ();
		Invoke("LoadNextLevel", autoLoadNextLevelDelay);
	}
	
	public void LoadNextLevel() {
		Cursor.visible = true;
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex +1);
	}
}
