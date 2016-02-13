using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

[RequireComponent (typeof (Brick))]
public class Doobie : MonoBehaviour {

	public int baseScore = 710;
	public AudioClip brick;
	public GameObject cherryEffect;
	public Sprite[] hitSprites;
	public GameObject smokeEffect;
	
	private LevelManager levelManager;
	private GameObject parent;
	private GameObject smoke;
	private int timesHit;
	
	void HandleHits() {
		timesHit++;
		int maxHits = hitSprites.Length + 1;
		
		// when a brick has been hit for the last time
		if (timesHit >= maxHits) {
			ScoreDoobie ();
			Puff();
			levelManager.BrickCountMinus();
			Destroy(gameObject, 0.05f);

		// if a brick can take a hit and stick around for more
		} else {
			ScoreHit ();
			Puff();
			LoadSprites();
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

	void OnCollisionEnter2D (Collision2D col) { 
		levelManager.CalculateScoreFactor();
		HandleHits();
		AudioSource.PlayClipAtPoint (brick, transform.position); // optional 3rd float value for volume
	}
	
	void Puff () {
		if (smoke) {
			GameObject smokePuff = Instantiate(smokeEffect, smoke.transform.position, Quaternion.identity) as GameObject;
			smokePuff.transform.parent = parent.transform;
			levelManager.EffectAdd (smokePuff);
			GameObject cherryPuff = Instantiate(cherryEffect, smoke.transform.position, Quaternion.identity) as GameObject;
			cherryPuff.transform.parent = parent.transform;
			levelManager.EffectAdd (cherryPuff);
		}
	}

	void ScoreHit () {
		// small score for small hit
		LevelManager.score += Mathf.Round (
								PlayerPrefsManager.GetSpeed() *
								LevelManager.scoreFactor *
								1.5f * baseScore * levelManager.GetSceneIndex() + 
								levelManager.BrickGetNumRemaining() * 10
							);
		levelManager.FreeBallin();
	}
	
	void ScoreDoobie () {
		// larger score for finishing hit
		LevelManager.score += Mathf.Round (
								PlayerPrefsManager.GetSpeed() *
								LevelManager.scoreFactor *
								15f * baseScore * levelManager.GetSceneIndex() + 
								levelManager.BrickGetNumRemaining() * 10
							);
		levelManager.FreeBallin();
	}
	
	void Start () {
		timesHit = 0;
		smoke = GameObject.Find("DoobieSmoke");
		levelManager = GameObject.FindObjectOfType<LevelManager>(); if (!levelManager) Debug.LogError (this + ": unable to attach to LevelManager");
		levelManager.BrickCountPlus();
		parent = GameObject.Find ("Effects"); if (!parent) parent = new GameObject ("Effects");
	}
}
