using UnityEngine;
using System.Collections;

public class Brick : MonoBehaviour {

	public AudioClip brick;
	public Sprite[] hitSprites;

	private int cTimesHit;
	private LevelManager levelManager;

	void Start () {
		cTimesHit = 0;
		levelManager = GameObject.FindObjectOfType<LevelManager>();
		if (this.tag == "breakable") levelManager.cBricksRemaining++;
	}

	void OnCollisionEnter2D (Collision2D collision) {
		if (this.tag == "breakable") HandleHits();
	} 

	void HandleHits () {
		CueAudio();

		int cMaxHits = hitSprites.Length +1;
		cTimesHit++;
		if (cTimesHit >= cMaxHits) 
		{
			levelManager.cBricksRemaining--;
			Destroy(gameObject);
		}
		else { LoadSprite(); }
	}

	void LoadSprite () {
		int spriteIndex = cTimesHit -1;
		this.GetComponent<SpriteRenderer>().sprite = hitSprites[spriteIndex];
	}

	void CueAudio () {
		AudioSource.PlayClipAtPoint(brick, transform.position);
	}
}
