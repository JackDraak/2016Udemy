using UnityEngine;
using System.Collections;

public class SpriteController : MonoBehaviour {
	// adjust/set in inspector!
	public Sprite[] mySprite;
	public int frameBuffer = 1;
	public bool randomSpeed;

	private int spriteIndex;
	private int frameCounter = 0;
	private SpriteRenderer myRenderer;

	void Start () {
		spriteIndex = 0;
		myRenderer = this.GetComponent<SpriteRenderer>();
		if (randomSpeed) frameBuffer = Random.Range(1,8);
	}

	void LoadSprite () {
		if (mySprite[spriteIndex]) { myRenderer.sprite = mySprite[spriteIndex]; } 
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
