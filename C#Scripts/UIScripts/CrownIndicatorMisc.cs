using UnityEngine;
using System.Collections;

public class CrownIndicatorMisc : MonoBehaviour {

	public Transform[] playerIndicators;
	public int[] scores;
	public int winningPlayer;
	GameControllerGlobal controller;


	// Use this for initialization
	void Start () {
		controller = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameControllerGlobal>();
	}
	
	// Update is called once per frame
	void Update () {
		compareScores();

		for (int i = 0; i < playerIndicators.Length; i++) {
			scores[i] = playerIndicators[i].parent.parent.GetComponent<CrystalCollectGlobal>().score;
			switch (i) {
			case 0:
				scores[i] = controller.cwindyScore;
				break;
			case 1:
				scores[i] = controller.eegorScore;
				break;
			case 2:
				scores[i] = controller.makScore;
				break;
			case 3:
				scores[i] = controller.scratchScore;
				break;
			}
		}

		if (scores [0] != 0 || scores [1] != 0 || scores [2] != 0 || scores [3] != 0){
			transform.position = Vector3.Lerp (transform.position,playerIndicators[winningPlayer].position,Time.deltaTime*15f);
		}
	}

	void compareScores(){
		for(int i = 0; i < scores.Length; i++) {
			if (scores[i] >= scores[winningPlayer]) {
				winningPlayer = i;
			}
		}
	}
}