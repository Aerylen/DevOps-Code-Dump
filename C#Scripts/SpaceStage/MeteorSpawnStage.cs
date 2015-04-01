using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MeteorSpawnStage : MonoBehaviour {
	
	public GameObject objectToSpawn;
	
	// Use this for initialization
	void Start () {
		StartCoroutine (SpawnObject ());
		StartCoroutine (Whoosh ());
	}

	IEnumerator Whoosh() {
		AudioGlobal audio = GameObject.Find ("_DialogController").GetComponent<AudioGlobal> ();
		audio.Play ("Space_MeteorWhoosh");
		yield return new WaitForSeconds (Random.Range(5f,7f));
		StartCoroutine (Whoosh ());
	}
	
	// waits, then spawns a new object
	IEnumerator SpawnObject() {
		
		// here is where the object is being spawned
		Instantiate(objectToSpawn, 
		            new Vector3 (Random.Range(transform.position.x - collider.bounds.size.x / 2, transform.position.x + collider.bounds.size.x / 2), 
		             transform.position.y,
		             Random.Range(transform.position.z - collider.bounds.size.z / 2,transform.position.z + collider.bounds.size.z / 2)),
		            transform.rotation);

		yield return new WaitForSeconds(Random.Range(0.5f, 1f));
		
		// spawns another object, unless isSpawning is false
		StartCoroutine(SpawnObject());
	}
}