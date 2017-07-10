using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartScreenS : MonoBehaviour {

	public string levelToPlay = "Scene_1_Game_Original";
	public GameObject optionsPanel;

	private GameObject rulesDD;
	private GameObject levelsDD;
	private List<string> rulesList = new List<string>();
	private List<string> levelsList = new List<string>();

	// Use this for initialization
	void Start () {
		optionsPanel = GameObject.Find ("OptionsPanel");
		optionsPanel.SetActive (false);

		rulesList.Add ("Mobile");
		rulesList.Add ("Board Game");

		levelsList.Add ("International");
		levelsList.Add ("India");

		
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
			break;

		case "India":
			levelToPlay = "Scene_2_Game_NewBoard";
			break;


		}

		optionsPanel.SetActive (false);






	}


}
