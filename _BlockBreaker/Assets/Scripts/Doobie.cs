using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Brick))]
public class Doobie : MonoBehaviour {

	public Sprite[] hitSprites;
	public int baseScore = 42;
	public AudioClip brick;
	public GameObject smokeEffect;
	public GameObject cherryEffect;
	
	private LevelManager levelManager;
	private GameObject parent;
	private GameObject smoke;
	private int timesHit;
	
	void Start () {
		timesHit = 0;
		smoke = GameObject.Find("DoobieSmoke");
		levelManager = GameObject.FindObjectOfType<LevelManager>();
		levelManager.BrickCountPlus();
		parent = GameObject.Find ("Effects"); if (!parent) parent = new GameObject ("Effects");
	}
	
	void OnCollisionEnter2D (Collision2D col) { 
		HandleHits();
		AudioSource.PlayClipAtPoint (brick, transform.position); // optional 3rd float value for volume
	}
	
	void HandleHits() {
		timesHit++;
		int maxHits = hitSprites.Length + 1;
		
		// when a brick has been hit for the last time
		if (timesHit >= maxHits) {
			ScoreDoobie ();
			Puff();
			levelManager.BrickCountMinus();
			Destroy(gameObject);

		// if a brick can take a hit and stick around for more
		} else {
			ScoreHit ();
			Puff();
			LoadSprites();
		}
	}

	void ScoreHit () {
		// small score for small hit
		LevelManager.score += Mathf.Round (
								PlayerPrefsManager.GetSpeed() *
								(LevelManager.scoreFactor *
								(baseScore * Application.loadedLevel) + 
								Brick.breakableCount * 5)
							);
//		aBrick.FreeBallin ();
	}
	
	void ScoreDoobie () {
		// larger score for finishing hit
		LevelManager.score += Mathf.Round (
								PlayerPrefsManager.GetSpeed() *
								(LevelManager.scoreFactor *
								(10 * baseScore * Application.loadedLevel) + 
								Brick.breakableCount * 5)
							);
//		aBrick.FreeBallin ();
	}
	
	void Puff () {
		if (smoke) {
			GameObject smokePuff = Instantiate(smokeEffect, smoke.transform.position, Quaternion.identity) as GameObject;
			smokePuff.transform.parent = parent.transform;
			levelManager.EffectAdd (smokePuff);
			Instantiate(cherryEffect, smoke.transform.position, Quaternion.identity);
		}
	}
	
	void LoadSprites () {
		int spriteIndex = (timesHit -1);
		if (hitSprites[spriteIndex]) {
			this.GetComponent<SpriteRenderer>().sprite = hitSprites[spriteIndex];
		} else {
			Debug.LogError (this + " missing sprite!");
		}
	}
}