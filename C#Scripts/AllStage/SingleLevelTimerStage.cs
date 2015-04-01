using UnityEngine;
using System.Collections;

public class SingleLevelTimerStage : MonoBehaviour {

	public float Timer;

	// Use this for initialization
	void Start () {
		Timer = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameControllerGlobal>().stageTime;
	}
	
	// Update is called once per frame
	void Update () {
		StartCoroutine (Countdown ());
	}

	IEnumerator Countdown() {
		yield return new WaitForSeconds(Timer);

		GameObject.FindGameObjectWithTag("GameController").GetComponent<GameControllerGlobal>().EndGame();
	}
}
