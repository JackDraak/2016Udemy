using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TitleManager : MonoBehaviour {

	public GameObject creditMessage;
	public GameObject startMessage;
	public Text buttonText;

	void Start () {
		SetTitle();
	}

	public void SetTitle () {
		if (creditMessage.activeInHierarchy) buttonText.text = "Help";
		else if (startMessage.activeInHierarchy) buttonText.text = "Credits";
	}

	void Update () {
		if (this.isActiveAndEnabled) SetTitle();
	}
}
