using UnityEngine;
using System.Collections;

public class CrystalGlobal : MonoBehaviour {

	public float fallRate; // the rate that the crystal moves to the target
	public float minRotationSpeed; // the minimum speed the crystal rotates
	public float maxRotationSpeed; // the maximum speed the crystal rotates
	public float growRate; // the rate the crystal grows when it is first spawned
	public float popSpeed; // the speed at which the crystal rises when it is dropped
	public float pullSpeed; // the speed at which the crystal falls when it is dropped
	public float pushSpeed; // the speed at which the crystal moves when it is dropped
	private State currentState; // the current behavior state of the crystal
	private GameObject targetObject; // the target gameobject the crystal is moving towards
	private Vector3 target; // the target position the crystal is moving towards
	private Vector3 randomRotation; // the current rotation of the crystals
	[HideInInspector]
	public bool canBeCollected;

	public float radius;
	public float attractionForce;
	Vector3 moveVel;

	public enum State {
		Falling, // in this state the crystal seeks the target
		Attract,
		Seek,

		Drop // in this state, the crystal pops up from it's current position and lands on the ground
	};

	// Use this for initialization
	void Start () {
		canBeCollected = false;
		currentState = State.Falling;
		targetObject = GameObject.FindGameObjectWithTag("Target"); // this scripts requires a gameobject in the scene with the tag "Target" and a collider attached
		target = targetObject.transform.position + new Vector3(Random.Range(-targetObject.collider.bounds.size.x / 2, targetObject.collider.bounds.size.x / 2),
		                                                       0,
		                                                       Random.Range(-targetObject.collider.bounds.size.z / 2, targetObject.collider.bounds.size.z / 2));
		randomRotation = new Vector3(Random.value * Random.Range(minRotationSpeed, maxRotationSpeed),
		                             Random.value * Random.Range(minRotationSpeed, maxRotationSpeed),
		                             Random.value * Random.Range(minRotationSpeed, maxRotationSpeed));
		StartCoroutine(Grow ());
		moveVel = Vector3.zero;
	}
	
	// Update is called once per frame
	void Update () {

		foreach (GameObject g in GameObject.FindGameObjectsWithTag("Player")) {
			if (Vector3.Distance(this.transform.position, g.transform.position) <= radius){ 
				currentState = State.Attract;
				break;
			}
		}
		if (currentState == State.Falling) {
			Seek(target);
		} else if (currentState == State.Attract) {
			Attract();
		} 
		transform.Rotate(randomRotation * Time.deltaTime, Space.World);
	}

	// seeks the target by finding the directional vector and moving along its path
	void Seek(Vector3 target) {
		transform.position += (target - this.transform.position).normalized * fallRate * Time.deltaTime;
	}

	// TODO: Have the crystals wander around. Not important right now. (Travis, 6/3/14)
	void Attract() {
		Vector3 moveAcc = Vector3.zero;

		foreach (Collider c in Physics.OverlapSphere(this.transform.position, radius)) {
			if (c.gameObject.tag == "Player") {
				if (Vector3.Distance(this.transform.position, c.transform.position) < 1) {
					currentState = State.Falling;
					target = c.transform.position;
					break;
				}
				moveAcc += (c.transform.position - this.transform.position).normalized * attractionForce / Vector3.Distance(c.transform.position, this.transform.position);
			}
			
		}
		
		moveVel += moveAcc * Time.deltaTime;
		
		transform.position += moveVel * Time.deltaTime;
	}

	IEnumerator Drop() {
		Vector3 startPos = transform.position - Vector3.one / 50;
		Vector3 moveDir = new Vector3(Random.Range (-1f, 1f), 0, Random.Range (-1f, 1f)).normalized;
		growRate = 1;
		while (transform.position.y > startPos.y) {
			transform.position += Vector3.up * popSpeed * Time.deltaTime;
			transform.position += moveDir * pushSpeed * Time.deltaTime;
			popSpeed -= pullSpeed * Time.deltaTime;
			currentState = State.Drop;
			yield return 0;
		}
		canBeCollected = true;
	}

	public void Dropped() {
		currentState = State.Drop;

		StartCoroutine(Drop ());

	}

	// grows the crystal to give the appearance of coming from a far away distance
	IEnumerator Grow() {
		Vector3 endScale = transform.localScale;
		transform.localScale = Vector3.one * 0.1f;
		while (transform.localScale.x < endScale.x) {
			transform.localScale += Vector3.one * growRate * Time.deltaTime;
			yield return 0;
		}
		transform.localScale = endScale;
		canBeCollected = true;

	}
	
	void OnTriggerEnter(Collider other) {
		if (other.tag == "Target" && currentState != State.Drop) {

			currentState = State.Attract; // once the crystal hits the target, it begins wandering
		}
	}
}
