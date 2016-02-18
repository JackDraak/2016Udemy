using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerController : MonoBehaviour {
	// adjust/set in inspector!
	public AudioClip damage;
	public AudioClip scuttle;
	public GameObject zappyBolt;
	public AudioClip zappySound;
	public GameObject smokeMachine;

	private float acceleration;
	private float bulletSpeed = 10f;
	private float chance;
	private Color currentColor;
	private float fireDelay = 0.25f;
	private float fireTime;
	private LevelManager levelManager;
	private float moveSpeed = 20f;
	private SpriteRenderer myRenderer;
	private float padding = 0.6f;
	private GameObject playerGun;
	private bool right;
	private Text scoreboard;
	private Vector3 tempPos;
	private float xMax, xMin;

	void OnTriggerEnter2D (Collider2D collider) {
		if (collider.tag == "EnemyBomb") {
			TakeDamage();
			Destroy (collider.gameObject);
		}
	}

	void Start () {
		levelManager = GameObject.FindObjectOfType<LevelManager>();
			if (!levelManager) Debug.LogError ("LEVEL_MANAGER_FAIL");
		myRenderer = GetComponent<SpriteRenderer>();
			if (!myRenderer) Debug.LogError ("FAIL renderer");
		playerGun = GameObject.FindGameObjectWithTag("PlayerGun");
			if (!playerGun) Debug.LogError (this + " cant attach to PlayerGun. ERROR");
		scoreboard = GameObject.FindWithTag("Scoreboard").GetComponent<Text>();
			if (!scoreboard) Debug.LogError("FAIL tag Scoreboard");

		float distance = transform.position.z - Camera.main.transform.position.z;
		Vector3 leftBoundary = Camera.main.ViewportToWorldPoint(new Vector3(0,0,distance));
		Vector3 rightBoundary = Camera.main.ViewportToWorldPoint(new Vector3(1,0,distance));
		xMin = leftBoundary.x + padding;
		xMax = rightBoundary.x - padding;

		fireTime = Time.time;
	}

	void Update () {
		if (acceleration > 0.001f) acceleration =- 0.01f; 

		if (Input.GetAxis("Fire1") > 0.9f) FireBlaster();

		Vector3 myPos = transform.position;
		myPos.x += Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeed;
		myPos.x = SetXClamps(myPos.x);
		transform.position = myPos;

		// desire: colour 1, 1, 1, 1 at full health slipping to 1, 0, 0, 1 at death
		float colourChange = levelManager.GetPlayerMaxHealth() / levelManager.GetPlayerHealth();
		currentColor = new Vector4 (1, 1/colourChange, 1/colourChange, 1f);
		myRenderer.color = currentColor;

		scoreboard.text = ("Score: " + levelManager.GetScore());
	}

	void Unwind () { smokeMachine.SetActive(false); }

	void SpawnPlayer () {
		transform.gameObject.SetActive(true);		
	}

	void TakeDamage () {
		// TODO typical time to do a visual & audible effect
		levelManager.PlayerChangeHealth(-(levelManager.GetPlayerHealth() * 0.1f) - 20f);
		AudioSource.PlayClipAtPoint (damage, transform.position);
		GameObject smoke = Instantiate(smokeMachine, transform.position, Quaternion.identity) as GameObject;
		if (levelManager.GetPlayerHealth() <= 0f) ScoreAndDestroy();
	}

	void ScoreAndDestroy () {
		// TODO typical time to do a visual & audible effect
		transform.gameObject.SetActive(false);
		levelManager.PlayerResetHitpoints();
		levelManager.ChangeScore(-100f);
		levelManager.PlayerDown();
		AudioSource.PlayClipAtPoint (scuttle, transform.position);
		if (levelManager.GetPlayerShips() <= 0) levelManager.LoseBattle();
		else Invoke("SpawnPlayer", 1.5f);
	}

	float SetXClamps (float position) {
		return Mathf.Clamp(position, xMin, xMax);
	}

	void FireBlaster () {
		if (fireTime + fireDelay <= Time.time) {
			GameObject discharge = Instantiate(zappyBolt, playerGun.transform.position, Quaternion.identity) as GameObject;
			Quaternion rot = Quaternion.Euler (0f, 0f, 180f);
			discharge.transform.rotation = rot;
			discharge.GetComponent<Rigidbody2D>().velocity += Vector2.up * bulletSpeed;
			AudioSource.PlayClipAtPoint (zappySound, transform.position);
			fireTime = Time.time;
		}
	}

	void InvokeShot () {
		InvokeRepeating ("FireBlaster", fireDelay, fireDelay);
	}
}
