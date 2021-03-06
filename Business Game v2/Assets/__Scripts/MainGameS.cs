﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;


public class MainGameS : MonoBehaviour {

	public GameObject rollNumberDisplay;
	public GameObject rollButton;


	private GameObject[,] moveSpots;
	public Player[] players;

	public GameObject[] playerNotes;

	public GameObject[] spaceSelectors;

	private int currentRoll;

	public int currentPlayer;


	public GameObject tradeButton;
	public GameObject mortgageButton;
	public GameObject developButton;
	public GameObject endTurnButton;

	public GameObject goBackbutton;


	// MORTGAGE
	public GameObject mortgageTitle;
	public GameObject mortgageText;
	public GameObject mortgageSelected;
	public GameObject mortgageEngageButton;

	//Develop
	public GameObject developTitle;
	public GameObject developText;
	public GameObject developSelected;
	public GameObject developEngageButton;

	//Trade
	public GameObject tradeTitle;
	public GameObject tradeText;
	public GameObject tradeSelected;
	public GameObject tradeInitiateButton;
	public float tradeOffer;
	public GameObject tradeOfferSlider;
	public GameObject tradeOfferText;
	public GameObject tradeAccept;
	public GameObject tradeReject;




	// You Lose
	public GameObject youLoseDialog;
	public GameObject loseButton;
	public GameObject dontLoseButton;
	private int loseCount;

	//You Win
	public GameObject youWinDialog;

	//Color Count
	int red;
	int green;
	int blue;
	int pink;

	List<Space> redProperties = new List<Space>();
	List<Space> greenProperties = new List<Space>();
	List<Space> blueProperties = new List<Space>();
	List<Space> pinkProperties = new List<Space>();


	public string mun;
	private string savepath;


	//NEW RULES
	public int die1 = -1;
	public int die2= -5;
	public GameObject doubleRollDisplay1;
	public GameObject doubleRollDisplay2;
	public int doublesCount;
	public bool wasJustInJail = false;
	public GameObject rerollDoublesButton;

	//Jail Rules
	public GameObject jailRollButton;
	public GameObject jailPayButton;
	public GameObject jailOkayButton;
	public GameObject turnIndicator;

	//NEW TRADE
	public GameObject[] playerButtons;
	public GameObject tradeTitle2;
	public GameObject tradePlayerText;
	public GameObject tradeText2;

	public GameObject tradeText3;
	public GameObject tradeText4;
	public GameObject tradeSelected2;
	public GameObject tradeInitiateButton2;
	public GameObject tradeOfferSlider2;
	public GameObject tradeOfferText2;
	public GameObject tradeAccept2;
	public GameObject tradeReject2;
	public GameObject tradeResult;
	public float cashDesire;
	public float cashOffer;
	//offer display
	public GameObject cashOfferDisplay;
	public GameObject cashDesireDisplay;
	public GameObject propOfferDisplay;
	public GameObject propDesireDisplay;
	public GameObject theyWant;
	public GameObject theyOffer;
	//public GameObject tradeOfferFor;

	public Sprite[,] housesVis = new Sprite[4,2];


