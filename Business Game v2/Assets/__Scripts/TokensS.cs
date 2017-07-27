using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TokensS : MonoBehaviour {
	public GameObject[] ownershipTokens;

	public Material redMat;
	public Material blueMat;
	public Material greenMat;
	public Material yellowMat;

	public Material redMortMat;
	public Material blueMortMat;
	public Material greenMortMat;
	public Material yellowMortMat;

	public Material redSelectedMat;
	public Material blueSelectedMat;
	public Material greenSelectedMat;
	public Material yellowSelectedMat;


	// Use this for initialization
	void Start () {
		ownershipTokens = new GameObject[36];
		for(int x = 0; x<36; x++)
		{
			ownershipTokens [x] = GameObject.Find ("T" + x.ToString());

		}
		
	}
	
	// Update is called once per frame
	public void ChangeOwnership(int space, int player){
		
		ownershipTokens [space].GetComponent<Renderer> ().enabled = true;

		if (player == 666)
			ownershipTokens [space].GetComponent<Renderer> ().enabled = false;
		else {

			switch (this.GetComponent<MainGameS>().players[player].color) {
			case "red":
				ownershipTokens [space].GetComponent<Renderer> ().material = redMat;
				break;

			case "blue":
				ownershipTokens [space].GetComponent<Renderer> ().material = blueMat;
				break;

			case "green":
				ownershipTokens [space].GetComponent<Renderer> ().material = greenMat;
				break;

			case "yellow":
				ownershipTokens [space].GetComponent<Renderer> ().material = yellowMat;
				break;
	
			}
		}





	}

	public void ChangeMortgage(int space, int player){
		//ownershipTokens [space].GetComponent<Renderer> ().enabled = true;


		if (player == 666)
			ownershipTokens [space].GetComponent<Renderer> ().enabled = false;
		else {

			switch (this.GetComponent<MainGameS>().players[player].color) {
			case "red":
				ownershipTokens [space].GetComponent<Renderer> ().material = redMortMat;
				break;

			case "blue":
				ownershipTokens [space].GetComponent<Renderer> ().material = blueMortMat;
				break;

			case "green":
				ownershipTokens [space].GetComponent<Renderer> ().material = greenMortMat;
				break;

			case "yellow":
				ownershipTokens [space].GetComponent<Renderer> ().material = yellowMortMat;
				break;

			}
		}



	}
	public void ChangeUnMortgage(int space, int player){

		if (player == 666)
			ownershipTokens [space].GetComponent<Renderer> ().enabled = false;
		else {

			switch (this.GetComponent<MainGameS>().players[player].color) {
			case "red":
				ownershipTokens [space].GetComponent<Renderer> ().material = redMat;
				break;

			case "blue":
				ownershipTokens [space].GetComponent<Renderer> ().material = blueMat;
				break;

			case "green":
				ownershipTokens [space].GetComponent<Renderer> ().material = greenMat;
				break;

			case "yellow":
				ownershipTokens [space].GetComponent<Renderer> ().material = yellowMat;
				break;

			}
		}

	}








	public void ChangeSelect(int space, int player){
		//ownershipTokens [space].GetComponent<Renderer> ().enabled = true;


		if (player == 666)
			ownershipTokens [space].GetComponent<Renderer> ().enabled = false;
		else {

			switch (this.GetComponent<MainGameS>().players[player].color) {
			case "red":
				ownershipTokens [space].GetComponent<Renderer> ().material = redSelectedMat;
				break;

			case "blue":
				ownershipTokens [space].GetComponent<Renderer> ().material = blueSelectedMat;
				break;

			case "green":
				ownershipTokens [space].GetComponent<Renderer> ().material = greenSelectedMat;
				break;

			case "yellow":
				ownershipTokens [space].GetComponent<Renderer> ().material = yellowSelectedMat;
				break;

			}
		}



	}
	public void ChangeUnselect(int space, int player){

		if (player == 666)
			ownershipTokens [space].GetComponent<Renderer> ().enabled = false;
		else {

			switch (this.GetComponent<MainGameS> ().players [player].color) {
			case "red":
				ownershipTokens [space].GetComponent<Renderer> ().material = redMat;
				break;

			case "blue":
				ownershipTokens [space].GetComponent<Renderer> ().material = blueMat;
				break;

			case "green":
				ownershipTokens [space].GetComponent<Renderer> ().material = greenMat;
				break;

			case "yellow":
				ownershipTokens [space].GetComponent<Renderer> ().material = yellowMat;
				break;

			}
		}
	}



}
