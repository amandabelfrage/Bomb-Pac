using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyController : MonoBehaviour {

	public float speed = 3.9f;
	public bool currentlyDamaged = false;
	private bool facingRight = true;

	public int scatterModeTimer1 = 7;
	public int chaseModeTimer1 = 20;
	public int frightenedModeTimer = 10;
	public int damagedTimer = 3;
	private float modeChangeTimer = 0;

	private Node currentNode, previousNode, targetNode;

	private Vector2 direction, nextDirection;

	public Transform player;

	public GameObject frightenedModeSource;

	public RuntimeAnimatorController enemyFrightened;
	public RuntimeAnimatorController enemyNormal;
	public RuntimeAnimatorController enemyDamaged;
	public RuntimeAnimatorController opossumDamaged;

	public enum Mode
	{
		Chase,
		Scatter,
		Frightened

	}
		
	Mode currentMode = Mode.Scatter;
	public string runningMode = "Scatter";
	Mode previousMode;

	void Awake() {
		Node node = getNodeAtPosition (transform.localPosition); 
		if (node != null) {
			currentNode = node;
		}
			
		previousNode = currentNode;
		targetNode = getNextNode ();

		UpdateAnimatorController ();

	}
		
		
	void Update () {

		ModeUpdate ();
		move ();

	}
	void OnTriggerEnter2D(Collider2D other){
		if (other.gameObject.tag == "Explosion" && !currentlyDamaged) {
			if (currentMode != Mode.Frightened) {
				transform.GetComponent<Animator> ().runtimeAnimatorController = enemyDamaged;
			} else if (currentMode == Mode.Frightened) {
				transform.GetComponent<Animator> ().runtimeAnimatorController = opossumDamaged;
			}
			currentlyDamaged = true;
			GetComponent<CircleCollider2D> ().isTrigger = true;
			Invoke ("damageController", 3);

		} if (other.gameObject.tag == "Box" || other.gameObject.tag == "Bomb" || other.gameObject.tag == "Enemy") {
			
			GoToNodeClosestToCollision ();

		}
	}


	void OnCollisionEnter2D(Collision2D other){
		
		if (other.gameObject.tag == "Box" || other.gameObject.tag == "Bomb" || other.gameObject.tag == "Enemy") {

			GoToNodeClosestToCollision ();

		} else if (other.gameObject.tag == "Player") {
			if (currentMode != Mode.Frightened) {
				GameObject player = other.gameObject;
				int health = player.GetComponent<PlayerHealth> ().currentHealth;
				var changeHealth = player.GetComponent<PlayerHealth> ();
				if (health > 0) {
					changeHealth.TakeDamage (1);
				}	
			}else if(currentMode == Mode.Frightened && !currentlyDamaged){
				transform.GetComponent<Animator> ().runtimeAnimatorController = opossumDamaged;
				currentlyDamaged = true;
				GetComponent<CircleCollider2D> ().isTrigger = true;
				Invoke ("damageController", 3);

			}
		}

	}


	void GoToNodeClosestToCollision (){
		Vector2 previousNodePos = previousNode.transform.position;

		for (int i = 0; i < targetNode.validDirections.Length; i++) {
			if ((Vector3)previousNodePos == targetNode.neighbours [i].transform.position) {
				direction = targetNode.validDirections [i];
				Node oldTarget = targetNode;
				targetNode = previousNode;
				previousNode = oldTarget;
				currentNode = null;
				break;
			}
		}
		move ();
	}


	void move () {
		if (targetNode != currentNode && targetNode != null) {
			
			if (OverShotTarget ()) {

				currentNode = targetNode;
				transform.localPosition = currentNode.transform.position;
				targetNode = getNextNode ();
				previousNode = currentNode;
				currentNode = null;

			} else {
				transform.localPosition += (Vector3)(direction * speed)* Time.deltaTime; 
			}

		}
	}
	 
	Node getNextNode () {

		Node moveToNode = null;

		if (currentMode == Mode.Chase)
			moveToNode = getNextNodeWhenChasing ();
		
		else if (currentMode == Mode.Scatter) 
			moveToNode = getNextNodeWhenScatter ();
		
		else if (currentMode == Mode.Frightened) 
			moveToNode = getNextNodeWhenFrightened ();

		return moveToNode;
	}

	Node getNextNodeWhenChasing () {

		Node moveToNode = null;
		int dist = int.MaxValue;
		Vector2 playerPos = new Vector2(Mathf.RoundToInt(player.position.x), Mathf.RoundToInt(player.position.y));

		for (int i = 0; i < currentNode.neighbours.Length; i++) {
			float neighbourX = currentNode.neighbours [i].transform.localPosition.x;
			float neighbourY = currentNode.neighbours [i].transform.localPosition.y;

			if ((previousNode != currentNode.neighbours [i]) && (Mathf.Abs (neighbourX - playerPos.x) + Mathf.Abs (neighbourY - playerPos.y) < dist)) {
				dist = Mathf.Abs ((int)neighbourX - (int)playerPos.x) + Mathf.Abs ((int)neighbourY - (int)playerPos.y);
				direction = currentNode.validDirections [i];
				moveToNode = currentNode.neighbours [i];
			}
		}
		flipOnDirection ();
		return moveToNode;
	}

	Node getNextNodeWhenScatter () {

		Node moveToNode = null;
		List<Node> neighbours = new List<Node>();
		List<Vector2> nodeDirections = new List<Vector2>();

		for (int i = 0; i < currentNode.neighbours.Length; i++) {

			if (previousNode != currentNode.neighbours [i]) {
				neighbours.Add (currentNode.neighbours [i]);
				nodeDirections.Add (currentNode.validDirections [i]);
			}

		}
			
		int getRandom = Random.Range (0, (neighbours.Count -1));
		moveToNode = neighbours [getRandom];
		direction = nodeDirections [getRandom];
		flipOnDirection ();
		return moveToNode;
	}

	Node getNextNodeWhenFrightened (){

		int minDist = int.MinValue;
		Node moveToNode = null;
		Vector2 playerPos = new Vector2(Mathf.RoundToInt(player.position.x), Mathf.RoundToInt(player.position.y));

		for (int i = 0; i < currentNode.neighbours.Length; i++) {

			float neighbourX = currentNode.neighbours [i].transform.localPosition.x;
			float neighbourY = currentNode.neighbours [i].transform.localPosition.y;

			if ((previousNode != currentNode.neighbours [i]) && (Mathf.Abs (neighbourX - playerPos.x) + Mathf.Abs (neighbourY - playerPos.y) > minDist)) {
				minDist = Mathf.Abs ((int)neighbourX - (int)playerPos.x) + Mathf.Abs ((int)neighbourY - (int)playerPos.y);
				direction = currentNode.validDirections [i];
				moveToNode = currentNode.neighbours [i];
			}
		}
		flipOnDirection ();
		return moveToNode;
	}

	void flipOnDirection () {
		if (direction.x == 1 && !facingRight) {
			Flip ();
		}
		if (direction.x == -1 && facingRight) {
			Flip ();
		}
	}
	
	void Flip () {
		facingRight = !facingRight;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

	void ModeUpdate () {
		if (!currentlyDamaged) {
			modeChangeTimer += Time.deltaTime;
			if (currentMode == Mode.Scatter && modeChangeTimer > scatterModeTimer1) {
				runningMode = "Chase";
				ChangeMode (Mode.Chase);
				modeChangeTimer = 0;
			}
			if (currentMode == Mode.Chase && modeChangeTimer > chaseModeTimer1) {
				runningMode = "Scatter";
				ChangeMode (Mode.Scatter);
				modeChangeTimer = 0;
			}
			if (currentMode == Mode.Frightened && modeChangeTimer > frightenedModeTimer) {
				if (previousMode == Mode.Chase) {
					runningMode = "Chase";
				} else if (previousMode == Mode.Scatter) {
					runningMode = "Scatter";
				}
				ChangeMode (previousMode);
				modeChangeTimer = 0;
			}
		}
	}

	void UpdateAnimatorController() {
		if (currentMode != Mode.Frightened) {
			transform.GetComponent<Animator> ().runtimeAnimatorController = enemyNormal;
		} else if(currentMode == Mode.Frightened){
			transform.GetComponent<Animator> ().runtimeAnimatorController = enemyFrightened;
		}
	}


	void ChangeMode (Mode mode) {
		if (currentMode != mode) {
			previousMode = currentMode;
			currentMode = mode;
			UpdateAnimatorController ();

		} else if (!currentlyDamaged) {
			UpdateAnimatorController ();
		}
	}

	public void startFrightenedMode () {
		if (!currentlyDamaged) {
			modeChangeTimer = 0;
			if (currentMode != Mode.Frightened) {
				runningMode = "Frightened";
				ChangeMode (Mode.Frightened);
				facingRight = !facingRight;
				Flip ();
				Instantiate (frightenedModeSource, Vector3.zero, Quaternion.identity);
				UpdateAnimatorController ();
			} else if (currentMode == Mode.Frightened) {
				Object[] FrightenedModeSounds = GameObject.FindGameObjectsWithTag ("FrightenedModeSound");
				foreach (GameObject o in FrightenedModeSounds) {
					o.GetComponent<FrightenedModeSound> ().resetTimer ();
				}
			}
		}
	}

	Node getNodeAtPosition (Vector2 pos){
		
		GameObject tile = GameObject.FindGameObjectWithTag("GameBoard").GetComponent<GameBoard>().board [(int)pos.x, (int)pos.y];

		if (tile != null) {
			if (tile.GetComponent<Node>() != null)
				return tile.GetComponent<Node>();
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

	float getDistance (Vector2 posA, Vector2 posB){
		float distx = posA.x - posB.x;
		float disty = posA.y - posB.y;

		float dist = Mathf.Sqrt (distx * distx + disty * disty);

		return dist;
	}

	void damageController (){
		modeChangeTimer = 20;
		currentlyDamaged = false;
		GetComponent<CircleCollider2D> ().isTrigger = false;
	}


}
