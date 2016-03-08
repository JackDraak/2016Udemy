using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EnemyController : MonoBehaviour {

	// adjust/set in inspector!
	public GameObject bomb, powerUp;
	public AudioClip bombSound;
	public AudioClip damage;
	public GameObject puffLocation;
	public GameObject puffMachine;
	public AudioClip scuttle;

	private bool armed, dearmed, insane;
	private float bombSpeed, chance, fireDelay, fireTime, hitPoints, maxHealth;
	private Color currentColor;
	private LevelManager levelManager;
	private SpriteRenderer myRenderer;
	private int myWave,_instance;

	// public function(s)
	public void Disarm () { dearmed = true; }
	public void Rearm () { dearmed = false; }

	void Start () {
		levelManager = GameObject.FindObjectOfType<LevelManager>(); 
			if (!levelManager) Debug.LogError ("Enemy Controller Start !levelManager");
		myRenderer = GetComponent<SpriteRenderer>();
			if (!myRenderer) Debug.LogError ("Enemy Controller Start !myRenderer");

		insane = levelManager.insane;

		armed = true;
		bombSpeed = 6f;
		dearmed = false;
		fireDelay = 0.2f;
		fireTime = Time.time;
		maxHealth = 111f; // 111

		myWave = levelManager.GetWaveNumber();

		// difficulty tuning: increases each wave
		fireDelay = 1 / (myWave * 0.3f);
		maxHealth += (myWave * 1.7f);
		hitPoints = maxHealth;
	}

	void Update () {
		// AI: fire control init
		if (!dearmed) {
			if (!armed) {
				InvokeRepeating ("DropBomb", fireDelay, fireDelay);
				armed = !armed;
			}
		}

		// AI: fire control resetter
		chance = Random.Range (1, 101);
		if (chance > 47 && chance < 53) armed = !armed;

		// conform damage haptics
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
		levelManager.ChangeScore(5 * levelManager.GetWaveNumber());
		if (hitPoints <= 0f) ScoreAndDestroy();
		if (insane) {
			if (Random.Range(0,2) < 1) DropPowerBonus(); 
		}
	}

	void ScoreAndDestroy () {
		AudioSource.PlayClipAtPoint (scuttle, transform.position);
		levelManager.ChangeScore(25 * levelManager.GetWaveNumber());
		levelManager.EnemyDown();
		int chance = Random.Range (1,101);
		if (chance > 38 && chance <= 62) DropPowerBonus(); 
		Destroy(this.gameObject, 0.05f);
	}
}
