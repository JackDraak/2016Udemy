using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SplashManager : MonoBehaviour {
	public float autoLoadNextLevelDelay = 2;
	private OptionsController optionsController;
	
	void Start () {
		Cursor.visible = false; // save for release... PITA while testing
		optionsController = GameObject.FindObjectOfType<OptionsController>();
		if (!optionsController) Debug.LogError (this + ": unable to attach to OptionsController");
		optionsController.SetDefaults();
		optionsController.Save ();
		PlayerPrefsManager.SetUsed(); // relic? was working on resetting state between session if I recall
		Invoke("LoadNextLevel", autoLoadNextLevelDelay);
	}
	
	public void LoadNextLevel() {
		Cursor.visible = true;
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex +1);
	}
}