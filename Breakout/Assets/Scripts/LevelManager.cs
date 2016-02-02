using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour {
	public void LoadLevel(string LevelName){
	print ("Level Manager : Load Level : " + LevelName );
	Application.LoadLevel(LevelName);
	}
}
