using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SplashManager : MonoBehaviour {

	public float autoLoadNextLevelDelay;
	
	private OptionsController optionsController;
	
	void Start () {
//		Screen.showCursor = false;
//		if (PlayerPrefsManager.GetUsed()) {
			optionsController = GameObject.FindObjectOfType<OptionsController>();
			optionsController.SetDefaults();
			optionsController.Save ();
//		}
		PlayerPrefsManager.SetUsed();
		Invoke("LoadNextLevel", autoLoadNextLevelDelay);
	}
	
	public void LoadNextLevel() {
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex +1);
	}
}