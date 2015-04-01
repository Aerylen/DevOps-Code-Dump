using UnityEngine;
using System.Collections;

public class BulletMechanic : MonoBehaviour {

	public GameObject shooter;
	private GameObject bulletSpawnPoint;
	private float bulletSpeed;

	// Initialization
	void Start () {
		StartCoroutine(Death());
	}

	public void SetBulletSpeed(float val) {
		bulletSpeed = val;
	}

	// Update
	void Update () {
		transform.Translate(Vector3.forward * bulletSpeed * Time.deltaTime);
	}

	// timer to prevent the bullet from taking up memory off screen
	IEnumerator Death() {
		yield return new WaitForSeconds(3);
		Destroy (this.gameObject);
	}

	// causes player to drop crystals on hit
	// TODO: add explode particle effect
	void OnTriggerEnter(Collider other) {
		if (other.tag == "Player" && other.gameObject != shooter) {
			AudioGlobal audio = GameObject.Find ("_DialogController").GetComponent<AudioGlobal> ();
			audio.Play ("Ship_CannonHit");
			other.gameObject.SendMessage("DropCrystal");
		}

	}
}
