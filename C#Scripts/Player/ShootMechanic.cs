using UnityEngine;
using System.Collections;

public class ShootMechanic: MonoBehaviour {

	// these values should be set in the inspector!! (Travis, 6/13/14)
	private bool canShoot = true;
	private float timer = .75f;
	public float bulletSpeed = 20f;
	public float bulletCooldown = .75f;
	public GameObject bulletPrefab;
	PlayerMovementMechanic player;
	public Vector3 shootPos;

	void Start() {
		player = GetComponent<PlayerMovementMechanic>();
	}	
	// Update
	void Update () {

		if (!canShoot) {
			timer -= Time.deltaTime;		
		}

		if(timer < 0){
			canShoot = true;
		}

		// spawns bullet on key press
		if(Input.GetKey(player.actionInput) && canShoot) {
			AudioGlobal audio = GameObject.Find ("_DialogController").GetComponent<AudioGlobal> ();
			audio.Play ("Sky_Shoot");

			canShoot = false;
			timer = bulletCooldown;
			GameObject bullet = Instantiate(bulletPrefab, transform.position, transform.rotation) as GameObject;
			bullet.transform.parent = this.transform;
			bullet.transform.localPosition = shootPos;
			bullet.GetComponent<BulletMechanic>().shooter = this.gameObject;
			bullet.transform.parent = null;
			bullet.SendMessage("SetBulletSpeed", bulletSpeed);
		}
	}
	public void AIShoot() {
		if (canShoot) {
			AudioGlobal audio = GameObject.Find ("_DialogController").GetComponent<AudioGlobal> ();
			audio.Play ("Sky_Shoot");

			canShoot = false;
			timer = bulletCooldown;
			GameObject bullet = Instantiate(bulletPrefab, transform.position, transform.rotation) as GameObject;
			bullet.transform.parent = this.transform;
			bullet.transform.localPosition = shootPos;
			bullet.GetComponent<BulletMechanic>().shooter = this.gameObject;
			bullet.transform.parent = null;
			bullet.SendMessage("SetBulletSpeed", bulletSpeed);
		}
	}
}
