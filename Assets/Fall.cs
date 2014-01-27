using UnityEngine;
using System.Collections;

public class Fall : MonoBehaviour {

	public float forceModifier;

	// Use this for initialization
	void Start () {
		float verticalSpeed = Screen.height/5;

		rigidbody2D.velocity = new Vector2(0, -1* forceModifier);
	}
	
	// Update is called once per frame
	void Update () {
		//vertical speed. 15 - just coefficient //TODO what is mesure of speed? 
		//TODO stop use public variable

	}
}
