using UnityEngine;
using System.Collections;

public class Paddle : MonoBehaviour {
	float MouseXPosInGameUnits;
	float PaddleXPosInGameUnits;

	void Update () {
		MouseXPosInGameUnits = Input.mousePosition.x / Screen.width *16f -6f;
		PaddleXPosInGameUnits = Mathf.Clamp(MouseXPosInGameUnits, -8.5f, 7.2f);
		Vector3 PaddlePosition = new Vector3 (PaddleXPosInGameUnits, this.transform.position.y, 0f);
		Debug.Log (this + " " + PaddleXPosInGameUnits);
		this.transform.position = PaddlePosition;
	}
}
