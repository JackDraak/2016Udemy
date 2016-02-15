using UnityEngine;
using System.Collections;
public class Scroll : MonoBehaviour {
	public float scrollSpeed = 0.005f;
	void Update () {
		Vector2 offset = new Vector2 (Time.time * scrollSpeed, 0);
		GetComponent<Renderer>().material.mainTextureOffset = offset;
	}
}
