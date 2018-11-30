using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropBomb : MonoBehaviour {

	public GameObject bomb;
	public int numberOfBombs = 1;
	public int firePower = 1;


	
	void Update () {
		if (Input.GetButtonDown ("Jump") && numberOfBombs > 0) {
			Vector2 spawnPos = new Vector2 (Mathf.RoundToInt (transform.position.x), Mathf.RoundToInt (transform.position.y)); 
			var newBomb = Instantiate (bomb, spawnPos, Quaternion.identity) as GameObject; 
			newBomb.GetComponent<BombController>().firePower = firePower;
			numberOfBombs--; 
			Invoke ("AddBomb", 2); 
		}
		
	}

	public void AddBomb(){
		numberOfBombs++;

	}

	public void IncreaseRange () {
		firePower++;
	}
}
