using UnityEngine;
using System.Collections;

public class OSIKillMisc : MonoBehaviour {

	// Use this for initialization
	void Start () {
		StartCoroutine (Kill ());
		GameObject.Find ("_HUDController").GetComponent<HUDGlobal>().StartCoroutine("ShowOSI", Application.loadedLevelName);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	IEnumerator Kill(){
		yield return new WaitForSeconds (5f);
		Destroy (this.gameObject);
	}
}
