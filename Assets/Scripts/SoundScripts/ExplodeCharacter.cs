using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodeCharacter : MonoBehaviour {

	public AudioClip explodePlayerSound;
	public AudioSource explodePlayerSource;

	void Start () {
		explodePlayerSource.PlayOneShot (explodePlayerSound);
		Destroy (this.gameObject, 3);
	}
	

}
