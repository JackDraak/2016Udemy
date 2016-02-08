using UnityEngine;
using System.Collections;

public class MusicPlayer : MonoBehaviour {

	static MusicPlayer instance = null;
	
	public AudioClip[] level_;
	
	private AudioSource music;

	void Start () {
		if (instance != null && instance != this) {
			Destroy (gameObject);

		} else {
			instance = this;
			GameObject.DontDestroyOnLoad(gameObject);
			if (GameObject.Find ("MusicPlayer")) {
				music = GameObject.Find ("MusicPlayer").GetComponent<AudioSource>();
			}
			music.loop = true;
		}
	}
	
	void OnLevelWasLoaded(int level){
		if (music) {
			music.Stop();
			music.clip = level_[level];
//			Debug.Log (music + " | " + music.clip + " | " + level);
//			if (level == 0) music.clip = null; // splash
//			if (level == 1) music.clip = startClip;
//			if (level == 2) music.clip = level1Clip;
//			if (level == 3) music.clip = level2Clip;
//			if (level == 4) music.clip = level3Clip;
//			if (level == 5) music.clip = level4Clip;
//			if (level == 6) music.clip = level5Clip;
//			if (level == 7) music.clip = winClip;
//			if (level == 8) music.clip = loseClip;
//			if (level == 9) music.clip = creditsClip;
//			if (level == 10) music.clip = optionsClip;
			music.loop = true;
			music.volume = 0.15f;
			music.Play();
		}
	}
}