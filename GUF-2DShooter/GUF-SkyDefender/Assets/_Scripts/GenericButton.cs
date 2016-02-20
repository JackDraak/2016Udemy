using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GenericButton : MonoBehaviour {

	public Text titleText;
	public string action;


	// Use this for initialization
	void Start () {
	
	}


	void OnMouseDown () {
		Debug.Log ("mouse down " + Time.time + " " + action);
	}

	// Update is called once per frame
	void Update () {
		
	}
}
