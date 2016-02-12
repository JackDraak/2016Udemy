using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour {

	public GameObject enemyPrefab;


	// Use this for initialization
	void Start () {
		GameObject enemy = Instantiate(enemyPrefab, new Vector3 (-5, -5, 0), Quaternion.identity) as GameObject;
		enemy.transform.position = this.transform.position;
	}
}
