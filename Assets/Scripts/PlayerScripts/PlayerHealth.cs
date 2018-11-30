using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour {

	public GameObject fire;
	public GameObject ExplodeSound;
	public GameObject gameOverPanel;

	public const int maxHealth = 3;
	public int currentHealth;
	public Image heart1; 
	public Image heart2;
	public Image heart3;


	void Awake () {
		currentHealth = maxHealth;
	}

	public void TakeDamage(int amount){
		if(!(transform.GetComponent<Player>().currentlyDamaged)){
			currentHealth -= amount;
			if (currentHealth <= 0) {
				currentHealth = 0;
				heart1.enabled = false;
				Instantiate (fire, transform.position, Quaternion.identity);
				Instantiate (ExplodeSound, Vector3.zero, Quaternion.identity);
				gameOverPanel.SetActive (true);
				gameObject.SetActive (false);
			} else if (currentHealth == 1) {
				heart2.enabled = false;
			} else if (currentHealth == 2) {
				heart3.enabled = false;
			} 
		}
	}
}