	// Use this for initialization
	void Start () {
		//this.GetComponent<Camera> ().orthographicSize = 5;
		savepath = GameMasterS.saveLoadLocation2;
		turnIndicator = GameObject.Find ("TurnIndicator");
		rollButton = GameObject.Find ("Roll Button");
		AssignMovementSpots();
		AssignPlayers ();
		if (!GameMasterS.continuingGame) {
			AssignHouseVisuals ();
			this.GetComponent<NameChangeS> ().LoadNames ();
			this.GetComponent<PlayerColorChangerS> ().InitializePiecesByColor ();
		}
		GatherSpaces ();
		GatherSpaceSelectors ();
		currentPlayer = 0;

		Screen.orientation = ScreenOrientation.Portrait;

		GatherPlayerNotes ();
		playerNotes [0].GetComponent<Text>().text = "Your Turn";
		loseCount = 0;




		mun = "#";

		if (GameMasterS.level == GameMasterS.INDIA)
			mun = "₹";
		if (GameMasterS.level ==GameMasterS.INTERN || GameMasterS.level ==GameMasterS.GENERIC)
			mun = "$";


		rollNumberDisplay = GameObject.Find ("Roll Number Display");
		doubleRollDisplay1= GameObject.Find ("Double Roll Display 1");
		doubleRollDisplay2= GameObject.Find ("Double Roll Display 2");
		if (GameMasterS.gameMode == GameMasterS.MOBILE) {
			doubleRollDisplay1.SetActive (false);
			doubleRollDisplay2.SetActive (false);

			foreach (Player player in players) {
				player.money = 12000;
			}
		}
		if (GameMasterS.gameMode == GameMasterS.BOARD) {
			rollNumberDisplay.SetActive (false);

			foreach (Player player in players) {
				player.money = 40000;
			}

		}

		if (GameMasterS.continuingGame) {
			LoadGameData ();
		}


				
			
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	void AssignMovementSpots(){
		moveSpots = new GameObject[4,36];

		for(int x = 0; x<36; x++)
		{
			GameObject temp = GameObject.Find ("B" + x.ToString ());
			moveSpots [0, x] = temp.transform.FindDeepChild ("P1 Spot").gameObject;
			moveSpots [1, x] = temp.transform.FindDeepChild ("P2 Spot").gameObject;
			moveSpots [2, x] = temp.transform.FindDeepChild ("P3 Spot").gameObject;
			moveSpots [3, x] = temp.transform.FindDeepChild ("P4 Spot").gameObject;
		}


	}

	void AssignPlayers(){
		players = new Player[4];

		players[0] = new Player(1, GameObject.Find("Player1_Object"),GameMasterS.colorChoice[0]);
		players[1] = new Player(2, GameObject.Find("Player2_Object"),GameMasterS.colorChoice[1]);	
		players[2] = new Player(3, GameObject.Find("Player3_Object"),GameMasterS.colorChoice[2]);
		players[3] = new Player(4, GameObject.Find("Player4_Object"),GameMasterS.colorChoice[3]);


	}

	void GatherSpaces(){
		for (int x = 0; x < 36; x++) {
			this.GetComponent<SpaceLogicS> ().Gameboard [x] = GameObject.Find ("B" + x.ToString()).GetComponentInChildren<Space>();

		}


	}

	void GatherSpaceSelectors(){

		for(int x = 0; x<36; x++)
		{
			//print ("B" + x.ToString ());
			//Button buttonn = GameObject.Find ("B" + x.ToString()).GetComponentInChildren<Button>();

			this.GetComponent<SpaceLogicS> ().Gameboard [x].spaceSelectButton = GameObject.Find ("B" + x.ToString()).GetComponentInChildren<Button>();

		}



	}

	void GatherPlayerNotes(){
		for (int x = 0; x < 4; x++) {
			playerNotes [x] = GameObject.Find (string.Format ("Player {0} Note", x + 1));

		}

	}

	public void LoadGameData(){
		char[] seperators = { ',' };
		StreamReader reader = new StreamReader(savepath);


		string trash = reader.ReadLine ();
		trash = reader.ReadLine ();
		int realCurrentPlayer = int.Parse (reader.ReadLine ());
		int numberOfPlayer = int.Parse(reader.ReadLine ());
		for (int x = 0; x < numberOfPlayer; x++) {
			string data = reader.ReadLine ();
			string[] playerData = data.Split (seperators);
			players [x].money = float.Parse(playerData [1]);
			players [x].location = int.Parse(playerData [2]);
			Teleport (x, players [x].location);
			players [x].lostTurn = bool.Parse(playerData [3]);
			players [x].lostTurnTime= int.Parse(playerData [4]);
			players [x].jail=  bool.Parse(playerData [5]);
			players [x].doNotCollectFromGo=  bool.Parse(playerData [6]);
			players [x].inTheGame=  bool.Parse(playerData [7]);
			players [x].color=  playerData [8];
			GameMasterS.customNames [x]=  playerData [9];
			if(players[x].inTheGame==false)
			{
				currentPlayer = x;
				LoseButtonPushed ();
			}

		}


		reader.Close ();
		AssignHouseVisuals ();
		this.GetComponent<PlayerColorChangerS> ().InitializePiecesByColor ();
		this.GetComponent<NameChangeS> ().LoadNames ();
		currentPlayer = realCurrentPlayer;
		turnIndicator.GetComponent<TurnIndicatorS> ().ChangePlayer (currentPlayer);

		playerNotes [currentPlayer].GetComponent<Text> ().text = "Your Turn";
		this.GetComponent<SpaceLogicS> ().LoadGameDataSecond ();
	}
	IEnumerator MovePlayer(){
		int x = currentRoll;
		int y = currentPlayer; 

		while (x != 0) {
			int temp = players [y].GetCurrentPos () + 1;
			if (temp >= 36)
				temp = 0;
			iTween.MoveTo (players [y].playerObject, moveSpots [y, temp].transform.position, 0.5f);
			yield return new WaitForSeconds (.5f);
			players [y].SetCurrentPos(players [y].GetCurrentPos() + 1);

			x--;

			//if pass start, add money;
			if (players [y].GetCurrentPos() == 36) {
				players [y].SetCurrentPos (0);
				if (!players [y].doNotCollectFromGo)
					players [y].MoneyChange (1500);

			}
			

		}
			



		/*if (currentPlayer == 3)
			currentPlayer = 0;
		else
			currentPlayer++;*/

		//rollButton.GetComponent<Button> ().interactable = true;  Test without turn actions

		rollNumberDisplay.GetComponent<Text> ().enabled = false;
		doubleRollDisplay1.GetComponent<Text> ().enabled = false;
		doubleRollDisplay2.GetComponent<Text> ().enabled = false;
		rollButton.SetActive (false);
		//TurnOnTurnActions ();
		this.GetComponent<SpaceLogicS>().ResolveSpace(y,players[y].GetCurrentPos(),currentRoll);
		
		

	}

	public void MovePlayerToLoc(int loc){
		
		int y = currentPlayer; 

			iTween.MoveTo (players [y].playerObject, moveSpots [y,loc].transform.position, 0.5f);
		players [y].SetCurrentPos (loc);
			
			


	}



	public void Teleport(int player, int loc){



		players [player].playerObject.transform.position = new Vector3 (moveSpots [player, loc].transform.position.x, moveSpots [player, loc].transform.position.y, moveSpots [player, loc].transform.position.z);

		players [player].SetCurrentPos (loc);


	}
		
	int DiceRoll(){
		if(GameMasterS.gameMode==GameMasterS.MOBILE)
			return( Random.Range (1, 7));
		if (GameMasterS.gameMode == GameMasterS.BOARD) {
			die1 = Random.Range (1, 7);
			die2 = Random.Range (1, 7);
		}
		return(die1 + die2);
		


	}

	public void OnRollButtonPush(){
		rollButton.GetComponent<Button> ().interactable = false;
		currentRoll = DiceRoll ();
		if (GameMasterS.gameMode == GameMasterS.MOBILE) {
			rollNumberDisplay.GetComponent<Text> ().enabled = true;
			rollNumberDisplay.GetComponent<Text> ().text = currentRoll.ToString ();
		}
		if (GameMasterS.gameMode == GameMasterS.BOARD) {
			doubleRollDisplay1.GetComponent<Text> ().enabled = true;
			doubleRollDisplay1.GetComponent<Text> ().text = die1.ToString ();

			doubleRollDisplay2.GetComponent<Text> ().enabled = true;
			doubleRollDisplay2.GetComponent<Text> ().text = die2.ToString ();

		}

		if (GameMasterS.gameMode == GameMasterS.BOARD && die1 == die2) {
			doublesCount++;
			if (doublesCount == 3) {
				//display note on screen
				this.GetComponent<SpaceLogicS> ().GoToJail ();
				//doubleRollDisplay1.GetComponent<Text> ().enabled = false;
				//doubleRollDisplay2.GetComponent<Text> ().enabled = false;
				rollButton.SetActive (false);
				die1 = -1;
				die2 = -3;
				int jailspace = this.GetComponent<SpaceLogicS> ().jailLoc;
				MovePlayerToLoc (jailspace);

			} else
				StartCoroutine (MovePlayer ());

		} else {
			StartCoroutine (MovePlayer ());
		}


	

	}

	public void MoveToJail(){
		die1 = -1;
		die2 = -3;
		int jailspace = this.GetComponent<SpaceLogicS> ().jailLoc;
		MovePlayerToLoc (jailspace);


	}

	public void MoveToRest(){
		die1 = -1;
		die2 = -3;
		int restSpace = this.GetComponent<SpaceLogicS> ().restLoc;
		MovePlayerToLoc (restSpace);


	}


	public void Reroll(){
		TurnOffTurnActions ();
		rollButton.SetActive (true);
		rollButton.GetComponent<Button> ().interactable = true;

	}
	public void DoubleMove(){
		rollNumberDisplay.GetComponent<Text> ().enabled = true;
		rollNumberDisplay.GetComponent<Text> ().text = currentRoll.ToString();
		StartCoroutine(MovePlayer());



	}

	public void TurnOnTurnActions(){
		//if (GameMasterS.gameMode == GameMasterS.BOARD && die1==die2&& !players[currentPlayer].lostTurn) {
			//Reroll ();

		//} else {
		doubleRollDisplay1.GetComponent<Text> ().enabled = false;
		doubleRollDisplay2.GetComponent<Text> ().enabled = false;
			tradeButton.SetActive (true);
			mortgageButton.SetActive (true);
			if (GameMasterS.gameMode == GameMasterS.BOARD)
				developButton.SetActive (true);
		if (GameMasterS.gameMode == GameMasterS.BOARD && die1 == die2 && !players [currentPlayer].lostTurn&&!wasJustInJail) {
			rerollDoublesButton.SetActive (true);
		}
			else{
			wasJustInJail = false;
			endTurnButton.SetActive (true);
			}
		


	}

	public void TurnOffTurnActions(){
		tradeButton.SetActive (false);
		mortgageButton.SetActive (false);
		developButton.SetActive (false);
		endTurnButton.SetActive (false);
		rerollDoublesButton.SetActive (false);


	}


		
	// =========================================================MORTGAGE==================================================
	public void pushMortgageButton(){
		TurnOffTurnActions ();
		goBackbutton.SetActive (true);
		mortgageTitle.SetActive (true);
		mortgageText.SetActive (true);

		int count = -1;
		foreach (Space space in this.GetComponent<SpaceLogicS>().Gameboard) {
			count++;
			if (space.owner == currentPlayer && !space.isMortgaged) {
				space.spaceSelectButton.GetComponent<Button> ().interactable = true;
				//space.spaceSelectButton.GetComponent<Button> ().onClick.AddListener () = pushMortgageSelectButton (space.sName, space.costToBuy/2);
				string x = space.sName;
				float y = space.costToBuy / 2;
				int z = count;
				print ("mortgage 1: " + y);
				space.spaceSelectButton.GetComponent<Button> ().onClick.AddListener (() => {
					pushMortgageSelectButton (x, y, z);
				});

			} else if (space.owner == currentPlayer && space.isMortgaged) {
				space.spaceSelectButton.GetComponent<Button> ().interactable = true;
				string x = space.sName;
				float y = space.costToBuy / 2;
				int z = count;

				print ("redeem 1: " + y);
				space.spaceSelectButton.GetComponent<Button> ().onClick.AddListener (() => {
					pushUnMortgageSelectButton (x, y, z);
				});

			}

		}
			
	}

	public void pushMortgageSelectButton(string sname, float mortgage, int space ){
			mortgageTitle.GetComponent<Text> ().text = "Mortgage";
			mortgageText.SetActive (false);
			mortgageSelected.SetActive (true);
			mortgageSelected.GetComponent<Text> ().text = sname + " - " + mortgage;
			mortgageEngageButton.SetActive (true);
			mortgageEngageButton.GetComponentInChildren<Text> ().text = "Mortgage";

			mortgageEngageButton.GetComponent<Button> ().onClick.RemoveAllListeners ();
			mortgageEngageButton.GetComponent<Button>().onClick.AddListener (() => {
			CompleteMortgage(sname, mortgage, space);

		});
			

	}

	public void pushUnMortgageSelectButton(string sname, float mortgage, int space ){
		mortgageTitle.GetComponent<Text> ().text = "Redeem";
		mortgageText.SetActive (false);
		mortgageSelected.SetActive (true);
		mortgageSelected.GetComponent<Text> ().text = sname + " - " + mortgage;
		mortgageEngageButton.SetActive (true);
		mortgageEngageButton.GetComponentInChildren<Text> ().text = "Redeem";

		if (players [currentPlayer].money > mortgage) {
			
			mortgageEngageButton.GetComponent<Button> ().onClick.RemoveAllListeners ();
			mortgageEngageButton.GetComponent<Button> ().onClick.AddListener (() => {
				CompleteUnMortgage (sname, mortgage, space);
			});
		} else {
			mortgageSelected.GetComponent<Text> ().text = "Not enough money";

		}

	}


	public void CompleteMortgage(string sname, float mortgage, int space){
		this.GetComponent<SpaceLogicS> ().Gameboard [space].isMortgaged = true;
		//this.GetComponent<SpaceLogicS> ().Gameboard [space].DeactivateVisuals ();
		players [currentPlayer].money += mortgage;
		mortgageSelected.GetComponent<Text> ().text = sname + " mortgaged!";
		mortgageEngageButton.GetComponent<Button> ().onClick.RemoveAllListeners ();
		mortgageEngageButton.SetActive (false);
		this.GetComponent<TokensS> ().ChangeMortgage (space, currentPlayer);
		ListenerCleanup ();

	}

	public void CompleteUnMortgage(string sname, float mortgage, int space){
		this.GetComponent<SpaceLogicS> ().Gameboard [space].isMortgaged = false;
		//this.GetComponent<SpaceLogicS> ().Gameboard [space].DeactivateVisuals ();
		players [currentPlayer].money -= mortgage;
		mortgageSelected.GetComponent<Text> ().text = sname + " redeemed!";
		mortgageEngageButton.GetComponent<Button> ().onClick.RemoveAllListeners ();
		mortgageEngageButton.SetActive (false);
		this.GetComponent<TokensS> ().ChangeUnMortgage (space, currentPlayer);
		ListenerCleanup ();

	}


	// =========================================================Develop==================================================

	public void pushDevelopButton(){
		TurnOffTurnActions ();
		developTitle.SetActive (true);
		developText.SetActive (true);
		goBackbutton.SetActive (true);

		redProperties.Clear ();
		blueProperties.Clear ();
		greenProperties.Clear ();
		pinkProperties.Clear ();

		ColorCount ();

		int count = -1;
		foreach (Space space in this.GetComponent<SpaceLogicS>().Gameboard) {
			count++;
			if (space.owner == currentPlayer) {
				if(DevelopCheckSpace(space)){
					//print ("develop check was true");
					space.spaceSelectButton.GetComponent<Button> ().interactable = true;
					//space.spaceSelectButton.GetComponent<Button> ().onClick.AddListener () = pushMortgageSelectButton (space.sName, space.costToBuy/2);
					string x = space.sName;
					float y = space.costPerHouse;
					int z = count;
					space.spaceSelectButton.GetComponent<Button> ().onClick.AddListener (() => {
						pushDevelopSelectButton (x, y,z);
					});

				}

			}

		}

	}

	public void ColorCount()
	{
		red = 0; 
		pink = 0;
		green = 0;
		blue = 0;

		foreach (Space space in this.GetComponent<SpaceLogicS>().Gameboard) {
			
			if (space.owner == currentPlayer) {
				switch (space.color){

				case colorGroup.red:
					red++;
					redProperties.Add (space);
					break;
				case colorGroup.pink:
					pink++;
					pinkProperties.Add (space);
					break;
				case colorGroup.green:
					green++;
					greenProperties.Add (space);
					break;
				case colorGroup.blue:
					blue++;
					blueProperties.Add (space);
					break;

				}

			}


		}



	}

	public bool DevelopCheckSpace(Space space)
	{
		bool flag = false;
		switch (space.color) {
		case colorGroup.red:
			if (red >= 2) {
				foreach (Space property in redProperties) {
					if(property !=space){
						if (space.numberOfHouses <= property.numberOfHouses) {
							//print("red true "+ space.sName);
							flag = true;
							} 
						else
							return false;
						}
				}
				return flag;
			}
			
			break;
		case colorGroup.pink:
			if (pink >= 2) {
				foreach (Space property in pinkProperties) {
					if(property !=space){
						if (space.numberOfHouses <= property.numberOfHouses) {
							flag = true;
						} 
						else
							return false;
					}
				}
				return flag;
			}

			break;
		case colorGroup.green:
			if (green >= 2) {
				foreach (Space property in greenProperties) {
					if(property !=space){
						if (space.numberOfHouses <= property.numberOfHouses) {
							flag = true;
						} else
							return false;
					}
				}
				return flag;
			}

			break;
		case colorGroup.blue:
			if (blue >= 2) {
				foreach (Space property in blueProperties) {
					if(property !=space){
						if (space.numberOfHouses <= property.numberOfHouses) {
							flag = true;
						} else {
							return false;
						}
					}
				}
				return flag;
			}

			break;


		}
		return false;


	}
	/*public float RedeemCheckPrice(Space space){
		if (space.numberOfHouses<3) {
			return space.costPerHouse

		}


	}*/

	public void pushDevelopSelectButton(string sname, float price, int space ){

		developText.SetActive (false);
		developSelected.SetActive (true);

		string houseOrHotel = "";

		if (this.GetComponent<SpaceLogicS> ().Gameboard [space].numberOfHouses < 3) {
			developSelected.GetComponent<Text> ().text = sname + " build house: "+mun + this.GetComponent<SpaceLogicS> ().Gameboard [space].costPerHouse;
			houseOrHotel = "house";

		} else if (!this.GetComponent<SpaceLogicS> ().Gameboard [space].hotel) {
			developSelected.GetComponent<Text> ().text = sname + " build hotel: "+mun+ this.GetComponent<SpaceLogicS> ().Gameboard [space].costPerHouse;
			houseOrHotel = "hotel";
		} else {
			developSelected.GetComponent<Text> ().text = sname + " is full";
		}

		developEngageButton.SetActive (true);


		developEngageButton.GetComponent<Button> ().onClick.RemoveAllListeners ();
		//print ("I'm here");
		//DO NOT ADD IF everything built
			if (this.GetComponent<SpaceLogicS> ().Gameboard [space].numberOfHouses == 3 && this.GetComponent<SpaceLogicS> ().Gameboard [space].hotel)
			developEngageButton.SetActive (false);
		//print ("and here");
		developEngageButton.GetComponent<Button>().onClick.AddListener (() => {
			CompleteDevelop(sname, price, space, houseOrHotel);

		});
	
	}



	public void CompleteDevelop(string sname, float cost, int space, string horH){

		if (players [currentPlayer].money < cost) {
			developSelected.GetComponent<Text> ().text = "Not enough money";
			developEngageButton.GetComponent<Button> ().onClick.RemoveAllListeners ();
			developEngageButton.SetActive (false);
		} else {
			
			if (horH == "house") {
				developSelected.GetComponent<Text> ().text = "Built house on " + sname;
				this.GetComponent<SpaceLogicS> ().Gameboard [space].numberOfHouses++;
				print ("name"+housesVis [currentPlayer, 0].name);
				this.GetComponent<SpaceLogicS> ().Gameboard [space].ActivateVisuals (housesVis[currentPlayer,0]);


			}

			if (horH == "hotel") {
				developSelected.GetComponent<Text> ().text = "Built hotel on " + sname;
				this.GetComponent<SpaceLogicS> ().Gameboard [space].hotel = true;
				this.GetComponent<SpaceLogicS> ().Gameboard [space].ActivateVisuals (housesVis[currentPlayer,1]);


			}



		

			players [currentPlayer].money -= cost;
			//redeemSelected.GetComponent<Text> ().text = "Success";
			developEngageButton.GetComponent<Button> ().onClick.RemoveAllListeners ();
			developEngageButton.SetActive (false);
			ListenerCleanup ();
		}


	}

	// =========================================================TRADE==================================================


	public void pushTradeButton(){
		
		if (GameMasterS.gameMode == GameMasterS.MOBILE) {
			TurnOffTurnActions ();
			tradeTitle.SetActive (true);
			tradeText.SetActive (true);
			goBackbutton.SetActive (true);

			tradeTitle.GetComponent<Text> ().text = "Trade";
			int count = -1;
			foreach (Space space in this.GetComponent<SpaceLogicS>().Gameboard) {
				count++;
				if (players [currentPlayer].money > 0) {
					if (space.owned == true && space.owner != currentPlayer) {
						space.spaceSelectButton.GetComponent<Button> ().interactable = true;
						//space.spaceSelectButton.GetComponent<Button> ().onClick.AddListener () = pushMortgageSelectButton (space.sName, space.costToBuy/2);
						string x = space.sName;
						int z = count;
	
						space.spaceSelectButton.GetComponent<Button> ().onClick.AddListener (() => {
							pushTradeSelectButton (x, z);
						});

					} 

				} else {
					tradeText.SetActive (false);
					tradeSelected.SetActive (true);
					tradeSelected.GetComponent<Text> ().text = "You don't have any money to trade";


				}
			}
		}
		if (GameMasterS.gameMode == GameMasterS.BOARD) {
			TurnOffTurnActions ();
			tradeTitle.SetActive (true);
			tradePlayerText.SetActive (true);
			goBackbutton.SetActive (true);

			foreach (GameObject button in playerButtons)
				button.GetComponent<Button> ().onClick.RemoveAllListeners ();
			int y = 0;
			for (int x = 0; x < 4; x++) {
				if (x != currentPlayer & players [x].inTheGame) {
					playerButtons [y].SetActive (true);
					string z = x.ToString ();
					playerButtons[y].GetComponent<Button> ().onClick.AddListener (() => {
						setDesire (int.Parse(z));
						});
					playerButtons [y].GetComponentInChildren<Text> ().text = "P" + (x + 1).ToString ();
					y++;
				}


			}




	
		}

	}

	public void setDesire(int enemy){
		foreach (GameObject button in playerButtons)
			button.SetActive (false);
		int count = -1;
		print ("enemy" + enemy.ToString());
		foreach (Space space in this.GetComponent<SpaceLogicS>().Gameboard) {
			count++;
			if (space.owned == true && space.owner == enemy) {
				space.spaceSelectButton.GetComponent<Button> ().interactable = true;
				string x = space.sName;
				int z = count;
				space.spaceSelectButton.GetComponent<Button> ().onClick.AddListener (() => {
					pushTradeSelectButton2 (x, z);
				});
			}
		}

		tradeOfferSlider2.SetActive (true);
		tradePlayerText.SetActive (false);
		tradeText2.SetActive (true);
		tradeOfferSlider2.GetComponent<Slider> ().maxValue = players [enemy].money;
		tradeOfferText2.SetActive (true);

		tradeInitiateButton2.SetActive (true);
		tradeInitiateButton2.GetComponentInChildren<Text> ().text = "Continue";
		tradeInitiateButton2.GetComponent<Button> ().onClick.RemoveAllListeners ();
		tradeInitiateButton2.GetComponent<Button> ().onClick.AddListener (() => {
			SetOffer(enemy);
		});

	}

	public void pushTradeSelectButton2(string sname, int count)
	{
		if (!this.GetComponent<SpaceLogicS> ().Gameboard [count].selected) {
			//desireList.Add (count);
			this.GetComponent<TokensS> ().ChangeSelect (count, this.GetComponent<SpaceLogicS> ().Gameboard [count].owner);
			this.GetComponent<SpaceLogicS> ().Gameboard [count].SetSelected (true);

		} else {
			if (this.GetComponent<SpaceLogicS> ().Gameboard [count].selected) {
				//desireList.
				this.GetComponent<SpaceLogicS> ().Gameboard [count].SetSelected (false);
				this.GetComponent<TokensS> ().ChangeUnselect (count, this.GetComponent<SpaceLogicS> ().Gameboard [count].owner);

			}
		}
		


	}

	public void SetOffer(int enemy){
		cashDesire = tradeOfferSlider2.GetComponent<Slider> ().value;
		tradeOfferSlider2.GetComponent<Slider> ().maxValue = players [currentPlayer].money;
		tradeOfferSlider2.GetComponent<Slider> ().value = 0;
		tradeOfferText2.GetComponent<Text> ().text = 0.ToString();
		tradeText3.SetActive (true);
		tradeText2.SetActive (false);
		ListenerCleanup ();
		int count = -1;
		foreach (Space space in this.GetComponent<SpaceLogicS>().Gameboard) {
			count++;
			if (space.owned == true && space.owner == currentPlayer) {
				space.spaceSelectButton.GetComponent<Button> ().interactable = true;
				string x = space.sName;
				int z = count;
				space.spaceSelectButton.GetComponent<Button> ().onClick.AddListener (() => {
					pushTradeSelectButton2 (x, z);
				});
			}
		}
			tradeInitiateButton2.GetComponentInChildren<Text> ().text = "Initiate";
			tradeInitiateButton2.GetComponent<Button> ().onClick.RemoveAllListeners ();
		tradeInitiateButton2.GetComponent<Button> ().onClick.AddListener (() => {
			InitiateTrade (enemy);
		});



	}

	public void InitiateTrade(int enemy){
		cashOffer = tradeOfferSlider2.GetComponent<Slider> ().value;
		tradeText3.SetActive (false);
		tradeOfferSlider2.SetActive (false);
		tradeOfferText2.SetActive (false);
		tradeInitiateButton2.SetActive (false);
		//tradeOfferFor.SetActive (true);
		goBackbutton.SetActive (false);
		theyWant.SetActive (true);
		theyOffer.SetActive (true);

		turnIndicator.GetComponent<TurnIndicatorS>().ChangeText(string.Format("P{0} Decision",enemy+1));
		tradeText4.GetComponent<Text> ().text = string.Format ("P{0} wants to trade", currentPlayer+1);

		tradeText4.SetActive (true);

		foreach (Space space in this.GetComponent<SpaceLogicS>().Gameboard) {
			if(space.selected && space.owner == currentPlayer)
				propOfferDisplay.SetActive(true);
			if (space.selected && space.owner == enemy)
				propDesireDisplay.SetActive(true);

		}

		if (cashOffer != 0) {
			cashOfferDisplay.SetActive (true);
			cashOfferDisplay.GetComponent<Text> ().text = string.Format ("{0}{1}", mun, cashOffer);
		}

		if (cashDesire != 0) {
			cashDesireDisplay.SetActive (true);
			cashDesireDisplay.GetComponent<Text> ().text = string.Format ("{0}{1}", mun, cashDesire);
		}

		tradeAccept2.GetComponent<Button> ().onClick.RemoveAllListeners ();
		tradeReject2.GetComponent<Button> ().onClick.RemoveAllListeners ();
		tradeAccept2.SetActive (true);
		tradeReject2.SetActive (true);

		tradeAccept2.GetComponent<Button> ().onClick.AddListener (() => {
			AcceptTrade2 (enemy);
		});

		tradeReject2.GetComponent<Button> ().onClick.AddListener (() => {
			RejectTrade2 (enemy);
		});

			

	}

	void AcceptTrade2(int enemy){
		//tradeOfferFor.SetActive (false);
		tradeAccept2.SetActive (false);
		tradeReject2.SetActive (false);
		cashOfferDisplay.SetActive (false);
		cashDesireDisplay.SetActive (false);
		tradeText4.SetActive (false);
		propDesireDisplay.SetActive(false);
		propOfferDisplay.SetActive(false);
		theyWant.SetActive (false);
		theyOffer.SetActive (false);

		players [enemy].money += cashOffer;
		players [enemy].money -= cashDesire;

		players [currentPlayer].money += cashDesire;
		players [currentPlayer].money -= cashOffer;
		int count = -1;
		foreach (Space space in this.GetComponent<SpaceLogicS>().Gameboard) {
			count++;
			if (space.selected && space.owner == currentPlayer) {
				this.GetComponent<SpaceLogicS> ().Gameboard [count].selected = false;

				this.GetComponent<TokensS> ().ChangeUnselect (count, this.GetComponent<SpaceLogicS> ().Gameboard [count].owner);
				this.GetComponent<TokensS> ().ChangeOwnership(count, enemy);
				space.owner = enemy;
				if (space.hotel) {
					this.GetComponent<SpaceLogicS> ().Gameboard [count].ActivateVisuals (housesVis [space.owner, 1]);

				} else {
					this.GetComponent<SpaceLogicS> ().Gameboard [count].ActivateVisuals (housesVis [space.owner, 0]);

				}
			} else {
				if (space.selected && space.owner == enemy) {
					this.GetComponent<SpaceLogicS> ().Gameboard [count].selected = false;
					this.GetComponent<TokensS> ().ChangeUnselect (count, this.GetComponent<SpaceLogicS> ().Gameboard [count].owner);
					this.GetComponent<TokensS> ().ChangeOwnership(count, currentPlayer);
					space.owner = currentPlayer;
					if (space.hotel) {
						this.GetComponent<SpaceLogicS> ().Gameboard [count].ActivateVisuals (housesVis [space.owner, 1]);

					} else {
						this.GetComponent<SpaceLogicS> ().Gameboard [count].ActivateVisuals (housesVis [space.owner, 0]);

					}
				}
			}

		}
		ListenerCleanup ();
		goBackbutton.SetActive (true);
		tradeResult.GetComponent<Text> ().text = "Trade Success!";
		tradeResult.SetActive (true);

		turnIndicator.GetComponent<TurnIndicatorS> ().ChangePlayer (currentPlayer);



	}
	void RejectTrade2(int enemy){
		tradeAccept2.SetActive (false);
		tradeReject2.SetActive (false);
		cashOfferDisplay.SetActive (false);
		cashDesireDisplay.SetActive (false);
		tradeText4.SetActive (false);
		propDesireDisplay.SetActive(false);
		propOfferDisplay.SetActive(false);
		theyWant.SetActive (false);
		theyOffer.SetActive (false);
		turnIndicator.GetComponent<TurnIndicatorS> ().ChangePlayer (currentPlayer);
		int count = -1;
		foreach (Space space in this.GetComponent<SpaceLogicS>().Gameboard) {
			count++;
			this.GetComponent<SpaceLogicS> ().Gameboard [count].selected = false;
		}
		ListenerCleanup ();
		goBackbutton.SetActive (true);
		tradeResult.GetComponent<Text> ().text = "Trade Rejected";
		tradeResult.SetActive (true);


	}
	public void pushTradeSelectButton(string sname, int count)
	{ 
		
		tradeOffer = 0;
		tradeOfferSlider.GetComponent<Slider> ().maxValue = players [currentPlayer].money;
		tradeOfferSlider.SetActive (true);

		tradeOfferText.SetActive (true);
		tradeText.SetActive (false); 
		tradeSelected.SetActive (true);
		tradeSelected.GetComponent<Text> ().text = sname + " - " + "Offer:";
		tradeInitiateButton.SetActive (true);


		tradeInitiateButton.GetComponent<Button> ().onClick.RemoveAllListeners ();
		tradeInitiateButton.GetComponent<Button>().onClick.AddListener (() => {
			OfferTrade(sname, count );
		});
	}


	public void OfferTrade(string sname, int count){
		tradeOfferSlider.SetActive (false);
		tradeOffer = tradeOfferSlider.GetComponent<Slider> ().value;
		tradeOfferSlider.GetComponent<Slider> ().value = 0;
		tradeOfferText.SetActive (false);
		tradeOfferText.GetComponent<Text>().text=mun+"0";
		//tradeOffer = tradeOfferSlider.GetComponent<Slider> ().value;
		tradeInitiateButton.SetActive (false);
		tradeInitiateButton.GetComponent<Button> ().onClick.RemoveAllListeners ();
		goBackbutton.SetActive (false);
		tradeReject.SetActive (true);
		tradeAccept.SetActive (true);
		tradeSelected.SetActive (false);

		tradeAccept.GetComponent<Button> ().onClick.RemoveAllListeners ();
		tradeAccept.GetComponent<Button>().onClick.AddListener (() => {
			AcceptTrade(sname, count );
		});

		tradeReject.GetComponent<Button> ().onClick.RemoveAllListeners ();
		tradeReject.GetComponent<Button>().onClick.AddListener (() => {
			RejectTrade(sname, count );
		});

		tradeText.SetActive (true);
		//tradeTitle.GetComponent<Text> ().text =string.Format ("P{0} Trade", this.GetComponent<SpaceLogicS>().Gameboard[count].owner+1);
		tradeTitle.GetComponent<Text> ().text = "Trade Offer";
		tradeText.GetComponent<Text> ().text =string.Format ("P{0} will trade {3}{1} for {2}", 
			currentPlayer+1, tradeOffer, sname,mun);
		turnIndicator.GetComponent<TurnIndicatorS>().ChangeText(string.Format("P{0} Decision", 
			this.GetComponent<SpaceLogicS>().Gameboard[count].owner+1));
		

	}

	public void AcceptTrade(string sname, int count)
	{
		tradeReject.SetActive (false);
		tradeAccept.SetActive (false);
		goBackbutton.SetActive (true);
		players [this.GetComponent<SpaceLogicS> ().Gameboard [count].owner].money += tradeOffer;
		players [currentPlayer].money -= tradeOffer;
		this.GetComponent<SpaceLogicS> ().Gameboard [count].owner = currentPlayer;

		this.GetComponent<TokensS> ().ChangeOwnership (count, currentPlayer);

		tradeSelected.SetActive (false);
		tradeText.GetComponent<Text> ().text = string.Format ("Traded P{0} {3}{1} for {2}", (this.GetComponent<SpaceLogicS> ().Gameboard [count].owner + 1).ToString (), tradeOffer, sname,mun);
		turnIndicator.GetComponent<TurnIndicatorS> ().ChangePlayer (currentPlayer);
		ListenerCleanup ();

	}


	public void RejectTrade(string sname, int count)
	{
		tradeReject.SetActive (false);
		tradeAccept.SetActive (false);
		goBackbutton.SetActive (true);
		tradeText.GetComponent<Text> ().text = string.Format ("P{0} rejected your offer", (this.GetComponent<SpaceLogicS> ().Gameboard [count].owner + 1).ToString ());
		turnIndicator.GetComponent<TurnIndicatorS> ().ChangePlayer (currentPlayer);
		ListenerCleanup ();
	}


	//===================================================================================================================================================================
	public void pushGoBackButton(){
		goBackbutton.SetActive (false);
		mortgageText.SetActive (false);
		mortgageTitle.SetActive (false);
		mortgageSelected.SetActive (false);
		mortgageEngageButton.SetActive (false);
		developText.SetActive (false);
		developTitle.SetActive (false);
		developSelected.SetActive (false);
		developEngageButton.SetActive (false);
		tradeText.SetActive (false);
		tradeTitle.SetActive (false);
		tradeSelected.SetActive (false);
		tradeInitiateButton.SetActive (false);

		tradeTitle2.SetActive (false);
		tradePlayerText.SetActive (false);
		tradeText2.SetActive (false);

		tradeText3.SetActive (false);
		tradeText4.SetActive (false);
		tradeSelected2.SetActive (false);
		tradeInitiateButton2.SetActive (false);
		tradeOfferSlider2.SetActive (false);
		tradeOfferText2.SetActive (false);
		foreach (GameObject button in playerButtons)
			button.SetActive (false);
		tradeResult.SetActive (false);


		ListenerCleanup ();
		foreach (Space space in this.GetComponent<SpaceLogicS>().Gameboard) {
			space.spaceSelectButton.GetComponent<Button> ().interactable = false;
		}
		TurnOnTurnActions ();


	}
	public void pushEndTurnButton(){
		bool flag = false;

		if (players [currentPlayer].money >= 0) {

			if (!players [currentPlayer].lostTurn)
				playerNotes [currentPlayer].GetComponent<Text> ().text = "";



			do {
				flag = false;
				currentPlayer++;
				if (currentPlayer == 4)
					currentPlayer = 0;
				
				
				if (players [currentPlayer].lostTurn &&!players [currentPlayer].jail ) {
					print("lostturn:"+players [currentPlayer].lostTurn+players [currentPlayer].lostTurnTime);
					players [currentPlayer].lostTurnTime--;
					if (players [currentPlayer].lostTurnTime == 1)
						playerNotes [currentPlayer].GetComponent<Text> ().text = "Lost Turns: 1";
					if (players [currentPlayer].lostTurnTime == 0) {
						players [currentPlayer].lostTurn = false;
						flag = true;
						playerNotes [currentPlayer].GetComponent<Text> ().text = "";
					}
					flag=true;
				}
				print(currentPlayer.ToString() + players [currentPlayer].inTheGame + flag);
			} while (flag == true || players [currentPlayer].inTheGame == false);

			if (players [currentPlayer].jail) {
				players [currentPlayer].lostTurnTime--;
				if (players [currentPlayer].lostTurnTime > 0) {
					turnIndicator.GetComponent<TurnIndicatorS> ().ChangePlayer (currentPlayer);
					playerNotes [currentPlayer].GetComponent<Text> ().text = "Lost Turns: " + players [currentPlayer].lostTurnTime;
					DisplayJailActions ();
				}
				else {
					if (players [currentPlayer].lostTurnTime == 0) {
						playerNotes [currentPlayer].GetComponent<Text> ().text = "";
						players [currentPlayer].jail = false;
					}
				}
			}

			if (!players [currentPlayer].jail) {
				turnIndicator.GetComponent<TurnIndicatorS> ().ChangePlayer (currentPlayer);
				playerNotes [currentPlayer].GetComponent<Text> ().text = "Your Turn";
				rollButton.SetActive (true);
				tradeButton.SetActive (false);
				mortgageButton.SetActive (false);
				developButton.SetActive (false);
				endTurnButton.SetActive (false);
				this.GetComponent<SaveGameS> ().SaveTheGame ();
				rollButton.GetComponent<Button> ().interactable = true;
			}

		} else {
			youLoseDialog.SetActive (true);
		}

		doublesCount = 0;

	}

	public void DisplayJailActions(){
		TurnOffTurnActions ();
		this.GetComponent<SpaceLogicS> ().spaceTitle.SetActive (true);
		this.GetComponent<SpaceLogicS> ().spaceTitle.GetComponent<Text> ().text = "Jail Actions";
		jailPayButton.SetActive (true);
		jailPayButton.GetComponentInChildren<Text> ().text = string.Format ("Pay {0}", mun);
		jailRollButton.SetActive (true);
	
	}

	public void JailRollButtonPush(){
		DiceRoll ();
		doubleRollDisplay1.GetComponent<Text> ().enabled = true;
		doubleRollDisplay1.GetComponent<Text> ().text = die1.ToString ();

		doubleRollDisplay2.GetComponent<Text> ().enabled = true;
		doubleRollDisplay2.GetComponent<Text> ().text = die2.ToString ();
		jailPayButton.SetActive (false);
		jailRollButton.SetActive (false);

		if (die1 == die2) {
			this.GetComponent<SpaceLogicS> ().spaceTitle.GetComponent<Text> ().text = "Out of Jail!";
			this.GetComponent<SpaceLogicS> ().spaceText.GetComponent<Text> ().text = "Escaped! Move next turn";
			this.GetComponent<SpaceLogicS> ().spaceText.SetActive (true);
			players [currentPlayer].jail = false;
			players [currentPlayer].lostTurn = false;
			players [currentPlayer].lostTurnTime = 0;
			playerNotes [currentPlayer].GetComponent<Text> ().text = "";
			//jailOkayButton.SetActive (true);
			wasJustInJail=true;
			this.GetComponent<SpaceLogicS> ().oKButton.SetActive (true);


		} else {
			this.GetComponent<SpaceLogicS> ().spaceTitle.GetComponent<Text> ().text = "Still in Jail";
			this.GetComponent<SpaceLogicS> ().oKButton.SetActive (true);

		}


	}

	public void JailPayButtonPush(){
		players [currentPlayer].money -= 500;
		players [currentPlayer].jail = false;
		players [currentPlayer].lostTurn = false;
		players [currentPlayer].lostTurnTime = 0;
		jailPayButton.SetActive (false);
		jailRollButton.SetActive (false);
		this.GetComponent<SpaceLogicS> ().spaceText.SetActive (true);
		this.GetComponent<SpaceLogicS> ().spaceTitle.GetComponent<Text> ().text = "Out of Jail!";
		this.GetComponent<SpaceLogicS> ().spaceText.GetComponent<Text> ().text = "Paid! Move next turn";
		playerNotes [currentPlayer].GetComponent<Text> ().text = "";
		wasJustInJail = true;
		this.GetComponent<SpaceLogicS> ().oKButton.SetActive (true);



	}

	public void jailOkayButtonPush(){
		doubleRollDisplay1.GetComponent<Text> ().enabled = false;
		doubleRollDisplay2.GetComponent<Text> ().enabled = false;

		jailOkayButton.SetActive (false);
		this.GetComponent<SpaceLogicS> ().spaceText.GetComponent<Text> ().text = "Escaped!";
		playerNotes [currentPlayer].GetComponent<Text> ().text = "";
		this.GetComponent<SpaceLogicS> ().oKButton.SetActive (true);


	}
		
	public void LoseButtonPushed(){
		youLoseDialog.SetActive (false);
		players [currentPlayer].inTheGame = false;
		jailPayButton.SetActive (false);
		jailRollButton.SetActive (false);
		GameObject.Find ("Player " + (currentPlayer + 1).ToString ()).SetActive (false);
		GameObject.Find ("Player" + (currentPlayer + 1).ToString () + "_Object").SetActive (false);
		loseCount++;

		int count = 0;
		foreach (Space space in this.GetComponent<SpaceLogicS>().Gameboard) {
			if (space.owner == currentPlayer) {
				space.owned = false;
				space.owner = 666;
				this.GetComponent<TokensS> ().ChangeOwnership (count, 666);
				space.DeactivateVisuals ();
			}

			count++;
		}

		if (loseCount <= 2)
			ForceEndTurn ();
		else
			GameWin ();
	}

	public void DontLoseButtonPushed(){
		youLoseDialog.SetActive (false);

	}

	public void ForceEndTurn (){
		bool flag = false;
		if (!players [currentPlayer].lostTurn)
			playerNotes [currentPlayer].GetComponent<Text> ().text = "";



		do {
			flag = false;
			currentPlayer++;
			if (currentPlayer == 4)
				currentPlayer = 0;


			if (players [currentPlayer].lostTurn) {
				players [currentPlayer].lostTurnTime--;
				if (players [currentPlayer].lostTurnTime == 1)
					playerNotes [currentPlayer].GetComponent<Text> ().text = "Lost Turns: 1";
				if (players [currentPlayer].lostTurnTime == 0) {
					players [currentPlayer].lostTurn = false;
					flag = true;
					playerNotes [currentPlayer].GetComponent<Text> ().text = "";
				}

			}	
		} while (flag == true || players [currentPlayer].inTheGame == false);


		playerNotes [currentPlayer].GetComponent<Text> ().text = "Your Turn";
		rollButton.SetActive (true);
		tradeButton.SetActive (false);
		mortgageButton.SetActive (false);
		developButton.SetActive (false);
		endTurnButton.SetActive (false);
		rollButton.GetComponent<Button> ().interactable = true;


	}

	public void GameWin(){
		int winner=666;
		rollButton.SetActive (false);
		tradeButton.SetActive (false);
		mortgageButton.SetActive (false);
		developButton.SetActive (false);
		endTurnButton.SetActive (false);
		int count = 0;

		foreach (Player player in players) {
			if (player.inTheGame)
				winner = count;
			
			count++;
		}

		youWinDialog.SetActive (true);
		youWinDialog.GetComponentInChildren<Text> ().text = "P" + (winner+1).ToString () + " Wins!";
		


	}

	public void ListenerCleanup(){
		foreach (Space space in this.GetComponent<SpaceLogicS>().Gameboard)
			space.spaceSelectButton.GetComponent<Button> ().onClick.RemoveAllListeners ();

	}

	public void xButton(){
		SceneManager.LoadSceneAsync ("Scene_0_Start", LoadSceneMode.Single);

	}

	public void loadHouseVisuals(){
		int count = -1;
		print ("loading visuals");
		foreach (Space space in this.GetComponent<SpaceLogicS>().Gameboard) {
			count++;
			if (space.owned){
				if (space.hotel) {
					this.GetComponent<SpaceLogicS> ().Gameboard [count].ActivateVisuals (housesVis [space.owner, 1]);

				} else {
					this.GetComponent<SpaceLogicS> ().Gameboard [count].ActivateVisuals (housesVis [space.owner, 0]);
				}
			}
		}


	}



	public void AssignHouseVisuals(){
		for (int x = 0; x < 4; x++) {
			print ("player color= " + players [x].color);
			switch (players [x].color) {

			case "red":
				housesVis [x, 0] = this.GetComponentInChildren<VisualHolderS> ().redPlayerHouse;
				housesVis [x, 1] = this.GetComponentInChildren<VisualHolderS> ().redPlayerHotel;
				break;

			case "blue":
				housesVis [x, 0] = this.GetComponentInChildren<VisualHolderS> ().bluePlayerHouse;
				housesVis [x, 1] = this.GetComponentInChildren<VisualHolderS> ().bluePlayerHotel;
				break;

			case "green":
				housesVis [x, 0] = this.GetComponentInChildren<VisualHolderS> ().greenPlayerHouse;
				housesVis [x, 1] = this.GetComponentInChildren<VisualHolderS> ().greenPlayerHotel;
				break;

			case "yellow":
				housesVis [x, 0] = this.GetComponentInChildren<VisualHolderS> ().yellowPlayerHouse;
				housesVis [x, 1] = this.GetComponentInChildren<VisualHolderS> ().yellowPlayerHotel;
				break;



			}

		}
		


	}



}
