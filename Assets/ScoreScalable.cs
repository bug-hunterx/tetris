using UnityEngine;
using System.Collections;

//TODO add scalability :) 
public class ScoreScalable : MonoBehaviour {

	//float virtualHeight = 480;
	//float virtualWidth = 320;
	//Matrix4x4 scaleMatrix;
	//GUIText scoreLabel;
	public int score ;
	private int previousScore;

	// Use this for initialization
	void Start () {
		//scaleMatrix = Matrix4x4.TRS (Vector3.zero, Quaternion.identity, new Vector3(Screen.width/virtualWidth, Screen.height/virtualHeight, 1.0f));
		//transform.position = new Vector3(0, transform.localScale.y/8-1*Screen.height/4, 0);
		// Setting up the reference.

	
	}



	
	// Update is called once per frame
	void Update () {
		// Set the score text.
		guiText.text = "Score: " + score;
		
		// If the score has changed...
		//if(previousScore != score)
			// ... play a taunt.
			//playerControl.StartCoroutine(playerControl.Taunt());
		
		// Set the previous score to this frame's score.
		previousScore = score;
	}

	public void updateScore(int numberCubesDestroyed){
		score = score + 100*numberCubesDestroyed;
	}
}
