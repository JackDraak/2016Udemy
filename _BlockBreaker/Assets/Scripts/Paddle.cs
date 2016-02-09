using UnityEngine;
using System.Collections;

public class Paddle : MonoBehaviour {

	public AudioClip paddle;

	private bool autoplay, begun, easy;
	private Ball ball;
	private Vector3 ballPos;
	private GameObject startNote;
	private LevelManager levelManager;
	
	void BeginPlay () {
		levelManager.HasStartedTrue();
		if (startNote) startNote.SetActive (false);
		ball.GetComponent<Rigidbody2D>().velocity = new Vector2 (Random.Range(-8f, 8f), Random.Range(8f, 10f));
	}

	void EasyFlip ()	{ if (easy) this.transform.localScale = new Vector2(2.5f,1); else this.transform.localScale = new Vector2(1.5f,1); }
	void EasyPlayOff ()	{ easy = false; EasySync(); }
	void EasyPlayOn ()	{ easy = true; EasySync(); }
	void EasySync ()	{ PlayerPrefsManager.SetEasy (easy); EasyFlip(); }
	
	void FixedUpdate () {
		PaddleMotion();
	}

	void OnTriggerEnter2D (Collider2D trigger) { 
		AudioSource.PlayClipAtPoint (paddle, transform.position); // optional 3rd float value for volume
	}
	
	float PaddleClamp (float xPos) {
		if (easy) {
			return Mathf.Clamp((xPos), 1.916f, 14.086f); // easy paddle
		} else {
			return Mathf.Clamp((xPos), 1.390f, 14.615f); // normal paddle
		}
	}
	
	void PaddleMotion () {
		Vector3 paddlePos = new Vector3 (8f, this.transform.position.y, 0f);
		if (!autoplay) 	{
			float mousePosInBlocks = (Input.mousePosition.x / Screen.width * 16);
			paddlePos.x = PaddleClamp(mousePosInBlocks);
		} 
		if (autoplay) 	{
			if (!levelManager.HasStartedReturn() && !begun) {
				begun = true;
				Invoke ("BeginPlay", 2);
			}
			if (levelManager.HasStartedReturn()) {
				ballPos = ball.transform.position;
				paddlePos.x = PaddleClamp(ballPos.x);
			}
		}
		this.transform.position = paddlePos;
	}

	void Start () {
		levelManager = GameObject.FindObjectOfType<LevelManager>();	if (!levelManager) Debug.LogError (this + ": unable to attach to LevelManager");
		if (GameObject.FindGameObjectWithTag ("StartNote")) startNote = GameObject.FindGameObjectWithTag ("StartNote");
		ball = GameObject.FindObjectOfType<Ball>();	if (!ball) Debug.LogError (this + ": unable to attach to Ball");
		autoplay = PlayerPrefsManager.GetAutoplay ();
		easy = PlayerPrefsManager.GetEasy ();
		EasyFlip();
	}
	
	void ToggleAuto () { autoplay = !autoplay; PlayerPrefsManager.SetAutoplay (autoplay); }
	void ToggleEasy () { easy = !easy; EasySync(); }

	// TODO this really ought to be in LevelManager or something, eh? y'think? meh... good enough for now....
	void Update () {
		if (Input.GetKeyDown(KeyCode.A)) {ToggleAuto();}
		if (Input.GetKeyDown(KeyCode.E)) {ToggleEasy();}
	}
}
