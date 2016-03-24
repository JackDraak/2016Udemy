﻿/*
Cave Generation tutorial, part 2
https://www.youtube.com/watch?v=v7yyZZjF1z4&list=WL&index=2
https://youtu.be/yOgIncKp0BE

*/
using UnityEngine;
using System.Collections;
using System;

public class MapGenerator : MonoBehaviour {

	public int height;
	public int width;
	public string seed;
	public bool useRandomSeed;
	[Range(0,100)]
	public int randomFillPercent;

	int [,] map;

	void Start () { GenerateMap(); }

	void Update () { GenerateMap(); }// if (Input.GetMouseButtonDown(0)) GenerateMap(); }

	void GenerateMap () { 
		map = new int[width,height];
		RandomFillMap();
		for (int i = 0; i < 5; i++) {
			SmoothMap();
		}

		MeshGenerator meshGen = GetComponent<MeshGenerator>();
		meshGen.GenerateMesh(map, 1);
	}

	void RandomFillMap () {
		if (useRandomSeed) { seed = Time.time.ToString(); }
		System.Random prng = new System.Random(seed.GetHashCode());
		for (int x = 0; x < width; x++) {
			for (int y = 0; y < height; y++) {
				if (x == 0 || x == width -1 || y == 0 || y == height -1) map[x,y] = 1;
				else map[x,y] = (prng.Next(0,100) < randomFillPercent)? 1: 0;
			}
		}
	}

	void SmoothMap () {
		for (int x = 0; x < width; x++) {
			for (int y = 0; y < height; y++) {
				int neightborWallTiles = GetSurroundingWallCount(x,y);
				if (neightborWallTiles > 4) map[x,y] = 1;
				else if (neightborWallTiles < 4) map[x,y] = 0;
			}
		}
	}

	int GetSurroundingWallCount (int gridX, int gridY) {
		int wallCount = 0;
		for (int neighbourX = gridX -1; neighbourX <= gridX +1; neighbourX++) {
			for (int neighbourY = gridY -1; neighbourY <= gridY +1; neighbourY++) {
				if (neighbourX >= 0 && neighbourX < width && neighbourY >= 0 && neighbourY < height) {
					if (neighbourX != gridX || neighbourY != gridY) {
						wallCount += map[neighbourX, neighbourY];
					}
				} else wallCount++;
			} 
		}
	return wallCount;
	}

	void OnDrawGizmos () {
		/*
		if (map != null) {
			for (int x = 0; x < width; x++) {
				for (int y = 0; y < height; y++) {
					Gizmos.color = (map[x,y] == 1)?Color.black:Color.white;
					Vector3 pos = new Vector3(-width/2 + x + 0.5f, 0, -height/2 + y + 0.5f);
					Gizmos.DrawCube(pos, Vector3.one);
				}
			}
		}
		*/
	}
}