using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour {
	public GameObject bomb;
	public AudioClip bombSound;
	public AudioClip damage;
	public AudioClip scuttle;

	private bool armed;
	private float bombSpeed = 360f;
	private float chance;
	private Color currentColor;
	private bool dearmed;
	private float fireDelay = 1.4f;
	private float fireTime;
	private LevelManager levelManager;
	private float hitPoints;
	private float maxHealth = 111f; // TODO tweak this
	private SpriteRenderer myRenderer;

	void Start () {
		levelManager = GameObject.FindObjectOfType<LevelManager>(); if (!levelManager) Debug.LogError ("LEVEL_MANAGER_FAIL");
		myRenderer = 	GetComponent<SpriteRenderer>(); if (!myRenderer) Debug.LogError ("FAIL renderer");
		armed = true;
		dearmed = false;
		fireTime = Time.time;
		hitPoints = maxHealth;
	}

	public void Disarm () { dearmed = true; }
	public void Rearm () { dearmed = false; }

	
	void OnTriggerEnter2D (Collider2D collider) {
		if (collider.tag == "PlayerProjectile") {
			TakeDamage();
			Destroy (collider.gameObject);
		}
	}

	void TakeDamage () {
		// TODO typical time to do a visual effect
		hitPoints = (hitPoints * 0.65f) - 4f;
		AudioSource.PlayClipAtPoint (damage, transform.position);
		if (hitPoints <= 0f) ScoreAndDestroy();
	}

	void ScoreAndDestroy () {
		// TODO typical time to randomly "drop a bonus"
		// TODO typical time to do a visual effect
		levelManager.ChangeScore(100f);
		AudioSource.PlayClipAtPoint (scuttle, transform.position);
		levelManager.EnemyDown();
		Destroy(this.gameObject, 0.001f);
	}

	void Update () {
		if (!dearmed) {
			if (!armed) {
				InvokeRepeating ("DropBomb", fireDelay, fireDelay);
				armed = !armed;
			}
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
}