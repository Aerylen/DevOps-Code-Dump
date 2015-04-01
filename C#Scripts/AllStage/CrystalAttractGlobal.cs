using UnityEngine;
using System.Collections;

public class CrystalAttractGlobal : MonoBehaviour {

	public float radius;
	Vector3 moveDir;
	Vector3 moveVel;
	Vector3 moveAcc;
	public float attractionForce;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		moveAcc = Vector3.zero;
		foreach (Collider c in Physics.OverlapSphere(this.transform.position, radius)) {
			if (c.gameObject.tag == "Player")
				moveAcc += (c.transform.position - this.transform.position).normalized * attractionForce;

		}

		moveVel += moveAcc * Time.deltaTime;

		transform.position += moveVel * Time.deltaTime;
	}

}
