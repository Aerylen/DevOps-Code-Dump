using UnityEngine;
using System.Collections;

public class LavaTideStage : MonoBehaviour {

	public int IntervalTime;
	Vector3 startPos;
	Vector3 endPos;
	public float speed;

	// Use this for initialization
	void Start () {
		StartCoroutine (Tide ());
		startPos = transform.position;
		endPos = transform.position + Vector3.up * 1.7f;
		StartCoroutine (Boil ());
	}

	IEnumerator Boil() {
		AudioGlobal audio = GameObject.Find ("_DialogController").GetComponent<AudioGlobal> ();
		audio.Play ("Lava_Boil");
		yield return new WaitForSeconds (Random.Range(5f,7f));
		StartCoroutine (Boil ());
	}
	
	IEnumerator Tide () {
		yield return new WaitForSeconds (IntervalTime); //Wait

		while (Vector3.Distance(transform.position, endPos) > 0.1f) {
			transform.position = Vector3.Lerp(transform.position,
		                                  endPos,
		                                  speed * Time.deltaTime); //Move up
			AudioGlobal audio = GameObject.Find ("_DialogController").GetComponent<AudioGlobal> ();
			audio.Play ("Lava_LavaRise");
			yield return 0;
		}

		yield return new WaitForSeconds (IntervalTime / 2); //Wait

		while(Vector3.Distance(transform.position, startPos) > 0.1f) {
			transform.position = Vector3.Lerp(transform.position,
		                                  startPos,
		                                  speed * Time.deltaTime); //Move Down
			yield return 0;
		}

		StartCoroutine (Tide ()); //Repeat
	}
}
