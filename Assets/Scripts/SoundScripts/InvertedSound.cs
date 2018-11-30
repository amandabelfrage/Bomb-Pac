using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvertedSound : MonoBehaviour {

	public AudioClip InvertedMoveSound;
	public AudioSource InvertedMoveSource;

	void Start () {
		InvertedMoveSource.PlayOneShot (InvertedMoveSound);
		Destroy (this.gameObject, 2f);
	}

}
