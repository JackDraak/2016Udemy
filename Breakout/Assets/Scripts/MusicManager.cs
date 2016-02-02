using UnityEngine;
using System.Collections;

public class MusicManager : MonoBehaviour {
	static MusicManager instance = null;
	
	void Awake () {
		Debug.Log ("This instance: Awake : MusicManager : " + this.GetInstanceID());
		if (instance != null) {
			Destroy (gameObject);
			Debug.Log ("destroy duplicate MusicManager -executed-");
		} else {
			instance = this;
			GameObject.DontDestroyOnLoad(gameObject);
		}
	}
	
	void Start () {
		Debug.Log ("This instance : Start : MusicManager : " + this.GetInstanceID());
	}
}

// pickup at lecture 71
