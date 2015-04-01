using UnityEngine;
using System.Collections;

public class MeteorStage : MonoBehaviour {

	public float minSpeed;
	public float maxSpeed;
	public float moveSpeed;
	public int lifeSpan;
	public float rotationSpeed;
	public float growRate;
	
	// Use this for initialization
	void Start () {
		moveSpeed = Random.Range(minSpeed, maxSpeed);
		rotationSpeed = Random.Range (1f,2f);
		StartCoroutine (Death ());
	}

	// Update is called once per frame
	void Update () {
		// TODO: Meteors need to rotate, to separate the movement from the rotation! (e.g. don't use vector3.down)
		transform.Translate(Vector3.up * moveSpeed * Time.deltaTime, Space.World);
		transform.Rotate (rotationSpeed,rotationSpeed,rotationSpeed);
	}
	
	IEnumerator Death() {
		yield return new WaitForSeconds (lifeSpan);
		Destroy (this.gameObject);
	}

	IEnumerator Grow() {
		Vector3 endScale = transform.localScale;
		transform.localScale = Vector3.one * 0.1f;
		while (transform.localScale.x < endScale.x) {
			transform.localScale += Vector3.one * growRate * Time.deltaTime;
			yield return 0;
		}
		transform.localScale = endScale;
		
	}

	void Explode() {
		AudioGlobal audio = GameObject.Find ("_DialogController").GetComponent<AudioGlobal> ();
		audio.Play ("Space_MeteorCollide");
		Destroy(this.gameObject);
	}

	void OnTriggerEnter(Collider other) {
		if (other.tag == "Player") {
			other.SendMessage("DropCrystal");
			Explode();
		}
	}
}
