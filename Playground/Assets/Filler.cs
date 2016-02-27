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

		for (int boxNumber = 0; boxNumber < numberOfBoxes ; boxNumber++) {
			int fill = Random.Range (0, numberOfFillers);
			textBox[boxNumber].text = filler[fill];
		}
	}
}
