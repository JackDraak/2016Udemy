﻿using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour {
	// adjust/set in inspector!
	public GameObject bomb;
	public GameObject powerUp;
	public AudioClip bombSound;
	public AudioClip damage;
	public AudioClip scuttle;
	public GameObject puffMachine;
	public GameObject puffLocation;

	private bool armed, dearmed;
	private float bombSpeed, chance, fireDelay, fireTime, hitPoints, maxHealth;
	private Color currentColor;
	private LevelManager levelManager;
	private SpriteRenderer myRenderer;
	private int myWave;
	
	public void Disarm () { dearmed = true; }
	public void Rearm () { dearmed = false; }

	void Start () {
		levelManager = GameObject.FindObjectOfType<LevelManager>(); 
			if (!levelManager) Debug.LogError ("LEVEL_MANAGER_FAIL");
		myRenderer = GetComponent<SpriteRenderer>();
			if (!myRenderer) Debug.LogError ("FAIL renderer");

		armed = true;
		bombSpeed = 6f;
		dearmed = false;
		fireDelay = 0.2f;
		fireTime = Time.time;
		maxHealth = 111f;

		myWave = levelManager.GetWaveNumber();

		// difficulty tuning: increase each wave
		fireDelay = 1 / (myWave * 0.3f);
		maxHealth += (myWave * 1.7f);
		hitPoints = maxHealth;
	}

	void Update () {
		// fire control
		if (!dearmed) {
			if (!armed) {
				InvokeRepeating ("DropBomb", fireDelay, fireDelay);
				armed = !armed;
			}
		}

		// fire control reset?
		chance = Random.Range (1, 100);
		if (chance > 49 && chance < 51) armed = !armed;

		// damage haptics -- desire: colour 1, 1, 1, 1 at full health slipping to 1, 0, 0, 1 at death
		Vector4 priorColour = currentColor;
		float colourDelta = maxHealth / hitPoints;
		currentColor = new Vector4 (1, 1/colourDelta, 1/colourDelta, 1f);
		if (!Mathf.Approximately(priorColour.y, currentColor.g))  myRenderer.color = currentColor;
	}
	
	void OnTriggerEnter2D (Collider2D collider) {
		if (collider.tag == "PlayerProjectile") {
			collider.gameObject.SetActive(false);
			TakeDamage();
		}
	}
	
	void DropBomb () {
		float progressiveDelay = fireDelay;
		if (fireTime + progressiveDelay <= Time.time) {
			AudioSource.PlayClipAtPoint (bombSound, transform.position);
			GameObject myBomb = Instantiate(bomb, transform.position,  Quaternion.identity) as GameObject;
			myBomb.GetComponent<Rigidbody2D>().velocity += Vector2.down * bombSpeed;
			fireTime = Time.time + (Random.Range(0.3f, 36f / (myWave * 1.3f)));
		}
	}

	void DropPowerBonus () {
		GameObject bonus = Instantiate(powerUp, transform.position,  Quaternion.identity) as GameObject;
		bonus.GetComponent<Rigidbody2D>().velocity += Vector2.down * bombSpeed * 0.85f;
	}

	void TakeDamage () {
		AudioSource.PlayClipAtPoint (damage, transform.position);
		GameObject trash = Instantiate(puffMachine, puffLocation.transform.position, Quaternion.identity) as GameObject;
		trash.GetComponent<ParticleSystem>().GetComponent<Renderer>().sortingLayerName = "EnemyDamage";
		hitPoints = (hitPoints * 0.93f) - 23f;
		if (hitPoints <= 0f) ScoreAndDestroy();
	}

	void ScoreAndDestroy () {
		// TODO typical time to randomly "drop a bonus"
		AudioSource.PlayClipAtPoint (scuttle, transform.position);
		levelManager = GameObject.FindObjectOfType<LevelManager>(); // why the heck do I need this here to prevent exception faults?
			if (!levelManager) Debug.LogError ("LEVEL_MANAGER_FAIL");
		levelManager.ChangeScore(5 * levelManager.GetWaveNumber());
		levelManager.EnemyDown();
		int chance = Random.Range (0,99);
		if (chance > 20 && chance <= 80) DropPowerBonus(); 
		Destroy(this.gameObject, 0.001f);
	}
}
