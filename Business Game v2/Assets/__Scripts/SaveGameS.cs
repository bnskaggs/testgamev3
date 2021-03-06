﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SaveGameS : MonoBehaviour {

	// Use this for initialization

	
	// Update is called once per frame


	public void SaveTheGame(){
		//string path = "Assets/Resources/savegame.txt";
		string path  = GameMasterS.saveLoadLocation2;
		print("paht:"+ path);
		//FileInfo f = new FileInfo (Application.persistentDataPath + "//" + "savegame.txt");
		File.WriteAllText (@path,string.Empty );
		StreamWriter sw = new StreamWriter (path,true);
		sw.WriteLine (GameMasterS.level);
		sw.WriteLine (GameMasterS.gameMode);
		sw.WriteLine (this.GetComponent<MainGameS> ().currentPlayer);
		sw.WriteLine (this.GetComponent<MainGameS> ().players.Length);
		foreach (Player player in this.GetComponent<MainGameS>().players) {
			sw.WriteLine(string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9}",
				player.playerNumber,player.money,player.location,player.lostTurn,player.lostTurnTime,player.jail,player.doNotCollectFromGo,player.inTheGame,player.color,player.cname));

		}
		//sw.WriteLine (GameMasterS.BOARD);
		sw.WriteLine (this.GetComponent<SpaceLogicS>().Gameboard.Length);
		foreach (Space space in this.GetComponent<SpaceLogicS>().Gameboard) {
			string boom = string.Format ("{0}&{1}&{2}", space.costWithHouses [0], space.costWithHouses [1], space.costWithHouses [2]);
		

			sw.WriteLine(string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13}",
				space.sName,space.type,space.owned,space.owner,space.costToBuy,space.costToRent,space.color,space.numberOfHouses,space.costPerHouse,boom,space.hotel,space.costWithHotel,space.isMortgaged, space.costPerHotel));
		}

		/*
		 * 
		 * public string sName;
	public spaceType type;
	public bool owned = false;
	public int owner = 666;
	public float costToBuy;
	public float costToRent;
	public colorGroup color;
	public bool house = false;
	public int numberOfHouses = 0;
	public float costPerHouse;
	public float[] costWithHouses = new float[3];
	public bool hotel=false;
	public float costPerHotel;
	public float costWithHotel;
	public bool isMortgaged = false;
	public GameObject spaceSelectButton;
*/

		sw.Close ();


	}

}
