using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NameChangeS : MonoBehaviour {

	public GameObject[] names = new GameObject[4];

	// Use this for initialization

	
	// Update is called once per frame

	public void LoadNames(){
		for (int x = 0; x < 4; x++) {
			names [x] = GameObject.Find ("Player " + (x+1).ToString() + " Name");
			names [x].GetComponent<Text> ().text = GameMasterS.customNames [x];
			this.GetComponent<MainGameS> ().players [x].cname = names [x].GetComponent<Text> ().text;
		}

	}



	void Update () {
		
	}
}
