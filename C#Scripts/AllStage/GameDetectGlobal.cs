using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameDetectGlobal : MonoBehaviour {


	// Use this for initialization
	void Start () {

		GameControllerGlobal controller = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameControllerGlobal>();

		if (controller.currentStage == GameControllerGlobal.Stage.Air ||
		    controller.currentStage == GameControllerGlobal.Stage.Ship ||
		    controller.currentStage == GameControllerGlobal.Stage.Desert)
			controller.ocean.SetActive(true);

		if (controller.currentStage == GameControllerGlobal.Stage.Air) {
			controller.ocean_Scene.transform.parent = GameObject.Find ("-Scene_Air").transform;
			controller.ocean_Scene.transform.localPosition = new Vector3(0, 840, 0);
		}

		//controller.hud.SetActive(true);
		//controller.hud.GetComponent<HUDGlobal>().InitHUD();
		controller.SetPlayerVals();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
