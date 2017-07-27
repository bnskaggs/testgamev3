using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class CameraTestS : MonoBehaviour {

	// Use this for initialization
	public float orthographicSize = 5;
	public float aspect = 1.33333f;
	void Start()
	{
		Camera.main.projectionMatrix = Matrix4x4.Ortho(
			-orthographicSize * aspect, orthographicSize * aspect,
			-orthographicSize, orthographicSize,
			this.GetComponent<Camera>().nearClipPlane, this.GetComponent<Camera>().farClipPlane);
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
