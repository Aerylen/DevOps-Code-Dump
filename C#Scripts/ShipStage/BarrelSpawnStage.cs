using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BarrelSpawnStage : MonoBehaviour {

	public GameObject objectToSpawn;
	
	// Use this for initialization
	void Start () {
		objectToSpawn.transform.Rotate (0, 0, 90);
		StartCoroutine (SpawnObject ());
	}
	
	// waits, then spawns a new object
	public IEnumerator SpawnObject() {
		yield return new WaitForSeconds(Random.Range(1f, 5f));
		if (GameObject.FindGameObjectsWithTag("Barrel").Length < 10){
			// here is where the object is being spawned
				GameObject g = Instantiate(objectToSpawn, 
				            new Vector3 (Random.Range(transform.position.x - collider.bounds.size.x / 2, transform.position.x + collider.bounds.size.x / 2), 
				             transform.position.y,
				             Random.Range(transform.position.z - collider.bounds.size.z / 2,transform.position.z + collider.bounds.size.z / 2)),
				            transform.rotation) as GameObject;
			g.transform.Rotate(0, 0, 90);

			g.rigidbody.AddForce(-Vector3.forward * 350);
		}
		
		// spawns another object, unless isSpawning is false
			StartCoroutine(SpawnObject());
	}
}
