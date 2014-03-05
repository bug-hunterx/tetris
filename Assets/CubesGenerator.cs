using UnityEngine;
using System.Collections;

public class CubesGenerator : MonoBehaviour {

	private int screenWidth;
	//public Camera camera;
	public GameObject[] cubePrefabs;
	public bool isGenerate = true;// false after cube is generated. true again after cube have fall down
	public bool isGameOver = false;
	private int columnNumber = 5;

	private float cubeSize;
	private int idCounter = 0;

	private GameObject lastGeneratedCube;
	FieldStateManager fieldStateManager;

	// Use this for initialization
	void Start () {

		//set cube size to 1/numberOfColumns of screen
		cubeSize = Screen.width/columnNumber;
		print ("Cube size: " + cubeSize);
        fieldStateManager = new FieldStateManager(columnNumber, (int) (Screen.height/cubeSize), cubeSize);
		//place generator in middle of top edge
		transform.position = new Vector3(0, Camera.main.orthographicSize+cubeSize/2, 0);

	}
	
	// Update is called once per frame
	void Update () {
		if(!isGameOver&&isGenerate)
		{
			lastGeneratedCube = generateNewCube();
			isGenerate = false;
		}
		if(!isGameOver&&!lastGeneratedCube.GetComponent<OneElementManager>().isFalling()){
			//if cube cannot be added into field map - game over
			isGameOver = !fieldStateManager.addCube(lastGeneratedCube);
			//generate new if last one has stoped
			isGenerate = true;
		}
		if (Input.GetKeyDown(KeyCode.Escape)) 
			Application.Quit(); 

	}

	private GameObject generateNewCube(){


		//get random prefab
		int prefabIndex = Random.Range(0, cubePrefabs.Length);
		GameObject currentCube = Instantiate(cubePrefabs[prefabIndex], transform.position, transform.rotation) as GameObject;

		currentCube.GetComponent<Transform>().localScale = new Vector3(cubeSize, cubeSize, 0.1f);

		currentCube.GetComponent<OneElementManager>().uniqueID = idCounter;
		idCounter++;
		print ("Cube generated with size: " + currentCube.transform.localScale);
		print ("Cube generated with sprite: " + currentCube.GetComponent<SpriteRenderer>().sprite.textureRect);
		currentCube.gameObject.layer = LayerMask.NameToLayer ("FallingCubes");


		return currentCube;
	}

}
