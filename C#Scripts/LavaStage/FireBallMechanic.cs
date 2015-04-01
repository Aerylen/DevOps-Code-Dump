using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FireBallMechanic : MonoBehaviour {
	
	GameControllerGlobal controller;
	public int minCrystalsSpawned;
	public int maxCrystalsSpawned;

	public GameObject explode;

	private List<GameObject> crystals; // the list of types of crystals (to spawn when "dropping" a gem)
	
	
	// Use this for initialization
	void Start () {
		controller = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameControllerGlobal>();
		crystals = controller.crystals;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void Explode() {
		AudioGlobal audio = GameObject.Find ("_DialogController").GetComponent<AudioGlobal> ();
		audio.Play ("Lava_FireballExplode");
		Instantiate(explode, this.transform.position, Quaternion.identity);
		Destroy(this.gameObject);
	}
	
	void DropCrystals() {
		int spawn = Random.Range(minCrystalsSpawned, maxCrystalsSpawned);
		for (int i = 0; i < spawn; i++){
			GameObject g = Instantiate(crystals[Random.Range (0, crystals.Count)], transform.position, Quaternion.identity) as GameObject;
			g.SendMessage("Dropped");
		}
		
	}
	
	void OnTriggerEnter(Collider other) {
		if (other.name == "Boundary Floor" || other.name == "Platform") {
			DropCrystals();
			Explode();
		}

		if (other.tag == "Player") {
			other.gameObject.SendMessage("DropCrystal");
			DropCrystals();
			Explode();
		}
	}
}