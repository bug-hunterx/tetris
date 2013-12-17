using UnityEngine;
using System.Collections;

public class BottomPanelManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
        //place bottom panel on bottom of the screen 
        transform.position = new Vector3(0, transform.localScale.y/8-1*Screen.height/4, 0);
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
