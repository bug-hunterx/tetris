using UnityEngine;
using System.Collections;

public class CameraSetUp : MonoBehaviour {

	// Use this for initialization
	void Start () {

		print ("Screen resolution: " + Screen.width + "; " + Screen.height);
		//TODO why 4?
		Camera.main.orthographicSize = Screen.height/4;
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
