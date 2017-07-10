using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMasterS : MonoBehaviour {

	public const string MOBILE = "mobile";
	public const string BOARD = "board";

	public const string INTERN = "intern";
	public const string INDIA = "india";



	public static string gameMode;

	public static string level;

	public static bool continuingGame = false;

	 


	// Use this for initialization
	void Start () {
		gameMode = MOBILE;
		level = INTERN;
		DontDestroyOnLoad (this.gameObject);



		
	}
	

}
