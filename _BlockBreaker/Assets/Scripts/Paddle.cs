using UnityEngine;
using System.Collections;

public class Paddle : MonoBehaviour {
	public AudioClip paddle;
	
	private float autoplay;
	private Ball ball;
	private Vector3 ballPos;
	private bool easy, begun;
	private GameObject startNote;
	
	void Start () {
		Cursor.visible = false;
		if (GameObject.FindGameObjectWithTag ("StartNote")) startNote = GameObject.FindGameObjectWithTag ("StartNote");
		ball = GameObject.FindObjectOfType<Ball>();
		autoplay = PlayerPrefsManager.GetAutoplay ();
		easy = (PlayerPrefsManager.GetEasy () != 0);
		if (easy) EasyPlayOn ();
		else EasyPlayOff ();
	}
	
	void AutoplayOn () {
		autoplay = 1;
		PlayerPrefsManager.SetAutoplay (autoplay);
	}
	
	void AutoplayOff () {
		autoplay = 0;
		PlayerPrefsManager.SetAutoplay (autoplay);
	}
	
	void EasyPlayOn () {
		this.transform.localScale = new Vector2(2.5f,1);
		easy = true;
		PlayerPrefsManager.SetEasy (1);
	}
	
	void EasyPlayOff () {
		this.transform.localScale = new Vector2(1.5f,1);
		easy = false;
		PlayerPrefsManager.SetEasy (0);
	}
	
	void FixedUpdate () {
		if (Input.GetKeyDown(KeyCode.O)) {AutoplayOff();}
		if (Input.GetKeyDown(KeyCode.P)) {AutoplayOn();}
		if (Input.GetKeyDown(KeyCode.L)) {EasyPlayOn();}
		if (Input.GetKeyDown(KeyCode.K)) {EasyPlayOff();}

		Vector3 paddlePos = new Vector3 (8f, this.transform.position.y, 0f);
			if (autoplay == 0) 	{
				float mousePosInBlocks = (Input.mousePosition.x / Screen.width * 16);
				if (easy) {
					paddlePos.x = Mathf.Clamp((mousePosInBlocks), 1.916f, 14.086f); // easy paddle
				} else {
					paddlePos.x = Mathf.Clamp((mousePosInBlocks), 1.390f, 14.615f); // normal paddle
				}
			} 
			if (autoplay == 1) 	{
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
