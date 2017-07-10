using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderTextChangeS : MonoBehaviour {

	public GameObject slider;

	// Use this for initialization
	void Start () {
		//slider = GameObject.Find ("Bid Slider");

		slider.GetComponent<Slider> ().onValueChanged.AddListener (delegate {
			ValueChangeCheck ();
		});
			{
		}
		
	}
	
	// Update is called once per frame
	void ValueChangeCheck () {
		this.GetComponent<Text> ().text = "$"+slider.GetComponent<Slider> ().value.ToString ();
	}
}
