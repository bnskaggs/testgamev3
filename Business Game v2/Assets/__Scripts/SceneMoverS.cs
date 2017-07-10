using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMoverS : MonoBehaviour {

	public string loadNextLevel;

	// Use this for initialization
	void Start () {
		SceneManager.LoadScene (loadNextLevel, LoadSceneMode.Single);
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
