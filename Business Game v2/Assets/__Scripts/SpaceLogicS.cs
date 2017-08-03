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
	public GameObject chancecommunityOkayButton;


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
	public int restLoc;
	//public GameObject[] hVisualHolders;

	private string savepath;

	private bool changeOwners;
	private bool chanceJail=false;
	private bool chanceRest=false;
	public bool eventHappened=false;
	public bool speeding = false;




	// Use this for initialization
	void Start () {
		
		savepath =   GameMasterS.saveLoadLocation;
		if(!GameMasterS.continuingGame)
			AssignVisuals ();

		//if (GameMasterS.level != GameMasterS.INTERN)
		//StartCoroutine(GetSpaceData());
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
		if (GameMasterS.level ==GameMasterS.INTERN || GameMasterS.level ==GameMasterS.GENERIC)
			mun = "$";
		

		if (GameMasterS.level == GameMasterS.GENERIC)
			PopulateSpaces ();

		
	}

	public void GetSpaceData(){

		string path = null;
		char[] seperators = { ',' };
		char[] seperators2 = { '&' };
		char[] seperators3 = { ';' };
		//char[] seperators4 = { ',' };
		TextAsset level = null;
		if (GameMasterS.level == GameMasterS.INDIA) {
			
			//path = Path.Combine( Application.streamingAssetsPath,"India.txt");
			level = Resources.Load<TextAsset> ("India") as TextAsset;
		}
		if (GameMasterS.level == GameMasterS.INTERN || GameMasterS.level ==GameMasterS.GENERIC) {
			print ("Level " + GameMasterS.level);
			//path = Path.Combine( Application.streamingAssetsPath,"International2.txt");
			level = Resources.Load<TextAsset> ("International") as TextAsset;
		}
		/*if (path.Contains ("://")) {
			WWW www = new WWW (path);
			print ("here");
			yield return www;
			print ("andhere");
			string result = www.text;
			Debug.Log (result);

		
			string[] lineSplit = www.text.Split (seperators3);
			string numberOfSpaces = lineSplit [1];
			for (int x = 0; x < int.Parse (numberOfSpaces); x++) {
				string[] spaceData = lineSplit[x].Split (seperators);

				Gameboard [x].sName = spaceData [0];
				SetSpaceType (spaceData [1], x);
				Gameboard [x].costToBuy = float.Parse (spaceData [2]);
				Gameboard [x].costToRent = float.Parse (spaceData [3]);
				SetColorType (spaceData [4], x);
				Gameboard [x].costPerHouse = float.Parse (spaceData [5]);
				string[] houserent = spaceData [6].Split (seperators2);

				Gameboard [x].costWithHouses = new float[3] {
					float.Parse (houserent [0]),
					float.Parse (houserent [1]),
					float.Parse (houserent [2])
				};
				Gameboard [x].costPerHotel = float.Parse (spaceData [7]);
				Gameboard [x].costWithHotel = float.Parse (spaceData [8]);



			}



		} else {*/
		print (level);
		if(level !=null)
		{
		StreamReader reader = new StreamReader (new MemoryStream(level.bytes)); 

			string headerLine = reader.ReadLine ();
			string numberOfSpaces = reader.ReadLine ();
			//while(reader.Peek()>=0)
			//print(reader.ReadLine());
			for (int x = 0; x < int.Parse (numberOfSpaces); x++) {
				string data = reader.ReadLine ();
				string[] spaceData = data.Split (seperators);

				Gameboard [x].sName = spaceData [0];
				SetSpaceType (spaceData [1], x);
				Gameboard [x].costToBuy = float.Parse (spaceData [2]);
				Gameboard [x].costToRent = float.Parse (spaceData [3]);
				SetColorType (spaceData [4], x);
				Gameboard [x].costPerHouse = float.Parse (spaceData [5]);
				string[] houserent = spaceData [6].Split (seperators2);

				Gameboard [x].costWithHouses = new float[3] {
					float.Parse (houserent [0]),
					float.Parse (houserent [1]),
					float.Parse (houserent [2])
				};
				Gameboard [x].costPerHotel = float.Parse (spaceData [7]);
				Gameboard [x].costWithHotel = float.Parse (spaceData [8]);

			}
			reader.Close ();
		}


	}

	public void LoadGameDataSecond()
	{
		savepath =   GameMasterS.saveLoadLocation2;
		char[] seperators = { ',' };
		char[] seperators2 = { '&' };
		StreamReader reader = new StreamReader(savepath); 

		string trash = reader.ReadLine ();
		trash = reader.ReadLine ();
		trash = reader.ReadLine ();
		int skip = int.Parse(reader.ReadLine ());
		for (int x = 0; x < skip; x++) {
			trash = reader.ReadLine ();
		}
		//print (reader.ReadLine ());

		int numberOfSpaces = int.Parse(reader.ReadLine ());

		for (int x = 0; x < numberOfSpaces; x++) {
			string data = reader.ReadLine ();
			string[] spaceData = data.Split (seperators);

			Gameboard [x].sName = spaceData [0];
			SetSpaceType (spaceData [1],x);
			Gameboard [x].owned = bool.Parse (spaceData [2]);
			Gameboard [x].owner = int.Parse (spaceData [3]);
			Gameboard [x].costToBuy = float.Parse(spaceData [4]);
			Gameboard [x].costToRent = float.Parse(spaceData [5]);
			SetSpaceType (spaceData [6],x);
			Gameboard [x].numberOfHouses = int.Parse (spaceData [7]);
			Gameboard [x].costPerHouse = float.Parse(spaceData [8]);
			string[] houserent = spaceData [9].Split (seperators2);
			Gameboard [x].costWithHouses = new float[3] {
				float.Parse (houserent [0]),
				float.Parse (houserent [1]),
				float.Parse (houserent [2])
			};
			Gameboard [x].hotel = bool.Parse (spaceData [10]);
			Gameboard [x].costToRent = float.Parse(spaceData [11]);
			Gameboard [x].isMortgaged = bool.Parse (spaceData [12]);
			Gameboard[x].costPerHotel = float.Parse(spaceData [13]);


		
		}

		reader.Close ();
		changeOwners = true;
		/*for (int x = 0; x < Gameboard.Length; x++) {
			this.GetComponent<TokensS> ().ChangeOwnership (x, Gameboard [x].owner);
			if (Gameboard [x].isMortgaged)
				this.GetComponent<TokensS> ().ChangeMortgage (x, Gameboard [x].owner);

		}*/
		AssignVisuals ();
		this.GetComponent<MainGameS> ().loadHouseVisuals ();
		if (GameMasterS.level == GameMasterS.GENERIC)
			PopulateSpaces ();

	}

	void Update(){
		if (changeOwners) {
			changeOwners = false;
			for (int x = 0; x < Gameboard.Length; x++) {
				this.GetComponent<TokensS> ().ChangeOwnership (x, Gameboard [x].owner);


				if (Gameboard [x].isMortgaged)
					this.GetComponent<TokensS> ().ChangeMortgage (x, Gameboard [x].owner);

			}

		}




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
			restLoc = count;
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
		print("assigning visuals");
		for(int x = 0; x<36; x++)
		{
			Gameboard [x].hVisualHolder = GameObject.Find (string.Format ("B{0}", x.ToString ())).transform.FindDeepChild ("HVisual").gameObject;
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

	public void PopulateSpaces()
	{
		
		string mun = "#";

		if (GameMasterS.level == GameMasterS.INDIA)
			mun = "₹";
		if (GameMasterS.level ==GameMasterS.INTERN || GameMasterS.level ==GameMasterS.GENERIC)
			mun = "$";
		
		for (int x = 0; x < 36; x++) {
			GameObject temp = GameObject.Find ("B" + x.ToString ());
			if (temp.tag != "cornerspace") {
				temp.transform.FindDeepChild ("TopText").GetComponent<Text> ().text = Gameboard [x].sName;

				if (Gameboard [x].type == spaceType.property) {
					temp.transform.FindDeepChild ("BotText").GetComponent<Text> ().text =string.Format("{0}{1}",mun, Gameboard [x].costToBuy);

					switch (Gameboard [x].color) {
					case colorGroup.red:
						temp.transform.FindDeepChild ("SpaceMiddle").GetComponent<Image> ().color = VisualHolderS.REDSPACECOLOR1;
						temp.transform.FindDeepChild ("SpaceOuter").GetComponent<Image> ().color = VisualHolderS.REDSPACECOLOR2;

						break;
					case colorGroup.pink:
						temp.transform.FindDeepChild ("SpaceMiddle").GetComponent<Image> ().color = VisualHolderS.PINKSPACECOLOR1;
						temp.transform.FindDeepChild ("SpaceOuter").GetComponent<Image> ().color = VisualHolderS.PINKSPACECOLOR2;

						break;
					case colorGroup.green:
						temp.transform.FindDeepChild ("SpaceMiddle").GetComponent<Image> ().color = VisualHolderS.GREENSPACECOLOR1;
						temp.transform.FindDeepChild ("SpaceOuter").GetComponent<Image> ().color = VisualHolderS.GREENSPACECOLOR2;
						break;
					case colorGroup.blue:
						temp.transform.FindDeepChild ("SpaceMiddle").GetComponent<Image> ().color = VisualHolderS.BLUESPACECOLOR1;
						temp.transform.FindDeepChild ("SpaceOuter").GetComponent<Image> ().color = VisualHolderS.BLUESPACECOLOR2;
						break;


					}

				}
				if (Gameboard [x].type == spaceType.chance || Gameboard [x].type == spaceType.wtax  || Gameboard [x].type == spaceType.itax) {
					temp.transform.FindDeepChild ("SpaceMiddle").GetComponent<Image> ().color = VisualHolderS.EXTRASPACECOLOR1;
					temp.transform.FindDeepChild ("SpaceOuter").GetComponent<Image> ().color = VisualHolderS.EXTRASPACECOLOR2;
					temp.transform.FindDeepChild ("BotText").gameObject.SetActive (false);
				}



			


				if (Gameboard [x].type == spaceType.community || Gameboard [x].type == spaceType.utility) {
					temp.transform.FindDeepChild ("SpaceMiddle").GetComponent<Image> ().color = VisualHolderS.EXTRASPACECOLOR2;
					temp.transform.FindDeepChild ("SpaceOuter").GetComponent<Image> ().color = VisualHolderS.EXTRASPACECOLOR2;
					temp.transform.FindDeepChild ("BotText").gameObject.SetActive (false);
					if (Gameboard [x].type == spaceType.utility) {
						temp.transform.FindDeepChild ("BotText").GetComponent<Text> ().text = string.Format ("{0}{1}", mun, Gameboard [x].costToBuy);
						temp.transform.FindDeepChild ("BotText").gameObject.SetActive (true);
					}
				}
			}




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
		float rentMoney = 0;

		print ("calculating rent");

		if (Gameboard [currentSpace].type == spaceType.utility)
			rentMoney = CalculateUtilityCost ();
		else {
			if (Gameboard [currentSpace].hotel)
				rentMoney = Gameboard [currentSpace].costWithHotel;
			else {

				if (Gameboard [currentSpace].numberOfHouses > 0)
					rentMoney = Gameboard [currentSpace].costWithHouses [Gameboard [currentSpace].numberOfHouses - 1];
				else {
					rentMoney = Gameboard[currentSpace].costToRent;
				}
			}
		}
				

		if (GameMasterS.gameMode == GameMasterS.BOARD)
			rentMoney /= 2;

		return rentMoney;
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
		if (GameMasterS.gameMode == GameMasterS.MOBILE) {
			if (currentRoll < 5) {
			
				this.GetComponent<MainGameS> ().players [currentPlayer].lostTurn = true;
				this.GetComponent<MainGameS> ().players [currentPlayer].lostTurnTime = 2;
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
		if (GameMasterS.gameMode == GameMasterS.BOARD) {
			this.GetComponent<MainGameS> ().players [currentPlayer].lostTurn = true;
			this.GetComponent<MainGameS> ().players [currentPlayer].lostTurnTime = 3;
			spaceTitle.GetComponent<Text> ().text = "Jail";
			this.GetComponent<MainGameS> ().players [currentPlayer].jail = true;
			spaceTitle.SetActive (true);
			if (!speeding)
				spaceText.GetComponent<Text> ().text = "Jailed for 3 turns";
			speeding = false;
			this.GetComponent<MainGameS> ().playerNotes [currentPlayer].GetComponent<Text> ().text = "In Jail: 3 ";
			spaceText.SetActive (true);
			oKButton.SetActive (true);


		}
		

	}

	public void GoToJail(){
		currentRoll = 0;

		spaceText.GetComponent<Text> ().text = "Jailed 3 turns for speeding";
		speeding = true;
		jailSpace ();
		//Display reason for jail


	}
	public void GoToRest(){
		currentRoll = 0;
		restSpace ();
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
			spaceText.GetComponent<Text> ().text = string.Format ("Everyone gave {0}100 to p{1}", mun, currentPlayer + 1);
		}
		spaceText.SetActive (true);
		oKButton.SetActive (true);




	}

	void chanceSpace()
	{
		if (GameMasterS.gameMode == GameMasterS.MOBILE) {
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
		if (GameMasterS.gameMode == GameMasterS.BOARD) {
			spaceTitle.GetComponent<Text> ().text = "Chance";
			spaceTitle.SetActive (true);

			print (currentRoll);

			switch (currentRoll) {
			case 2:
				spaceText.GetComponent<Text> ().text = string.Format ("Lost {0}2000 in share market", mun);
				this.GetComponent<MainGameS> ().players [currentPlayer].money -= 2000;
				break;
			case 3:
				spaceText.GetComponent<Text> ().text = string.Format ("Lottery Prize of {0}2500", mun);
				this.GetComponent<MainGameS> ().players [currentPlayer].money += 2500;
				break;
			case 4:
				spaceText.GetComponent<Text> ().text = string.Format ("Fined {0}1000 for drunk driving", mun);
				this.GetComponent<MainGameS> ().players [currentPlayer].money -= 1000;
				break;
			case 5:
				spaceText.GetComponent<Text> ().text = string.Format ("Won {0}1000 at Crossword Competition", mun);
				this.GetComponent<MainGameS> ().players [currentPlayer].money += 1000;
				break;
			case 6:
				spaceText.GetComponent<Text> ().text = string.Format ("House Repair, lose {0}1000", mun);
				this.GetComponent<MainGameS> ().players [currentPlayer].money -= 1500;
				break;
			case 7:
				spaceText.GetComponent<Text> ().text = string.Format ("Won Jackpot of {0}2000!", mun);
				this.GetComponent<MainGameS> ().players [currentPlayer].money += 2000;
				break;
			case 8:
				spaceText.GetComponent<Text> ().text = string.Format ("Fire! Pay {0}3000 for damage", mun);
				this.GetComponent<MainGameS> ().players [currentPlayer].money -= 3000;
				break;
			case 9:
				spaceText.GetComponent<Text> ().text = string.Format ("Inheritance! Recieve {0}4000", mun);
				this.GetComponent<MainGameS> ().players [currentPlayer].money += 3000;
				break;
			case 10:
				spaceText.GetComponent<Text> ().text = string.Format ("Go to Jail!");
				chanceJail = true;
				break;
			case 11:
				spaceText.GetComponent<Text> ().text = string.Format ("Prize for best performance! Receive {0}3000 ", mun);
				this.GetComponent<MainGameS> ().players [currentPlayer].money += 3000;
				break;
			case 12:

				spaceText.GetComponent<Text> ().text = string.Format ("Go to {0} and lose turn", Gameboard [restLoc].sName);
				chanceRest = true;
				break;

			}

			spaceText.SetActive (true);
			chancecommunityOkayButton.SetActive (true);


		}

	}

	public void ChanceCommunityOkayPushed(){
		chancecommunityOkayButton.SetActive (false);
		spaceText.SetActive (false);
		spaceTitle.SetActive (false);
		print ("rest"+ chanceRest);

		if (chanceJail) {
			chanceJail = false;
			GoToJail ();
			eventHappened = true;
			this.GetComponent<MainGameS> ().MoveToJail ();
		} else {
			if (chanceRest) {
				chanceRest = false;
				eventHappened = true;
				this.GetComponent<MainGameS>().players [currentPlayer].lostTurn = true;
				this.GetComponent<MainGameS>().players[currentPlayer].lostTurnTime = 1;
				this.GetComponent<MainGameS> ().playerNotes [currentPlayer].GetComponent<Text> ().text = "Lost Turns: 1";
				restSpace ();
				this.GetComponent<MainGameS> ().MoveToRest ();


			}
			else{
				oKButton.SetActive (false);
				spaceText.SetActive (false);
				spaceTitle.SetActive (false);

				this.GetComponent<MainGameS> ().TurnOnTurnActions ();



			}
		}
		}


	void startSpace()
	{
		okButtonPushed ();

	}

	void communitySpace()
	{
		if (GameMasterS.gameMode == GameMasterS.MOBILE) {
			this.GetComponent<MainGameS> ().players [currentPlayer].money -= 500;
			spaceTitle.GetComponent<Text> ().text = "Community";
			spaceTitle.SetActive (true);
			spaceText.GetComponent<Text> ().text = "-" + mun + "500";
			spaceText.SetActive (true);
			oKButton.SetActive (true);
		}


		if (GameMasterS.gameMode == GameMasterS.BOARD) {
			spaceTitle.GetComponent<Text> ().text = "Community Chest";
			spaceTitle.SetActive (true);

			print (currentRoll);

			switch (currentRoll) {
			case 2:
				spaceText.GetComponent<Text> ().text = string.Format ("Birthday! Get {0}500 from each player", mun);
				int count;
				int x = 0;
				for (count=0; count < 4; count++) {
					if (this.GetComponent<MainGameS> ().players [count].inTheGame && count != currentPlayer) {
						x++;
						this.GetComponent<MainGameS> ().players [count].money -= 500;
					}

				}

				this.GetComponent<MainGameS> ().players [currentPlayer].money += 500*x;

				break;
			case 3:
				spaceText.GetComponent<Text> ().text = string.Format ("Go to Jail!");
				chanceJail = true;
				break;
			case 4:
				spaceText.GetComponent<Text> ().text = string.Format ("Reality TV Prize! Get {0}2500", mun);
				this.GetComponent<MainGameS> ().players [currentPlayer].money += 2500;
				break;
			case 5:
				spaceText.GetComponent<Text> ().text = string.Format ("Medical Fees, lose {0}1000", mun);
				this.GetComponent<MainGameS> ().players [currentPlayer].money -= 1000;
				break;
			case 6:
				spaceText.GetComponent<Text> ().text = string.Format ("Income Tax Refund! Get {0}2000", mun);
				this.GetComponent<MainGameS> ().players [currentPlayer].money += 2000;
				break;
			case 7:
				spaceText.GetComponent<Text> ().text = string.Format ("Marriage Celebration, Pay {0}2000", mun);
				this.GetComponent<MainGameS> ().players [currentPlayer].money -= 2000;
				break;
			case 8:
				spaceText.GetComponent<Text> ().text = string.Format ("Go to {0} and lose turn", Gameboard [restLoc].sName);
				chanceRest = true;
				break;
			case 9:
				float repairCost = 0;
				int houseCount = 0;
				int hotelCount = 0;
				foreach (Space space in Gameboard) {
					if (space.owner == currentPlayer) {
						houseCount += space.numberOfHouses;
						if (space.hotel)
							hotelCount++;
					}

				}
				repairCost = (100 * hotelCount) + (50 * houseCount);


				spaceText.GetComponent<Text> ().text = string.Format ("Property Repair, paid {0}{1}", mun,repairCost);
				this.GetComponent<MainGameS> ().players [currentPlayer].money -= repairCost;
				break;
			case 10:
				spaceText.GetComponent<Text> ().text = string.Format ("Interest on Stocks, get {0}1500", mun);
				this.GetComponent<MainGameS> ().players [currentPlayer].money += 1500;
				break;
			case 11:
				spaceText.GetComponent<Text> ().text = string.Format ("Pay Insurance Premium of {0}1500 ", mun);
				this.GetComponent<MainGameS> ().players [currentPlayer].money -= 3000;
				break;
			case 12:
				spaceText.GetComponent<Text> ().text = string.Format ("Sell Stocks, get {0}3000", mun);
				this.GetComponent<MainGameS> ().players [currentPlayer].money += 3000;
				break;

			}

			spaceText.SetActive (true);
			chancecommunityOkayButton.SetActive (true);


		}
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
			spaceText.GetComponent<Text> ().text = string.Format("Everyone got {0}100 from p{1}",mun,currentPlayer+1);




		}
		spaceText.SetActive (true);
		oKButton.SetActive (true);

	}

	public void okButtonPushed(){
		oKButton.SetActive (false);
		spaceText.SetActive (false);
		spaceTitle.SetActive (false);


		this.GetComponent<MainGameS> ().TurnOnTurnActions ();
		eventHappened = false;



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
