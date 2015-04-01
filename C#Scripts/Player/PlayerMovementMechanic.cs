using UnityEngine;
using System.Collections;

// Require a character controller to be attached to the same game object
[RequireComponent (typeof (CharacterMotorC))]


public class PlayerMovementMechanic : MonoBehaviour {
	
	public int id; // which play the script is controlling. 1-4.
	[HideInInspector]
	public CharacterMotorC motor; // access to the motor to move the player
	public KeyCode actionInput; // button for player's action
	public Action currentAction;
	public float turnSpeed; //TODO implement turning

	// Slow vars, for when the player is hit by some obstacles / other players
	private bool isSlowed; 
	public float slowMultiplier;
	public float slowDuration;
	public float slowCooldown;

	// stun vars, for when the player cannot control their ship (i.e. ramming, cacti, etc)
	private bool isStunned;
	public float stunDuration;
	public float stunMultiplier;

	public bool isPlayer;

	public bool canRotate;
	public Vector3 moveDir;
	public Character character;

	public Vector3 startLocation;

	public float pushPower;

	// list of support inputs
	public enum InputType {
		Keyboard,
		XBox360,
		PS4
	};

	public enum Action {
		Ram,
		Shoot,
		Jump,
		Dig,
		Shield
	};

	public enum Character {
		Mak,
		Eegor,
		Scratch,
		Cwindy
	}

	// the player's input
	public InputType type;

	void Awake() {

		motor = GetComponent<CharacterMotorC>();
		// determining the input type of the player.
		if (Input.GetJoystickNames().Length >= id) {
			if (Input.GetJoystickNames()[id - 1] == "Sony Computer Entertainment Wireless Controller")
				type = InputType.PS4;
			else
				type = InputType.XBox360;
		} else {
			type = InputType.Keyboard;
		}

		SetPlayerInputById();

		currentAction = Action.Ram;
		isStunned = false;
		isSlowed = false;
		//canRotate = false;
	}

	public void SetPlayerInputById() {
		// assigning the correct button depending on what player id
		switch (id) {
		case 1:
			switch(type) {
			case InputType.PS4:
				actionInput = KeyCode.Joystick1Button1;
				break;
			case InputType.XBox360:
				actionInput = KeyCode.Joystick1Button0;
				break;
			case InputType.Keyboard:
				if (id == 1)
					actionInput = KeyCode.Space;
				break;
			}
			break;
		case 2:
			switch(type) {
			case InputType.PS4:
				actionInput = KeyCode.Joystick2Button1;
				break;
			case InputType.XBox360:
				actionInput = KeyCode.Joystick2Button0;
				break;
			case InputType.Keyboard:
				actionInput = KeyCode.RightShift;
				break;
			}
			break;
			
		case 3:
			switch(type) {
			case InputType.PS4:
				actionInput = KeyCode.Joystick3Button1;
				break;
			case InputType.XBox360:
				actionInput = KeyCode.Joystick3Button0;
				break;
			}
			break;
			
		case 4:
			switch(type) {
			case InputType.PS4:
				actionInput = KeyCode.Joystick4Button1;
				break;
			case InputType.XBox360:
				actionInput = KeyCode.Joystick4Button0;
				break;
			}
			break;
		}
	}

	void Update () {
		if (GetComponent<AIMechanic>() == null || GetComponent<AIMechanic>().enabled == false) {
			// Get the input vector from keyboard or analog stick
			moveDir = Vector3.zero;

			if (!isStunned) {

				//Debug.Log ("Player " + id + " input type: " + InputType.Keyboard.ToString());

				// DEBUG
				type = InputType.XBox360;
				switch(id) {
				
				case 1:
					actionInput = KeyCode.Joystick1Button0;
					break;
				case 2:
					actionInput = KeyCode.Joystick2Button0;
					break;
				case 3:
					actionInput = KeyCode.Joystick3Button0;
					break;
				case 4:
					actionInput = KeyCode.Joystick4Button0;
					break;
				}
				//if (id == 1)
				//actionInput = KeyCode.Joystick1Button1;

				if (type == InputType.Keyboard && id <= 2)
					moveDir = new Vector3(Input.GetAxis("HorizontalK"+ id), 0, Input.GetAxis("VerticalK" + id));
				else
					moveDir = new Vector3(Input.GetAxis("Horizontal" + id), 0, Input.GetAxis("Vertical" + id));

				//Debug.Log ("Player: " + id + " | MoveDir: " + moveDir);
				if (moveDir != Vector3.zero) {
					// Get the length of the directon vector and then normalize it
					// Dividing by the length is cheaper than normalizing when we already have the length anyway
					float directionLength = moveDir.magnitude;
					moveDir = moveDir / directionLength;
					
					// Make sure the length is no bigger than 1
					directionLength = Mathf.Min(1, directionLength);
					
					// Make the input vector more sensitive towards the extremes and less sensitive in the middle
					// This makes it easier to control slow speeds when using analog sticks
					directionLength = directionLength * directionLength;
					
					// Multiply the normalized direction vector by the modified length
					moveDir = moveDir * directionLength;
				}
				motor.inputJump = Input.GetKey(actionInput);
			}
		}

		if (moveDir.x == 0 && moveDir.z != 0) {
			moveDir.x += Random.Range(-0.1f, 0.1f);
		}

		if (!isStunned)
		// Apply the direction to the CharacterMotor
			motor.inputMoveDirection = moveDir;

		if (canRotate && moveDir != Vector3.zero)
			transform.eulerAngles = new Vector3(0, Mathf.LerpAngle(transform.eulerAngles.y,
		                                                       	   Mathf.Atan2 (moveDir.normalized.x, moveDir.normalized.z) * Mathf.Rad2Deg,
		                                                           Time.deltaTime * turnSpeed), 0);
	}

