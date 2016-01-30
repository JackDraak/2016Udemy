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
		
		text.text ="=========================";
		text.text ="Welcome to Number Wizard!";
		text.text ="Please take a moment to think of a 'sceret' number, which I will try to ascertain.";
		text.text ="the highest numer in the range is " + RangeTop;
		text.text ="the lowest numer in the range is " + RangeBottom;
		RangeTop ++;
		NW_Guess();
	}
	
	void Update () {
       		if (Input.GetKeyDown(KeyCode.UpArrow)) { 
       			text.text ="Up Cusror pressed"; 
       			RangeBottom = CurrentGuess;
			NW_Guess ();
     		}
		else if (Input.GetKeyDown(KeyCode.DownArrow)) { 
			text.text ="Down Cusror pressed"; 
			RangeTop = CurrentGuess;
			NW_Guess ();
			}
		else if (Input.GetKeyDown(KeyCode.Space)) { 
			text.text ="Space bar pressed";
			bGameOver = true;
			NW_GameOver();
			}
	}
	
	void NW_Guess () {
		// CurrentGuess = (RangeTop + RangeBottom)/ 2;
		CurrentGuess = Random.Range (RangeBottom, RangeTop);
		
		text.text ="My guess is " + CurrentGuess +".";
		text.text ="If you number is higher than my guess tap the Up arrow, if it's lower the Down arrow, and if it's a Match please tap the Space bar.";
	}
	
	void NW_GameOver () {
		if (bGameOver) {
			text.text ="I'll always win! But feel free to play again!";
			StartNWGame();
		}
	}
}

//lecture 17