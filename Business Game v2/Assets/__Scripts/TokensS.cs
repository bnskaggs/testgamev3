using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TokensS : MonoBehaviour {
	public GameObject[] ownershipTokens;

	public Material p1Mat;
	public Material p2Mat;
	public Material p3Mat;
	public Material p4Mat;

	public Material p1MortMat;
	public Material p2MortMat;
	public Material p3MortMat;
	public Material p4MortMat;



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


		switch (player) {
		case 0:
			ownershipTokens [space].GetComponent<Renderer> ().material = p1Mat;
			break;

		case 1:
			ownershipTokens [space].GetComponent<Renderer> ().material = p2Mat;
			break;

		case 2:
			ownershipTokens [space].GetComponent<Renderer> ().material = p3Mat;
			break;

		case 3:
			ownershipTokens [space].GetComponent<Renderer> ().material = p4Mat;
			break;
		case 666:
			ownershipTokens [space].GetComponent<Renderer> ().enabled = false;
			break;

		}





	}

	public void ChangeMortgage(int space, int player){
		//ownershipTokens [space].GetComponent<Renderer> ().enabled = true;


		switch (player) {
		case 0:
			
				ownershipTokens [space].GetComponent<Renderer> ().material = p1MortMat;
		

			break;

		case 1:
			
				ownershipTokens [space].GetComponent<Renderer> ().material = p2MortMat;

				
			break;
		case 2:
			
				ownershipTokens [space].GetComponent<Renderer> ().material = p3MortMat;

			break;

		case 3:
			
				ownershipTokens [space].GetComponent<Renderer> ().material =p4MortMat;
			
			break;

		case 666:
			ownershipTokens [space].GetComponent<Renderer> ().enabled = false;
			break;

		}



	}
	public void ChangeUnMortgage(int space, int player){

		switch (player) {
		case 0:

			ownershipTokens [space].GetComponent<Renderer> ().material = p1Mat;


			break;

		case 1:

			ownershipTokens [space].GetComponent<Renderer> ().material = p2Mat;


			break;
		case 2:

			ownershipTokens [space].GetComponent<Renderer> ().material = p3Mat;

			break;

		case 3:

			ownershipTokens [space].GetComponent<Renderer> ().material = p4Mat;

			break;

		case 666:
			ownershipTokens [space].GetComponent<Renderer> ().enabled = false;
			break;


		}
	}
}
