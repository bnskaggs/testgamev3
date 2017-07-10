using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMasterS : MonoBehaviour {

	public const string MOBILE = "mobile";
	public const string BOARD = "board";

	public static string gameMode;


	// Use this for initialization
	void Start () {
		gameMode = MOBILE;
		DontDestroyOnLoad (this.gameObject);
		
	}
	

}
