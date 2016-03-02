using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Filler : MonoBehaviour {

	public Text[] textBox;
	public string[] filler;

	private int numberOfBoxes;
	private int numberOfFillers;

	void Start () {
		numberOfBoxes = textBox.Length;
		numberOfFillers = filler.Length;

		FillBoard();
	}

	/*
		Note: if this were intended to be a Boggle board you should draw fillers 
		from a struct(?) that gives each latter a score value and leans away from 
		high scoring letters, i.e. Q & Z (letter frequency is inversely proprtional 
		to it's score value; score 1 = most frequent, 10 = least.) 

		ideas: 
			- have a limit on duplicate high letters?
			- have a "score range" that the sum of the board must fall within dependent
				upon skill level? 
	*/
	public void FillBoard () {
		for (int boxNumber = 0; boxNumber < numberOfBoxes ; boxNumber++) {
			int fill = Random.Range (0, numberOfFillers);
			textBox[boxNumber].text = filler[fill];
		}
	}
}
