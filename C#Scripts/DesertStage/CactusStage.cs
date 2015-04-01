using UnityEngine;
using System.Collections;

public class CactusStage : MonoBehaviour {

	public float pushForce;

	//initialize
	void Start() {
		transform.FindChild("Dust Up").GetComponent<ParticleSystem>().Play();
	}
	// Update is called once per frame
	void Update () {

	}

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == "Player") {
			transform.FindChild("CactusAttack").GetComponent<ParticleSystem>().Play();
			AudioGlobal audio = GameObject.Find ("_DialogController").GetComponent<AudioGlobal> ();
			audio.Play ("Desert_Cactus");
			other.GetComponent<AIMechanic>().StartCoroutine("Rerout");
			PlayerMovementMechanic m = other.GetComponent<PlayerMovementMechanic>();
			m.LongStun(-m.moveDir, pushForce, .3f);
		}
	}
}
