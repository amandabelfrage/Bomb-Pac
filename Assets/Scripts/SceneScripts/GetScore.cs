using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetScore : MonoBehaviour {

	public Text scoreText;

	void Start () {
		int score1 = Player.score;
		scoreText.text = "Score: " + score1.ToString () + " p";

	}

}
