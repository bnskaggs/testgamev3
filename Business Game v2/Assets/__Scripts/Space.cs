using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum spaceType{
	utility, jail, rest, tax, chance, property, start, community, club

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
	public GameObject spaceSelectButton;


	public GameObject hVisualHolder;
	public GameObject[] hVisuals;



	public void ActivateVisuals(){
		for (int x = 0; x < numberOfHouses; x++) {
			hVisuals [x].GetComponent<MeshRenderer> ().enabled = true;
		}
		if (hotel)
			hVisuals [3].GetComponent<MeshRenderer> ().enabled = true;
				

		

	}

	public void DeactivateVisuals(){

		foreach (GameObject visual in hVisuals)
			visual.GetComponent<MeshRenderer> ().enabled = false;
	}






}
