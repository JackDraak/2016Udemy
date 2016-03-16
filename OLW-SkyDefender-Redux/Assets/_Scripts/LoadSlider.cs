using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LoadSlider : MonoBehaviour {

	public Slider slider;
	public float loadTimeInSeconds = 20;

	private float timeAwake;

	void Awake () { timeAwake = 0; }

	void Update () {
		if (timeAwake == 0) timeAwake = Time.time;
		slider.value = (Time.time - timeAwake) / loadTimeInSeconds;
	}
}
