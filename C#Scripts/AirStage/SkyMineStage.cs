using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SkyMineStage : MonoBehaviour {

	GameControllerGlobal controller;
	public int minCrystalsSpawned;
	public int maxCrystalsSpawned;
	public float speed;

	private List<GameObject> crystals; // the list of types of crystals (to spawn when "dropping" a gem)


	// Use this for initialization
	void Start () {
		controller = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameControllerGlobal>();
		crystals = controller.crystals;
	}
	
	// Update is called once per frame
	void Update () {
		transform.Translate (Vector3.up * Time.deltaTime * speed,Space.World);
	}
	
	IEnumerator Explode() {
		AudioGlobal audio = GameObject.Find ("_DialogController").GetComponent<AudioGlobal> ();
		audio.Play ("Sky_AirMineExplode");
		transform.FindChild("Explosion").particleSystem.Play();
		this.renderer.enabled = false;
		yield return new WaitForSeconds(1f);
		Destroy(this.gameObject);
	}

	// spawns a random amount of crystals
	// TODO: Add explode particle effect
	void DropCrystals() {
		int spawn = Random.Range(minCrystalsSpawned, maxCrystalsSpawned);
		for (int i = 0; i < spawn; i++) {
			GameObject g = Instantiate(crystals[Random.Range (0, crystals.Count)], transform.position, Quaternion.identity) as GameObject;
			g.transform.parent = GameObject.Find ("-Scene_Air").transform;
			g.SendMessage("Dropped");
		}

	}

	// explodes on contact with player or other obstacle (e.g. bullet)
	void OnTriggerEnter(Collider other) {
		if (other.tag == "Player" || other.tag == "Obstacle") {
			if (other.tag == "Player"){
				other.gameObject.SendMessage("DropCrystal");
			} else if (other.tag == "Obstacle"){
				Destroy(other.gameObject);
			}
			DropCrystals();
			StartCoroutine(Explode());
		}
	}
}
