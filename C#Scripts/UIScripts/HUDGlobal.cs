using UnityEngine;
using System.Collections;

public class HUDGlobal : MonoBehaviour {
	
	GameObject player1;
	GameObject player2;
	GameObject player3;
	GameObject player4;

	public GameObject OSI;

	int player1Score;
	int player2Score;
	int player3Score;
	int player4Score;

	public Texture AButtonUp;
	public Texture AButtonDown;

	public int[] scores;

	//Initialize
	void Start () {
		DontDestroyOnLoad (GameObject.Find("_HUDController"));
		OSI.SetActive (false);
		scores = new int[5];
	}

	public void SetPlayers() {
		// Get players
		GameObject[] players = GameObject.FindGameObjectsWithTag ("Player");
		
		// Set player references
		foreach (GameObject player in players) {
			int id = player.GetComponent<PlayerMovementMechanic>().id;

			switch (id) {
				case 1:	player1 = player; break;
				case 2:	player2 = player; break;
				case 3:	player3 = player; break;
				case 4:	player4 = player; break;
			}

			scores[id] = player.GetComponent<CrystalCollectGlobal>().score;
		}
	}

	public IEnumerator ShowOSI (string level) {
		
		OSI.SetActive (true);

		string text = "";
		switch (level) {
			case "Space":  text = "Spin Attack";         break;
			case "Air":    text = "Fire Turret";         break; 
			case "Ship":  text = "Evasive Jump";        break;
			case "Desert": text = "Mine Crystal Mounds"; break;
			case "Lava":   text = "Bumper Shield"; 	     break;
		}
		GameObject.Find ("TextMechanic").GetComponent<GUIText> ().text = text;
		StartCoroutine (AnimateActionButton ());
		yield return new WaitForSeconds(5);
		OSI.SetActive (false);
	}

	IEnumerator AnimateActionButton (int count = 0) {
		Texture buttonTexture;
		if (count % 2 == 0) buttonTexture = AButtonUp;
		else 				buttonTexture = AButtonDown;
		if (GameObject.Find("AButton") == null) yield break;
		GameObject.Find ("AButton").GetComponent<GUITexture> ().texture = buttonTexture;
		yield return new WaitForSeconds(.1f);
		StartCoroutine (AnimateActionButton (++count));
	}

	public PlayerMovementMechanic.Character[] getChars() {
		PlayerMovementMechanic.Character[] chars = new PlayerMovementMechanic.Character[5];
		chars [1] = player1.GetComponent<PlayerMovementMechanic> ().character;
		chars [2] = player2.GetComponent<PlayerMovementMechanic> ().character;
		chars [3] = player3.GetComponent<PlayerMovementMechanic> ().character;
		chars [4] = player4.GetComponent<PlayerMovementMechanic> ().character;
		return chars;
	}

	void Update () {

		SetPlayers ();

		if (OSI.activeSelf) {
			GUITexture aBtn = GameObject.Find ("AButton").GetComponent<GUITexture> ();

			int aBtnSize = Screen.width / 16;
			int width1 = Screen.width / 2 - aBtnSize / 2;
			int height1 = Screen.height / 2 - aBtnSize / 2;

			// height1 = (int) (height1 - (Screen.height * .10f));

			aBtn.pixelInset = new Rect (width1, height1, aBtnSize, aBtnSize);

			GUIText mechTxt = GameObject.Find ("TextMechanic").GetComponent<GUIText> ();

			int width2 = Screen.width / 2;
			int height2 = Screen.height / 2;

			height2 = (int) (height1 + (Screen.height * .05f));

			mechTxt.pixelOffset = new Vector2(width2, height2);

		}
	}
}