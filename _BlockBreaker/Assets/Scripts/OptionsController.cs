using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class OptionsController : MonoBehaviour {
	
	public Slider fireballsSlider, trailsSlider, autoplaySlider, speedSlider, easySlider;
	private LevelManager levelManager;
	
//	private MusicManager musicManager;
	
	void Start () {
		levelManager = GameObject.FindObjectOfType<LevelManager>();
		fireballsSlider.value = PlayerPrefsManager.GetFireBalls ();
		trailsSlider.value = PlayerPrefsManager.GetTrails ();
		autoplaySlider.value = PlayerPrefsManager.GetAutoplay ();
		speedSlider.value = PlayerPrefsManager.GetSpeed ();
		easySlider.value = PlayerPrefsManager.GetEasy ();
	}
	
//	void Update () {
//		musicManager.SetVolume (volumeSlider.value);
//	}
	
	public void SaveAndExit () {
		Save ();
		levelManager.LoadLevel ("_Start Menu");
	}
	
	public void Save () {
//		PlayerPrefsManager.SetMasterVolume (volumeSlider.value);
		PlayerPrefsManager.SetFireBalls (fireballsSlider.value);
		PlayerPrefsManager.SetTrails (trailsSlider.value);
		PlayerPrefsManager.SetAutoplay (autoplaySlider.value);
		PlayerPrefsManager.SetSpeed (speedSlider.value);
		PlayerPrefsManager.SetEasy (easySlider.value);
	}
	
	public void SetDefaults () {
//		volumeSlider.value = 0.8f;
		fireballsSlider.value = 0;
		trailsSlider.value = 1;
		autoplaySlider.value = 0;
		speedSlider.value = 0.7f;
		easySlider.value = 0;
	}
}
