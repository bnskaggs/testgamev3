using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoneyTextChangerS : MonoBehaviour {

	private bool playersNullFlag =true;

	public GameObject[] MoneyText;

	private string mun;


	void Start(){
		MoneyText = new GameObject[4];

		MoneyText [0] = GameObject.Find ("Player 1 Money");
		MoneyText [1] = GameObject.Find ("Player 2 Money");
		MoneyText [2] = GameObject.Find ("Player 3 Money");
		MoneyText [3] = GameObject.Find ("Player 4 Money");

		mun = "#";

		if (GameMasterS.level == GameMasterS.INDIA)
			mun = "₹";
		if (GameMasterS.level ==GameMasterS.INTERN || GameMasterS.level ==GameMasterS.GENERIC)
			mun = "$";
		



	}

	// Update is called once per frame
	void Update () {
		if (this.GetComponent<MainGameS>().players [0] != null)
			playersNullFlag = false;

		if(!playersNullFlag){
			for(int x = 0; x<4;x++){
				MoneyText [x].GetComponent<Text> ().text =mun+ this.GetComponent<MainGameS>().players [x].money.ToString();

			}

		}
		
	}
}
