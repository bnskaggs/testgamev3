using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class SpaceLogicS : MonoBehaviour {





	public GameObject spaceTitle;
	public GameObject spaceText;
	public GameObject oKButton;
	public GameObject auctionButton;
	public GameObject buyButton;
	public GameObject setBidButton;
	public GameObject bidText;
	public GameObject bidSlider;


	public Space[] Gameboard;



	private int currentPlayer;
	private int currentSpace;
	private int currentRoll;
	private int currentBidder;
	private int currentBidWinner;
	private float highestBid;


	public bool auctionFlag = false;

	private string mun;
	public int jailLoc;
	//public GameObject[] hVisualHolders;

	// Use this for initialization
	void Start () {

		AssignVisuals ();

		//if (GameMasterS.level != GameMasterS.INTERN)
			GetSpaceData ();
		
		/*	int count = -1;
		foreach (Space space in Gameboard) {
			count++;
			if (space.type == spaceType.jail)
				jailLoc = count;
		}*/
			
		
		/*foreach(Space space in Gameboard)
		{
			space.owned = true;
			space.owner = 1;
		}*/

		//int count = -1;

		mun = "#";

		if (GameMasterS.level == GameMasterS.INDIA)
			mun = "₹";
		if (GameMasterS.level ==GameMasterS.INTERN)
			mun = "$";
		
	


		
	}

	public void GetSpaceData(){

		string path = null;
		char[] seperators = { ',' };
		char[] seperators2 = { '&' };

		if(GameMasterS.level == GameMasterS.INDIA)
			path = "Assets/Resources/india.txt";
		if(GameMasterS.level == GameMasterS.INTERN)
			path = "Assets/Resources/international.txt";
		
		StreamReader reader = new StreamReader(path); 

		string headerLine = reader.ReadLine();
		string numberOfSpaces = reader.ReadLine ();
		//while(reader.Peek()>=0)
			//print(reader.ReadLine());
		for (int x = 0; x < int.Parse(numberOfSpaces);x++)
		{
			string data = reader.ReadLine ();
			string[] spaceData = data.Split (seperators);

			Gameboard [x].sName = spaceData [0];
			SetSpaceType (spaceData [1],x);
			Gameboard [x].costToBuy = float.Parse(spaceData [2]);
			Gameboard [x].costToRent = float.Parse(spaceData [3]);
			SetSpaceType (spaceData [4],x);
			Gameboard [x].costPerHouse = float.Parse(spaceData [5]);
			string[] houserent = spaceData [6].Split (seperators2);

			Gameboard [x].costWithHouses = new float[3] {
				float.Parse (houserent [0]),
				float.Parse (houserent [1]),
				float.Parse (houserent [2])
			};
			Gameboard [x].costPerHotel = float.Parse(spaceData [7]);
			Gameboard [x].costWithHotel = float.Parse(spaceData [8]);

		}
		reader.Close();


	}

	void SetSpaceType(string type, int count){
		switch (type) {
		case "property":
			Gameboard [count].type = spaceType.property;
			break;
		case "utility":
			Gameboard [count].type = spaceType.utility;
			break;
		case "jail":
			jailLoc = count;
			Gameboard [count].type = spaceType.jail;
			break;
		case "tax":
			Gameboard [count].type = spaceType.tax;
			break;
		case "itax":
			Gameboard [count].type = spaceType.itax;
			break;
		case "wtax":
			Gameboard [count].type = spaceType.wtax;
			break;
		case "chance":
			Gameboard [count].type = spaceType.chance;
			break;
		case "start":
			Gameboard [count].type = spaceType.start;
			break;
		case "community":
			Gameboard [count].type = spaceType.community;
			break;
		case "club":
			Gameboard [count].type = spaceType.club;
			break;
		case "rest":
			Gameboard [count].type = spaceType.rest;
			break;



		}
	}

		void SetColorType(string type, int count){
		switch (type) {
		case "blue":
			Gameboard [count].color = colorGroup.blue;
			break;
		case "red":
			Gameboard [count].color = colorGroup.red;
			break;
		case "pink":
			Gameboard [count].color = colorGroup.pink;
			break;
		case "green":
			Gameboard [count].color = colorGroup.green;
			break;
		case "none":
			Gameboard [count].color = colorGroup.none;
			break;



		}
	}



		
	void AssignVisuals (){
		//hVisualHolders = new GameObject[36];

		for(int x = 0; x<36; x++)
		{
			Gameboard[x].hVisualHolder = GameObject.Find (string.Format ("H ({0})", x.ToString ()));
		}
		int z =0;
		foreach (Space space in Gameboard) {
			Gameboard[z].hVisuals = new GameObject[4];
			for(int y = 0; y<4;y++)
			{
				Gameboard[z].hVisuals[y] = Gameboard[z].hVisualHolder.transform.GetChild(y).gameObject;

			}

			z++;
		}


	}

	public void ResolveSpace (int player, int space, int roll)
	{
		currentPlayer = player;
		currentSpace = space;
		currentRoll = roll;
		spaceType temp = Gameboard [space].type;

		switch (temp) {

		case spaceType.tax:
			taxSpace ();
			break;

		case spaceType.itax:
			if (GameMasterS.gameMode == GameMasterS.MOBILE) 
				taxSpace ();
			if (GameMasterS.gameMode == GameMasterS.BOARD) 
				itaxSpace ();
			break;

		case spaceType.wtax:
				if (GameMasterS.gameMode == GameMasterS.MOBILE) 
					taxSpace ();
				if (GameMasterS.gameMode == GameMasterS.BOARD) 
					wtaxSpace ();
			break;

		case spaceType.property:
			propertySpace ();
			break;

		case spaceType.utility:
			propertySpace ();
			break;
		
		/*case spaceType.transport:
			propertySpace ();
			break;*/

		case spaceType.jail:
			jailSpace ();
			break;

		case spaceType.rest:
			restSpace ();
			break;

		case spaceType.chance:
			chanceSpace ();
			break;
		
		case spaceType.start:
			startSpace ();
			break;

		case spaceType.community:
			communitySpace ();
			break;

		case spaceType.club:
			clubSpace ();
			break;

		}



	}

	void taxSpace()
	{
		this.GetComponent<MainGameS>().players [currentPlayer].MoneyChange (-500);
		spaceTitle.GetComponent<Text> ().text = "Income Tax";
		spaceTitle.SetActive (true);
		spaceText.GetComponent<Text> ().text = "-"+mun+"500";
		spaceText.SetActive (true);
		oKButton.SetActive (true);

	}

	void wtaxSpace(){
		int count = 0;
		foreach (Space space in Gameboard) {
			if (space.owner == currentPlayer) {
				count += space.numberOfHouses;
				if (space.hotel)
					count++;
			}

		}

		float moneyLost = count * 50;
		if (moneyLost > 500)
			moneyLost = 500;
		this.GetComponent<MainGameS> ().players [currentPlayer].money -= moneyLost;
		spaceTitle.GetComponent<Text> ().text = "Wealth Tax";
		spaceTitle.SetActive (true);
		spaceText.GetComponent<Text> ().text = string.Format("Lost {0}{1} in wealth taxes",mun,moneyLost);
		spaceText.SetActive (true);
		oKButton.SetActive (true);
		

	}

	void itaxSpace(){
		int count = 0;
		foreach (Space space in Gameboard) {
			if (space.owner == currentPlayer)
				count++;

		}

		float moneyLost = count * 50;
		if (moneyLost > 500)
			moneyLost = 500;
		this.GetComponent<MainGameS> ().players [currentPlayer].money -= moneyLost;
		spaceTitle.GetComponent<Text> ().text = "Income Tax";
		spaceTitle.SetActive (true);
		spaceText.GetComponent<Text> ().text = string.Format("Lost {0}{1} in income taxes",mun,moneyLost);
		spaceText.SetActive (true);
		oKButton.SetActive (true);
		

	}

	void propertySpace()
	{
		float rent;
		if (!Gameboard [currentSpace].owned) {
			spaceTitle.GetComponent<Text> ().text = Gameboard [currentSpace].sName;
			spaceTitle.SetActive (true);
			spaceText.GetComponent<Text> ().text = mun + Gameboard [currentSpace].costToBuy.ToString ();
			spaceText.SetActive (true);
			buyButton.SetActive (true);
			auctionButton.SetActive (true);
		} else {
			if (Gameboard [currentSpace].owner != currentPlayer) {
				spaceTitle.GetComponent<Text> ().text = Gameboard [currentSpace].sName;
				spaceTitle.SetActive (true);

				if (!Gameboard [currentSpace].isMortgaged) {
					rent = CalculateRent ();
					spaceText.GetComponent<Text> ().text = "Paid "+mun + rent + " to P" + (Gameboard [currentSpace].owner + 1).ToString ();
					this.GetComponent<MainGameS>().players [currentPlayer].MoneyChange (-rent);
					this.GetComponent<MainGameS>().players [Gameboard [currentSpace].owner].MoneyChange (rent);
					spaceText.SetActive (true);
					oKButton.SetActive (true);
				} else {
					spaceText.GetComponent<Text> ().text = "Mortgaged!";
					spaceText.SetActive (true);
					oKButton.SetActive (true);

				}
					
			} else {
				spaceTitle.GetComponent<Text> ().text = Gameboard [currentSpace].sName;
				spaceTitle.SetActive (true);
				//if (Gameboard [currentSpace].owner != currentPlayer)
					spaceText.GetComponent<Text> ().text = "You own this!";
				spaceText.SetActive (true);
				oKButton.SetActive (true);

			}

		}
		

	}

	public float CalculateRent()
	{
		//float rentMoney = 0;

		print ("calculating rent");

		if (Gameboard [currentSpace].type == spaceType.utility)
			return(CalculateUtilityCost());

		if (Gameboard [currentSpace].hotel)
			return Gameboard [currentSpace].costWithHotel;

		if (Gameboard[currentSpace].numberOfHouses>0)
			return Gameboard[currentSpace].costWithHouses[Gameboard[currentSpace].numberOfHouses-1];
				

		return Gameboard[currentSpace].costToRent;
	}

	public float CalculateUtilityCost(){
		int water = 2;
		int airport = 24;
		int railway = 3;
		int best = 13;
		int electric = 12;
		int motoboat = 34;

		//water and airport
		if (Gameboard [currentSpace].tag == "utility1") {
			if (currentSpace == water) {
				if (Gameboard [currentSpace].owner == Gameboard [airport].owner)
					return Gameboard [water].costWithHouses[0];
				else
					return Gameboard [water].costToRent;
			} else {
				if (Gameboard [currentSpace].owner == Gameboard [water].owner)
					return Gameboard [airport].costWithHouses[0];
					else
					return Gameboard [airport].costToRent;
				
			}

		}
		//rail and best
		if (Gameboard [currentSpace].tag == "utility2") {
			if (currentSpace == railway) {
				if (Gameboard [currentSpace].owner == Gameboard [best].owner)
					return Gameboard [railway].costWithHouses[0];
				else
					return Gameboard [railway].costToRent;
			} else {
				if (Gameboard [currentSpace].owner == Gameboard [railway].owner)
					return Gameboard [best].costWithHouses[0];
				else
					return Gameboard [best].costToRent;

			}


		}
		//electric and boat
		if (Gameboard [currentSpace].tag == "utility3") {
			if (currentSpace == electric) {
				if (Gameboard [currentSpace].owner == Gameboard [motoboat].owner)
					return Gameboard [electric].costWithHouses[0] * currentRoll;
				else
					return Gameboard [electric].costToRent * currentRoll;
			} else {
				if (Gameboard [currentSpace].owner == Gameboard [electric].owner)
					return Gameboard [motoboat].costWithHouses[0] * currentRoll;
				else
					return  Gameboard [motoboat].costToRent*currentRoll;

			}


		}

		return 0;


	}

	void jailSpace()
	{
		if (currentRoll < 5) {
			
			this.GetComponent<MainGameS>().players [currentPlayer].lostTurn = true;
			this.GetComponent<MainGameS>().players[currentPlayer].lostTurnTime = 2;
			spaceTitle.GetComponent<Text> ().text = "Jail";
			spaceTitle.SetActive (true);
			spaceText.GetComponent<Text> ().text = "Lose 2 Turns";
			this.GetComponent<MainGameS> ().playerNotes [currentPlayer].GetComponent<Text> ().text = "Lost Turns: 2";
			spaceText.SetActive (true);
			oKButton.SetActive (true);
		} else {
			spaceTitle.GetComponent<Text> ().text = "Jail";
			spaceTitle.SetActive (true);
			spaceText.GetComponent<Text> ().text = "Passing By";
			spaceText.SetActive (true);
			oKButton.SetActive (true);
		}
		

	}

	public void GoToJail(){
		currentRoll = 0;
		jailSpace ();
		//Display reason for jail


	}

	void restSpace()
	{
		if (GameMasterS.gameMode == GameMasterS.MOBILE) {
			this.GetComponent<MainGameS> ().players [currentPlayer].lostTurn = true;
			this.GetComponent<MainGameS> ().players [currentPlayer].lostTurnTime = 1;
			spaceTitle.GetComponent<Text> ().text = "Rest House";
			spaceTitle.SetActive (true);
			spaceText.GetComponent<Text> ().text = "Lose Turn";
			this.GetComponent<MainGameS> ().playerNotes [currentPlayer].GetComponent<Text> ().text = "Lost Turns: 2";

		}

		if (GameMasterS.gameMode == GameMasterS.BOARD) {

			int count = 0;
			int payCount = 0;
			foreach (Player player in this.GetComponent<MainGameS> ().players) {
				if (this.GetComponent<MainGameS> ().players [count].inTheGame && count != currentPlayer) {
					this.GetComponent<MainGameS> ().players [count].money -= 100;
					payCount++;

				}
				count++;

			}
			this.GetComponent<MainGameS> ().players [currentPlayer].money += 100 * payCount;
			spaceText.GetComponent<Text> ().text = string.Format ("Everyone gave {0}100 to p{1}", mun, (currentPlayer + 1.ToString ()));
		}
		spaceText.SetActive (true);
		oKButton.SetActive (true);




	}

	void chanceSpace()
	{
		int chance = Random.Range (1, 3);

		if (chance == 1) {
			
			spaceTitle.GetComponent<Text> ().text = "ReRoll!";
			spaceTitle.SetActive (true);

			this.GetComponent<MainGameS> ().Reroll ();
		} else {
			spaceTitle.GetComponent<Text> ().text = "Double Move!";
			spaceTitle.SetActive (true);

			this.GetComponent<MainGameS> ().DoubleMove ();
			

		}
	}

	void startSpace()
	{
		okButtonPushed ();

	}

	void communitySpace()
	{
		this.GetComponent<MainGameS>().players [currentPlayer].money -= 500;
		spaceTitle.GetComponent<Text> ().text = "Community";
		spaceTitle.SetActive (true);
		spaceText.GetComponent<Text> ().text = "-"+mun+"500";
		spaceText.SetActive (true);
		oKButton.SetActive (true);

	}

	void clubSpace()
	{
		spaceTitle.GetComponent<Text> ().text = Gameboard[currentSpace].sName;
		spaceTitle.SetActive (true);
		if (GameMasterS.gameMode == GameMasterS.MOBILE) {
			this.GetComponent<MainGameS> ().players [currentPlayer].money -= 200;
			//spaceTitle.GetComponent<Text> ().text = "Club";

			spaceText.GetComponent<Text> ().text = "-" + mun + "200";


		}

		if (GameMasterS.gameMode == GameMasterS.BOARD) {

			int count = 0;
			int payCount = 0;
				foreach(Player player in this.GetComponent<MainGameS> ().players){
				if (this.GetComponent<MainGameS> ().players [count].inTheGame && count != currentPlayer){
					this.GetComponent<MainGameS> ().players [count].money += 100;
					payCount++;
				
					}
				count++;

			}
			this.GetComponent<MainGameS> ().players [currentPlayer].money -= 100 * payCount;
			spaceText.GetComponent<Text> ().text = string.Format("Everyone got {0}100 from p{1}",mun,(currentPlayer+1.ToString()));




		}
		spaceText.SetActive (true);
		oKButton.SetActive (true);

	}

	public void okButtonPushed(){
		oKButton.SetActive (false);
		spaceText.SetActive (false);
		spaceTitle.SetActive (false);

		this.GetComponent<MainGameS> ().TurnOnTurnActions ();



	}

	public void buyButtonPushed(){

		if (this.GetComponent<MainGameS>().players [currentPlayer].money >= Gameboard [currentSpace].costToBuy) {

			this.GetComponent<MainGameS>().players [currentPlayer].money -= Gameboard [currentSpace].costToBuy;
			Gameboard [currentSpace].owned = true;
			Gameboard [currentSpace].owner = currentPlayer;
			this.GetComponent<TokensS> ().ChangeOwnership (currentSpace, currentPlayer);
			spaceText.GetComponent<Text> ().text = "Bought!";
			auctionButton.SetActive (false);
			buyButton.SetActive (false);
			oKButton.SetActive (true);


		}


	}


	public void auctionButtonPushed(){
		
		currentBidWinner = 0;
		while (this.GetComponent<MainGameS> ().players [currentBidWinner].inTheGame == false && currentBidWinner < 4)
			currentBidWinner++;
		highestBid = 0;


		auctionButton.SetActive (false);
		buyButton.SetActive (false);
		spaceTitle.GetComponent<Text> ().text = "Auction";
		spaceTitle.SetActive (true);
		setBidButton.SetActive (true);
		currentBidder = 0;


		LetsAuction ();



	}

	public void LetsAuction(){

		while (this.GetComponent<MainGameS> ().players [currentBidder].inTheGame == false && currentBidder < 4)
			currentBidder++;

		if (currentBidder < 4) {
			spaceText.SetActive (true);
			spaceText.GetComponent<Text> ().text = "P" + (currentBidder + 1).ToString () + " bid";

			bidText.SetActive (true);
			bidSlider.SetActive (true);
			bidText.GetComponent<Text> ().text = mun+"0";
			bidSlider.GetComponent<Slider> ().value = 0;

			bidSlider.GetComponent<Slider> ().maxValue = this.GetComponent<MainGameS> ().players [currentBidder].money;
		}
		



	}

	public void setBidButtonPushed(){
		if (bidSlider.GetComponent<Slider> ().value > highestBid) {
			highestBid = bidSlider.GetComponent<Slider> ().value;
			currentBidWinner = currentBidder;

		}

		do {
			currentBidder++;
		} while (currentBidder <4 && this.GetComponent<MainGameS> ().players [currentBidder].inTheGame == false );
		if (currentBidder < 4 && this.GetComponent<MainGameS> ().players [currentBidder].inTheGame == true) {
			LetsAuction ();

		} else {
			bidText.SetActive (false);
			bidSlider.SetActive (false);
			setBidButton.SetActive (false);
			this.GetComponent<MainGameS>().players [currentBidWinner].money-=highestBid;
			spaceText.GetComponent<Text> ().text = "P" + (currentBidWinner+1).ToString () + " wins with "+mun+ highestBid;
			Gameboard [currentSpace].owned = true;
			Gameboard [currentSpace].owner = currentBidWinner;
			this.GetComponent<TokensS> ().ChangeOwnership (currentSpace, currentBidWinner);

			oKButton.SetActive (true);


		}




		

	}



}
