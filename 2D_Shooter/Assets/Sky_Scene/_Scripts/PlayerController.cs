using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerController : MonoBehaviour {
	// adjust/set in inspector!
	public AudioClip damage;
	public AudioClip scuttle;
	public GameObject smokeMachine;
	public GameObject zappyBolt;
	public AudioClip zappySound;

	private float acceleration, bulletSpeed, chance, driftScale, driftSpeed, fireDelay, fireTime, moveSpeed, padding, xMax, xMin;
	private Color currentColor;
	private LevelManager levelManager;
	private SpriteRenderer myRenderer;
	private GameObject playerGun;
	private bool right, up;
	private Vector3 myScale, tempPos;
	
	float SetXClamps (float position) { return Mathf.Clamp(position, xMin, xMax); }
	void SpawnPlayer () { transform.gameObject.SetActive(true); }

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

		bulletSpeed = 10f;
		fireDelay = 0.25f;
		moveSpeed = 20f;
		padding = 0.7f;

		driftScale = 1f;
		driftSpeed = 0.0017f;

		fireTime = Time.time;
		SetMinMaxX();
	}

	void Update () {
		// fire control
		if (Input.GetAxis("Fire1") > 0.9f) FireBlaster();

		// movement
		Vector3 myPos = transform.position;
		myPos.x += Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeed;
		string logPosition = "myPos.x (clamped) " + myPos.x;
		myPos.x = SetXClamps(myPos.x);
		logPosition += " (" + myPos.x + ")";
		Debug.Log (logPosition);
		transform.position = myPos;

		// damage haptics -- desire: colour 1, 1, 1, 1 at full health slipping to 1, 0, 0, 1 at death
		float colourChange = levelManager.GetPlayerMaxHealth() / levelManager.GetPlayerHealth();
		currentColor = new Vector4 (1, 1/colourChange, 1/colourChange, 1f);
		myRenderer.color = currentColor;
	}

	// scale-drifter to give floating appearance to player
	void FixedUpdate () {
		// toggle direction / set boundary
		if (driftScale <= 0.98) up = true;
		if (driftScale >= 1.02) up = false;

		// adjust scale
		if (up) driftScale += driftSpeed;
		else driftScale -= driftSpeed;

		// scale
		myScale = new Vector3 (driftScale, driftScale, 1);
		transform.localScale = myScale;
	}

	void SetMinMaxX () {
		float distance = transform.position.z - Camera.main.transform.position.z;
		Vector3 leftBoundary = Camera.main.ViewportToWorldPoint(new Vector3(0,0,distance));
		Vector3 rightBoundary = Camera.main.ViewportToWorldPoint(new Vector3(1,0,distance));
		xMin = leftBoundary.x + padding;
		xMax = rightBoundary.x - padding;
		Debug.Log (leftBoundary + " : " + xMin + " --- " + xMax + " : " + rightBoundary);
	}

	void TakeDamage () {
		levelManager.PlayerChangeHealth(-(levelManager.GetPlayerHealth() * 0.1f) - 20f);
		AudioSource.PlayClipAtPoint (damage, transform.position);
		GameObject trash = Instantiate(smokeMachine, transform.position, Quaternion.identity) as GameObject;
		if (levelManager.GetPlayerHealth() <= 0f) ScoreAndDestroy();
	}

	void ScoreAndDestroy () {
		transform.gameObject.SetActive(false);
		levelManager.PlayerResetHitpoints();
		levelManager.ChangeScore(-100f);
		levelManager.PlayerDown();
		AudioSource.PlayClipAtPoint (scuttle, transform.position);
		if (levelManager.GetPlayerShips() <= 0) levelManager.LoseBattle();
		else Invoke("SpawnPlayer", 1.5f);
	}

	void FireBlaster () {
		if (fireTime + fireDelay <= Time.time) {
			GameObject discharge = Instantiate(zappyBolt, playerGun.transform.position, Quaternion.Euler (0f, 0f, 180f)) as GameObject;
			discharge.GetComponent<Rigidbody2D>().velocity += Vector2.up * bulletSpeed;
			AudioSource.PlayClipAtPoint (zappySound, transform.position);
			fireTime = Time.time;
		}
	}
}
