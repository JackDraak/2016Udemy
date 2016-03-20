using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SkyDefender : MonoBehaviour {
	public void LoadSkyDefender () { SceneManager.LoadScene("SkyDefender"); }
	public void Quit () { Application.Quit(); }
}
