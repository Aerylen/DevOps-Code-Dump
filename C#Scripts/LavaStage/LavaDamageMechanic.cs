﻿using UnityEngine;
using System.Collections;

public class LavaDamageMechanic : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter (Collider other){
		if (other.tag == "Player") {
			other.GetComponent<ShieldMechanic>().inDangerZone = true;
		}
	}

	void OnTriggerExit(Collider other) {
		if (other.tag == "Player") {
			other.GetComponent<ShieldMechanic>().inDangerZone = false;
		}
	}
}
