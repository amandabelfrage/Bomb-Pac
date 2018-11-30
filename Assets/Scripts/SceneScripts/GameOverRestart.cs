using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameOverRestart : MonoBehaviour {

	public AudioClip GameOverSound;
	public AudioSource GameOverSource;

	void Start (){
		GameOverSource.clip = GameOverSound;
		GameOverSource.Play ();
	}

	void Update () {
		if (Input.GetKey(KeyCode.Space)) {
			GameOverSource.Stop ();
			SceneManager.LoadScene ("Level1");
		}

	}
}
