using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player {
	public int playerNumber;
	public GameObject playerObject = null;
	public float money = 12000;
	public int location;
	public bool lostTurn =false;
	public int lostTurnTime = 0;
	public bool jail = false;
	public bool doNotCollectFromGo=false;
	public bool inTheGame = true;


	public Player()
	{
		playerNumber = 0;
		location = 0;

	}

	public Player(int number, GameObject pO){
		playerNumber = number;
		location = 0;
		playerObject = pO;

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
