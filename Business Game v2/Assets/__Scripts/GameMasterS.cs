using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class GameMasterS : MonoBehaviour {

	public const string MOBILE = "mobile";
	public const string BOARD = "board";

	public const string INTERN = "intern";
	public const string INDIA = "india";



	public static string gameMode;

	public static string level;

	public static bool continuingGame = false;

	public static string saveLoadLocation;

	public static string saveLoadLocation2;


	 


	// Use this for initialization
	void Start () {
		saveLoadLocation  = Application.streamingAssetsPath+"/"+"savegame.txt";
		saveLoadLocation2  = Path.Combine(Application.persistentDataPath,"savegame.txt");
		gameMode = MOBILE;
		level = INTERN;
		Debug.Log ("Saved");
		DontDestroyOnLoad (this.gameObject);



		
	}
	

}
