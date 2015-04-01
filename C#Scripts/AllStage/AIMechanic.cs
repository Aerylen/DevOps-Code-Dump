using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AIMechanic : MonoBehaviour {

	public float crystalNearRadius;
	public float playerNearRadius;
	public float mineNearRadius;
	public float fleeRadius;
	public float crystalScoreModifier;
	public float playerScoreModifier;
	public float fleeModifier;

	public bool isDigging;
	public bool inDigZone;
	public bool canDig;

	private delegate void MakeDecision();
	private MakeDecision makeDecision;

	private delegate void UseAction();
	private UseAction useAction;

	private PlayerMovementMechanic movement;

	GameObject seekTarget;
	GameObject fleeTarget;
	GameObject t;

	float seekTimer;

	// Use this for initialization
	void Start () {
		seekTarget = null;
		UpdateDecisionTree();
		movement = GetComponent<PlayerMovementMechanic>();
		seekTimer = 3;
		inDigZone = false;
		t = null;
		canDig = false;
		isDigging = true;
	}
	
	// Update is called once per frame
	void Update () {

		if (seekTarget != null && seekTarget != t) {
			seekTimer = 3;
			t = seekTarget;
		}
		else
			seekTimer -= Time.deltaTime;

		if (seekTimer <= 0) {
			seekTarget = null;
			t = null;
			Rerout();
		}

		if (GetComponent<PlayerMovementMechanic>().isPlayer) {
			this.enabled = false;
		}

		makeDecision();
	}

	public void UpdateDecisionTree() {
		switch (GameObject.Find ("_GameController").GetComponent<GameControllerGlobal>().currentStage) {
		case GameControllerGlobal.Stage.Hanger:
		case GameControllerGlobal.Stage.Space:
			makeDecision = ActionTreeSpace;
			useAction = ActionSpin;
			break;
		case GameControllerGlobal.Stage.Air:
			makeDecision = ActionTreeAir;
			useAction = ActionShoot;
			break;
		case GameControllerGlobal.Stage.Ship:
			makeDecision = ActionTreeShip;
			useAction = ActionJump;
			break;
		case GameControllerGlobal.Stage.Desert:
			makeDecision = ActionTreeDesert;
			useAction = ActionDig;
			break;
		case GameControllerGlobal.Stage.Lava:
			makeDecision = ActionTreeLava;
			useAction = ActionShield;
			break;
		}
	}

	/*
	 * FUNDAMENTAL BEHAVIORS
	 */
	void Seek(Vector3 target) {
		if (Vector3.Distance(target, this.transform.position) > 1) {
			Vector3 move = target - this.transform.position;
			move.y = 0;
			movement.moveDir = move.normalized;
		} else {
			movement.moveDir = Vector3.zero;
			seekTarget = null;
		}

	}

	bool SeekNearbyCrystals() {
		if (seekTarget != null) {
			Seek (seekTarget.transform.position);
			return true;
		} else {
			
			float low = Mathf.Infinity;
			
			// find nearest best crystal
			foreach (Collider c in Physics.OverlapSphere(transform.position, crystalNearRadius)) {
				if (c.gameObject.tag == "Crystal1" ||
				    c.gameObject.tag == "Crystal2" ||
				    c.gameObject.tag == "Crystal3" ||
				    c.gameObject.tag == "Crystal4") {
					// inversely proportional to distance from crystal, with modifiers for crystal value and arbitrary balancing float
					if (Vector3.Distance(transform.position, c.gameObject.transform.position) 
					    / int.Parse(c.gameObject.tag.Substring(7)) * crystalScoreModifier < low) {
						seekTarget = c.gameObject;
						low = Vector3.Distance(transform.position, c.gameObject.transform.position)
							/ int.Parse(c.gameObject.tag.Substring(7)) * crystalScoreModifier;
					}
				}
			}
			
			if (low == Mathf.Infinity) {
				return false;
			} else {
				Seek(seekTarget.transform.position);
				return true;
			}
		}
	}

	public void NewAIDecision() {
		seekTarget = null;
	}

	bool SeekPlayer() {
		//Debug.Log("Seeking Players");
		if (seekTarget != null) {
			Seek (seekTarget.transform.position);
			return true;
		} else {
			
			float low = Mathf.Infinity;
			
			// find players
			foreach (GameObject g in GameObject.FindGameObjectsWithTag("Player")) {
				if (g.GetComponent<CrystalCollectGlobal>().score == 0)
					continue;
				if (this.gameObject == g) 
					continue;
					// inversely proportional to distance from player, with modifiers for player score and arbitrary balancing float
				if (Vector3.Distance(transform.position, g.transform.position) 
				    / g.GetComponent<CrystalCollectGlobal>().score * playerScoreModifier < low) {
					seekTarget = g;
					low = Vector3.Distance(transform.position, g.transform.position)
						/ g.GetComponent<CrystalCollectGlobal>().score * playerScoreModifier;

				}
			}
			
			if (low == Mathf.Infinity) {
				seekTarget = null;
				return false;
			} else {
				Seek(seekTarget.transform.position);
				return true;
			}
		}
	}

	void Stop() {
		movement.moveDir = Vector3.zero;
	}

	bool SeekMine() {
		if (seekTarget != null) {
			Seek (seekTarget.transform.position);
			return true;
		} else {
			
			float low = -Mathf.Infinity;
			
			// find players
			foreach (GameObject g in GameObject.FindGameObjectsWithTag("SkyMine")) {
				if (g.transform.position.y > transform.position.y - 10)
					continue;

				if (g.transform.position.y >= low) {
					seekTarget = g;
					low = g.transform.position.y;
					
				}
			}
			
			if (low == -Mathf.Infinity) {
				seekTarget = null;
				return false;
			} else {
				Seek(seekTarget.transform.position);
				return true;
			}
		}
	}

	bool SeekBarrel() {
		if (seekTarget != null) {
			Seek (seekTarget.transform.position);
			return true;
		} else {
			GameObject[] g = GameObject.FindGameObjectsWithTag("Barrel");
			List<GameObject> go = new List<GameObject>();
			foreach (GameObject o in g){
				go.Add (o);
			}
			while (seekTarget == null && go.Count > 0) {
				int r = Random.Range(0, go.Count);
				if (!go[r].GetComponent<BarrelMechanic>().canBeDestroyed)
					seekTarget = go[r];
				else {
					go.Remove(go[r]);
				}
			}
			if (seekTarget == null)
				return false;
			return true;
		}
	}

	void SeekCenter() {
		seekTarget = GameObject.Find ("Center");
	}

	bool SeekDigSite() {
		if (seekTarget != null) {
			Seek (seekTarget.transform.position);
			return true;
		} else {
			
			float low = Mathf.Infinity;

			foreach (GameObject g in GameObject.FindGameObjectsWithTag("DigSite")) {
				if (Vector3.Distance(transform.position, g.transform.position) < low) {
					seekTarget = g;
					low = Vector3.Distance(transform.position, g.transform.position);
				}
			}
			
			if (low == Mathf.Infinity) {
				return false;
			} else {
				Seek(seekTarget.transform.position);
				return true;
			}
		}
	}

	void SeekPlatform() {
		float low = Mathf.Infinity;
		if (seekTarget != null) {
			Seek (seekTarget.transform.position);
			if (Vector3.Distance(transform.position, seekTarget.transform.position) < 5f) {
				GameObject ga = null;
				foreach (GameObject g in GameObject.FindGameObjectsWithTag("PlatformZone")) {
					if (Vector3.Distance(transform.position, g.transform.position) < low) {
						ga = g;
						low = Vector3.Distance(transform.position, g.transform.position);
					}
				}

				switch (seekTarget.name) {
				case "Platform1":
					seekTarget = ga.transform.parent.FindChild ("Platform2").gameObject;
					break;
				case "Platform2":
					seekTarget = ga.transform.parent.FindChild ("Platform3").gameObject;
					break;
				case "Platform3":
					seekTarget = ga.transform.parent.FindChild ("Platform4").gameObject;
					break;
				case "Platform4":
					seekTarget = ga.transform.parent.FindChild ("Platform5").gameObject;
					break;
				case "Platform5":
					Stop ();
					break;
				}
			}
		} else {
			
			foreach (GameObject g in GameObject.FindGameObjectsWithTag("PlatformZone")) {
				if (Vector3.Distance(transform.position, g.transform.position) < low) {
					seekTarget = g;
					low = Vector3.Distance(transform.position, g.transform.position);
				}
			}

			seekTarget = seekTarget.transform.parent.FindChild("Platform1").gameObject;

			Seek(seekTarget.transform.position);
		}
	}

	float DistanceToNearestPlayer() {
		float low = Mathf.Infinity;
		foreach (GameObject g in GameObject.FindGameObjectsWithTag("Player")) {
			if (g == this.gameObject)
				continue;
			if (Vector3.Distance(this.transform.position, g.transform.position) < low)
				low = Vector3.Distance(this.transform.position, g.transform.position);
		}
		return low;
	}

	public IEnumerator Rerout() {
		seekTarget = new GameObject("Temp");
		seekTarget.transform.position = transform.position + new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)) * 10;
		yield return new WaitForSeconds(2);
		Destroy (GameObject.Find ("Temp"));
		seekTarget = null;
	}

	/*
	 * ACTION DECISIONS
	 */
	void ActionTreeSpace() {

		if (seekTarget != null) {
			if (seekTarget.tag == "Player" && Vector3.Distance(this.transform.position, seekTarget.transform.position) < playerNearRadius) {
				useAction();
				seekTarget = null;
			} else
				Seek (seekTarget.transform.position);
		} else {
			if (Random.value > 0.15f) {
				if (SeekNearbyCrystals()) {} 
				else if (SeekPlayer()) {} 
			} else {
				if (SeekPlayer()) {} 
				else if (SeekNearbyCrystals()) {} 
			}
		}
	}

	void ActionTreeAir() {
		if (seekTarget != null) {
			if (seekTarget.tag == "SkyMine" && Vector3.Distance(this.transform.position, 
			                                                    new Vector3(seekTarget.transform.position.x, 0, seekTarget.transform.position.z)) < mineNearRadius) {
				useAction();
				seekTarget = null;
			}
			else
				Seek(seekTarget.transform.position);
		} else {
			if (Random.value > 0.85f) {
				if (SeekNearbyCrystals()) {}
				else if (SeekMine ()){}
			} else {
				if (SeekMine()) {}
				else if (SeekNearbyCrystals()){}
			}
		}
		
	}

	void ActionTreeShip() {
		if (seekTarget != null) {
			Seek(seekTarget.transform.position);
		} else {
			if (Random.value > 0.85f) {
				if (SeekBarrel()) {}
				else if (SeekNearbyCrystals()){}
			} else if (Random.value > 0.5f) {
				SeekCenter();
			} else {
				if (SeekNearbyCrystals()) {}
				else if (SeekBarrel()) {}
				else if (SeekPlayer()) {}
			}
		}

		foreach (GameObject g in GameObject.FindGameObjectsWithTag("Obstacle")) {
			if (Vector3.Distance(this.transform.position, g.transform.position) < playerNearRadius) {
				useAction();
			}
		}
	}

	void ActionTreeDesert() {
		if (seekTarget != null) {
			if (inDigZone) {
				canDig = true;
				Stop();
			} else
				Seek (seekTarget.transform.position);
		} else {
			if (GameObject.FindGameObjectsWithTag("DigSite").Length == 0) {
				SeekCenter();
			} else if (SeekNearbyCrystals()) {}
			else if (SeekDigSite()) {}
			else if (SeekPlayer()) {}
		}
	}

	void ActionTreeLava() {
		if (seekTarget != null) {
			if (GetComponent<ShieldMechanic>().inDangerZone && seekTarget.name.Contains("Platform"))
				SeekPlatform();

			if (seekTarget.tag == "Player" && Vector3.Distance(this.transform.position, seekTarget.transform.position) < playerNearRadius) {
				useAction();
				seekTarget = null;

			}
		} else  {
			if (GetComponent<ShieldMechanic>().inDangerZone) {
				SeekPlatform ();
			} else {
				if (SeekNearbyCrystals()){}
				else if (SeekPlayer()){}
			}
		}

		if (GetComponent<ShieldMechanic>().inDangerZone) {
			useAction();
		}
	}


	/*
	 * ACTION USES
	 */
	void ActionSpin() {
		GetComponent<SpinMechanic>().AISpin();
	}
	
	void ActionShoot() {
		GetComponent<ShootMechanic>().AIShoot();
	}

	void ActionJump() {
		StartCoroutine(ActionJumpC());
	}
	IEnumerator ActionJumpC() {
		AudioGlobal audio = GameObject.Find ("_DialogController").GetComponent<AudioGlobal> ();
		audio.Play ("Ship_Jump");
		movement.motor.inputJump = true;
		yield return new WaitForSeconds(0.5f);
		movement.motor.inputJump = false;
	}

	public void ActionDig() {
		if (isDigging) {
			StartCoroutine(ActionDigC());
		}
	}
	IEnumerator ActionDigC() {

		AudioGlobal audio = GameObject.Find ("_DialogController").GetComponent<AudioGlobal> ();
		audio.Play ("Desert_Dig");
		isDigging = false;
		yield return new WaitForSeconds(0.2f);
		isDigging = true;
	}
	
	void ActionShield() {
		GetComponent<ShieldMechanic>().AIShield();
	}
}
