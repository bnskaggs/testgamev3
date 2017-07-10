using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SaveGameS : MonoBehaviour {

	// Use this for initialization

	
	// Update is called once per frame


	public void SaveTheGame(){
		string path = path = "Assets/Resources/savegame.txt";

		StreamWriter sw = new StreamWriter (path, true);
		foreach (Player player in this.GetComponent<MainGameS>().players) {
			sw.WriteLine(string.Format("{0},{1},{2},{3},{4},{5},{6},{7}",
				player.playerNumber,player.money,player.location,player.lostTurn,player.lostTurnTime,player.jail,player.doNotCollectFromGo,player.inTheGame));

		}

		foreach (Space space in this.GetComponent<SpaceLogicS>().Gameboard) {
			string boom = string.Format ("{0}&{1}&{2}", space.costWithHouses [0], space.costWithHouses [1], space.costWithHouses [2]);
		

			sw.WriteLine(string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12}",
				space.sName,space.type,space.owned,space.owner,space.costToBuy,space.costToRent,space.color,space.numberOfHouses,space.costPerHouse,boom,space.hotel,space.costWithHotel,space.isMortgaged));
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
