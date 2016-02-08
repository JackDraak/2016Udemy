using UnityEngine;
using System.Collections;

public class Paddle : MonoBehaviour {
	public AudioClip paddle;

	private bool autoplay, easy, begun;
	private Ball ball;
	private Vector3 ballPos;
	private GameObject startNote;
	private LevelManager levelManager;
	
	void Start () {
	// TODO re-enable at some point for release?
	//	Cursor.visible = false; 
		levelManager = GameObject.FindObjectOfType<LevelManager>();
		if (!levelManager) Debug.LogError (this + ": unable to attach to LevelManager");
		if (GameObject.FindGameObjectWithTag ("StartNote")) startNote = GameObject.FindGameObjectWithTag ("StartNote");
		ball = GameObject.FindObjectOfType<Ball>();
		autoplay = PlayerPrefsManager.GetAutoplay ();
		easy = PlayerPrefsManager.GetEasy ();
		EasyFlip();
	}
	
	void EasyPlayOn () {
		easy = true;
		PlayerPrefsManager.SetEasy (easy);
		this.transform.localScale = new Vector2(2.5f,1);
	}
	
	void EasyPlayOff () {
		easy = false;
		PlayerPrefsManager.SetEasy (easy);
		this.transform.localScale = new Vector2(1.5f,1);
	}

	public void ToggleAuto () {
		autoplay = !autoplay;
	}

	public void EasyFlip () {
		easy = !easy;
		if (easy) this.transform.localScale = new Vector2(2.5f,1);
		else this.transform.localScale = new Vector2(1.5f,1);
	}
	
	void FixedUpdate () {

		Vector3 paddlePos = new Vector3 (8f, this.transform.position.y, 0f);
			if (!autoplay) 	{
				float mousePosInBlocks = (Input.mousePosition.x / Screen.width * 16);
				if (easy) {
					paddlePos.x = Mathf.Clamp((mousePosInBlocks), 1.916f, 14.086f); // easy paddle
				} else {
					paddlePos.x = Mathf.Clamp((mousePosInBlocks), 1.390f, 14.615f); // normal paddle
				}
			} 
			if (autoplay) 	{
				if (!levelManager.HasStartedTest() && !begun) {
					begun = true;
					Invoke ("BeginPlay", 2);
				}
				if (!levelManager.HasStartedTest()) {
					ballPos = ball.transform.position;
					if (easy) {
						paddlePos.x = Mathf.Clamp((ballPos.x), 1.916f, 14.086f); // easy paddle
					} else {
					paddlePos.x = Mathf.Clamp((ballPos.x), 1.390f, 14.615f); // normal paddle
					}
				}
			}
			this.transform.position = paddlePos;
	}
	
	void BeginPlay () {
		levelManager.HasStartedSet();
		if (startNote) startNote.SetActive (false);
		ball.GetComponent<Rigidbody2D>().velocity = new Vector2 (Random.Range(-12f, 12f), Random.Range(8f, 10f));
	}
	
	void OnTriggerEnter2D (Collider2D trigger) { 
		AudioSource.PlayClipAtPoint (paddle, transform.position); // optional 3rd float value for volume
	}
}
