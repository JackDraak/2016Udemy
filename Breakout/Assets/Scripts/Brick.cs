using UnityEngine;
using System.Collections;

public class Brick : MonoBehaviour {

	public AudioClip brick;
	public Sprite[] hitSprites;
	public GameObject brickEffect;

	private int cTimesHit;
	private LevelManager levelManager;

	void Start () {
		cTimesHit = 0;
		levelManager = GameObject.FindObjectOfType<LevelManager>();
		if (this.tag == "breakable") levelManager.BricksPlus();
	}

	void OnCollisionEnter2D (Collision2D collision) {
		if (this.tag == "breakable") HandleHits();
	} 

	// cue audio & swap sprite or destroy
	void HandleHits () {
		CueAudio();

		int cMaxHits = hitSprites.Length +1;
		cTimesHit++;
		if (cTimesHit >= cMaxHits) 
		{
			levelManager.BricksMinus();
			DoBrickEffect();
			Destroy(gameObject);
		}
		else { LoadSprite(); DoBrickEffect(); }
	}

	void LoadSprite () {
		int spriteIndex = cTimesHit -1;
		if (hitSprites[spriteIndex]) {
			this.GetComponent<SpriteRenderer>().sprite = hitSprites[spriteIndex];
		} else {
			Debug.LogError ("Brick.cs ERROR: " + this + " spriteIndex " + spriteIndex + " mismatch.");
		}
	}

	// TODO getting strange errors on the console after adding this
	void DoBrickEffect () {
		GameObject effect = Instantiate(brickEffect, this.transform.position, Quaternion.identity) as GameObject;
		effect.GetComponent<ParticleSystem>().startColor = this.GetComponent<SpriteRenderer>().color;
		Destroy(effect, 1.2f); // partially working.. need to add them to array and recycle them? looking into object-pooling
	}

	void CueAudio () {
		AudioSource.PlayClipAtPoint(brick, transform.position);
	}
}
