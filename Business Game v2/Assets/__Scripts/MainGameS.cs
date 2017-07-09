using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainGameS : MonoBehaviour {

	public GameObject rollNumberDisplay;
	public GameObject rollButton;


	private GameObject[,] moveSpots;
	public Player[] players;

	public GameObject[] playerNotes;

	public GameObject[] spaceSelectors;

	private int currentRoll;

	private int currentPlayer;


	public GameObject tradeButton;
	public GameObject mortgageButton;
	public GameObject redeemButton;
	public GameObject endTurnButton;

	public GameObject goBackbutton;


	// MORTGAGE
	public GameObject mortgageTitle;
	public GameObject mortgageText;
	public GameObject mortgageSelected;
	public GameObject mortgageEngageButton;

	//REDEEM
	public GameObject redeemTitle;
	public GameObject redeemText;
	public GameObject redeemSelected;
	public GameObject redeemEngageButton;



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



	// Use this for initialization
	void Start () {
		rollNumberDisplay = GameObject.Find ("Roll Number Display");
		rollButton = GameObject.Find ("Roll Button");
		AssignMovementSpots();
		AssignPlayers ();
		GatherSpaceSelectors ();
		currentPlayer = 0;

		Screen.orientation = ScreenOrientation.Portrait;

		playerNotes [0].GetComponent<Text>().text = "Your Turn";
		loseCount = 0;
		
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
					players [y].MoneyChange (2000);

			}
			

		}
			



		/*if (currentPlayer == 3)
			currentPlayer = 0;
		else
			currentPlayer++;*/

		//rollButton.GetComponent<Button> ().interactable = true;  Test without turn actions

		rollNumberDisplay.GetComponent<Text> ().enabled = false;
		rollButton.SetActive (false);
		//TurnOnTurnActions ();
		this.GetComponent<SpaceLogicS>().ResolveSpace(y,players[y].GetCurrentPos(),currentRoll);
		
		

	}
		
	int DiceRoll(){
		//return 7;
		return( Random.Range (1, 7));


	}

	public void OnRollButtonPush(){
		rollButton.GetComponent<Button> ().interactable = false;
		currentRoll = DiceRoll ();
		rollNumberDisplay.GetComponent<Text> ().enabled = true;
		rollNumberDisplay.GetComponent<Text> ().text = currentRoll.ToString();
		StartCoroutine(MovePlayer());

	

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


		tradeButton.SetActive (true);
		mortgageButton.SetActive (true);
		redeemButton.SetActive (true);
		endTurnButton.SetActive (true);


	}

	public void TurnOffTurnActions(){
		tradeButton.SetActive (false);
		mortgageButton.SetActive (false);
		redeemButton.SetActive (false);
		endTurnButton.SetActive (false);


	}


	public void pushTradeButton(){
		TurnOffTurnActions ();
		goBackbutton.SetActive (true);

	}
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




	public void pushRedeemButton(){
		TurnOffTurnActions ();
		redeemTitle.SetActive (true);
		redeemText.SetActive (true);
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
				if(RedeemCheckSpace(space)){
					//print ("redeem check was true");
					space.spaceSelectButton.GetComponent<Button> ().interactable = true;
					//space.spaceSelectButton.GetComponent<Button> ().onClick.AddListener () = pushMortgageSelectButton (space.sName, space.costToBuy/2);
					string x = space.sName;
					float y = space.costPerHouse;
					int z = count;
					space.spaceSelectButton.GetComponent<Button> ().onClick.AddListener (() => {
						pushRedeemSelectButton (x, y,z);
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

	public bool RedeemCheckSpace(Space space)
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

	public void pushRedeemSelectButton(string sname, float price, int space ){

		redeemText.SetActive (false);
		redeemSelected.SetActive (true);

		string houseOrHotel = "";

		if (this.GetComponent<SpaceLogicS> ().Gameboard [space].numberOfHouses < 3) {
			redeemSelected.GetComponent<Text> ().text = sname + " build house: $" + this.GetComponent<SpaceLogicS> ().Gameboard [space].costPerHouse;
			houseOrHotel = "house";

		} else if (!this.GetComponent<SpaceLogicS> ().Gameboard [space].hotel) {
			redeemSelected.GetComponent<Text> ().text = sname + " build hotel: $" + this.GetComponent<SpaceLogicS> ().Gameboard [space].costPerHouse;
			houseOrHotel = "hotel";
		} else {
			redeemSelected.GetComponent<Text> ().text = sname + " is full";
		}

		redeemEngageButton.SetActive (true);


		redeemEngageButton.GetComponent<Button> ().onClick.RemoveAllListeners ();
		//print ("I'm here");
		//DO NOT ADD IF everything built
			if (this.GetComponent<SpaceLogicS> ().Gameboard [space].numberOfHouses == 3 && this.GetComponent<SpaceLogicS> ().Gameboard [space].hotel)
				redeemEngageButton.SetActive (false);
		//print ("and here");
		redeemEngageButton.GetComponent<Button>().onClick.AddListener (() => {
			CompleteRedeem(sname, price, space, houseOrHotel);

		});
	
	}



	public void CompleteRedeem(string sname, float cost, int space, string horH){

		if (players [currentPlayer].money < cost) {
			redeemSelected.GetComponent<Text> ().text = "Not enough money";
			redeemEngageButton.GetComponent<Button> ().onClick.RemoveAllListeners ();
			redeemEngageButton.SetActive (false);
		} else {
			
			if (horH == "house") {
				redeemSelected.GetComponent<Text> ().text = "Built house on " + sname;
				this.GetComponent<SpaceLogicS> ().Gameboard [space].numberOfHouses++;


			}

			if (horH == "hotel") {
				redeemSelected.GetComponent<Text> ().text = "Built hotel on " + sname;
				this.GetComponent<SpaceLogicS> ().Gameboard [space].hotel = true;


			}



		
			this.GetComponent<SpaceLogicS> ().Gameboard [space].ActivateVisuals ();
			players [currentPlayer].money -= cost;
			//redeemSelected.GetComponent<Text> ().text = "Success";
			redeemEngageButton.GetComponent<Button> ().onClick.RemoveAllListeners ();
			redeemEngageButton.SetActive (false);
			ListenerCleanup ();
		}


	}

	public void pushGoBackButton(){
		goBackbutton.SetActive (false);
		mortgageText.SetActive (false);
		mortgageTitle.SetActive (false);
		mortgageSelected.SetActive (false);
		mortgageEngageButton.SetActive (false);
		redeemText.SetActive (false);
		redeemTitle.SetActive (false);
		redeemSelected.SetActive (false);
		redeemEngageButton.SetActive (false);
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
			redeemButton.SetActive (false);
			endTurnButton.SetActive (false);
			rollButton.GetComponent<Button> ().interactable = true;
		} else {
			youLoseDialog.SetActive (true);
		}


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
		redeemButton.SetActive (false);
		endTurnButton.SetActive (false);
		rollButton.GetComponent<Button> ().interactable = true;


	}

	public void GameWin(){
		int winner=666;
		rollButton.SetActive (false);
		tradeButton.SetActive (false);
		mortgageButton.SetActive (false);
		redeemButton.SetActive (false);
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
