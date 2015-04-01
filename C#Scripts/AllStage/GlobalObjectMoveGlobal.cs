using UnityEngine;
using System.Collections;

public class GlobalObjectMoveGlobal : MonoBehaviour {

	public GameControllerGlobal controller;

	// how long each stage lasts, in seconds
	public float[] stageTimes;
	// the positions at which the stage transitions to the next
	public Vector3[] transitionPoints;
	// time between transitions
	public float transitionInterval;

	// the current point the stage is moving towards
	private Vector3 currentTransitionPoint;
	private int transitionIndex;

	//speed vars
	private float moveSpeed;
	private Vector3 moveDir;
	private bool isMoving;
	
	// Use this for initialization
	void Start () {

		if (GameObject.FindGameObjectWithTag("GameController") != null)
			controller = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameControllerGlobal>();

		transitionIndex = 0;
		currentTransitionPoint = transitionPoints[0];

		// get the direction to move in by subtracting the vectors
		moveDir = (currentTransitionPoint - transform.position).normalized;
		moveSpeed = (currentTransitionPoint.y - transform.position.y) / controller.stageTime;
		if (moveSpeed == 0) {
			moveSpeed = Mathf.Abs((currentTransitionPoint.z - transform.position.z) / controller.stageTime);
			// Debug.Log (moveSpeed);
		}
		isMoving = true;
	}
	
	// Update is called once per frame
	void Update () {
		if (isMoving) {
			transform.position += moveDir * moveSpeed * Time.deltaTime;

			// this is slightly inaccurate when the stage depends on small increments (lava stage)
			if (Vector3.Distance(transform.position, currentTransitionPoint) < 1f) {
				transform.position = currentTransitionPoint;
				StartCoroutine(Transition());
			}
		}
	}

	// calls the transition function and if there is another transition point, continue moving after the interval
	IEnumerator Transition() {
		isMoving = false;
		controller.Transition(controller.currentStage);

		yield return new WaitForSeconds(transitionInterval);

		if (++transitionIndex < transitionPoints.Length) {
			currentTransitionPoint = transitionPoints[transitionIndex];
			moveDir = (currentTransitionPoint - transform.position).normalized;
			isMoving = true;
		}
	}
}
