using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvertedMoveFakePowerUp : MonoBehaviour {

	public GameObject invertedSound;

	void OnTriggerEnter2D(Collider2D other){
		if (other.gameObject.tag == "Player") {
			other.gameObject.GetComponent<Player> ().InvertKeys ();
			Instantiate (invertedSound, Vector3.zero, Quaternion.identity);
			Destroy (gameObject);
			other.gameObject.GetComponent<Player> ().Invoke ("InvertKeys", 10);
		} else if (other.gameObject.tag == "Enemy") {
			Destroy (gameObject);
		}
	}
}
