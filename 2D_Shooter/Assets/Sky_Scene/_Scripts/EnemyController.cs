using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour {
	public AudioClip bombSound;
	public AudioClip damage;
	public AudioClip scuttle;
	public GameObject bomb;

	private float bombSpeed = 420f;
	//private GameObject playerGun;
	private float fireDelay = 1.4f;
	private float fireTime;
	private LevelManager levelManager;
	private float hitPoints;

	void Start () {
		levelManager = GameObject.FindObjectOfType<LevelManager>(); if (!levelManager) Debug.LogError ("LEVEL_MANAGER_FAIL");
	//	playerGun = GameObject.FindGameObjectWithTag("PlayerGun"); if (!playerGun) Debug.LogError (this + " cant attach to PlayerGun. ERROR");
		fireTime = Time.time;
		hitPoints = 251f;
	}
	
	void OnTriggerEnter2D (Collider2D collider) {
		if (collider.tag == "PlayerProjectile") {
			TakeDamage();
			Destroy (collider.gameObject);
		}
	}

	void TakeDamage () {
		// typical time to do a visual effect
		hitPoints = (hitPoints * 0.8f) - 5f;
		AudioSource.PlayClipAtPoint (damage, transform.position);
		Debug.Log ("HitPoints: " + hitPoints);
		if (hitPoints <= 0f) ScoreAndDestroy();
	}

	void ScoreAndDestroy () {
		// typical time to randomly "drop a bonus"
		// typical time to do a visual effect
		levelManager.ChangeScore(100f);
		AudioSource.PlayClipAtPoint (scuttle, transform.position);
		Destroy(this.gameObject, 0.001f);
	}

	void Update () {
		InvokeRepeating ("DropBomb", fireDelay, fireDelay);
	}

	void DropBomb () {
		if (fireTime + fireDelay <= Time.time) {
			GameObject discharge = Instantiate(bomb, transform.position, Quaternion.identity) as GameObject;
			discharge.GetComponent<Rigidbody2D>().velocity += Vector2.down * bombSpeed * Time.deltaTime;
			AudioSource.PlayClipAtPoint (bombSound, transform.position);
			fireTime = Time.time + Random.Range(0.0f, 4.0f);
		}
	}

	void InvokeShot () {
		InvokeRepeating ("DropBomb", fireDelay, fireDelay);
	}	
}