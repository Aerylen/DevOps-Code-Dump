using UnityEngine;
using System.Collections;

public class CannonShootStage : MonoBehaviour {

	public int maxDelay = 5;
	public int minDelay = 2;
	public GameObject cannonBall;

	// Use this for initialization
	void Start () {
		StartCoroutine(Fire());
		//Physics.IgnoreCollision(this.collider, GameObject.FindGameObjectWithTag("Respawn").collider);
		if (this.collider.enabled == true) {
			Physics.IgnoreCollision(this.collider, GameObject.FindGameObjectWithTag("Boundary").transform.FindChild("Left").collider);
			Physics.IgnoreCollision(this.collider, GameObject.FindGameObjectWithTag("Boundary").transform.FindChild("Right").collider);
			Physics.IgnoreCollision(this.collider, GameObject.Find ("Top").transform.collider);
			Physics.IgnoreCollision(this.collider, GameObject.Find ("Bottom").transform.collider);
		}

	}
	
	IEnumerator Fire(){
		AudioGlobal audio = GameObject.Find ("_DialogController").GetComponent<AudioGlobal> ();
		audio.Play ("Ship_CannonFire");
		animation.Play();
		int shotTimer = Random.Range(minDelay, maxDelay); 
		yield return new WaitForSeconds(shotTimer);
		Transform t = transform.FindChild("Cannon_Main").FindChild("Shoot");

		GameObject g = Instantiate (cannonBall, t.position, t.parent.rotation) as GameObject;
		// note here a 90 degree rotation, due to the cannonball object model facing sideways by default
		g.transform.Rotate (transform.rotation.x, 90, transform.rotation.z);
		//g.transform.eulerAngles = this.transform.eulerAngles + new Vector3(transform.rotation.x, 90, transform.rotation.y);
		g.SendMessage("SetBulletSpeed", 10);
		g.GetComponent<BulletMechanic>().shooter = this.gameObject;
		StartCoroutine(Fire());
	}
}
