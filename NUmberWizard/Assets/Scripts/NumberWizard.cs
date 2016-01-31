using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NumberWizard : MonoBehaviour {
	int RangeTop;
	int RangeBottom;
	bool bGameOver;
	int CurrentGuess;

	public Text text;

	void Start () {
        	StartNWGame ();
    	}

	void StartNWGame ()
	{
		RangeTop = 1000;
		RangeBottom = 1;
		bGameOver = false;
		
		text.text ="=========================\n" + 
			"Welcome to Number Wizard!\n" +
			"Please take a moment to think of a 'sceret' number, which I will try to ascertain.\n" +
			"the highest numer in the range is " + RangeTop + "\n" +
			"the lowest numer in the range is " + RangeBottom;
		RangeTop ++;
		NW_Guess();
	}
	
	void Update () {
       		if (Input.GetKeyDown(KeyCode.UpArrow)) { 
       			Bu_Up();
     		}
		else if (Input.GetKeyDown(KeyCode.DownArrow)) { 
			Bu_Down();
			}
		else if (Input.GetKeyDown(KeyCode.Space)) { 
			Bu_Match();
			}
	}
	
	public void Bu_Up () { 
		RangeBottom = CurrentGuess;
		Debug.Log ("Up Arrow Button");
		NW_Guess ();
	}
	
	public void Bu_Down () { 
		RangeTop = CurrentGuess;
		Debug.Log ("Down Arrow Button");
		NW_Guess ();
	}
	
	public void Bu_Match () { 
		Debug.Log ("Space Bar Button");
		NW_GameOver ();
	}
	
	
	void NW_Guess () {
		// CurrentGuess = (RangeTop + RangeBottom)/ 2;
		CurrentGuess = Random.Range (RangeBottom, RangeTop);
		
		text.text ="My guess is " + CurrentGuess +".\n" +
			"If your number is higher than my guess tap the Up arrow, if it's lower the Down arrow, and if it's a Match please tap the Space bar.";
	}
	
	void NW_GameOver () {
		if (bGameOver) {
			text.text ="I'll always win! But feel free to play again!";
			StartNWGame();
		}
	}
}

//lecture 17