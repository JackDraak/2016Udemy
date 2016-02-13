using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour {

	public GameObject enemyPrefab;


	// Use this for initialization
	void Start () {
		GameObject enemy = Instantiate(enemyPrefab, new Vector3 (1, 1, 0), Quaternion.identity) as GameObject;
		enemy.transform.parent = this.transform;
	}
}
