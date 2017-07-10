using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

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

	// Use this for initialization
	void Start () {
		savepath = Application.persistentDataPath+"//"+"savegame.txt";
		rollButton = GameObject.Find ("Roll Button");
		AssignMovementSpots();
		AssignPlayers ();
		GatherSpaceSelectors ();
		currentPlayer = 0;

		Screen.orientation = ScreenOrientation.Portrait;

		playerNotes [0].GetComponent<Text>().text = "Your Turn";
		loseCount = 0;


		mun = "#";

		if (GameMasterS.level == GameMasterS.INDIA)
			mun = "₹";
		if (GameMasterS.level ==GameMasterS.INTERN)
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
			moveSpots [0, x] = GameObject.Find ("P1 S" + x.ToString());
			moveSpots [1, x] = GameObject.Find ("P2 S" + x.ToString());
			moveSpots [2, x] = GameObject.Find ("P3 S" + x.ToString());
			moveSpots [3, x] = GameObject.Find ("P4 S" + x.ToString());
		}


	}

	void AssignPlayers(){
		players = new Player[4];

		players[0] = new Player(1, GameObject.Find("Player1_Object"));
		players[1] = new Player(2, GameObject.Find("Player2_Object"));	
		players[2] = new Player(3, GameObject.Find("Player3_Object"));
		players[3] = new Player(4, GameObject.Find("Player4_Object"));


	}

	void GatherSpaceSelectors(){

		for(int x = 0; x<36; x++)
		{
			this.GetComponent<SpaceLogicS> ().Gameboard [x].spaceSelectButton = GameObject.Find ("B" + x.ToString());

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
			if(players[x].inTheGame==false)
			{
				currentPlayer = x;
				LoseButtonPushed ();
			}

		}


		reader.Close ();

		currentPlayer = realCurrentPlayer;

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
			
			


	}

	public void Teleport(int player, int loc){



		players [player].playerObject.transform.position = new Vector3 (moveSpots [player, loc].transform.position.x, moveSpots [player, loc].transform.position.y, moveSpots [player, loc].transform.position.z);




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
				doubleRollDisplay1.GetComponent<Text> ().enabled = false;
				doubleRollDisplay2.GetComponent<Text> ().enabled = false;
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

	public void Reroll(){
		rollButton.SetActive (true);
		rollButton.GetComponent<Button> ().interactable = true;

	}
	public void DoubleMove(){
		rollNumberDisplay.GetComponent<Text> ().enabled = true;
		rollNumberDisplay.GetComponent<Text> ().text = currentRoll.ToString();
		StartCoroutine(MovePlayer());



	}

	public void TurnOnTurnActions(){
		if (GameMasterS.gameMode == GameMasterS.BOARD && die1==die2) {
			Reroll ();

		} else {

			tradeButton.SetActive (true);
			mortgageButton.SetActive (true);
			if (GameMasterS.gameMode == GameMasterS.BOARD)
				developButton.SetActive (true);
			endTurnButton.SetActive (true);
		}


	}

	public void TurnOffTurnActions(){
		tradeButton.SetActive (false);
		mortgageButton.SetActive (false);
		developButton.SetActive (false);
		endTurnButton.SetActive (false);


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


			}

			if (horH == "hotel") {
				developSelected.GetComponent<Text> ().text = "Built hotel on " + sname;
				this.GetComponent<SpaceLogicS> ().Gameboard [space].hotel = true;


			}



		
			this.GetComponent<SpaceLogicS> ().Gameboard [space].ActivateVisuals ();
			players [currentPlayer].money -= cost;
			//redeemSelected.GetComponent<Text> ().text = "Success";
			developEngageButton.GetComponent<Button> ().onClick.RemoveAllListeners ();
			developEngageButton.SetActive (false);
			ListenerCleanup ();
		}


	}

	// =========================================================TRADE==================================================


	public void pushTradeButton(){

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
		tradeTitle.GetComponent<Text> ().text =string.Format ("P{0} Trade", (this.GetComponent<SpaceLogicS>().Gameboard[count].owner+1).ToString());

		tradeText.GetComponent<Text> ().text =string.Format ("P{0} will trade {4}{1} for {2}", 
			(currentPlayer+1).ToString(), tradeOffer.ToString(), sname,mun);
		

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
		tradeText.GetComponent<Text> ().text = string.Format ("Traded P{0} {4}{1} for {2}", (this.GetComponent<SpaceLogicS> ().Gameboard [count].owner + 1).ToString (), tradeOffer, sname,mun);
		
		ListenerCleanup ();

	}


	public void RejectTrade(string sname, int count)
	{
		tradeReject.SetActive (false);
		tradeAccept.SetActive (false);
		goBackbutton.SetActive (true);
		tradeText.GetComponent<Text> ().text = string.Format ("P{0} rejected your offed", (this.GetComponent<SpaceLogicS> ().Gameboard [count].owner + 1).ToString ());

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
			this.GetComponent<SaveGameS> ().SaveTheGame ();
			rollButton.GetComponent<Button> ().interactable = true;

		} else {
			youLoseDialog.SetActive (true);
		}

		doublesCount = 0;

	}
		
	public void LoseButtonPushed(){
		youLoseDialog.SetActive (false);
		players [currentPlayer].inTheGame = false;

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



}
