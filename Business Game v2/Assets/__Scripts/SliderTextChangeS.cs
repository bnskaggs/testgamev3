using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderTextChangeS : MonoBehaviour {

	public GameObject slider;
	private string mun;

	// Use this for initialization
	void Start () {
		//slider = GameObject.Find ("Bid Slider");

		slider.GetComponent<Slider> ().onValueChanged.AddListener (delegate {
			ValueChangeCheck ();
		});
			{
		}

		mun = "#";

		if (GameMasterS.level == GameMasterS.INDIA)
			mun = "₹";
		if (GameMasterS.level ==GameMasterS.INTERN)
			mun = "$";
		
		
	}
	
	// Update is called once per frame
	void ValueChangeCheck () {
		this.GetComponent<Text> ().text = "$"+slider.GetComponent<Slider> ().value.ToString ();
	}
}
