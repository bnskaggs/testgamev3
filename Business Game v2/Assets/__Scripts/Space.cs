using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public enum spaceType{
	utility, jail, rest, tax, chance, property, start, community, club, wtax,itax

}

public enum colorGroup{
	red, transport, utility, blue, green, pink, none
}


public class Space : MonoBehaviour {


	public string sName;
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
	public Button spaceSelectButton;
	public bool selected = false;


	public GameObject hVisualHolder;
	public GameObject[] hVisuals;



	public void ActivateVisuals(Sprite vis){
		for (int x = 0; x < numberOfHouses; x++) {
			print (x + " " + numberOfHouses);
			hVisuals [x].GetComponent<SpriteRenderer> ().sprite = vis;
			hVisuals [x].GetComponent<SpriteRenderer> ().enabled = true;
		}
		if (hotel) {
			for (int x = 0; x < numberOfHouses; x++) {
				hVisuals [x].GetComponent<SpriteRenderer> ().enabled = false;
			}
			hVisuals [3].GetComponent<SpriteRenderer> ().sprite = vis;
			hVisuals [3].GetComponent<SpriteRenderer> ().enabled = true;
		}
				

		

	}

	public void DeactivateVisuals(){

		foreach (GameObject visual in hVisuals)
			visual.GetComponent<MeshRenderer> ().enabled = false;
	}

	public void SetSelected(bool things){
		selected = things;

	}






}
