using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SplashManager : MonoBehaviour {
	public float autoLoadNextLevelDelay = 2;
	private OptionsController optionsController;
	
	void Start () {
//		Screen.showCursor = false;
		optionsController = GameObject.FindObjectOfType<OptionsController>();
		if (!optionsController) Debug.LogError (this + ": unable to attach to OptionsController");
		optionsController.SetDefaults();
		optionsController.Save ();
		PlayerPrefsManager.SetUsed();
		Invoke("LoadNextLevel", autoLoadNextLevelDelay);
	}
	
	public void LoadNextLevel() {
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex +1);
	}
}