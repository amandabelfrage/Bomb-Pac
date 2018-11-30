using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrightenedModeSound : MonoBehaviour {

	public AudioClip frightenedModeSound;
	public AudioSource frightenedModeSource;
	private float playingMusicTimer = 0;

	void Start () {
		frightenedModeSource.clip = frightenedModeSound;
		frightenedModeSource.Play ();
	}

	void Update () {
		playingMusicTimer += Time.deltaTime;
		if (playingMusicTimer >= 10)
			Destroy (this.gameObject);

		else if (playingMusicTimer > 7) {
			frightenedModeSource.volume -= 0.005f;
		}

	}

	public void resetTimer () {
		playingMusicTimer = 0;
	}
		
}
