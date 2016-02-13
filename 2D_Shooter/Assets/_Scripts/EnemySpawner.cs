using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour {

	public GameObject enemyPrefab;
//	public GameObject positionPrefab;

	void Start () {
//		SpawnPosition (-1f, -1f);
//		SpawnPosition (-1f, 1f);
//		SpawnPosition (1f, -1f);
//		SpawnPosition (1f, 1f);
		foreach (Transform child in transform) {
			GameObject enemy = Instantiate(enemyPrefab, child.transform.position, Quaternion.identity) as GameObject;
			enemy.transform.parent = child;
		}
	}

	void OnDrawGizmos () {
		Gizmos.DrawWireCube(transform.position, new Vector3 (4,4,4));
	}

//	void SpawnEnemy () {
//		GameObject enemy = Instantiate(enemyPrefab, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
//		enemy.transform.parent = transform;
//	}

//	void SpawnPosition (float PosX, float PosY) {
//		GameObject position = Instantiate(positionPrefab, new Vector3(PosX, PosY, 0), Quaternion.identity) as GameObject;
//		position.transform.parent = transform;
//	}

}
