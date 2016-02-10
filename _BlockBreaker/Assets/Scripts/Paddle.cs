using UnityEngine;
using System.Collections;

public class Paddle : MonoBehaviour {

	public AudioClip paddle;

	private bool autoplay, begun, driftDirection, easy;
	private Ball ball;
	private Vector3 ballPos;
	private float driftSkew, driftSpan, driftSpeed, driftThat, driftThis;
	private LevelManager levelManager;
	private GameObject startNote;
	
	void AutoMove () {
		Vector3 paddlePosition = new Vector3 (PaddleClamp (ball.transform.position.x + (driftSkew * driftSpan)), this.transform.position.y, 0f);
		this.transform.position = paddlePosition;
	}
  
	void BeginPlay () {
		levelManager.HasStartedTrue();
		if (startNote) startNote.SetActive (false);
		ball.GetComponent<Rigidbody2D>().velocity = new Vector2 (Random.Range(-8f, 8f), Random.Range(8f, 10f));
	}

	void EasyFlip () { if (easy) this.transform.localScale = new Vector2(2.5f,1); else this.transform.localScale = new Vector2(1.5f,1); }
	void EasyPlayOff () { easy = false; EasySync(); }
	void EasyPlayOn () { easy = true; EasySync(); }
	void EasySync () { PlayerPrefsManager.SetEasy (easy); EasyFlip(); }

	void FixedUpdate () { PaddleMotion(); }

	void OnTriggerEnter2D (Collider2D trigger) { 
		AudioSource.PlayClipAtPoint (paddle, transform.position);
	}
	
	float PaddleClamp (float xPos) {
		if (easy) return Mathf.Clamp((xPos), 1.916f, 14.086f); // easy paddle
		else return Mathf.Clamp((xPos), 1.390f, 14.615f); // normal paddle
	}
	
	void PaddleMotion () {
		Vector3 paddlePos = new Vector3 (8f, this.transform.position.y, 0f);
		if (!autoplay) 	{
			float mousePosInBlocks = (Input.mousePosition.x / Screen.width * 16);
			paddlePos.x = PaddleClamp(mousePosInBlocks);
			this.transform.position = paddlePos;
		} 
		if (autoplay) 	{
			if (!levelManager.HasStartedReturn() && !begun) {
				begun = true;
				Invoke ("BeginPlay", 2);
			}
			if (levelManager.HasStartedReturn()) {
				paddlePos.x = PaddleClamp(ball.transform.position.x);
				if (driftDirection) { // drifting right, +x
					driftSpan = driftSpan + driftSpeed;
					SetupDrift();
					AutoMove();
				} else { // drifting left, -x
					driftSpan = driftSpan - driftSpeed;
					SetupDrift();
					AutoMove();
				}
			}
		}
	}

	void SetupDrift () {
		TestDriftBoundary();
		// test distance to ball, & skew span...
		// objective, allow ball to "drift away from paddle collumn" 
		// (X-skew between paddle and ball, relative to ball's Y-distance from the lower boundary)
		// curently driftSpan is used directly to: 
		// Automove() the paddle, TestDriftBoundary(), Obv. Set Span for PaddleMotion(), 
		// ergo, calculate a skew factor here, apply it on Automove(), and Bob's your uncle.
		if (ball.transform.position.y > 1.3f) driftSkew = ball.transform.position.y * 0.55f;
		else driftSkew = 1f;
//		Debug.Log (ball.transform.position.y + " " + driftSkew);
	}

	void SetupDriftBoundary () {
		if (driftDirection) driftThat = Random.Range (0.21f, 0.79f);
		else driftThis = Random.Range (-0.21f, -0.79f);
	}

	void SetupDriftReset () {
		SetupDriftBoundary();
		ToggleDriftDirection();
		SetupDriftSpeed();
//		Debug.Log ("This: " + driftThis + ", That: " + driftThat + ", Direction: " + driftDirection + ", Velocity: " + driftSpeed + ", Span: " + driftSpan);
	}

	void SetupDriftSpeed () {
		driftSpeed = Random.Range (0.005f, 0.05f);
	}

	void Start () {
		levelManager = GameObject.FindObjectOfType<LevelManager>(); if (!levelManager) Debug.LogError (this + ": unable to attach to LevelManager");
		if (GameObject.FindGameObjectWithTag ("StartNote")) startNote = GameObject.FindGameObjectWithTag ("StartNote");
		ball = GameObject.FindObjectOfType<Ball>(); if (!ball) Debug.LogError (this + ": unable to attach to Ball");
		autoplay = PlayerPrefsManager.GetAutoplay ();
		easy = PlayerPrefsManager.GetEasy ();
		EasyFlip();
		driftDirection = true;
		driftSkew = 1;
		driftSpan = 0;
		driftSpeed = 0.02f;
	}
	
	void TestDriftBoundary () {
		if (driftDirection && driftSpan > driftThat) SetupDriftReset();
		if (!driftDirection && driftSpan < driftThis) SetupDriftReset();
	}
	
	void ToggleAuto () { autoplay = !autoplay; PlayerPrefsManager.SetAutoplay (autoplay); }
	void ToggleDriftDirection () { driftDirection = !driftDirection; }
	void ToggleEasy () { easy = !easy; EasySync(); }

	void Update () {
		if (Input.GetKeyDown(KeyCode.A)) ToggleAuto();
		if (Input.GetKeyDown(KeyCode.P)) ToggleEasy();
	}
}
