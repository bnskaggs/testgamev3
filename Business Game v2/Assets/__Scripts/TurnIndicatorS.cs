using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnIndicatorS : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	public void ChangePlayer(int player){
		this.GetComponent<Text> ().text = string.Format ("P{0} Turn", player+1);

	}

	public void ChangeText (string stext)
	{
		this.GetComponent<Text> ().text = stext;
	}
}
