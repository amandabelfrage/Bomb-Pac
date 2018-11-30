using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDropBomb : MonoBehaviour {

	public GameObject bomb;
	public int firePower = 1;
	public int numberOfBombs = 1;


	void OnTriggerEnter2D(Collider2D other){
		if (other.gameObject.tag == "Box") {
			if (numberOfBombs > 0)
				SpawnBomb ();
		}
	}

	void OnCollisionEnter2D(Collision2D other){
		if (other.gameObject.tag == "Box") {
			if (numberOfBombs > 0)
				SpawnBomb ();
		}
	}


	void SpawnBomb(){
		Vector2 spawnPos = new Vector2 (Mathf.Round (transform.position.x), Mathf.Round (transform.position.y));
		var newBomb = Instantiate (bomb, spawnPos, Quaternion.identity) as GameObject;
		newBomb.GetComponent<BombController> ().firePower = firePower;
		numberOfBombs--; 
		Invoke ("AddBomb", 3); 
	}

	public void AddBomb(){
		numberOfBombs++;
	}

	public void IncreaseRange () {
		firePower++;
	}
}
