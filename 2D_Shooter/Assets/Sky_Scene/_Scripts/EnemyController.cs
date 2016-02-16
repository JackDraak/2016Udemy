using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour {
	// adjust/set in inspector!
	public GameObject bomb;
	public AudioClip bombSound;
	public AudioClip damage;
	public AudioClip scuttle;

	private bool armed;
	private float bombSpeed = 8f;
	private float chance;
	private Color currentColor;
	private bool dearmed;
	private float fireDelay = 1.4f;
	private float fireTime;
	private float hitPoints;
	private LevelManager levelManager;
	private float maxHealth = 111f; // TODO tweak this
	private SpriteRenderer myRenderer;

	void Start () {
		levelManager = GameObject.FindObjectOfType<LevelManager>(); 
			if (!levelManager) Debug.LogError ("LEVEL_MANAGER_FAIL");
		myRenderer = GetComponent<SpriteRenderer>();
			if (!myRenderer) Debug.LogError ("FAIL renderer");
		armed = true;
		dearmed = false;
		fireTime = Time.time;
		hitPoints = maxHealth;
	}

	void Update () {
		if (!dearmed) {
			if (!armed) {
				InvokeRepeating ("DropBomb", fireDelay, fireDelay);
				armed = !armed;
			}
		}
		float colourChange = maxHealth / hitPoints;
		// desire: colour 1, 1, 1, 1 at full health slipping to 1, 0, 0, 1 at death
		currentColor = new Vector4 (1f, 1/colourChange, 1/colourChange, 1f);
		myRenderer.color = currentColor;
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
		// TODO typical time to do a visual effect
		hitPoints = (hitPoints * 0.90f) - 17f;
		AudioSource.PlayClipAtPoint (damage, transform.position);
		if (hitPoints <= 0f) ScoreAndDestroy();
	}

	// TODO typical time to do a visual effect
	void ScoreAndDestroy () {
		// TODO typical time to randomly "drop a bonus"
		levelManager = GameObject.FindObjectOfType<LevelManager>(); // why the heck do I need this here to prevent exception faults?
			if (!levelManager) Debug.LogError ("LEVEL_MANAGER_FAIL");
		levelManager.ChangeScore(100f);
		AudioSource.PlayClipAtPoint (scuttle, transform.position);
		levelManager.EnemyDown();
		Debug.Log (this.gameObject + " DestroyMessage @ " + Time.time);
		Destroy(this.gameObject, 0.001f);
	}
}