using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;
using System;

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

	public GameObject playerSetup;
	public GameObject nameInputField;
	public GameObject[] colorSelect = new GameObject[4];
	public GameObject[] colorSelectArrows = new GameObject[4];

	public string[] tempNames = new string[4];
	public string[] tempColors = new string[4];
	public int[] tempColorNum = new int[4];
	public string colorSelection;
	public int colorSelectionNum = 666;
	private int setupCount = 0;
	public GameObject playerText;
	public GameObject psNextButton;
	public GameObject psCancelButton;
	public GameObject psBackButton;
	public GameObject psStartButton;
	//private int defaultChoice = 0;
	public bool settingUpPlayer = false;


	// Use this for initialization
	void Start () {
		tempColorNum = new int[4];
		optionsPanel = GameObject.Find ("OptionsPanel");
		nameInputField = GameObject.Find ("NameInput");
		optionsPanel.SetActive (false);

		continueButton= GameObject.Find ("ContinueGame");

		rulesList.Add ("Mobile");
		rulesList.Add ("Board Game");

		levelsList.Add ("International");
		levelsList.Add ("India");
		Debug.Log (savepath);
		//testText.GetComponent<Text> ().text = GameMasterS.saveLoadLocation2;

		if (File.Exists (savepath2))
			continueButton.GetComponent<Button> ().interactable = true;
		else
			continueButton.GetComponent<Button> ().interactable = false;

		playerSetup = GameObject.Find ("PlayerSelect");
		colorSelect [0] = GameObject.Find ("RedChoice");
		colorSelect [1] = GameObject.Find ("BlueChoice");
		colorSelect [2] = GameObject.Find ("GreenChoice");
		colorSelect [3] = GameObject.Find ("YellowChoice");
		colorSelectArrows [0] = GameObject.Find ("RedText");
		colorSelectArrows [1] = GameObject.Find ("BlueText");
		colorSelectArrows [2] = GameObject.Find ("GreenText");
		colorSelectArrows [3] = GameObject.Find ("YellowText");
		playerText = GameObject.Find ("PlayerText");
		psCancelButton = GameObject.Find ("CancelButton");
		psNextButton = GameObject.Find ("NextButton");
		psBackButton = GameObject.Find ("GoBackButton");
		psStartButton = GameObject.Find ("StartButton2");

		psStartButton.SetActive (false);
		psBackButton.SetActive (false);
		foreach (GameObject arrow in colorSelectArrows) {
			arrow.SetActive (false);

		}

		playerSetup.SetActive (false);

	/*	int x = -1;
		foreach (GameObject button in colorSelect) {
			print ("button name " + button.name);
			
			x++;
			print ("button name 2 " + button.GetComponentInChildren<Button> ().name);
			print (x);
			button.GetComponentInChildren<Button>().onClick.RemoveAllListeners ();
			button.GetComponentInChildren<Button>().onClick.AddListener (() => {
				ColorChoicePush();
			});

		}*/
	
	}
	
	// Update is called once per frame
	void Update () {
		if (settingUpPlayer) {
			if (colorSelectionNum != 666) {
				psNextButton.GetComponent<Button> ().interactable = true;
				psStartButton.GetComponent<Button> ().interactable = true;
			} else {


				psNextButton.GetComponent<Button> ().interactable = false;
				psStartButton.GetComponent<Button> ().interactable = false;



			}
			

		}
		
	}

	public void StartButtonPushed(){
		playerSetup.SetActive (true);
		//print (colorSelectionNum);
		settingUpPlayer = true;




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
		SceneManager.LoadSceneAsync (levelToPlay, LoadSceneMode.Single);

	}

	public void CancelButtonPushed(){
		playerSetup.SetActive (false);
		settingUpPlayer = false;
		foreach (GameObject choice in colorSelect) {
			choice.SetActive (true);
			Array.Clear (tempNames, 0, tempNames.Length);

			Array.Clear (tempColors, 0, tempNames.Length);

		}
		setupCount = 0;

	}

	public void BackButtonPushed(){
		Array.Clear (tempNames, setupCount, 1);
		setupCount--;
		colorSelect [tempColorNum[setupCount]].SetActive (true);
		Array.Clear (tempColorNum, setupCount, 1);
		Array.Clear (tempColors, setupCount,1);

		nameInputField.GetComponent<InputField> ().text = string.Format("Player{0}", setupCount + 1);
		playerText.GetComponent<Text> ().text = string.Format ("Player {0} Setup", setupCount + 1);
		colorSelectionNum = 666;

		if (setupCount >0) {
			psCancelButton.SetActive (false);
			psBackButton.SetActive (true);
		}
		if (setupCount ==0) {
			psCancelButton.SetActive (true);
			psBackButton.SetActive (false);
		}

		if (setupCount == 3) {
			psNextButton.SetActive (false);
			psStartButton.SetActive (true);


		}
		if (setupCount < 3) {
			psNextButton.SetActive (true);
			psStartButton.SetActive (false);

		}



	}

	public void NextButtonPushed(){
		tempNames [setupCount] = nameInputField.GetComponent<InputField> ().text;
		tempColors [setupCount] = colorSelection;
		print ("setupnum" + setupCount);
		colorSelectArrows [colorSelectionNum].SetActive (false);
		tempColorNum [setupCount] = colorSelectionNum;
		colorSelect [tempColorNum[setupCount]].SetActive (false);
		setupCount++;
		nameInputField.GetComponent<InputField> ().text = string.Format("Player{0}", setupCount + 1);
		playerText.GetComponent<Text> ().text = string.Format ("Player {0} Setup", setupCount + 1);
		colorSelectionNum = 666;

		if (setupCount >0) {
			psCancelButton.SetActive (false);
			psBackButton.SetActive (true);
		}
		if (setupCount ==0) {
			psCancelButton.SetActive (true);
			psBackButton.SetActive (false);
		}

		if (setupCount == 3) {
			psNextButton.SetActive (false);
			psStartButton.SetActive (true);


		}
		if (setupCount < 3) {
			psNextButton.SetActive (true);
			psStartButton.SetActive (false);

		}



	}

	public void psStartButtonPushed(){
		tempNames [setupCount] = nameInputField.GetComponent<InputField> ().text;
		tempColors [setupCount] = colorSelection;
	
		tempColorNum [setupCount] = colorSelectionNum;
		GameMasterS.customNames = tempNames;
		GameMasterS.colorChoice = tempColors;
		SceneManager.LoadSceneAsync (levelToPlay, LoadSceneMode.Single);

	}

	public void ColorChoicePush(int choice){
		print ("clicked " +choice );
		foreach (GameObject arrow in colorSelectArrows) {
			arrow.SetActive (false);

		}

		colorSelectArrows [choice].SetActive (true);
		switch (choice) {
		case 0:
			colorSelection = "red";
			colorSelectionNum = 0;
			break;
		case 1:
			colorSelection = "blue";
			colorSelectionNum = 1;
			break;
		case 2:
			colorSelection = "green";
			colorSelectionNum = 2;
			break;
		case 3:
			colorSelection = "yellow";
			colorSelectionNum = 3;
			break;





		}



	}


}
