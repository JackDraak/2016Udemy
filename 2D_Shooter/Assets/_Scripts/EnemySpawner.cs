using UnityEngine;
using System.Collections;
public class EnemySpawner : MonoBehaviour {
	public GameObject enemyPrefab;
	void Start () {
		foreach (Transform child in transform) {
			GameObject enemy = Instantiate(enemyPrefab, child.transform.position, Quaternion.identity) as GameObject;
			enemy.transform.parent = child;
		}
	}
	void OnDrawGizmos () {
		Gizmos.DrawWireCube(transform.position, new Vector3 (8,8,1));
	}
}
