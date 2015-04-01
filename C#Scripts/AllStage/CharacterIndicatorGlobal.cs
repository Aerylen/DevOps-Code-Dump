using UnityEngine;
using System.Collections;

public class CharacterIndicatorGlobal : MonoBehaviour {

	private PlayerMovementMechanic ID;

	// Use this for initialization
	void Start () {
		ID = transform.parent.GetComponent <PlayerMovementMechanic> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (ID.isPlayer == true) {
			GetComponent<TextMesh> ().text = "P" + ID.id.ToString ();
		} else {
			GetComponent<TextMesh> ().text = "CPU";	
		}
	}
}
