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
	private float playerHitPoints, moveSpeed;
	[SerializeField]
	private bool boostFire;
	[SerializeField]
	private float fireBoostTime;
	[SerializeField]
	private bool boostSpeed;
	[SerializeField]
	private float speedBoostTime;

	private float bulletSpeed, boostTerm, boostMessageTime, driftScale, driftSpeed, fireDelay, fireTime, padding, playerMaxHealth, xMax, xMin;
	private Color currentColor;
	private LevelManager levelManager;
	private Vector3 myPos, myScale;
	private SpriteRenderer myRenderer;
	private GameObject playerGun;
	private bool up;

	// public function(s)
	public void Debuff () { boostFire = false; boostSpeed = false; }
	
	void Start () {
		levelManager = GameObject.FindObjectOfType<LevelManager>();
			if (!levelManager) Debug.LogError ("PlayerController Start !levelManager");
		myRenderer = GetComponent<SpriteRenderer>();
			if (!myRenderer) Debug.LogError ("PlayerController Start !myRenderer");
		playerGun = GameObject.FindGameObjectWithTag("PlayerGun");
			if (!playerGun) Debug.LogError ("PlayerController Start !playerGun");

		playerMaxHealth = 420f;
		playerHitPoints = playerMaxHealth;

		boostSpeed = false;
		boostFire = false;
		boostTerm = 10f;
		speedBoostTime = Time.time - boostTerm;
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
		// clear boost mesasge when needed
		if (boostMessageTime + 2.5f < Time.time) ClearBoostMessage();

		// conform fire control
		if (Input.GetAxis("Fire1") > 0.9f) FireBlaster();

		// testing mode autofire
//		FireBlaster();

		// conform movement
		float priorX = myPos.x;
		myPos.x += Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeed;
		myPos.x = SetXClamps(myPos.x);
		if (priorX != myPos.x) transform.position = myPos; 

		// conform scale
		myScale = new Vector3 (driftScale, driftScale, 1);
		transform.localScale = myScale;

		// conform damage haptics
		Vector4 priorColour = currentColor;
		float colourDelta = playerMaxHealth / playerHitPoints;
		currentColor = new Vector4 (1, 1/colourDelta, 1/colourDelta, 1f);
		if (!Mathf.Approximately(priorColour.y, currentColor.g))  myRenderer.color = currentColor;

		// conform boost states
		if (boostSpeed && speedBoostTime + boostTerm > Time.time) moveSpeed = 40f; else { moveSpeed = 20f; boostSpeed = false; }
		if (boostFire && fireBoostTime + boostTerm > Time.time) fireDelay = 0.185f; else { fireDelay = 0.37f; boostFire = false; }
	}
	
	// scale-drifter to give floating appearance to player sprite
	void FixedUpdate () {
		// toggle direction / set boundary %
		if (driftScale <= 0.98) up = true;
		if (driftScale >= 1.02) up = false;
		
		// conform scale
		if (up) driftScale += driftSpeed;
		else driftScale -= driftSpeed;
	}

	void OnTriggerEnter2D (Collider2D collider) {
		if (collider.tag == "EnemyProjectile") {
			TakeDamage();
			Destroy (collider.gameObject);
		} else if (collider.tag == "PowerUp") {
			PowerUp();
			Destroy (collider.gameObject);
		}
	}
	
	void ClearBoostMessage () { boostMessage.text = ""; }

	void FireBlaster () {
		if (fireTime + fireDelay <= Time.time) {
			AudioSource.PlayClipAtPoint (zappySound, transform.position, 0.8f);
			GameObject obj = GenericPooler.current.GetPooledObject();
			obj.transform.position = playerGun.transform.position;
			obj.transform.localRotation = Quaternion.Euler (0f, 0f, 180f);
			obj.SetActive(true);
			obj.GetComponent<Rigidbody2D>().velocity += Vector2.up * bulletSpeed;
			fireTime = Time.time;
		}
	}

	void PowerUp () {
		levelManager.ChangeScore(levelManager.GetWaveNumber() * 50);
		int selection = Random.Range (1,4);
		switch (selection) {
			case 1:
				boostFire = true;
				fireBoostTime = Time.time;
				boostMessage.text = "Firepower Boost!";
				boostMessageTime = Time.time;
				break;
			case 2:
				boostSpeed = true;
				speedBoostTime = Time.time;
				boostMessage.text = "Speed Boost!";
				boostMessageTime = Time.time;
				break;
		case 3:
				playerHitPoints = Mathf.Clamp((playerHitPoints +  (0.25f * playerMaxHealth)), 1, playerMaxHealth);
				boostMessage.text = "Health Boost!";
				boostMessageTime = Time.time;
				break;
		}
	}

	void ScoreAndDestroy () {
		AudioSource.PlayClipAtPoint (scuttle, transform.position, 1f);
		transform.gameObject.SetActive(false);
		levelManager.ChangeScore(-100f);
		levelManager.PlayerDown();
		playerHitPoints = playerMaxHealth;
		if (levelManager.GetPlayerShips() <= 0) {
			boostFire = false;
			boostSpeed = false;
			levelManager.LoseBattle();
		}
		else {
			boostFire = false;
			boostSpeed = false;
			Invoke("SpawnPlayer", 2.1f);
		}
	}

	float SetXClamps (float position) { return Mathf.Clamp(position, xMin, xMax); }

	void SetMinMaxX () {
		float distance = transform.position.z - Camera.main.transform.position.z;
		Vector3 leftBoundary = Camera.main.ViewportToWorldPoint(new Vector3(0,0,distance));
		Vector3 rightBoundary = Camera.main.ViewportToWorldPoint(new Vector3(1,0,distance));
		xMin = leftBoundary.x + padding;
		xMax = rightBoundary.x - padding;
	}

	void SpawnPlayer () { transform.gameObject.SetActive(true); }
	
	void TakeDamage () {
		playerHitPoints -= ((playerHitPoints * 0.1f) + 60f);
		AudioSource.PlayClipAtPoint (damage, transform.position, 1f);
		GameObject trash = Instantiate(smokeMachine, smokeLoc.transform.position, Quaternion.identity) as GameObject;
		trash.GetComponent<Renderer>().sortingLayerName = "PlayerDamage";
		if (playerHitPoints <= 0f) ScoreAndDestroy();
	}
}
