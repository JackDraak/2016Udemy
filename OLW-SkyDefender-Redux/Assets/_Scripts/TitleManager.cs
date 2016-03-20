using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TitleManager : MonoBehaviour {

	public GameObject creditMessage;
	public GameObject startMessage;
	public Text buttonText;

	private string myTitle;

	void Start () { SetTitle(); }
	void Update () { if (this.isActiveAndEnabled) SetTitle(); }

	public void SetTitle () {
		if (creditMessage.activeInHierarchy && buttonText.text == "Credits") buttonText.text = "Help";
		else if (startMessage.activeInHierarchy &&  buttonText.text == "Help") buttonText.text = "Credits";
	}
}
