using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TextController : MonoBehaviour { 

	public Text text;
	private enum States {Bedroom_1, Bedroom_T, Bedroom_W, Bedroom_A, Bedroom_D, Bedroom_R, Bedroom_S, Bathroom_1};
	private States MyState;
	  

	// Use this for initialization
	void Start () {
		MyState = States.Bedroom_1;
	
	}
	
	// Update is called once per frame
	void Update () {
		print (MyState);
		
		if (MyState == States.Bedroom_1) State_Bedroom_1();
		if (MyState == States.Bedroom_T) State_Bedroom_T();
		if (MyState == States.Bedroom_W) State_Bedroom_W();
		if (MyState == States.Bedroom_A) State_Bedroom_A();
		if (MyState == States.Bedroom_D) State_Bedroom_D();
		if (MyState == States.Bedroom_R) State_Bedroom_R();
		if (MyState == States.Bedroom_S) State_Bedroom_S();
		if (MyState == States.Bathroom_1) State_Bathroom_1();
	}
	void State_Bedroom_1 () {
		text.text = "There it goes again. Every stinking day... \n\n" +
			"The din from the alarm clock is muffled by the pillows and " +
			"sheets strewn uncerimonously over you, head included. Ted or " +
			"Jill must have thrown them over you when your inebriation got " +
			"the better of you sometime last night. That's probably also the reason " +
			"you would like to...\n\n" +
			"<T>hrow a shoe at the alarm clock\n" +
			"<W>ait it out, muffling your hung-over head with a couple of pillows";
		if (Input.GetKeyDown(KeyCode.T)) MyState = States.Bedroom_T;
		if (Input.GetKeyDown(KeyCode.W)) MyState = States.Bedroom_W;
	}
	void State_Bedroom_T () {
		text.text = "YES!!!!\n\n" +
			"It certainly seemed like a good idea at the time, but now that you've " +
			"missed the alarm clock you also realize that what you chucked was an " +
			"empty bottle. Of course it exploded against the wall. And, of course, " +
			"the alarm is growing louder and faster (because You needed the *special* " +
			"one!) the longer you ignore it. Re-considering your options, you...\n\n" +
			"<A>void the broken glass, and go to turn off the alarm clock\n" +
			"<D>ouble-down and throw a show at the alarm clock";
		if (Input.GetKeyDown(KeyCode.A)) MyState = States.Bedroom_A;
		if (Input.GetKeyDown(KeyCode.D)) MyState = States.Bedroom_D;
	}
	void State_Bedroom_W () {
		text.text = "mmmmmHmmm...\n\n" +
			"This isn't getting you anywhere. The alarm is getting louder and more " +
			"frantic. You grudingly ease your self from your nest and trip over " +
			"yourself in the darkness to finally bring about a welcomed silence. " +
			"You can't recall teh last time you partied that hard, but then again " +
			"It isn't every day three friends find the discovery of the century...\n\n" +
			"<R>ight! Forgot about that! Time to do some thinking!\n" +
			"<S>hake it off, your memory of yesterday must be off. Time for a hot shower";
		if (Input.GetKeyDown(KeyCode.R)) MyState = States.Bedroom_R;
		if (Input.GetKeyDown(KeyCode.S)) MyState = States.Bathroom_1;
	}
	void State_Bedroom_A () {
		text.text = "Bedroom_A\n\n" +
			"space to restart";
		if (Input.GetKeyDown(KeyCode.Space)) MyState = States.Bedroom_1;
	}
	void State_Bathroom_1 () {
		text.text = "Bathroom_1\n\n" +
			"space to restart";
		if (Input.GetKeyDown(KeyCode.Space)) MyState = States.Bedroom_1;
	}
	void State_Bedroom_R () {
		text.text = "Bedroom_R\n\n" +
			"space to restart";
		if (Input.GetKeyDown(KeyCode.Space)) MyState = States.Bedroom_1;
	}
	void State_Bedroom_S () {
		text.text = "Bedroom_R\n\n" +
			"space to restart";
		if (Input.GetKeyDown(KeyCode.Space)) MyState = States.Bedroom_1;
	}
	void State_Bedroom_D () {
		text.text = "Bedroom_D\n\n" +
			"space to restart";
		if (Input.GetKeyDown(KeyCode.Space)) MyState = States.Bedroom_1;
	}
}