	public void Slow() {
		if (!isSlowed)
			StartCoroutine(Slowed());
	}

	// Player is slowed but can move
	IEnumerator Slowed() {
		AudioGlobal audio = GameObject.Find ("_DialogController").GetComponent<AudioGlobal> ();
		audio.Play ("All_Stunned");
		this.SendMessage("PlayLightning");
		this.gameObject.SendMessage("PlaySparks", SendMessageOptions.DontRequireReceiver);
		isSlowed = true;
		motor.movement.maxGroundAcceleration /= slowMultiplier;
		motor.movement.maxForwardSpeed /= slowMultiplier;
		motor.movement.maxBackwardsSpeed /= slowMultiplier;
		motor.movement.maxSidewaysSpeed /= slowMultiplier;
		yield return new WaitForSeconds(slowDuration);
		this.SendMessage("StopLightning");
		motor.movement.maxGroundAcceleration *= slowMultiplier;
		motor.movement.maxForwardSpeed *= slowMultiplier;
		motor.movement.maxBackwardsSpeed *= slowMultiplier;
		motor.movement.maxSidewaysSpeed *= slowMultiplier;
		yield return new WaitForSeconds(slowCooldown);
		isSlowed = false;
	}

	public void Stun(Vector3 stunDir) {
		if (!isStunned)
			StartCoroutine(Stunned (stunDir));
	}

	public void LongStun(Vector3 stunDir, float multi, float length) {
		StartCoroutine(LongStuned(stunDir, multi, length));
	}

	// longer duration stuns... TODO: needs to be merged with Stunned. (e.g. give Stunned the multi and length params)
	IEnumerator LongStuned(Vector3 stunDir, float multi, float length) {
		isStunned = true;
		motor.inputMoveDirection = stunDir;
		motor.movement.maxGroundAcceleration *= stunMultiplier * multi;
		motor.movement.maxForwardSpeed *= stunMultiplier * multi;
		motor.movement.maxBackwardsSpeed *= stunMultiplier * multi;
		motor.movement.maxSidewaysSpeed *= stunMultiplier * multi;
		yield return new WaitForSeconds(length);
		motor.movement.maxGroundAcceleration /= stunMultiplier * multi;
		motor.movement.maxForwardSpeed /= stunMultiplier * multi;
		motor.movement.maxBackwardsSpeed /= stunMultiplier * multi;
		motor.movement.maxSidewaysSpeed /= stunMultiplier * multi;
		isStunned = false;
	}

	// Player cannot move. Short duration only!
	IEnumerator Stunned(Vector3 stunDir) {
		isStunned = true;
		motor.inputMoveDirection = stunDir;
		motor.movement.maxGroundAcceleration *= stunMultiplier;
		motor.movement.maxForwardSpeed *= stunMultiplier;
		motor.movement.maxBackwardsSpeed *= stunMultiplier;
		motor.movement.maxSidewaysSpeed *= stunMultiplier;
		yield return new WaitForSeconds(stunDuration);
		motor.movement.maxGroundAcceleration /= stunMultiplier;
		motor.movement.maxForwardSpeed /= stunMultiplier;
		motor.movement.maxBackwardsSpeed /= stunMultiplier;
		motor.movement.maxSidewaysSpeed /= stunMultiplier;
		isStunned = false;
	}

	// updates the action depending on what the new stage is
	public void ChangeAction(GameControllerGlobal.Stage stage) {
		switch (stage) {
		case GameControllerGlobal.Stage.Air:
			currentAction = Action.Shoot;
			GetComponent<SpinMechanic>().enabled = false;
			GetComponent<ShootMechanic>().enabled = true;
			break;
		case GameControllerGlobal.Stage.Ship:
			currentAction = Action.Jump;
			GetComponent<ShootMechanic>().enabled = false;
			motor.jumping.enabled = true;
			break;
		case GameControllerGlobal.Stage.Desert:
			currentAction = Action.Dig;
			motor.jumping.enabled = false;
			break;
		case GameControllerGlobal.Stage.Lava:
			currentAction = Action.Shield;
			GetComponent<ShieldMechanic>().enabled = true;
			break;
		}
	}

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == "Player" && other.gameObject != this.gameObject) {
			AudioGlobal audio = GameObject.Find ("_DialogController").GetComponent<AudioGlobal> ();
			audio.Play ("All_Collide");
			this.gameObject.SendMessage("PlaySparks", SendMessageOptions.DontRequireReceiver);
			other.gameObject.SendMessage("Stun", moveDir.normalized);
			this.gameObject.SendMessage("NewAIDecision", SendMessageOptions.DontRequireReceiver);
		}
	}

	
	void OnControllerColliderHit (ControllerColliderHit hit) {
		if (hit.gameObject.tag == "Barrel" && hit.gameObject.GetComponent<BarrelMechanic>().canBeDestroyed) {
			Rigidbody body = hit.collider.attachedRigidbody;
			
			// no rigidbody
			if (body == null || body.isKinematic) { return; }
			
			// We dont want to push objects below us
			if (hit.moveDirection.y < -0.3) { return; }
			
			// Calculate push direction from move direction,
			// we only push objects to the sides never up and down
			Vector3 pushDir =  new Vector3 (hit.moveDirection.x, 0, hit.moveDirection.z);
			
			// If you know how fast your character is trying to move,
			// then you can also multiply the push velocity by that.
			
			// Apply the push
			body.velocity = pushDir * pushPower;
		}
	}
}
