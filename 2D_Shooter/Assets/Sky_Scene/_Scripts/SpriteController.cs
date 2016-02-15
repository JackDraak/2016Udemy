using UnityEngine;
using System.Collections;

public class SpriteController : MonoBehaviour {
	public Sprite[] mySprite;

	private int spriteIndex;
	private int frameBuffer = 1;
	private int frameCounter = 0;

	void Start () { spriteIndex = 0; }

	void LoadSprite () {
		if (mySprite[spriteIndex]) { this.GetComponent<SpriteRenderer>().sprite = mySprite[spriteIndex]; } 
		else { Debug.LogError ("SpriteController.cs ERROR: " + this + " spriteIndex " + spriteIndex + " mismatch."); }
	}

	void Update () {
		if (frameCounter++ > frameBuffer) {
			frameCounter = 0;
			if (spriteIndex++ >= mySprite.Length -1) spriteIndex = 0; 
			LoadSprite();
		}
	}
}
