using UnityEngine;
using System.Collections;

public class Position : MonoBehaviour {

//	public GameObject enemyPrefab;

//	void Start () {
//		SpawnEnemy();
//	}

//	void SpawnEnemy () {
//		GameObject enemy = Instantiate(enemyPrefab, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
//		enemy.transform.parent = transform;
//	}

	void OnDrawGizmos () {
		Gizmos.DrawWireSphere(transform.position, 1);
	}
}
