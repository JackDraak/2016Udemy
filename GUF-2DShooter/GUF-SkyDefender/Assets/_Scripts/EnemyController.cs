﻿using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour {
	// adjust/set in inspector!
	public GameObject bomb;
	public AudioClip bombSound;
	public AudioClip damage;
	public AudioClip scuttle;
	public GameObject puffMachine;

	private bool armed, dearmed;
	private float bombSpeed, chance, fireDelay, fireTime, hitPoints, maxHealth;
	private Color currentColor;
	private GameManager gameManager;
	private SpriteRenderer myRenderer;
	private Vector3 myScale;
	
	public void Disarm () { dearmed = true; }
	public void Rearm () { dearmed = false; }

	void Start () {
		gameManager = GameObject.FindObjectOfType<GameManager>(); 
		if (!gameManager) Debug.LogError ("GAME_MANAGER_FAIL");
		myRenderer = GetComponent<SpriteRenderer>();
			if (!myRenderer) Debug.LogError ("FAIL renderer");

		armed = true;
		bombSpeed = 6f;
		dearmed = false;
		fireDelay = 1.4f;
		fireTime = Time.time;
		maxHealth = 111f;

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
			TakeDamage();
			if (collider.gameObject) Destroy (collider.gameObject, 0.01f);
		}
	}
	
	void DropBomb () {
		if (fireTime + fireDelay <= Time.time) {
			GameObject discharge = Instantiate(bomb, transform.position, Quaternion.identity) as GameObject;
			discharge.GetComponent<Rigidbody2D>().velocity += Vector2.down * bombSpeed;
			AudioSource.PlayClipAtPoint (bombSound, transform.position);
			fireTime = Time.time + Random.Range(0.0f, 4.0f);
		}
	}
	
	void TakeDamage () {
		hitPoints = (hitPoints * 0.93f) - 23f;
		AudioSource.PlayClipAtPoint (damage, transform.position);
		GameObject trash = Instantiate(puffMachine, transform.position, Quaternion.identity) as GameObject;
		Destroy (trash, 2);
		if (hitPoints <= 0f) ScoreAndDestroy();
	}

	void ScoreAndDestroy () {
		// TODO typical time to randomly "drop a bonus"
		gameManager = GameObject.FindObjectOfType<GameManager>(); // why the heck do I need this here to prevent exception faults?
			if (!gameManager) Debug.LogError ("LEVEL_MANAGER_FAIL");
		gameManager.ChangeScore(10f); // TODO multiply by wace#, ergo move wave# into gameManager
		AudioSource.PlayClipAtPoint (scuttle, transform.position);
		gameManager.EnemyDown();
		Destroy(this.gameObject, 0.001f);
	}
}
