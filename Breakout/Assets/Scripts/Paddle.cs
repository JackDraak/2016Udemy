using UnityEngine;
using System.Collections;

public class Paddle : MonoBehaviour {
	float MouseXPos;
	float PaddleXPos;

	void Update () {
		MouseXPos = (Input.mousePosition.x / Screen.width * 16); // in game units, 16 grid units wide
		PaddleXPos = Mathf.Clamp (MouseXPos, 0.8f, 15.2f);
		Vector3 PaddlePosition = new Vector3 (PaddleXPos -8, this.transform.position.y, 0f);
		//Debug.Log (this + " " + MouseXPos);
		this.transform.position = PaddlePosition;
	}
}
