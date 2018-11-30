using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour {

	public Node[] neighbours;
	public Vector2[] validDirections;

	void Awake () {
	
		validDirections = new Vector2[neighbours.Length];  

		//For every neighbour, create a node. 
		for (int i = 0; i < neighbours.Length; i++) {
			Node neighbour = neighbours [i];
			Vector2 temp = neighbour.transform.localPosition - transform.localPosition;
			validDirections [i] = temp.normalized; 			
		}
	}

}
