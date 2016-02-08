using UnityEngine;
using System.Collections;

public class Paddle : MonoBehaviour {
	public AudioClip paddle;
	
	private bool autoplay;
	private Ball ball;
	private Vector3 ballPos;
	private bool easy, begun;
	private GameObject startNote;
	
	void Start () {
		Cursor.visible = false;
		if (GameObject.FindGameObjectWithTag ("StartNote")) startNote = GameObject.FindGameObjectWithTag ("StartNote");
		ball = GameObject.FindObjectOfType<Ball>();
		autoplay = PlayerPrefsManager.GetAutoplay ();
		easy = PlayerPrefsManager.GetEasy ();
		if (easy) EasyPlayOn ();
		else EasyPlayOff ();
	}

	void AutoplayOn () {
		autoplay = true;
		PlayerPrefsManager.SetAutoplay (autoplay);
	}
	
	void AutoplayOff () {
		autoplay = false;
		PlayerPrefsManager.SetAutoplay (autoplay);
	}

	void ToggleAuto () {
		autoplay = !autoplay;
		PlayerPrefsManager.SetAutoplay (autoplay);
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

	void ToggleEasy () {
		easy = !easy;
		PlayerPrefsManager.SetEasy (easy);
		if (easy) this.transform.localScale = new Vector2(2.5f,1);
		else this.transform.localScale = new Vector2(1.5f,1);
	}
	
	void FixedUpdate () {
		if (Input.GetKeyDown(KeyCode.A)) {ToggleAuto();}
		if (Input.GetKeyDown(KeyCode.E)) {ToggleEasy();}
		if (Input.GetKeyDown(KeyCode.O)) {AutoplayOff();}
		if (Input.GetKeyDown(KeyCode.P)) {AutoplayOn();}
		if (Input.GetKeyDown(KeyCode.L)) {EasyPlayOn();}
		if (Input.GetKeyDown(KeyCode.K)) {EasyPlayOff();}

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
				if (!ball.hasStarted && !begun) {
					begun = true;
					Invoke ("BeginPlay", 2);
				}
				if (ball.hasStarted) {
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
		ball.hasStarted = true;
		if (startNote) startNote.SetActive (false);
		ball.GetComponent<Rigidbody2D>().velocity = new Vector2 (Random.Range(-12f, 12f), Random.Range(8f, 10f));
	}
	
	void OnTriggerEnter2D (Collider2D trigger) { 
		AudioSource.PlayClipAtPoint (paddle, transform.position); // optional 3rd float value for volume
	}
}
