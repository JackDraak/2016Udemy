using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;


[RequireComponent (typeof (LevelManager))]
public class Brick : MonoBehaviour {

	public int baseScore = 4;
	public AudioClip brick;
	public Sprite[] hitSprites;
	public GameObject dustEffect;

	private int timesHit;
	private bool isBreakable;
	private LevelManager levelManager;
	private GameObject parent;
	
	void Start () {
		timesHit = 0;
		parent = GameObject.Find ("Effects");
		if (!parent) parent = new GameObject ("Effects");
		levelManager = GameObject.FindObjectOfType<LevelManager>();
		if (!levelManager) Debug.LogError (this + ": unable to attach to LevelManager");
		isBreakable = (this.tag == "breakable");
		if (isBreakable) {
			levelManager.BrickCountPlus();
		}
	}
	
	void OnCollisionEnter2D (Collision2D col) { 
		levelManager.CalculateScoreFactor();
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
			levelManager.BrickCountMinus();
			ScoreBrick();
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
								LevelManager.scoreFactor *
								0.1f * baseScore * levelManager.GetSceneIndex() + 
								levelManager.BrickGetNumRemaining() * 5
							);

		levelManager.FreeBallin();
	}
	
	void ScoreBrick () {
		// larger score, multiply by level# & dynamic scoreFactor
		LevelManager.score += Mathf.Round (
								PlayerPrefsManager.GetSpeed() *
								LevelManager.scoreFactor *
								0.5f * baseScore * levelManager.GetSceneIndex() + 
								levelManager.BrickGetNumRemaining() * 5
							);
		levelManager.FreeBallin();
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