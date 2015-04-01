using UnityEngine;
using System.Collections;

public class SpinMechanic : MonoBehaviour {

	PlayerMovementMechanic player;
	bool isSpinning;
	bool canSpin;
	public float speed;
	public float spinSpeed;
	public float spinDuration;
	public float spinCooldown;
	public Quaternion originalRotationValue;
	
	// Use this for initialization
	void Start () {
		canSpin = true;
		player = GetComponent<PlayerMovementMechanic>();
		isSpinning = false;
		originalRotationValue = this.transform.rotation;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey(player.actionInput) && !isSpinning) {
			StartCoroutine(Spin ());
		}
		if (isSpinning) {
			transform.Rotate (0, 0, spinSpeed * Time.deltaTime);
		}
	}

	public void AISpin() {
		if (canSpin)
			StartCoroutine(Spin ());
	}
	
	// simply increase acceleration and max speed. 
	IEnumerator Spin() {
		AudioGlobal audio = GameObject.Find ("_DialogController").GetComponent<AudioGlobal> ();
		audio.Play ("Space_Spin");

		canSpin = false;
		isSpinning = true;
		player.motor.movement.maxGroundAcceleration *= speed;
		player.motor.movement.maxForwardSpeed *= speed;
		player.motor.movement.maxBackwardsSpeed *= speed;
		player.motor.movement.maxSidewaysSpeed *= speed;
		yield return new WaitForSeconds(spinDuration);
		player.motor.movement.maxGroundAcceleration /= speed;
		player.motor.movement.maxForwardSpeed /= speed;
		player.motor.movement.maxBackwardsSpeed /= speed;
		player.motor.movement.maxSidewaysSpeed /= speed;
		transform.rotation = originalRotationValue;
		isSpinning = false;
		yield return new WaitForSeconds(spinCooldown);
		canSpin = true;
		//transform.rotation = Quaternion.Slerp(transform.rotation, originalRotationValue, Time.time * spinSpeed);
		//transform.eulerAngles = new Vector3(90, 0, 0);
	}
	
	// ram causes players to drop a crystal when they are hit
	void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == "Player" && isSpinning) {
			other.gameObject.SendMessage("DropCrystal");
		}
	}
}