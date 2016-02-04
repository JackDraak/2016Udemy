using UnityEngine;
using System.Collections;

public class Brick : MonoBehaviour {

	public Sprite[] hitSprites;

	private int cTimesHit;
//	private LevelManager levelManager;

	void Start () {
//		levelManager = GameObject.FindObjectOfType<LevelManager>();
		cTimesHit = 0;
	}

	void OnCollisionEnter2D (Collision2D collision) {
		bool bBreakable = (this.tag == "breakable");
		if (bBreakable) HandleHits();
	} 

	void HandleHits () {
		int cMaxHits = hitSprites.Length +1;
		cTimesHit++;
		if (cTimesHit >= cMaxHits) { Destroy(gameObject); }
		else { LoadSprite(); }
	}

	void LoadSprite () {
		int spriteIndex = cTimesHit -1;
		Debug.Log (this + " " + spriteIndex);
		this.GetComponent<SpriteRenderer>().sprite = hitSprites[spriteIndex];
	}
}
