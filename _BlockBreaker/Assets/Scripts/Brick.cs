using System.Collections;
using UnityEngine;

[RequireComponent (typeof (LevelManager))]
public class Brick : MonoBehaviour {

	public int baseScore = 4;
	public AudioClip brick;
	public Sprite[] hitSprites;
	public static int breakableCount = 0;
	public GameObject dustEffect;
	
//	private ArrayList dustPuffs = new ArrayList();
	private int timesHit;
	private bool isBreakable;
	private LevelManager levelManager;
	private GameObject parent;
	
	void Start () {
		timesHit = 0;
		parent = GameObject.Find ("Effects"); if (!parent) parent = new GameObject ("Effects");
		levelManager = GameObject.FindObjectOfType<LevelManager>();
		if (!levelManager) Debug.LogError (this + ": unable to attach to LevelManager");
		else Debug.Log (this + ": found: " + levelManager);
		isBreakable = (this.tag == "breakable");
		
		//increment static breakable-brick-count for each breakable object of the class created
		if (isBreakable) {
			breakableCount ++;
			levelManager.BrickCountPlus();
		}
	}
	
	void OnCollisionEnter2D (Collision2D col) { 
		if (isBreakable) {
			HandleHits();
			AudioSource.PlayClipAtPoint (brick, transform.position); // optional 3rd float value for volume
		} else {
			AudioSource.PlayClipAtPoint (brick, transform.position, 0.3f); // i.e. quiet unbreakable bricks	
			ScoreHit ();
		}
	}
	
	void HandleHits() {
		timesHit++;
		int maxHits = hitSprites.Length + 1;
		
		// if a brick has taken it's final hit
		if (timesHit >= maxHits) {
			Puff();
			ScoreBrick();
		// not fixing the problem:	levelManager.BrickDestroyed();
//			if (breakableCount == 0) {
//				levelManager.LoadNextLevel();
//			}
			breakableCount --;
			levelManager.BrickCountMinus();
			Destroy(gameObject);
			
		// if a brick can take a hit and stick around for more
		} else {
			Puff();
			ScoreHit ();
			LoadSprites();
		}
	}
	
	void ScoreHit () {
		// small score, multiply by level# & dynamic scoreFactor
		LevelManager.score += Mathf.Round (
								PlayerPrefsManager.GetSpeed() * 
								(LevelManager.scoreFactor *
								(baseScore * (Application.loadedLevel)))
							);
		FreeBallin();
	}
	
	void ScoreBrick () {
		// larger score, multiply by level# & dynamic scoreFactor
		LevelManager.score += Mathf.Round (
								PlayerPrefsManager.GetSpeed() *
								(LevelManager.scoreFactor *
								(10 * baseScore * Application.loadedLevel))
							);
		FreeBallin();
	}
	
	public void FreeBallin () { // set reward levels where free plays are granted
		if (PlayerPrefsManager.GetAward() == 0) {
			if (LevelManager.score > 5000) {
				LevelManager.ballCount++;
				levelManager.ShowMyBalls();
				PlayerPrefsManager.SetAward(1);
			}
		}
		if (PlayerPrefsManager.GetAward() == 1) {
			if (LevelManager.score > 15000) {
				LevelManager.ballCount++;
				levelManager.ShowMyBalls();
				PlayerPrefsManager.SetAward(2);
			}
		}
		if (PlayerPrefsManager.GetAward() == 2) {
			if (LevelManager.score > 50000) {
				LevelManager.ballCount++;
				levelManager.ShowMyBalls();
				PlayerPrefsManager.SetAward(3);
			}
		}
	}
	
	void Puff () {
		if (dustEffect) {
			GameObject dustPuff = Instantiate (dustEffect, this.transform.position, Quaternion.identity) as GameObject;
			dustPuff.GetComponent<ParticleSystem>().startColor = this.GetComponent<SpriteRenderer>().color;
			dustPuff.transform.parent = parent.transform;
			levelManager.EffectAdd (dustPuff);
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