using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameControllerGlobal : MonoBehaviour {

	// the obstacle prefabs
	public List<GameObject> crystals;
	public List<GameObject> fireballs;
	public List<GameObject> meteors;
	public List<GameObject> mines;
	public List<GameObject> cacti;
	public List<GameObject> digSites;

	public GameObject ocean;
	public GameObject ocean_Sun;
	public GameObject ocean_Scene;
	public GameObject hud;

	public List<BarrelSpawnStage> bar;
	public List<CannonShootStage> can;

	// is this scene a single level?
	public bool isSingleStage;

	// separate boundaries for different stages
	public GameObject desertBoundary;
	public GameObject lavaBoundary;
	public GameObject shipBoundary;

	public float stageTime;

	public enum Stage {
		Hanger, Space, Air, Ship, Desert, Lava
	};

	public Stage currentStage;

	// For Game Over scene
	public int[] scores;
	public PlayerMovementMechanic.Character[] characters;

	public bool isPlayer1;
	public bool isPlayer2;
	public bool isPlayer3;
	public bool isPlayer4;

	public int makID;
	public int scratchID;
	public int cwindyID;
	public int eegorID;

	public int makScore;
	public int scratchScore;
	public int cwindyScore;
	public int eegorScore;

	// Use this for initialization
	void Start () {
		ocean = GameObject.Find ("Ocean Grid");
		ocean.SetActive(false);
		DontDestroyOnLoad(ocean_Scene);
		DontDestroyOnLoad(GameObject.Find ("_DialogController"));
		DontDestroyOnLoad(this.gameObject);
		scores = new int[5];
		characters = new PlayerMovementMechanic.Character[5];
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void SetPlayerVals() {
		foreach (GameObject g in GameObject.FindGameObjectsWithTag("Player")) {
			if (g.name == "CharacterMak") {
				g.GetComponent<PlayerMovementMechanic>().id = makID;
				switch (makID) {
				case 1:
					if (isPlayer1)
						g.GetComponent<PlayerMovementMechanic>().isPlayer = true;
					break;
				case 2:
					if (isPlayer2)
						g.GetComponent<PlayerMovementMechanic>().isPlayer = true;
					break;
				case 3:
					if (isPlayer3)
						g.GetComponent<PlayerMovementMechanic>().isPlayer = true;
					break;
				case 4:
					if (isPlayer4)
						g.GetComponent<PlayerMovementMechanic>().isPlayer = true;
					break;
				}
			} else if (g.name == "CharacterEegor") {
				g.GetComponent<PlayerMovementMechanic>().id = eegorID;
				switch (eegorID) {
				case 1:
					if (isPlayer1)
						g.GetComponent<PlayerMovementMechanic>().isPlayer = true;
					break;
				case 2:
					if (isPlayer2)
						g.GetComponent<PlayerMovementMechanic>().isPlayer = true;
					break;
				case 3:
					if (isPlayer3)
						g.GetComponent<PlayerMovementMechanic>().isPlayer = true;
					break;
				case 4:
					if (isPlayer4)
						g.GetComponent<PlayerMovementMechanic>().isPlayer = true;
					break;
				}
			} else if (g.name == "CharacterScratch") {
				g.GetComponent<PlayerMovementMechanic>().id = scratchID;
				switch (scratchID) {
				case 1:
					if (isPlayer1) 
						g.GetComponent<PlayerMovementMechanic>().isPlayer = true;
					break;
				case 2:
					if (isPlayer2)
						g.GetComponent<PlayerMovementMechanic>().isPlayer = true;
					break;
				case 3:
					if (isPlayer3)
						g.GetComponent<PlayerMovementMechanic>().isPlayer = true;
					break;
				case 4:
					if (isPlayer4)
						g.GetComponent<PlayerMovementMechanic>().isPlayer = true;
					break;
				}
			} else if (g.name == "CharacterCwindy"){
				g.GetComponent<PlayerMovementMechanic>().id = cwindyID;				
				switch (cwindyID) {
				case 1:
					if (isPlayer1)
						g.GetComponent<PlayerMovementMechanic>().isPlayer = true;
					break;
				case 2:
					if (isPlayer2)
						g.GetComponent<PlayerMovementMechanic>().isPlayer = true;
					break;
				case 3:
					if (isPlayer3)
						g.GetComponent<PlayerMovementMechanic>().isPlayer = true;
					break;
				case 4:
					if (isPlayer4)
						g.GetComponent<PlayerMovementMechanic>().isPlayer = true;
					break;
				}
			}
			//g.GetComponent<PlayerMovementMechanic>().SetPlayerInputById();
		}
	}

	// this function is called in between stages. it removes and instantiates parts of the scene to create the next minigame
	public void Transition(Stage stage) {
		DialogGlobal dialog = GameObject.Find ("_DialogController").GetComponent<DialogGlobal> ();
		if (!isSingleStage || currentStage == Stage.Lava) {
			switch (stage) {
			case Stage.Hanger:
				currentStage = Stage.Space;
				SetPlayerVals();
				dialog.StartCoroutine("PlayClip", "Dev5_Space");
				//GameObject.Find ("_HUDController").GetComponent<HUDGlobal>().StartCoroutine("ShowOSI", "space");
				break;
			case Stage.Space:
				currentStage = Stage.Air;
				dialog.StartCoroutine("PlayClip", "Dev5_Sky");
				SetPlayerVals();

				//GameObject.Find ("_HUDController").GetComponent<HUDGlobal>().StartCoroutine("ShowOSI", "sky");
				ocean.SetActive(true);
				ocean_Sun.SetActive(true);
				break;
			case Stage.Air:
				ocean_Scene.transform.parent = null;
				ocean.SetActive(true);
				ocean_Sun.SetActive(true);
				ocean_Scene.transform.position = new Vector3(0, 815, 75);
				currentStage = Stage.Ship;
				dialog.StartCoroutine("PlayClip", "Dev5_Water");
				SetPlayerVals();

				//GameObject.Find ("_HUDController").GetComponent<HUDGlobal>().StartCoroutine("ShowOSI", "water");
				break;
			case Stage.Ship:
				ocean.SetActive(true);
				ocean_Sun.SetActive(true);
				ocean_Scene.transform.position = new Vector3(0, 815, 75);
				currentStage = Stage.Desert;
				dialog.StartCoroutine("PlayClip", "Dev5_Desert");
				SetPlayerVals();
				//GameObject.Find ("_HUDController").GetComponent<HUDGlobal>().StartCoroutine("ShowOSI", "desert");
				break;
			case Stage.Desert:
				ocean.SetActive(false);
				ocean_Sun.SetActive(false);
				Destroy (ocean_Scene);
				currentStage = Stage.Lava;
				dialog.StartCoroutine("PlayClip", "Dev5_Lava");
				SetPlayerVals();
				//GameObject.Find ("_HUDController").GetComponent<HUDGlobal>().StartCoroutine("ShowOSI", "lava");
				break;
			}
			Application.LoadLevelAsync("Loading");

			// update the action every time the stage is changed
			foreach (GameObject p in GameObject.FindGameObjectsWithTag("Player")) {
				//p.SendMessage ("ChangeAction", currentStage);
				p.GetComponent<AIMechanic>().UpdateDecisionTree();
			}
		} else {
			Destroy(ocean_Scene);
			EndGame ();
		}

	}


	public void EndGame() {
		//scores = (int[]) GameObject.Find ("_HUDController").GetComponent<HUDGlobal> ().scores.Clone ();
	
		scores [1] = (int)GameObject.Find ("CrownSprite").GetComponent<CrownIndicatorMisc> ().scores [0];
		scores [2] = (int)GameObject.Find ("CrownSprite").GetComponent<CrownIndicatorMisc> ().scores [1];
		scores [3] = (int)GameObject.Find ("CrownSprite").GetComponent<CrownIndicatorMisc> ().scores [2];
		scores [4] = (int)GameObject.Find ("CrownSprite").GetComponent<CrownIndicatorMisc> ().scores [3];

		characters = (PlayerMovementMechanic.Character[]) GameObject.Find ("_HUDController").GetComponent<HUDGlobal> ().getChars ().Clone ();
		Destroy (GameObject.Find("_HUDController"));
		DontDestroyOnLoad (GameObject.Find("_GameController"));
		Application.LoadLevel ("GameOver");
	}
}