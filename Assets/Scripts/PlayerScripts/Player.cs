using System;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Player : MonoBehaviour {

	public static int score;
	public Text scoreText;
	public float speed = 8f;
	public bool currentlyDamaged = false;
	private bool invertedMove = false;
	public int numberOfDeadEmenies = 0;

	private Node currentNode, previousNode, targetNode;
	private Vector2 direction, nextDirection;


	public RuntimeAnimatorController moveUp;
	public RuntimeAnimatorController moveDown;
	public RuntimeAnimatorController moveLeft;
	public RuntimeAnimatorController moveRight;
	public RuntimeAnimatorController sleeping;
	public RuntimeAnimatorController damaged;


	void Start () {
		Node node = getNodeAtPosition (transform.localPosition);
		if (node != null) {
			currentNode = node;
		}
			
		score = 0;
		scoreText.text = "Score: " + score.ToString () + " p";
	}
	
	void Update () {

		getInputFromPlayer ();
		Move ();
		UpdateAnimatorController ();
		if (numberOfDeadEmenies >= 3) {
			Invoke ("LoadVictoryScene", 2);
		}

	}

	void LoadVictoryScene () {
		SceneManager.LoadScene ("VictoryScene");
	}

	void getInputFromPlayer () {
		if (!invertedMove) {
			if (Input.GetKeyDown (KeyCode.RightArrow))
				changePosition (Vector2.right);

			else if (Input.GetKeyDown (KeyCode.LeftArrow))
				changePosition (Vector2.left);

			else if (Input.GetKeyDown (KeyCode.UpArrow))
				changePosition (Vector2.up);

			else if (Input.GetKeyDown (KeyCode.DownArrow))
				changePosition (Vector2.down);

		} else if (invertedMove) {
			if (Input.GetKeyDown (KeyCode.RightArrow))
				changePosition (Vector2.left);

			else if (Input.GetKeyDown (KeyCode.LeftArrow))
				changePosition (Vector2.right);

			else if (Input.GetKeyDown (KeyCode.UpArrow))
				changePosition (Vector2.down);

			else if (Input.GetKeyDown (KeyCode.DownArrow))
				changePosition (Vector2.up);
		}
	}

	public void InvertKeys () {
		invertedMove = !invertedMove;
	}

	void UpdateAnimatorController () {

		if (!currentlyDamaged) {

			if (direction == Vector2.left)
				transform.GetComponent<Animator> ().runtimeAnimatorController = moveLeft;
			else if (direction == Vector2.right)
				transform.GetComponent<Animator> ().runtimeAnimatorController = moveRight;
			else if (direction == Vector2.up)
				transform.GetComponent<Animator> ().runtimeAnimatorController = moveUp;
			else if (direction == Vector2.down)
				transform.GetComponent<Animator> ().runtimeAnimatorController = moveDown;
			else if (direction == Vector2.zero)
				transform.GetComponent<Animator> ().runtimeAnimatorController = sleeping;
			
		}
	}

	void changePosition (Vector2 d) {
		if (d != direction)
			nextDirection = d;

		if (currentNode != null) {
			Node moveTo = canMove (d);

			if (moveTo != null) {
				direction = d;
				targetNode = moveTo;
				previousNode = currentNode;
				currentNode = null;
			}
		} else if (currentNode == null) {

			//Change direction when in between nodes
			Vector2 previousNodePos = previousNode.transform.position;
			for (int i = 0; i < targetNode.validDirections.Length; i++) {
				if ((Vector3)previousNodePos == targetNode.neighbours [i].transform.position) {
					if (d == targetNode.validDirections [i]) {
						direction = d;
						Node temp = targetNode;
						targetNode = previousNode;
						previousNode = temp;
						currentNode = null;
					}
				}
			}
		}
	}

	void moveToNode (Vector2 d){
		
		Node moveTo = canMove (d);

		if (moveTo != null) {
			transform.localPosition = moveTo.transform.position;
			currentNode = moveTo;
		}
	}

	void Move () {
		if(targetNode != currentNode && targetNode != null){
			if (OverShotTarget ()) {
				
				currentNode = targetNode;

				transform.localPosition = currentNode.transform.position;

				Node moveTo = canMove (nextDirection);

				if (moveTo != null) {
					direction = nextDirection;
				}if (moveTo == null) {
					moveTo = canMove (direction);
				}if (moveTo != null) {
					targetNode = moveTo;
					previousNode = currentNode;
					currentNode = null;
				} else {
					direction = Vector2.zero;
				}
			}else
				transform.localPosition += (Vector3)(direction * speed) * Time.deltaTime;
		}
	}

	Node canMove (Vector2 d){
		Node moveTo = null;

		for (int i = 0; i < currentNode.neighbours.Length; i++) {
			if (currentNode.validDirections [i] == d) {
				moveTo = currentNode.neighbours [i];
				break;
			}
		}
		return moveTo;
	}

	Node getNodeAtPosition (Vector2 position) {
		GameObject tile = GameObject.FindGameObjectWithTag ("GameBoard").GetComponent<GameBoard> ().board [(int)position.x, (int)position.y];
		if (tile != null) {
			return tile.GetComponent<Node> ();
		}
		return null;
	}

	bool OverShotTarget () {
		float nodeToTarget = lengthFromNode (targetNode.transform.position);
		float nodeToSelf = lengthFromNode (transform.localPosition);

		return nodeToSelf > nodeToTarget;
	}

	float lengthFromNode (Vector2 targetPosition){
		Vector2 vec = targetPosition - (Vector2)previousNode.transform.position;
		return vec.sqrMagnitude;

	}

	void OnTriggerEnter2D(Collider2D other){
		
		if (other.gameObject.tag == "Pacdot") {
			other.gameObject.SetActive (false);
			score++;
			scoreText.text = "Score: " + score.ToString () + " p";
		} 

		else if (other.gameObject.tag == "Fish") {
			other.gameObject.SetActive (false);
			score += 2;
			scoreText.text = "Score: " + score.ToString () + " p";

			GameObject[] enemies = GameObject.FindGameObjectsWithTag ("Enemy");

			foreach (GameObject enemy in enemies) {
				enemy.GetComponent<EnemyController> ().startFrightenedMode ();
			}
		} else if (other.gameObject.tag == "Box") {
			direction = Vector2.zero;
		}

		if (other.gameObject.tag == "Explosion") {
			transform.GetComponent<Animator> ().runtimeAnimatorController = damaged;
			currentlyDamaged = true;
			GetComponent<CircleCollider2D> ().isTrigger = true;
			Invoke ("isDamaged", 3);
		}


	}

	void OnCollisionEnter2D(Collision2D coll){
		if (coll.gameObject.tag == "Box") {
			direction = Vector2.zero;
		}else if (coll.gameObject.tag == "Enemy") {

			GameObject enemy = coll.gameObject;

			if (enemy.GetComponent<EnemyController> ().runningMode == "Frightened") {
				int eHealth = enemy.GetComponent<EnemyHealth> ().currentHealth;
				var changeEHealth = enemy.GetComponent<EnemyHealth> ();
				if (eHealth > 0) {
					changeEHealth.TakeDamage (1);
					score += 10;
					scoreText.text = "Score: " + score.ToString () + " p";
				}
			} else {
				transform.GetComponent<Animator> ().runtimeAnimatorController = damaged;
				currentlyDamaged = true;
				GetComponent<CircleCollider2D> ().isTrigger = true;
				Invoke ("isDamaged", 3);
			}
		}
	}

	void isDamaged () {
		currentlyDamaged = false;
		GetComponent<CircleCollider2D> ().isTrigger = false;
	}

	public void increaseNumberOfDead () {
		numberOfDeadEmenies++;
	}

}
