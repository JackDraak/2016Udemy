using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyController : MonoBehaviour {
	// adjust/set in inspector!
	public GameObject bomb;
	public AudioClip bombSound;
	public AudioClip damage;
	public AudioClip scuttle;
	public GameObject puffMachine;
	public GameObject puffLocation;
	public int bombPool = 3;

	private bool armed, dearmed;
	private float bombSpeed, chance, fireDelay, fireTime, hitPoints, maxHealth;
	private Color currentColor;
	private LevelManager levelManager;
	private List<GameObject> bombs;
	private SpriteRenderer myRenderer;
	private Vector3 myScale;
	
	public void Disarm () { dearmed = true; }
	public void Rearm () { dearmed = false; }

	void Start () {
		levelManager = GameObject.FindObjectOfType<LevelManager>(); 
			if (!levelManager) Debug.LogError ("LEVEL_MANAGER_FAIL");
		myRenderer = GetComponent<SpriteRenderer>();
			if (!myRenderer) Debug.LogError ("FAIL renderer");

		bombs = new List<GameObject>();
		for (int i = 0; i < bombPool; i++) {
			GameObject obj = (GameObject)Instantiate(bomb);
			obj.SetActive(false);
			bombs.Add(obj);
		}

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

		// fire control reset?
		chance = Random.Range (1, 100);
		if (chance > 49 && chance < 51) armed = !armed;

		// damage haptics -- desire: colour 1, 1, 1, 1 at full health slipping to 1, 0, 0, 1 at death
		Vector4 priorColour = currentColor;
		float colourDelta = maxHealth / hitPoints;
		currentColor = new Vector4 (1, 1/colourDelta, 1/colourDelta, 1f);
		if (!Mathf.Approximately(priorColour.y, currentColor.g))  myRenderer.color = currentColor;
	}
	
	void OnTriggerEnter2D (Collider2D collider) {
		if (collider.tag == "PlayerProjectile") {
			TakeDamage();
			if (collider.gameObject) Destroy (collider.gameObject, 0.01f);
		}
	}
	
	void DropBomb () {
		if (fireTime + fireDelay <= Time.time) {

	/*		for (int i = 0; i < bombs.Count; i++) { // TODO should this be <= ??
				if (!bombs[i].activeInHierarchy) {
					bombs[i].transform.position = transform.position;
					bombs[i].transform.rotation = transform.rotation;
					bombs[i].SetActive(true);
					bombs[i].GetComponent<Rigidbody2D>().velocity += Vector2.down * bombSpeed;
					break;
				}
			} */

			GameObject obj = GenericPooler.current.GetPooledObject();
			if (obj == null) return;
			obj.transform.position = transform.position;
			obj.transform.rotation = transform.rotation;
			obj.SetActive(true);
			obj.GetComponent<Rigidbody2D>().velocity += Vector2.down * bombSpeed;

//			GameObject discharge = Instantiate(bomb, transform.position, Quaternion.identity) as GameObject;
//			discharge.GetComponent<Rigidbody2D>().velocity += Vector2.down * bombSpeed;
			AudioSource.PlayClipAtPoint (bombSound, transform.position);
			fireTime = Time.time + Random.Range(0.0f, 4.0f);
		}
	}
	
	void TakeDamage () {
		hitPoints = (hitPoints * 0.93f) - 23f;
		AudioSource.PlayClipAtPoint (damage, transform.position);
		GameObject trash = Instantiate(puffMachine, puffLocation.transform.position, Quaternion.identity) as GameObject;
		trash.GetComponent<ParticleSystem>().GetComponent<Renderer>().sortingLayerName = "EnemyDamage";
		if (hitPoints <= 0f) ScoreAndDestroy();
	}

	void ScoreAndDestroy () {
		// TODO typical time to randomly "drop a bonus"
		levelManager = GameObject.FindObjectOfType<LevelManager>(); // why the heck do I need this here to prevent exception faults?
			if (!levelManager) Debug.LogError ("LEVEL_MANAGER_FAIL");
		levelManager.ChangeScore(5 * levelManager.GetWaveNumber());
		AudioSource.PlayClipAtPoint (scuttle, transform.position);
		levelManager.EnemyDown();
		gameObject.SetActive(false); 
	}
}
