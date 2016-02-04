using UnityEngine;
using System.Collections;

public class Brick : MonoBehaviour {

	public static int bricksRemaining = 0;
	public Sprite[] hitSprites;

	private int cTimesHit;
	private LevelManager levelManager;

	void Start () {
		levelManager = GameObject.FindObjectOfType<LevelManager>();
		cTimesHit = 0;
		if (this.tag == "breakable") bricksRemaining++;
		Debug.Log (this + " -bricks remaining- " + bricksRemaining);
	}

	void OnCollisionEnter2D (Collision2D collision) {
		if (this.tag == "breakable") HandleHits();
	} 

	void HandleHits () {
		int cMaxHits = hitSprites.Length +1;
		cTimesHit++;
		if (cTimesHit >= cMaxHits) {
			bricksRemaining--;
			Debug.Log (this + " -bricks remaining- " + bricksRemaining);
			Destroy(gameObject);
			if (bricksRemaining <= 0) levelManager.LoadNextLevel(); 
		}
		else { LoadSprite(); }
	}

	void LoadSprite () {
		int spriteIndex = cTimesHit -1;
		Debug.Log (this + " -spriteindex- " + spriteIndex);
		this.GetComponent<SpriteRenderer>().sprite = hitSprites[spriteIndex];
	}
}
