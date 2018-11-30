using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour {

	public GameObject fire;
	public GameObject ExplodeSound;

	public const int maxHealth = 3;
	public int currentHealth;

	void Awake () {
		currentHealth = maxHealth;

	}

	public void TakeDamage(int amount){
		if (!(transform.GetComponent<EnemyController> ().currentlyDamaged)) {
			currentHealth -= amount;
			if (currentHealth <= 0) {
				GameObject.FindGameObjectWithTag ("Player").GetComponent<Player> ().increaseNumberOfDead();
				Instantiate (fire, transform.position, Quaternion.identity);
				Instantiate (ExplodeSound, Vector3.zero, Quaternion.identity);
				gameObject.SetActive (false);
			}
		}
	}
}
