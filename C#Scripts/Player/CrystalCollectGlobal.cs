using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CrystalCollectGlobal : MonoBehaviour {

	public int score; // the current score of the player
	public List<string> collectedCrystals; // the list of crystals the player has collected
	private List<GameObject> crystals; // the list of types of crystals (to spawn when "dropping" a gem)
	private GameControllerGlobal controller; // the global game controller

	void Start(){
		controller = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameControllerGlobal>();
		crystals = controller.crystals;
		score = 0;
		collectedCrystals = new List<string>();
	}

	void OnTriggerEnter (Collider other) {

		// if a crystal hits the player, add to the score and collected crystals list
		if (controller != null && other.tag.Contains("Crystal") && other.GetComponent<CrystalGlobal>().canBeCollected) {

			//score++; // TODO: make score particular to type of crystal
			switch (GetComponent<PlayerMovementMechanic>().character) {
			case PlayerMovementMechanic.Character.Cwindy:
				controller.cwindyScore++;
				break;
			case PlayerMovementMechanic.Character.Eegor:
				controller.eegorScore++;
				break;
			case PlayerMovementMechanic.Character.Mak:
				controller.makScore++;
				break;
			case PlayerMovementMechanic.Character.Scratch:
				controller.scratchScore++;
				break;
			}
			collectedCrystals.Add(other.gameObject.tag);
			Destroy(other.gameObject);

			AudioGlobal audio = GameObject.Find ("_DialogController").GetComponent<AudioGlobal> ();
			audio.Play ("All_CrystalCollect");
			
			// Play clip 33% of time
			int clip = Random.Range(1, 100);

			if (clip < 90) return;

			// Grab from pool of collect clips
			clip = Random.Range(1, 3);

			DialogGlobal dialog = GameObject.Find ("_DialogController").GetComponent<DialogGlobal> ();

			string character = GetComponent<PlayerMovementMechanic>().character.ToString();

			dialog.StartCoroutine ("PlayClip", character + "_Collect" + clip);
		}
	}

	// drops a random crystal from the player's collection
	// TODO: allow for more than one drop
	public void DropCrystal() {
		if(score > 0){
			List<GameObject> heldCrystalTypes = new List<GameObject>();
			for (int i = 0; i < crystals.Count; i++) {
				if (collectedCrystals.Contains("Crystal" + (i + 1)))
					heldCrystalTypes.Add(crystals[i]);
			}
			switch (GetComponent<PlayerMovementMechanic>().character) {
			case PlayerMovementMechanic.Character.Cwindy:
				controller.cwindyScore--;
				break;
			case PlayerMovementMechanic.Character.Eegor:
				controller.eegorScore--;
				break;
			case PlayerMovementMechanic.Character.Mak:
				controller.makScore--;
				break;
			case PlayerMovementMechanic.Character.Scratch:
				controller.scratchScore--;
				break;
			}
			GameObject g = (GameObject)Instantiate(heldCrystalTypes[Random.Range(0, heldCrystalTypes.Count)],this.transform.position, transform.rotation);

			collectedCrystals.Remove(g.tag);
			g.SendMessage("Dropped");
		}
		this.SendMessage("Slow");
	
		AudioGlobal audio = GameObject.Find ("_DialogController").GetComponent<AudioGlobal> ();
		audio.Play ("All_CrystalLose");

		// Play clip 33% of time
		int clip = Random.Range(1, 100);
		
		if (clip < 90) return;
		
		// Grab from pool of collect clips
		clip = Random.Range(1, 3);
		
		DialogGlobal dialog = GameObject.Find ("_DialogController").GetComponent<DialogGlobal> ();
		
		string character = GetComponent<PlayerMovementMechanic>().character.ToString();
		
		dialog.StartCoroutine ("PlayClip", character + "_Hit" + clip);
	}
}