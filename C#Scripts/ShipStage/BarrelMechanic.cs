using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BarrelMechanic : MonoBehaviour {

	//GameControllerGlobal controller;
	public int minCrystalsSpawned;
	public int maxCrystalsSpawned;
	
	private List<GameObject> crystals; // the list of types of crystals (to spawn when "dropping" a gem)
	private GameControllerGlobal controller;

	public bool canBeDestroyed;
	
	// Use this for initialization
	void Start () {
		controller = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameControllerGlobal>();
		crystals = controller.crystals;
		//Physics.IgnoreCollision(this.collider, GameObject.FindGameObjectWithTag("Respawn").collider);
		StartCoroutine (Expiration ());
		Physics.IgnoreCollision(this.collider, GameObject.FindGameObjectWithTag("Boundary").transform.FindChild("Top").collider);
		canBeDestroyed = false;
		StartCoroutine(SpawnInvince());
	}

	IEnumerator SpawnInvince() {
		yield return new WaitForSeconds(2);
		canBeDestroyed = true;
		Physics.IgnoreCollision(this.collider, GameObject.FindGameObjectWithTag("Boundary").transform.FindChild("Top").collider, false);

	}

	// Update is called once per frame
	void Update () {
		
	}
	
	IEnumerator Explode() {
		AudioGlobal audio = GameObject.Find ("_DialogController").GetComponent<AudioGlobal> ();
		audio.Play ("Ship_BarrelExplode");
		renderer.enabled = false;
		Destroy (rigidbody);
		transform.eulerAngles = new Vector3(0, 0, 0);
		collider.enabled = false;
		transform.FindChild("BarrelBreak").GetComponent<ParticleSystem>().Play();

		yield return new WaitForSeconds(2);
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
		if (other.tag == "Obstacle" && canBeDestroyed) {
			DropCrystals();
			StartCoroutine(Explode());
		}
	}

	IEnumerator Expiration(){
		yield return new WaitForSeconds (20);
		StartCoroutine(Explode());
	}
}
