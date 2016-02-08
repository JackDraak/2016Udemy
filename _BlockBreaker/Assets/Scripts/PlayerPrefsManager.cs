﻿using UnityEngine;
using System.Collections;

public class PlayerPrefsManager : MonoBehaviour {
	
	const string MASTER_VOLUME_KEY = "master_volume";
	const string FIREBALLS_KEY = "fireballs";
	const string TRAILS_KEY = "trails";
	const string AUTOPLAY_KEY = "autoplay";
	const string SPEED_KEY = "speed";
	const string TOPSCORE_KEY = "topscore";
	const string FIRSTUSE_KEY = "firstUse";
	const string EASYMODE_KEY = "easymode";
	const string AWARD_KEY = "award";
	
	public static void SetMasterVolume (float volume) {
		if (volume >= 0f && volume <= 1f) {
			PlayerPrefs.SetFloat (MASTER_VOLUME_KEY, volume);
		} else {
			Debug.LogError ("Master volume out of range");
		}
	}
	public static float GetMasterVolume () {
		return PlayerPrefs.GetFloat (MASTER_VOLUME_KEY);
	}

	public static void SetSpeed (float yorn) {
		PlayerPrefs.SetFloat (SPEED_KEY, yorn);
	}
	public static float GetSpeed () {
		return PlayerPrefs.GetFloat (SPEED_KEY);
	}
	
	public static void SetAward (float yorn) {
		PlayerPrefs.SetFloat (AWARD_KEY, yorn);
	}
	public static float GetAward () {
		return PlayerPrefs.GetFloat (AWARD_KEY);
	}
	
	public static void SetTopscore (float yorn) {
		PlayerPrefs.SetFloat (TOPSCORE_KEY, yorn);
	}
	public static float GetTopscore () {
		return PlayerPrefs.GetFloat (TOPSCORE_KEY);
	}

	public static void SetFireBalls (bool set) {
		if (set) PlayerPrefs.SetInt (FIREBALLS_KEY, 1);
		else  PlayerPrefs.SetInt (FIREBALLS_KEY, 0);
	}
	public static bool GetFireBalls () {
		if (PlayerPrefs.GetInt (FIREBALLS_KEY) == 1) return true;
		else return false;
	}

	public static void SetTrails (bool set) {
		if (set) PlayerPrefs.SetInt (TRAILS_KEY, 1);
		else PlayerPrefs.SetInt (TRAILS_KEY, 0);
	}
	public static bool GetTrails () {
		if (PlayerPrefs.GetInt (TRAILS_KEY) == 1) return true;
		else return false;
	}
	
	public static void SetEasy (bool set) {
		if (set) PlayerPrefs.SetInt (EASYMODE_KEY, 1);
		else PlayerPrefs.SetInt (EASYMODE_KEY, 0);
	}
	public static bool GetEasy () {
		if (PlayerPrefs.GetInt (EASYMODE_KEY) == 1) return true;
		else return false; 
	}
	
	public static void SetAutoplay (bool set) {
		if (set) PlayerPrefs.SetInt (AUTOPLAY_KEY, 1);
		else PlayerPrefs.SetInt (AUTOPLAY_KEY, 0);
	}
	public static bool GetAutoplay () {
		if (PlayerPrefs.GetInt (AUTOPLAY_KEY) == 1) return true;
		else return false;
	}
	
	public static void SetUsed () {
		PlayerPrefs.SetInt (FIRSTUSE_KEY, 1);
	}
	public static bool GetUsed () {
		if (PlayerPrefs.GetInt (FIRSTUSE_KEY) == 1) return true;
		else return false;
	}
}