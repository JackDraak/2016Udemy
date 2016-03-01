using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerController : MonoBehaviour {
	// adjust/set in inspector!
	public Text boostMessage;
	public AudioClip damage;
	public AudioClip scuttle;
	public GameObject smokeLoc;
	public GameObject smokeMachine;
	public GameObject thruster;
	public GameObject zappyBolt;
	public AudioClip zappySound;

	[SerializeField]
	private float bulletSpeed, boostDelay, driftScale, driftSpeed, fireBoostTime, fireDelay, fireTime, moveSpeed, padding, playerMaxHealth, playerHitPoints, speedBoostTime, xMax, xMin;
	private Color currentColor;
	private LevelManager levelManager;
	private Vector3 myPos, myScale;
	private SpriteRenderer myRenderer;
	private GameObject playerGun;
	[SerializeField]
	private bool boostSpeed, boostFire; 
	private bool up;
	
	float SetXClamps (float position) { return Mathf.Clamp(position, xMin, xMax); }
	void SpawnPlayer () { transform.gameObject.SetActive(true); }

	void OnTriggerEnter2D (Collider2D collider) {
		if (collider.tag == "EnemyProjectile") {
			TakeDamage();
			Destroy (collider.gameObject);
		} else if (collider.tag == "PowerUp") {
			PowerUp();
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

		playerMaxHealth = 420f;
		playerHitPoints = playerMaxHealth;

		boostSpeed = false;
		boostFire = false;
		boostDelay = 15f;
		speedBoostTime = Time.time - boostDelay;
		fireBoostTime = speedBoostTime;

		bulletSpeed = 10f;
		fireDelay = 0.37f;
		moveSpeed = 20f;
		padding = 0.7f;

		driftScale = 1.0f;
		driftSpeed = 0.0017f;

		fireTime = Time.time;
		SetMinMaxX();
		ClearBoostMessage();

		smokeMachine.GetComponent<Renderer>().sortingLayerName = "PlayerDamage";
		thruster.GetComponent<Renderer>().sortingLayerName = "Projectiles";
		myPos = transform.position;
	}

	void Update () {
		// fire control
		if (Input.GetAxis("Fire1") > 0.9f) FireBlaster();

		// apply movement
		float priorX = myPos.x;
		myPos.x += Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeed;
		myPos.x = SetXClamps(myPos.x);
		if (priorX != myPos.x) transform.position = myPos; 

		// apply scale
		myScale = new Vector3 (driftScale, driftScale, 1);
		transform.localScale = myScale;

		// damage haptics -- desire: colour 1, 1, 1, 1 at full health slipping to 1, 0, 0, 1 at death
		Vector4 priorColour = currentColor;
		float colourDelta = playerMaxHealth / playerHitPoints;
		currentColor = new Vector4 (1, 1/colourDelta, 1/colourDelta, 1f);
		if (!Mathf.Approximately(priorColour.y, currentColor.g))  myRenderer.color = currentColor;

		// conform boost states
		if (boostSpeed && speedBoostTime + boostDelay > Time.time) moveSpeed = 40f; else { moveSpeed = 20f; boostSpeed = false; }
		if (boostFire && fireBoostTime + boostDelay > Time.time) fireDelay = 0.185f; else { fireDelay = 0.37f; boostFire = false; }
	}

	// scale-drifter to give floating appearance to player
	void FixedUpdate () {
		// toggle direction / set boundary %
		if (driftScale <= 0.98) up = true;
		if (driftScale >= 1.02) up = false;

		// adjust scale
		if (up) driftScale += driftSpeed;
		else driftScale -= driftSpeed;
	}

	void ClearBoostMessage () {
		boostMessage.text = "";
	}

	void PowerUp () { // TODO add health boost option?
		levelManager.ChangeScore(levelManager.GetWaveNumber() * 50);
		int selection = Random.Range (1,4);
		switch (selection) {
			case 1:
				boostFire = true;
				fireBoostTime = Time.time;
				boostMessage.text = "Firepower Boost!";
				Invoke ("ClearBoostMessage", 2.5f);
				break;
			case 2:
				boostSpeed = true;
				speedBoostTime = Time.time;
				boostMessage.text = "Speed Boost!";
				Invoke ("ClearBoostMessage", 2.5f);
				break;
		case 3:
				playerHitPoints = playerMaxHealth;
				boostMessage.text = "Health Boost!";
				Invoke ("ClearBoostMessage", 2.5f);
				break;
		}
	}

	void SetMinMaxX () {
		float distance = transform.position.z - Camera.main.transform.position.z;
		Vector3 leftBoundary = Camera.main.ViewportToWorldPoint(new Vector3(0,0,distance));
		Vector3 rightBoundary = Camera.main.ViewportToWorldPoint(new Vector3(1,0,distance));
		xMin = leftBoundary.x + padding;
		xMax = rightBoundary.x - padding;
	}

	void TakeDamage () {
		playerHitPoints -= ((playerHitPoints * 0.1f) + 60f);
		AudioSource.PlayClipAtPoint (damage, transform.position);
		GameObject trash = Instantiate(smokeMachine, smokeLoc.transform.position, Quaternion.identity) as GameObject;
		trash.GetComponent<Renderer>().sortingLayerName = "PlayerDamage";
		if (playerHitPoints <= 0f) ScoreAndDestroy();
	}

	void ScoreAndDestroy () {
		AudioSource.PlayClipAtPoint (scuttle, transform.position);
		transform.gameObject.SetActive(false);
		levelManager.ChangeScore(-100f);
		levelManager.PlayerDown();
		playerHitPoints = playerMaxHealth;
		if (levelManager.GetPlayerShips() <= 0) levelManager.LoseBattle();
		else {
			boostFire = false;
			boostSpeed = false;
			Invoke("SpawnPlayer", 1.8f);
		}
	}

	void FireBlaster () {
		if (fireTime + fireDelay <= Time.time) {
			AudioSource.PlayClipAtPoint (zappySound, transform.position, 0.5f);
			GameObject obj = GenericPooler.current.GetPooledObject();
			obj.transform.position = playerGun.transform.position;
			obj.transform.localRotation = Quaternion.Euler (0f, 0f, 180f);
			obj.SetActive(true);
			obj.GetComponent<Rigidbody2D>().velocity += Vector2.up * bulletSpeed;
			fireTime = Time.time;
		}
	}
}
