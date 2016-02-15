using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerController : MonoBehaviour {
	// adjust/set in inspector!
	public GameObject zappyBolt;
	public AudioClip zappySound;
	public AudioClip damage;
	public AudioClip scuttle;

	private float bulletSpeed = 420f;
	private GameObject playerGun;
	private float acceleration;
	private float baseAcceleration;
	private float fireDelay = 0.2f;
	private float fireTime;
	private float lateralVelocity;
	private float maxAcceleration = 0.4f;
	private float maxSpeed = 3f;
	private float padding = 0.6f;
	private bool right;
	private Vector3 tempPos;
	private float xMax, xMin;
	private LevelManager levelManager;
	private Color currentColor;
	private SpriteRenderer myRenderer;
	private float chance;
	private Text scoreboard;

	void OnTriggerEnter2D (Collider2D collider) {
	//	Debug.Log(collider);
		if (collider.tag == "EnemyBomb") {
			TakeDamage();
			Destroy (collider.gameObject);
		}
	}

	void TakeDamage () {
		// TODO typical time to do a visual & audible effect
		levelManager.PlayerChangeHealth(-(levelManager.GetPlayerHealth() * 0.3f) - 11f);
		//hitPoints = (hitPoints * 0.7f) - 11f;
		AudioSource.PlayClipAtPoint (damage, transform.position);
	//	Debug.Log ("PlayerHitPoints: " + hitPoints);
		if (levelManager.GetPlayerHealth() <= 0f) ScoreAndDestroy();
	}

	void ScoreAndDestroy () {
		// TODO typical time to do a visual & audible effect
		levelManager.ChangeScore(-100f);
		levelManager.PlayerDown();
		AudioSource.PlayClipAtPoint (scuttle, transform.position);
		if (levelManager.GetPlayerShips() <= 0) levelManager.LoseBattle();
		else levelManager.PlayerResetHitpoints();
	}

	void SetLeftward () {
		if (right) { right = !right; lateralVelocity = 0f; acceleration = 0f;}
		SetVelocity();
		SetNextPos();
	}

	void SetRightward () {
		if (!right) { right = !right; lateralVelocity = 0f; acceleration = 0f;}
		SetVelocity();
		SetNextPos();
	}

	void SetNextPos () {
		tempPos = transform.position;
		if (right) transform.position = new Vector3(SetXClamps(tempPos.x + lateralVelocity), tempPos.y, tempPos.z);
		else  transform.position = new Vector3(SetXClamps(tempPos.x - lateralVelocity), tempPos.y, tempPos.z);
	}
	
	void SetVelocity () {
		if (acceleration < maxAcceleration) acceleration = acceleration + baseAcceleration;
		if (lateralVelocity < maxSpeed) lateralVelocity = lateralVelocity + acceleration;

	}
	float SetXClamps (float position) {
		return Mathf.Clamp(position, xMin, xMax);
	}
	
	void Start () {
		playerGun = GameObject.FindGameObjectWithTag("PlayerGun"); if (!playerGun) Debug.LogError (this + " cant attach to PlayerGun. ERROR");
		levelManager = GameObject.FindObjectOfType<LevelManager>(); if (!levelManager) Debug.LogError ("LEVEL_MANAGER_FAIL");
		myRenderer = 	GetComponent<SpriteRenderer>(); if (!myRenderer) Debug.LogError ("FAIL renderer");

		scoreboard = GameObject.FindWithTag("Scoreboard").GetComponent<Text>(); if (!scoreboard) Debug.LogError("FAIL tag Scoreboard");
		float distance = transform.position.z - Camera.main.transform.position.z;
		Vector3 leftBoundary = Camera.main.ViewportToWorldPoint(new Vector3(0,0,distance));
		Vector3 rightBoundary = Camera.main.ViewportToWorldPoint(new Vector3(1,0,distance));
		xMin = leftBoundary.x + padding;
		xMax = rightBoundary.x - padding;
		acceleration = 0f;
		baseAcceleration = 0.025f;
		lateralVelocity = 0f;
		maxAcceleration = 0.7f;
		maxSpeed = 5f;
		fireTime = Time.time;
	//	hitPoints = maxHealth;
	//	shipCount = maxShips;
	}

	void Update () {
		if (acceleration > 0.001f) acceleration =- 0.01f; 
		if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) SetLeftward();
		if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) SetRightward();
		if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W)) InvokeShot();
		if (Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.W)) CancelInvoke();

		// desire: colour 1, 1, 1, 1 at full health slipping to 1, 0, 0, 1 at death
		float colourChange = levelManager.GetPlayerMaxHealth() / levelManager.GetPlayerHealth();
		currentColor = new Vector4 (1, 1/colourChange, 1/colourChange, 1f);
		myRenderer.color = currentColor;

		scoreboard.text = ("Score: " + levelManager.GetScore());
	}

	void FireBlaster () {
		if (fireTime + fireDelay <= Time.time) {
			GameObject discharge = Instantiate(zappyBolt, playerGun.transform.position, Quaternion.identity) as GameObject;
			discharge.GetComponent<Rigidbody2D>().velocity += Vector2.up * bulletSpeed * Time.deltaTime;
			AudioSource.PlayClipAtPoint (zappySound, transform.position);
			fireTime = Time.time;
		}
	}

	void InvokeShot () {
		InvokeRepeating ("FireBlaster", fireDelay, fireDelay);
	}
}

// lecture 106