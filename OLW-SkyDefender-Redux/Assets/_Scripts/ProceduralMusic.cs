using UnityEngine;
using System.Collections;

public class ProceduralMusic : MonoBehaviour {

	public AudioClip[] audioClip_;	// the array of audioClip chords; set this up in the inspector with the following additional details:
	public float vol = 0.5f;		// volume for music playback, you will probably want to tie this into your PlayerPrefs
	public float clipLength = 1;	// each audioclip in the chord array should be this length in seconds for seemless playback
	public bool stepping = false;	// if false, each clip is randomly selected. if true, apply a small degree of order defined below:
	public int stepDistance = 1;	// while stepping, the # of clips to step, should be smaller than setLength (# of chords in the array)
	public int stepRange = 1;		// while stepping, the range of clips at the step-point to select from should be much smaller than setLength (# of chords in the array)

	private bool playing = false;
	private int clip, myClip, setLength;

	void Start () {
		setLength = audioClip_.Length;
	//		Debug.Log (setLength);
	}

	public void Begin() {
		// if not playing music, then immediatly start and flag as playing
		if (!playing) {
			PlayChord();
			playing = true;
		}
	}

	public void End() {
		if (playing) {
			CancelInvoke();
			CancelInvoke();
			playing = false;
		}
	}

	void Update () {
	}

	// invokations to keep the music going indefinately
	void PlayMusic () { Invoke ("PlayChord", clipLength); }
	void MusicStep () { Invoke ("StepMusic", clipLength); }

	// sans 'stepping' mode, this will repeat every clipLength
	void PlayChord () {
		clip = Random.Range(0, setLength -1);
		if (clip == myClip) {
	//			Debug.Log ("duplicate sequence prevention");
			if (clip > setLength /2) clip--;
			else if (clip < setLength /2) clip ++;
		}
		myClip = clip;
	//		Debug.Log ("step one: " + clip);
		AudioSource.PlayClipAtPoint (audioClip_[clip], transform.position, vol);
		if (!stepping) PlayMusic();
		else MusicStep();
	}

	// if 'stepping', select a specific or range of clips to step into for clipLength
	void StepMusic () {
		int step = Random.Range (stepDistance, stepDistance + stepRange);
		clip = clip +step;
		if (clip >= setLength -1) clip = clip - (step + 1);
	//		Debug.Log ("step two: " + step + " clip: " + clip);
		AudioSource.PlayClipAtPoint (audioClip_[clip], transform.position, vol);
		PlayMusic();
	}
}
