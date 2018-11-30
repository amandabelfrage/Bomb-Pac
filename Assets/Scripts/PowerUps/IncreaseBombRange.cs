using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncreaseBombRange : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D other){
		if (other.gameObject.tag == "Player") {
			other.gameObject.GetComponent<DropBomb> ().IncreaseRange ();
			Destroy (gameObject);
		} else if (other.gameObject.tag == "Enemy") {
			other.gameObject.GetComponent<EnemyDropBomb> ().IncreaseRange ();
			Destroy (gameObject);
		}
	}
}
