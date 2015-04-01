using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CactusSpawnerStage : MonoBehaviour {

	private GameControllerGlobal controller;
	private List<GameObject> cacti;
	private List<GameObject> digSites;

	bool spawning;
	public float spawnTimer;
	public float numResets;
	public int spawnMin;
	public int spawnMax;
	public int minCactiPerDigsite;
	public int maxCactiPerDigsite;

	public ParticleSystem earthquake;
	
	// Use this for initialization
	void Start () {
		controller = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameControllerGlobal>();
		cacti = controller.cacti;
		digSites = controller.digSites;
		spawning = false;
		earthquake.Stop();

		if (controller.stageTime == 60)
			numResets = 7;
		else if (controller.stageTime == 90)
			numResets = 9;
		else if (controller.stageTime == 120)
			numResets = 12;
	}
	
	// Update is called once per frame
	void Update () {
		if (!spawning && GameObject.FindGameObjectsWithTag ("DigSite").Length == 0) {
			StartCoroutine (SpawnLevel ());
		}
	}

	IEnumerator SpawnLevel() {
		GameObject[] currentCacti = GameObject.FindGameObjectsWithTag ("Obstacle");
		spawning = true;
		for (int i = currentCacti.Length; i > 0 ; i--){
			Destroy (currentCacti [i-1]);
		}
		if (numResets > 0) {
			yield return new WaitForSeconds(spawnTimer);
			List<int> zones = new List<int>{0, 1, 2, 3, 4, 5, 6, 7, 8};
			int numToSpawn = Random.Range (spawnMin, spawnMax + 1);
			int zone = zones[Random.Range(0, zones.Count)];
			zones.Remove(zone);
			for (int i = 0; i < 9; i++) {

				if (zones.Count > 0 && zone == i) {
					SpawnDesertObstacles(zone, Random.Range(minCactiPerDigsite, maxCactiPerDigsite + 1), true);
					zone = zones[Random.Range(0, zones.Count)];
					zones.Remove(zone);
				} else {
					SpawnDesertObstacles(i, Random.Range(minCactiPerDigsite, maxCactiPerDigsite + 1), false);
				}
				if (zones.Contains(i))
					zones.Remove(i);
			}
		} else if (numResets == 0) {
			Instantiate (digSites [0], transform.position, Quaternion.Euler(-90,0,0));
			earthquake.Play();
			Camera.main.animation.Play("EartquakeCam");
		} else {
			GameObject.FindGameObjectWithTag("GameController").GetComponent<GameControllerGlobal>().Transition(controller.currentStage);
		}
		numResets--;
		spawning = false;
	}

	// -10  + 10
	// -25 + 8
	void SpawnDesertObstacles(int zone, int numCacti, bool spawnDigsite) {
		Vector3 pos = new Vector3(-10 + (zone % 3) * 10, 16.3f, -25 - (zone / 3) * 8);
		if (spawnDigsite)
			Instantiate(digSites[0], pos, Quaternion.Euler(-90,0,0));

		for(int i = 0; i < numCacti; i++) {
			Instantiate(cacti[0], pos + new Vector3(Random.Range(-3f, 3f), -1f, Random.Range(-3f, 3f)).normalized * 3 + Vector3.up * 0.6f, Quaternion.Euler(0, Random.value * 360, 0));
		}
	}

}
