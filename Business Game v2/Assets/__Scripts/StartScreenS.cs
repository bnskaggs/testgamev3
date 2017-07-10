using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

public class StartScreenS : MonoBehaviour {

	public string levelToPlay = "Scene_1_Game_Original";
	public GameObject optionsPanel;

	private GameObject rulesDD;
	private GameObject levelsDD;
	private List<string> rulesList = new List<string>();
	private List<string> levelsList = new List<string>();

	private string savepath = GameMasterS.saveLoadLocation;
	private string savepath2= GameMasterS.saveLoadLocation2;

	public GameObject continueButton;

	public GameObject testText;

	// Use this for initialization
	void Start () {
		optionsPanel = GameObject.Find ("OptionsPanel");
		optionsPanel.SetActive (false);

		continueButton= GameObject.Find ("ContinueGame");

		rulesList.Add ("Mobile");
		rulesList.Add ("Board Game");

		levelsList.Add ("International");
		levelsList.Add ("India");
		Debug.Log (savepath);
		testText.GetComponent<Text> ().text = GameMasterS.saveLoadLocation2;

		if (File.Exists (savepath2))
			continueButton.GetComponent<Button> ().interactable = true;
		else
			continueButton.GetComponent<Button> ().interactable = false;
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void StartButtonPushed(){
		SceneManager.LoadSceneAsync (levelToPlay, LoadSceneMode.Single);




	}

	public void OptionsButtonPushed(){

		if (optionsPanel == null)
			optionsPanel = GameObject.Find ("OptionsPanel");

		optionsPanel.SetActive (true);
		rulesDD = GameObject.Find ("RulesDD");
		levelsDD = GameObject.Find ("LevelDD");

		rulesDD.GetComponent<Dropdown> ().ClearOptions ();
		levelsDD.GetComponent<Dropdown> ().ClearOptions ();

		rulesDD.GetComponent<Dropdown>().AddOptions(rulesList);
		levelsDD.GetComponent<Dropdown>().AddOptions(levelsList);

	}

	public void ConfirmButtonPushed(){
		switch (rulesDD.GetComponent<Dropdown> ().captionText.text) {

		case "Mobile":
			GameMasterS.gameMode = GameMasterS.MOBILE;
		
			break;

		case "Board Game":
			GameMasterS.gameMode = GameMasterS.BOARD;

			break;

		
		}


		switch (levelsDD.GetComponent<Dropdown> ().captionText.text) {

		case "International":
			levelToPlay = "Scene_1_Game_Original";
			GameMasterS.level = GameMasterS.INTERN;
			break;

		case "India":
			levelToPlay = "Scene_2_Game_NewBoard";
			GameMasterS.level = GameMasterS.INDIA;
			break;


		}

		optionsPanel.SetActive (false);






	}

	public void ContinueGamePushed(){
		GameMasterS.continuingGame = true;
		char[] seperators = { ',' };


		StreamReader reader = new StreamReader(savepath2);
		string cLevel = reader.ReadLine ();
		string cMode = reader.ReadLine ();


		switch (cLevel) {

		case GameMasterS.INTERN:
			levelToPlay = "Scene_1_Game_Original";
			GameMasterS.level = GameMasterS.INTERN;
			break;

		case GameMasterS.INDIA:
			levelToPlay = "Scene_2_Game_NewBoard";
			GameMasterS.level = GameMasterS.INDIA;
			break;


		}

		switch (cMode) {

		case GameMasterS.MOBILE:
			GameMasterS.gameMode = GameMasterS.MOBILE;
			break;

		case GameMasterS.BOARD:
			GameMasterS.gameMode = GameMasterS.BOARD;
			break;


		}



		reader.Close ();
		StartButtonPushed ();

	}


}
