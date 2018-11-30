using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionController : MonoBehaviour {

	public AudioClip explosionSound;
	public AudioSource explosionSource;

	void Start () {
		explosionSource.clip = explosionSound;
		explosionSource.PlayOneShot (explosionSound);
		Destroy (gameObject, 0.8f);
	}
				
	
	public void OnTriggerEnter2D(Collider2D coll){
		
		if (coll.gameObject.tag == "Player") {
			GameObject player = coll.gameObject;
			int health = player.GetComponent<PlayerHealth> ().currentHealth;
			var changeHealth = player.GetComponent<PlayerHealth> ();
			if (health > 0) {
				changeHealth.TakeDamage(1);
			}

		}if (coll.gameObject.tag == "Enemy") {
			GameObject enemy = coll.gameObject;
			int eHealth = enemy.GetComponent<EnemyHealth> ().currentHealth;
			var changeEHealth = enemy.GetComponent<EnemyHealth> ();
			if (eHealth > 0) {
				changeEHealth.TakeDamage (1);
			}
		}
	}
		

}
