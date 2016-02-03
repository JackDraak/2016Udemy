using UnityEngine;
using System.Collections;

public class Brick : MonoBehaviour {

	public int cMaxHits;
	private int cTimesHit;
	private LevelManager levelManager;
//	private Brick brick;

	void Start () {
		levelManager = GameObject.FindObjectOfType<LevelManager>();
//		brick = GameObject.FindObjectOfType<Brick>();
		cTimesHit = 0;
	}

	void OnCollisionEnter2D (Collision2D collision) {
		cTimesHit++;
		if (cTimesHit >= cMaxHits) Destroy(gameObject);
	} 

	void Update () {
		// if no bricks: levelManager.LoadNextLevel();
	//	if (!brick) Debug.Log ("bricks gone?");
	}

	void SimulateWin () {
		levelManager.LoadNextLevel();
	}
}
