using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TextController : MonoBehaviour { 

	public Text text;
	private enum States {Bed, Dresser, Bathroom};
	private States MyState;
	private bool bBedDouble = false;
	private bool bBedThrow = false;
	private bool bBedWait = false;
	private bool bBedSceneExit = false;
	private bool bBedSleep = false;
	private bool bDresserThinking = false;
	private int cBedDouble = 0;
	
	// Use this for initialization
	void Start () {
		MyState = States.Bed;	
	}

	// Update is called once per frame
	void Update () {
		print (MyState);
		if (MyState == States.Bed) State_Bed();
		else if (MyState == States.Dresser) State_Dresser();
		else if (MyState == States.Bathroom) State_Bathroom();
	}
	
	void State_Bed() {
		if (!bBedThrow && !bBedWait && !bBedSceneExit && !bBedDouble && !bBedSleep) {
			text.text = "There it goes again. Every stinking day... \n\n" +
			"The din from the alarm clock is muffled by the pillows and " +
			"sheets strewn uncerimonously over you, head included. Ted or " +
			"Jill must have thrown them over you when your inebriation got the " +
			"better of you sometime last night. That's probably also the reason " +
			"you would like to...\n\n" +
			"<T>hrow a shoe at the alarm clock\n" +
			"<W>ait it out, muffling your hung-over head with a couple of pillows";
			if (Input.GetKeyDown(KeyCode.T)) {cBedDouble++; bBedThrow = true;}
			else if (Input.GetKeyDown(KeyCode.W)) bBedWait = true;
		}
		else if (bBedThrow && !bBedWait && !bBedSceneExit && !bBedDouble && !bBedSleep) {
			text.text = "YES!!!!\n\n" +
			"It certainly seemed like a good idea at the time, but now that you've " +
			"missed the alarm clock you also realize that what you chucked was an " +
			"empty bottle. Of course it exploded against the wall. And, of course, " +
			"the alarm is growing louder and faster the longer you ignore it (because " +
			"You needed the *special* one!). Re-considering your options, you...\n\n" +
			"<A>void the broken glass, and go to turn off the alarm clock\n" +
			"<D>ouble-down and throw a shoe at the alarm clock";
			if (Input.GetKeyDown(KeyCode.A)) { bBedSceneExit = true; MyState = States.Dresser; }
			else if (Input.GetKeyDown(KeyCode.D)) { cBedDouble++; bBedDouble = true;}	
		}
		else if (bBedWait && !bBedThrow && !bBedSceneExit && !bBedDouble && !bBedSleep) {
			text.text = "mmmmmHmmm...\n\n" +
			"This isn't getting you anywhere. The alarm is getting louder and more " +
			"frantic. You grudingly ease your self from your nest and trip over " +
			"yourself in the darkness to finally bring about a welcomed silence. " +
			"You can't recall the last time you partied that hard, but then again " +
			"It isn't every day three friends find the discovery of the century...\n\n" +
			"<R>ight! Forgot about that! Time to do some thinking!\n" +
			"<S>hake it off, your memory of yesterday must be off. Head back to bed.";
			if (Input.GetKeyDown(KeyCode.R)) {bDresserThinking = true; MyState = States.Dresser; }
			else if (Input.GetKeyDown(KeyCode.S)) bBedSleep = true;
		}
		else if (bBedDouble && !bBedWait && !bBedSceneExit && !bBedSleep) {
			text.text = "WHY!!!!\n\n" +
			"A part of you is rather uncertain why you'd go and do this again. " +
			"Another part of you wonders how much glass " + cBedDouble + " bottles " +
			"have left between you and the alarm clock on the dresser at the other " +
			"side of the room. Yet another part of you decides to actually, you know, " +
			"do something, like...\n\n" +
			"<A>void the broken glass, and go to turn off the alarm clock\n" +
			"<T>riple-down? and throw a shoe at the alarm clock";
			if (Input.GetKeyDown(KeyCode.A)) { bBedSceneExit = true; MyState = States.Dresser; }
			else if (Input.GetKeyDown(KeyCode.T)) { cBedDouble++; bBedDouble = true; }
		}
		else if (bBedSleep) {
			text.text = "Good Night\n\n" +
			"Hopefully you're less discombobulated the next timne you wake-up...\n\n" +
			"<R>eset game state\n" +
			"<S>oft-restart";
			if (Input.GetKeyDown(KeyCode.S)) { bBedSleep=false; }
			else if (Input.GetKeyDown(KeyCode.R)) { 
				bBedDouble = false;
				bBedThrow = false;
				bBedWait = false;
				bBedSceneExit = false;
				bBedSleep = false;
				bDresserThinking = false;
				cBedDouble = 0;
				bBedSceneExit = false; 
				MyState = States.Bed; 
			}
		}
	}
	
	void State_Dresser () {
		if (!bDresserThinking) {
			text.text = "Dresser, not thinking, per se\nPress R to reset game state";
			if (Input.GetKeyDown(KeyCode.R)) { 
				bBedDouble = false;
				bBedThrow = false;
				bBedWait = false;
				bBedSceneExit = false;
				bBedSleep = false;
				bDresserThinking = false;
				cBedDouble = 0;
				bBedSceneExit = false; 
				MyState = States.Bed; 
			}
		}
		else if (bDresserThinking) {
			text.text = "Dresser, thinking.\nPress R to reset game state";
			if (Input.GetKeyDown(KeyCode.R)) { 
				bBedDouble = false;
				bBedThrow = false;
				bBedWait = false;
				bBedSceneExit = false;
				bBedSleep = false;
				cBedDouble = 0;
				bBedSceneExit = false; 
				Debug.Log ("this line executed: bDresserThinking = " + bDresserThinking );
				MyState = States.Bed; // note this does not prevent following lines from executing
				bDresserThinking = false;
				Debug.Log ("this line executed: bDresserThinking = " + bDresserThinking );
			}
		}
	}
	
	void State_Bathroom () {
		text.text = "Bathroom\n\n" +
			"space to restart";
		if (Input.GetKeyDown(KeyCode.Space)) MyState = States.Bed;
	}

}
