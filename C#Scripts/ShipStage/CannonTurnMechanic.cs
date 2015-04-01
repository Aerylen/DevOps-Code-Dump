using UnityEngine;
using System.Collections;

public class CannonTurnMechanic : MonoBehaviour {
	
	public int direction;
	
	void Start(){
		StartCoroutine (Turn ());
	}

	void Update () {
		if (direction == 1) {
			transform.Rotate (Vector3.up*0.5f);
		} else {
			transform.Rotate (Vector3.down*0.5f);
		}
	}

	IEnumerator Turn(){
		yield return new WaitForSeconds (3);

		if (direction == 1) {
			direction = 2;		
		} else {
			direction = 1;
		}
		StartCoroutine (Turn ());
	}
}