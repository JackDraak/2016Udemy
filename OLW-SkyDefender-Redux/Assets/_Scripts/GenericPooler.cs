using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GenericPooler : MonoBehaviour {

	public static GenericPooler current;
	public GameObject pooledObject;
	public int pooledNumber = 3;
	public bool willGrow = true;

	List<GameObject> pooledObjects;

	void Awake () { current = this; }

	void Start () {
		pooledObjects = new List<GameObject>();
		for (int i = 0; i < pooledNumber; i++) {
			GameObject obj = (GameObject)Instantiate(pooledObject);
			obj.SetActive(false);
			pooledObjects.Add(obj);
		}
	}

	public GameObject GetPooledObject () {
		for (int i = 0; i < pooledObjects.Count; i++) {
			if (!pooledObjects[i].activeInHierarchy) {
				return pooledObjects[i];
			}
		} if (willGrow) {
			GameObject obj = (GameObject)Instantiate(pooledObject);
			obj.SetActive(false);
			pooledObjects.Add(obj);
			return obj;
		} else {
			Debug.Log ("pool shortage");
			return null;
		}
	}
}
