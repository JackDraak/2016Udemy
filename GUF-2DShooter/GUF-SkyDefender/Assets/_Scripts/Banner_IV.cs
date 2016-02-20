using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Banner_IV : MonoBehaviour {

	public Text bannerText;
	public LevelController levelController;

	void Update () {
		int i = levelController.GetIV();
		bannerText.text = i.ToString();
	}
}
