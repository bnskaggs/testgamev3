using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

	//public GameObject[] hVisualHolders;

	// Use this for initialization
	void Start () {

		AssignVisuals ();

		/*foreach(Space space in Gameboard)
		{
			space.owned = true;
			space.owner = 1;
		}*/

		//int count = -1;
	


		
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
		spaceText.GetComponent<Text> ().text = "-$500";
		spaceText.SetActive (true);
		oKButton.SetActive (true);

	}

	void propertySpace()
	{
		float rent;
		if (!Gameboard [currentSpace].owned) {
			spaceTitle.GetComponent<Text> ().text = Gameboard [currentSpace].sName;
			spaceTitle.SetActive (true);
			spaceText.GetComponent<Text> ().text = "$" + Gameboard [currentSpace].costToBuy.ToString ();
			spaceText.SetActive (true);
			buyButton.SetActive (true);
			auctionButton.SetActive (true);
		} else {
			if (Gameboard [currentSpace].owner != currentPlayer) {
				spaceTitle.GetComponent<Text> ().text = Gameboard [currentSpace].sName;
				spaceTitle.SetActive (true);

				if (!Gameboard [currentSpace].isMortgaged) {
					rent = CalculateRent ();
					spaceText.GetComponent<Text> ().text = "Paid $" + rent + " to P" + (Gameboard [currentSpace].owner + 1).ToString ();
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
					return 1000;
				else
					return 500;
			} else {
				if (Gameboard [currentSpace].owner == Gameboard [water].owner)
						return 1500;
					else
						return 1200;
				
			}

		}
		//rail and best
		if (Gameboard [currentSpace].tag == "utility2") {
			if (currentSpace == railway) {
				if (Gameboard [currentSpace].owner == Gameboard [best].owner)
					return 1350;
				else
					return 1000;
			} else {
				if (Gameboard [currentSpace].owner == Gameboard [railway].owner)
					return 1100;
				else
					return 600;

			}


		}
		//electric and boat
		if (Gameboard [currentSpace].tag == "utility3") {
			if (currentSpace == electric) {
				if (Gameboard [currentSpace].owner == Gameboard [motoboat].owner)
					return 100 * currentRoll;
				else
					return 50 * currentRoll;
			} else {
				if (Gameboard [currentSpace].owner == Gameboard [electric].owner)
					return 200 * currentRoll;
				else
					return 100 * currentRoll;

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

	void restSpace()
	{
		this.GetComponent<MainGameS>().players [currentPlayer].lostTurn = true;
		this.GetComponent<MainGameS>().players [currentPlayer].lostTurnTime = 1;
		spaceTitle.GetComponent<Text> ().text = "Rest House";
		spaceTitle.SetActive (true);
		spaceText.GetComponent<Text> ().text = "Lose Turn";
		this.GetComponent<MainGameS>().playerNotes [currentPlayer].GetComponent<Text>().text = "Lost Turns: 2";
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
		spaceText.GetComponent<Text> ().text = "-$500";
		spaceText.SetActive (true);
		oKButton.SetActive (true);

	}

	void clubSpace()
	{
		this.GetComponent<MainGameS>().players [currentPlayer].money -= 200;
		spaceTitle.GetComponent<Text> ().text = "Club";
		spaceTitle.SetActive (true);
		spaceText.GetComponent<Text> ().text = "-$200";
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
			bidText.GetComponent<Text> ().text = "$0";
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
			spaceText.GetComponent<Text> ().text = "P" + (currentBidWinner+1).ToString () + " wins with $"+ highestBid;
			Gameboard [currentSpace].owned = true;
			Gameboard [currentSpace].owner = currentBidWinner;
			this.GetComponent<TokensS> ().ChangeOwnership (currentSpace, currentBidWinner);

			oKButton.SetActive (true);


		}




		

	}



}
