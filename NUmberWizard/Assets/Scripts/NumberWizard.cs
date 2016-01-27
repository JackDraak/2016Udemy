using UnityEngine;
using System.Collections;

public class NumberWizard : MonoBehaviour {
	int RangeTop;
	int RangeBottom;
	bool bGameOver;
	int CurrentGuess;

	void Start () {
        	StartNWGame ();
    	}

	void StartNWGame ()
	{
		RangeTop = 1000;
		RangeBottom = 1;
		bGameOver = false;
		
		print ("=========================");
		print ("Welcome to Number Wizard!");
		print ("Please take a moment to think of a 'sceret' number, which I will try to ascertain.");
		print ("the highest numer in the range is " + RangeTop);
		print ("the lowest numer in the range is " + RangeBottom);
		RangeTop ++;
		NW_Guess();
	}
	
	void Update () {
       		if (Input.GetKeyDown(KeyCode.UpArrow)) { 
       			print ("Up Cusror pressed"); 
       			RangeBottom = CurrentGuess;
			NW_Guess ();
     		}
		else if (Input.GetKeyDown(KeyCode.DownArrow)) { 
			print ("Down Cusror pressed"); 
			RangeTop = CurrentGuess;
			NW_Guess ();
			}
		else if (Input.GetKeyDown(KeyCode.Space)) { 
			print ("Space bar pressed");
			bGameOver = true;
			NW_GameOver();
			}
	}
	
	void NW_Guess () {
		// CurrentGuess = (RangeTop + RangeBottom)/ 2;
		CurrentGuess = Random.Range (RangeBottom, RangeTop);
		
		print ("My guess is " + CurrentGuess +".");
		print ("If you number is higher than my guess tap the Up arrow, if it's lower the Down arrow, and if it's a Match please tap the Space bar.");
	}
	
	void NW_GameOver () {
		if (bGameOver) {
			print ("I'll always win! But feel free to play again!");
			StartNWGame();
		}
	}
}

//lecture 17