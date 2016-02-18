using UnityEngine;
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
	private LevelManager levelManager;
	private SpriteRenderer myRenderer;
	private Vector3 myScale;

	void Start () {
		levelManager = GameObject.FindObjectOfType<LevelManager>(); 
			if (!levelManager) Debug.LogError ("LEVEL_MANAGER_FAIL");
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

		// haptic health indicator
		float colourChange = maxHealth / hitPoints;
		// desire: colour 1, 1, 1, 1 at full health slipping to 1, 0, 0, 1 at death
		currentColor = new Vector4 (1f, 1/colourChange, 1/colourChange, 1f);
		myRenderer.color = currentColor;

		// fire control reset
		chance = Random.Range (1, 100);
		if (chance > 98) armed = !armed;
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
	
	public void Disarm () { dearmed = true; }

	public void Rearm () { dearmed = false; }
	
	void TakeDamage () {
		hitPoints = (hitPoints * 0.90f) - 17f;
		AudioSource.PlayClipAtPoint (damage, transform.position);
		GameObject trash = Instantiate(puffMachine, transform.position, Quaternion.identity) as GameObject;
		if (hitPoints <= 0f) ScoreAndDestroy();
	}

	void ScoreAndDestroy () {
		// TODO typical time to randomly "drop a bonus"
		levelManager = GameObject.FindObjectOfType<LevelManager>(); // why the heck do I need this here to prevent exception faults?
			if (!levelManager) Debug.LogError ("LEVEL_MANAGER_FAIL");
		levelManager.ChangeScore(100f);
		AudioSource.PlayClipAtPoint (scuttle, transform.position);
		levelManager.EnemyDown();
		Destroy(this.gameObject, 0.001f);
	}
}
