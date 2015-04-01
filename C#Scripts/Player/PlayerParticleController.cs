using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerParticleController : MonoBehaviour {

	float lazerTimer;

	public List<ParticleSystem> sparks;
	public List<ParticleSystem> lazers;

	public ParticleSystem lightning;

	// Use this for initialization
	void Start () {
		lazerTimer = 0;

	}
	
	// Update is called once per frame
	void Update () {
		if (lazers.Count > 0) {
			if (lazerTimer <= 0)
				foreach (ParticleSystem p in lazers)
					p.Stop();
			else
				lazerTimer -= Time.deltaTime;
		}
	
	}

	public void PlaySparks() {
		foreach (ParticleSystem p in sparks) {
			p.Play();
		}
	}

	public void PlayLightning() {
		lightning.Play();
	}

	public void StopLightning() {
		lightning.Stop();
	}

	public void PlayLazers() {
		foreach (ParticleSystem p in lazers)
			if (!p.isPlaying)
				p.Play();
		lazerTimer = 1;
	}

}
