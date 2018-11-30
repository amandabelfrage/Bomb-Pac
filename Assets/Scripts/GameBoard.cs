using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBoard : MonoBehaviour {

	private static int boardWidht = 28;
	private static int boardHeight = 36;

	public GameObject[,] board = new GameObject[boardWidht,boardHeight];

	void Awake () {
		Object[] objects = GameObject.FindGameObjectsWithTag ("Node"); 

		foreach (GameObject o in objects) {

			Vector2 pos = o.transform.position;
			board [(int)pos.x, (int)pos.y] = o;

		}

	}

}
