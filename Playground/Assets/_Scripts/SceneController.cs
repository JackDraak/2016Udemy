using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour {

	//public Button scene1, scene2, scene3;

	public void Scene1Button () { SceneManager.LoadScene("scene1"); }
	public void Scene2Button () { SceneManager.LoadScene("scene2"); }	
	public void Scene3Button () { SceneManager.LoadScene("scene3"); }	
}
