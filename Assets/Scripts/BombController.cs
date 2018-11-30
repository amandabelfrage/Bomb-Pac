using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombController : MonoBehaviour {

	public GameObject fire;
	public GameObject bombPowerUp;
	public GameObject rangePowerUp;
	public GameObject fakePowerUp;
	private float bombFuse = 3.0f;
	public int firePower;
	private int numberOfPowerUps = 0;

	void Start () {
		Invoke ("Explode", bombFuse);
	}

	public void OnTriggerExit2D(Collider2D coll) {
		GetComponent<BoxCollider2D> ().isTrigger = false; 
	}
	
	void Explode () {
		Instantiate (fire, transform.position, Quaternion.identity);
		createExplosions (Vector2.left);
		createExplosions (Vector2.right);
		createExplosions (Vector2.up);
		createExplosions (Vector2.down);
		Destroy (gameObject); 
	}


	private void createExplosions (Vector2 direction) {
		ContactFilter2D contactFilter = new ContactFilter2D ();
		float radiusCircleCollider = fire.GetComponent<CircleCollider2D> ().radius;
		Vector2 explosionDimentions = new Vector2(radiusCircleCollider, radiusCircleCollider);
		Vector2 explosionPosition = (Vector2)this.gameObject.transform.position + (explosionDimentions.x * direction);

		for (int explosionIndex = 1; explosionIndex <= firePower; explosionIndex++) {
			
			Collider2D[] colliders = new Collider2D[4];
			Physics2D.OverlapBox (explosionPosition, explosionDimentions, 0.0f, contactFilter, colliders);
			bool foundBoxOrWall = false;

			foreach (Collider2D collider in colliders) {
				if(collider){
					
					foundBoxOrWall = (collider.tag == "Box") || (collider.tag == "wall");

					if (collider.tag == "Box") {
						Instantiate (fire, explosionPosition, Quaternion.identity);
						int getRandom = Random.Range (0, 100);
						if (getRandom < 50 && numberOfPowerUps < 20) {
							
							Instantiate (getPowerUpVersion (), explosionPosition, Quaternion.identity);
						}
						collider.gameObject.SetActive (false);

					}if (foundBoxOrWall)
						break;
				}
			}
			if (foundBoxOrWall)
				break;
			
			Instantiate (fire, explosionPosition, Quaternion.identity);
			explosionPosition += (explosionDimentions.x * direction);
		}
	}

	GameObject getPowerUpVersion () {

		int random1 = Random.Range (1, 9);
		GameObject powerUp = null;

		if (random1 <= 3) {
			powerUp = bombPowerUp;
		} else if (random1 <= 6) {
			powerUp = rangePowerUp;
		} else if (random1 <= 9) {
			powerUp = fakePowerUp;
		}
		numberOfPowerUps++;
		return powerUp;

	}
}
