using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DigSiteStage : MonoBehaviour {

	public int durability; //How much does it take for the digsite to yield gems
	public List<GameObject> playersInSite;

	GameControllerGlobal controller;
	public int minCrystalsSpawned;
	public int maxCrystalsSpawned;
	
	private List<GameObject> crystals; // the list of types of crystals (to spawn when "dropping" a gem)

	public GameObject worm;

	void Start() {
		controller = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameControllerGlobal>();
		crystals = controller.crystals;
		playersInSite = new List<GameObject>();
	}

	// Update is called once per frame
	void Update () {

		// note: this needs a cooldown.
		// also, it would be more efficient to have this in a new player script... (Travis, 6/13/14)
		foreach (GameObject g in playersInSite) {
			if (Input.GetKeyDown(g.GetComponent<PlayerMovementMechanic>().actionInput)) {
				AudioGlobal audio = GameObject.Find ("_DialogController").GetComponent<AudioGlobal> ();
				audio.Play ("Desert_Dig");
			    durability--;
				DropCrystals();
			}
			if (g.GetComponent<AIMechanic>().canDig) {


				if (g.GetComponent<AIMechanic>().isDigging) {
					durability--;
					DropCrystals();
				}

				g.GetComponent<AIMechanic>().ActionDig();
			}
		}

		//If the durability decreases, then spawn gems and destroy the dig site
		if (durability <= 0) {
			if (Random.value > 0.5f)
				DropCrystals();
			else {
				DropCrystals();
			}
			foreach (GameObject g in GameObject.FindGameObjectsWithTag("Player")) {
				if (g.GetComponent<AIMechanic>() != null)
					g.GetComponent<AIMechanic>().inDigZone = false;
			}
			Destroy (this.gameObject);
		}
	}

	void DropCrystals() {
		AudioGlobal audio = GameObject.Find ("_DialogController").GetComponent<AudioGlobal> ();
		audio.Play ("Desert_CrystalGeneration");
		int spawn = Random.Range(minCrystalsSpawned, maxCrystalsSpawned);
		for (int i = 0; i < spawn; i++){
			GameObject g = Instantiate(crystals[Random.Range (0, crystals.Count)], transform.position, Quaternion.identity) as GameObject;
			g.SendMessage("Dropped");
		}
		
	}

	void SpawnWorm(){
		Instantiate(worm, transform.position, Quaternion.identity);
	}

	//Checks whether the player is on the digsite
	void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == "Player") {
			playersInSite.Add(other.gameObject);	
			if (other.GetComponent<AIMechanic>() != null) {
				other.GetComponent<AIMechanic>().inDigZone = true;
			}
		}
	}
	//Checks whether the player is off the digsite
	void OnTriggerExit(Collider other){
		if (other.gameObject.tag == "Player") {
			playersInSite.Remove(other.gameObject);
			if (other.GetComponent<AIMechanic>() != null) {
				other.GetComponent<AIMechanic>().inDigZone = false;
			}
		}
	}
}