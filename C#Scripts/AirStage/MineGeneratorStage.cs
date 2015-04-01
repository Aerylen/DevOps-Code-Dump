using UnityEngine;
using System.Collections;

public class MineGeneratorStage : MonoBehaviour {

	public float startHeight;
	public float endHeight;
	public int numIntervals;

	float interval;

	GameControllerGlobal controller;

	// Use this for initialization
	void Start () {

		if (GameObject.Find ("-Ocean")!= null)
		    GameObject.Find ("-Ocean").transform.parent = transform.parent;

		Debug.Log ("Generating Mines");
		controller = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameControllerGlobal>();
		interval = (endHeight - startHeight) / (float)numIntervals;
		GenerateMines();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void GenerateMines() {
		for (int i = 0; i < numIntervals; i++) {
			CreateMinefields(Vector3.up * startHeight + Vector3.up * i * interval);
		}

	}

	void CreateMinefields(Vector3 pos) {
		GameObject g = Instantiate(controller.mines[0], pos + new Vector3(Random.Range(-8f, 8f), 0, Random.Range(-5f, 5f)), Quaternion.identity) as GameObject;
		g.transform.parent = GameObject.Find ("-Scene_Air").transform;
	}
}
