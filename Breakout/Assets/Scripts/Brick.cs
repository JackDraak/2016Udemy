using UnityEngine;
using System.Collections;

public class Brick : MonoBehaviour {

	public AudioClip brick;
	public static int cBricksRemaining = 0;
	public Sprite[] hitSprites;

	private int cTimesHit;
	private LevelManager levelManager;

	void Start () {
		levelManager = GameObject.FindObjectOfType<LevelManager>();
		cTimesHit = 0;
		if (this.tag == "breakable") cBricksRemaining++;
	}

	void OnCollisionEnter2D (Collision2D collision) {
		if (this.tag == "breakable") HandleHits();
	} 

	void HandleHits () {
		int cMaxHits = hitSprites.Length +1;
		cTimesHit++;
		CueAudio();

		if (cTimesHit >= cMaxHits) 
		{
			cBricksRemaining--;
			Destroy(gameObject);
			if (cBricksRemaining <= 0) levelManager.LoadNextLevel(); 
		}
		else { LoadSprite(); }
	}

	void LoadSprite () {
		int spriteIndex = cTimesHit -1;
		this.GetComponent<SpriteRenderer>().sprite = hitSprites[spriteIndex];
	}

	void CueAudio () {
		// GetComponent<AudioSource>().Play();
		AudioSource.PlayClipAtPoint(brick, transform.position);
	}
}
