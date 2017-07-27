using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerColorChangerS : MonoBehaviour {
	public GameObject[] topImages = new GameObject[4];
	public GameObject[] gamePieces = new GameObject[4];


	// Use this for initialization
	void Start () {
		
		
	}

	public void InitializePiecesByColor(){
		for (int x = 0; x < 4; x++) {
			topImages [x] = GameObject.Find (string.Format("Player {0} Image",x+1));
			gamePieces[x] = GameObject.Find(string.Format("Player{0}_Image",x+1));

		}
		for (int x = 0; x < 4; x++) {
			print (this.GetComponent<MainGameS> ().players [x].color);
			print (topImages [x].GetComponent<SpriteRenderer> ().sprite.name);
			print (this.GetComponentInChildren<VisualHolderS> ().redPlayerChar.name);
			switch (this.GetComponent<MainGameS> ().players [x].color) {
			case "red":
				topImages [x].GetComponent<SpriteRenderer> ().sprite = this.GetComponentInChildren<VisualHolderS> ().redPlayerChar;
				topImages [x].GetComponent<SpriteRenderer> ().color = Color.red;
				gamePieces [x].GetComponent<SpriteRenderer> ().sprite = this.GetComponentInChildren<VisualHolderS> ().redPlayerChar;
				gamePieces [x].GetComponent<SpriteRenderer> ().color = Color.red;
				break;
			case "blue":
				topImages [x].GetComponent<SpriteRenderer> ().sprite = this.GetComponentInChildren<VisualHolderS> ().bluePlayerChar;
				gamePieces [x].GetComponent<SpriteRenderer> ().sprite = this.GetComponentInChildren<VisualHolderS> ().bluePlayerChar;
				break;
			case "yellow":
				topImages [x].GetComponent<SpriteRenderer> ().sprite = this.GetComponentInChildren<VisualHolderS> ().yellowPlayerChar;
				gamePieces [x].GetComponent<SpriteRenderer> ().sprite = this.GetComponentInChildren<VisualHolderS> ().yellowPlayerChar;
				break;
			case "green":
				topImages [x].GetComponent<SpriteRenderer> ().sprite = this.GetComponentInChildren<VisualHolderS> ().greenPlayerChar;
				gamePieces [x].GetComponent<SpriteRenderer> ().sprite = this.GetComponentInChildren<VisualHolderS> ().greenPlayerChar;
				break;


			}

		}

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
