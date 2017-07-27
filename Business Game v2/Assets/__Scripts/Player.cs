using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player {
	public int playerNumber;
	public string cname = "Sam";
	public GameObject playerObject = null;
	public float money = 12000;
	public int location;
	public bool lostTurn =false;
	public int lostTurnTime = 0;
	public bool jail = false;
	public bool doNotCollectFromGo=false;
	public bool inTheGame = true;
	public string color = "red";


	public Player()
	{
		playerNumber = 0;
		location = 0;

	}

	public Player(int number, GameObject pO,string color2){
		playerNumber = number;
		location = 0;
		playerObject = pO;
		color = color2;
	}

	public Player(int number, GameObject pO,string color2, string name){
		playerNumber = number;
		location = 0;
		playerObject = pO;
		color = color2;
		cname = name; 
	}

	public void MoneyChange(float moneyGainedOrLost){
		money += moneyGainedOrLost;

	}

	public void SetCurrentPos(int newPos){
		location = newPos;

	}

	public int GetCurrentPos(){
		return location;

	}


}
