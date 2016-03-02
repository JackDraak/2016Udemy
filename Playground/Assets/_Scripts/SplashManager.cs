using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SplashManager : MonoBehaviour {
	public float autoLoadNextLevelDelay = 3;  // adjust/set in inspector!

	void Start () {
		Cursor.visible = false;
		Invoke("LoadNextLevel", autoLoadNextLevelDelay);
	}
	public void LoadNextLevel() {
		Cursor.visible = true;
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex +1);
	}
}
