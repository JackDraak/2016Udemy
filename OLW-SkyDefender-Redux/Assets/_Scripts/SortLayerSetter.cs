using UnityEngine;
using System.Collections;
public class SortLayerSetter : MonoBehaviour {
	public string layerSelection;
	void Start () { 
		if (layerSelection != null) gameObject.GetComponent<Renderer>().sortingLayerName = layerSelection;
		else gameObject.GetComponent<Renderer>().sortingLayerName = "BackgroundClouds"; 
	}
}
