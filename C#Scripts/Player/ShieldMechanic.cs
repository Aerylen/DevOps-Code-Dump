using UnityEngine;
using System.Collections;

public class ShieldMechanic : MonoBehaviour {

	//number of shields or shield objects to be initiated
	public int numberOfShields = 1;

	//Bool check if shield is active
	private bool activeShield = false;

	//the transform object to be initiated
	public GameObject shield;

	//the duration of the shield activation in seconds
	public float shieldDuration = 2;

	//the controllable player
	private GameObject Player;

	//time stamp for cooldown timer
	private float timeStamp;

	//shield cooldown duration
	public float shieldCooldown = 5;

	//how many shields have been created
	private int generatedShieldCount = 0;

	GameObject shieldObject;

	public bool inDangerZone;
	bool canBurn;
	public float burnCooldown;


	// Use this for initialization
	void Start () {
		//Setting the timeStamp for the cooldown on load.
		timeStamp = Time.time;

		//assign the player game object
		Player = this.gameObject;
		canBurn = true;
		inDangerZone = false;
	}
	
	// Update is called once per frame
	void Update () {
		//check if the shield is already active or on cooldown
		if(activeShield == false && timeStamp <= Time.time) {
			//player activates shield
			if(Input.GetKey(GetComponent<PlayerMovementMechanic>().actionInput)) {
				StartCoroutine("Activate");
			}
		}

		if (inDangerZone && canBurn) {
			StartCoroutine(Burn());
		}

		//sticks the shield to the player
		shield.transform.position = Player.gameObject.transform.position;
			
	}

	IEnumerator Burn() {
		canBurn = false;
		this.SendMessage("DropCrystal");
		yield return new WaitForSeconds(burnCooldown);
		canBurn = true;
	}

	public void AIShield() {
		if (activeShield == false && timeStamp <= Time.time) {
			StartCoroutine("Activate");
		}
	}

	void CreateShield() {

		for(int i = 0; i < numberOfShields; i++) {
			//update total shields created
			generatedShieldCount++;

			shield.SetActive(true);
		}
	}

	//key press event
	IEnumerator Activate() {

		AudioGlobal audio = GameObject.Find ("_DialogController").GetComponent<AudioGlobal> ();
		audio.Play ("Lava_ShieldActivate");

		//set shield to active
		activeShield = true;
		
		//create shield
		CreateShield();
		canBurn = false;

		//yield allows us to give the shield a 2 second duration
		//I just don't really understand coroutines. Appologies.
		yield return new WaitForSeconds(shieldDuration);

		audio.Play ("Lava_ShieldDeactivate");

		//Start the cooldown of the shield
		timeStamp = Time.time + shieldCooldown;

		//destroy the shield
		shield.SetActive(false);
		canBurn = true;
		//the shield is no longer active
		//this should be in the coroutine, I feel.
		activeShield = false;
	}

	void OnControllerColliderHit(ControllerColliderHit hit) {
		if (hit.gameObject.tag == "Player" && activeShield) {
			Debug.Log ("Shield");
			PlayerMovementMechanic m = hit.gameObject.GetComponent<PlayerMovementMechanic>();
			m.LongStun(-m.moveDir, 1.1f, 0.2f);
		}
	}
}
