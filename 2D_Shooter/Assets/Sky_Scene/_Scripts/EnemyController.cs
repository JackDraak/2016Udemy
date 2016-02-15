using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour {
	public AudioClip bombSound;
	public AudioClip damage;
	public AudioClip scuttle;
	public GameObject bomb;

	private float chance;
	private Color currentColor;// = new Color(0f,0f,0f,1f); // or = Color.black;
	private bool armed;
	private float bombSpeed = 420f;
	private float fireDelay = 1.4f;
	private float fireTime;
	private LevelManager levelManager;
	private float hitPoints;
	private float maxHealth = 222f;
//	private Color offColor = new Color (1f, 0f, 0f, 1f), onColor = new Color (1f, 1f, 1f, 1f);
	private SpriteRenderer myRenderer;

	void Start () {
		levelManager = GameObject.FindObjectOfType<LevelManager>(); if (!levelManager) Debug.LogError ("LEVEL_MANAGER_FAIL");
		myRenderer = 	GetComponent<SpriteRenderer>(); if (!myRenderer) Debug.LogError ("FAIL renderer");
		fireTime = Time.time;
		hitPoints = maxHealth;
		armed = true;
	}
	
	void OnTriggerEnter2D (Collider2D collider) {
		if (collider.tag == "PlayerProjectile") {
			TakeDamage();
			Destroy (collider.gameObject);
		}
	}

	void TakeDamage () {
		// typical time to do a visual effect
		hitPoints = (hitPoints * 0.8f) - 4f;
	
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
		if (!armed) {
			InvokeRepeating ("DropBomb", fireDelay, fireDelay);
			armed = !armed;
		}
		float colourChange = (maxHealth - hitPoints) / maxHealth;
		currentColor = new Vector4 (1/colourChange, 1/colourChange, colourChange, 1f);
		myRenderer.color = currentColor;
		chance = Random.Range (1, 100);
		if (chance > 98) armed = !armed;
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