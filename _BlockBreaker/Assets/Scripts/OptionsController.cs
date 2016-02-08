﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class OptionsController : MonoBehaviour {
	
	public Slider fireballsSlider, trailsSlider, autoplaySlider, speedSlider, easySlider;
	private LevelManager levelManager;
	
	void Start () {
		levelManager = GameObject.FindObjectOfType<LevelManager>();
		if (!levelManager) Debug.LogWarning (this + ": unable to attach to LevelManager");
		speedSlider.value = PlayerPrefsManager.GetSpeed ();
		if (PlayerPrefsManager.GetFireBalls () == true) fireballsSlider.value = 1; else fireballsSlider.value = 0;
		if (PlayerPrefsManager.GetTrails () == true) trailsSlider.value = 1; else trailsSlider.value =0;
		if (PlayerPrefsManager.GetAutoplay () == true) autoplaySlider.value = 1; else autoplaySlider.value =0;
		if (PlayerPrefsManager.GetEasy () == true) easySlider.value = 1; else easySlider.value =0;
	}

	public void SaveAndExit () {
		Save ();
		levelManager.LoadLevel ("_Start Menu");
	}
	
	public void Save () {
		PlayerPrefsManager.SetSpeed (speedSlider.value);
		if (fireballsSlider.value == 1) PlayerPrefsManager.SetFireBalls(true); else PlayerPrefsManager.SetFireBalls(false);
		if (trailsSlider.value == 1) PlayerPrefsManager.SetTrails(true); else PlayerPrefsManager.SetTrails(false);
		if (autoplaySlider.value == 1) PlayerPrefsManager.SetAutoplay(true); else PlayerPrefsManager.SetAutoplay(false);
		if (easySlider.value == 1) PlayerPrefsManager.SetEasy(true); else PlayerPrefsManager.SetEasy(false);
	}
	
	public void SetDefaults () {
		fireballsSlider.value = 0;
		trailsSlider.value = 1;
		autoplaySlider.value = 0;
		speedSlider.value = 0.7f;
		easySlider.value = 0;
	}
}
