using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class SplashManager : MonoBehaviour {
	public float autoLoadNextLevelDelay = 3;  // adjust/set in inspector!
	public Slider slider;
//	public float GetDelay () { return Time.time - (startTime + autoLoadNextLevelDelay); }

	private float delta, startTime;

	void Start () {
		startTime = Time.time;
		slider.value = Time.time - startTime;
		Cursor.visible = false;
		Invoke("LoadNextLevel", autoLoadNextLevelDelay);
	}

	void Update () {
		if (delta < autoLoadNextLevelDelay) {
			delta = Time.time - startTime;
			slider.value = delta;
		}
	}

	public void LoadNextLevel() {
		Cursor.visible = true;
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex +1);
	}
}
