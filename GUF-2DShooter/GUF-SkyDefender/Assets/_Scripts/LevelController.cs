using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LevelController : MonoBehaviour {

	static LevelController instance = null;
	public static int integerValue;

	void Awake () {
		if (instance != null && instance != this) { Destroy (gameObject); }
		else {
			instance = this;
			GameObject.DontDestroyOnLoad(gameObject);
		}
	}

	public void LoadLevel(string name){
	ConfigureAnyLevel();
	if (name == "Game") ConfigureGame (); 
	SceneManager.LoadScene(name);
	}
	
	void LoadNextLevel() {
		ConfigureAnyLevel();
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex +1);
	}

	public int GetIV () { return integerValue; }
	public void SetIV (int i) { integerValue = i; }
	public void UpIV () { integerValue++; }
	public void DownIV () { integerValue--; }
	public void ChangeIV (int delta) { integerValue += delta; }

	void ConfigureAnyLevel () { Cursor.visible = true; }
	void ConfigureGame () {	} //Cursor.visible = false;	}
}
