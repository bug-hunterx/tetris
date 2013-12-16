using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class OneElementManager : MonoBehaviour {

	public int uniqueID;

	private bool falling = true;
	private Rigidbody2D myRigidBody;

	private float size;
	private float verticalSpeed;

	private int groundMask = 1 << LayerMask.NameToLayer("Ground");
	private int placedCubeMask = 1 << LayerMask.NameToLayer ("PlacedCubes");

	private float delta = 0.01f;

	private float moveDownTime = 0.9f;

	private float screenWidth = Screen.width;

	// Use this for initialization
	void Start () {
		myRigidBody = GetComponent<Rigidbody2D>();
		size = transform.localScale.x;

		//vertical speed. 15 - just coefficient //TODO what is mesure of speed? 
		verticalSpeed = Screen.height/15;
		myRigidBody.velocity = new Vector2(0, -1*verticalSpeed);

	}
	
	// Update is called once per frame
	void Update () {

		moveIfNecessary();
		//checkForNeightbors();	

	}

	private void moveIfNecessary(){
		stopOrStartCubeIfNecessary();
		if(falling){
			Vector3 position = transform.position;
			//print ("Current possition of " + uniqueID +" is " + position);
			if(Input.GetKeyDown(KeyCode.LeftArrow) && !isBlockedFromLeft()){
				position.x -= size/2;
				transform.position = position;
			}

			if(Input.GetKeyDown(KeyCode.RightArrow) && !isBlockedFromRight()){
				position.x += size/2;
				transform.position = position;
			}

			if(Input.GetKeyDown(KeyCode.DownArrow)){
				//get coordinates of top edge of object below
				RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector3.down, Mathf.Infinity, groundMask | placedCubeMask);
				Vector3 startPos = transform.position;
				Vector3 endPos = new Vector3(transform.position.x, hit.point.y + size/4, 0);
				transform.position = Vector3.Lerp(startPos, endPos, moveDownTime);

			}
		}
	}

	private void stopOrStartCubeIfNecessary(){
		if(isStayOnGroundOrOtherCube()){
			falling = false;
			myRigidBody.velocity = new Vector2(0,0);
			gameObject.layer = LayerMask.NameToLayer ("PlacedCubes");
		}else{
			falling = true;
			myRigidBody.velocity = new Vector2(0, -1*verticalSpeed);
			gameObject.layer = LayerMask.NameToLayer ("FallingCubes");
		}
	}

	private bool isStayOnGroundOrOtherCube(){
		return getObjectTouchedBottom();
	}

	private bool isBlockedFromLeft(){
		float currentX = transform.position.x;
		//is on left edge of screen
		if(currentX<=(-Screen.width/4+size/2)){
			return true;
		}
		return getObjectTouchedLeft();
	}

	private bool isBlockedFromRight(){
		float currentX = transform.position.x;

		if(currentX>=(Screen.width/4-size/2)){
			return true;
		}
		return getObjectTouchedRight();
	}

	private RaycastHit2D getObjectTouchedBottom(){
		float currentX = transform.position.x;
		float currentY = transform.position.y;
		float z =0;
		RaycastHit2D hit = Physics2D.Linecast(
			new Vector3(currentX, currentY - size/4f-delta, z), 
			new Vector3(currentX, currentY - size/4f -delta, z), 
			placedCubeMask | groundMask );
		Debug.Log("Touch smth on down is " + (bool) hit);
		return hit;
	}

	
	private RaycastHit2D getObjectTouchedLeft(){
		float currentX = transform.position.x;
		float currentY = transform.position.y;
		float z =0;
		//run linecast from left-bottom corner horizontaly
		RaycastHit2D hit = Physics2D.Linecast(
			new Vector3(currentX - size/4f-delta , currentY - size/4f - delta, z), 
			new Vector3(currentX - size/2 + delta, currentY, z), 
			placedCubeMask | groundMask );
		Debug.Log("Touch smth on left is " + (bool) hit);
		return hit;
	}

	private RaycastHit2D getObjectTouchedRight(){
		float currentX = transform.position.x;
		float currentY = transform.position.y;
		float z =0;
		//chech from right-bottom corner
		RaycastHit2D hit = Physics2D.Linecast(
			new Vector3(currentX + size/4f + delta , currentY - size/4f - delta, z), 
			new Vector3(currentX + size/2 - delta, currentY, z), 
			placedCubeMask | groundMask );
		Debug.Log("Touch smth on right is " + (bool) hit);
		return hit;
	}



	//If this box is not failing and touch more then two other boxes with same texture - delte them all and increase score
	void checkForNeightbors ()
	{
		if(!falling){
			List<RaycastHit2D> hits = new List<RaycastHit2D>();
			RaycastHit2D down = getObjectTouchedBottom();
			RaycastHit2D left = getObjectTouchedLeft();
			RaycastHit2D right = getObjectTouchedRight();
			hits.Add(down);
			hits.Add(left);
			hits.Add(right);
			filterListBySprite(hits);
			if(hits.Count>=2){
				Debug.Log("One cube has " + hits.Count + " neightbirs with same texture");
				destroyAll(hits);
				Destroy(gameObject);
			}
		}
	}

	void filterListBySprite (List<RaycastHit2D> hits)
	{
		for(int i=0;i<hits.Count;i++){
			if(!hits[i]){
				hits.Remove(hits[i]);
				i--;
			}else{
				Sprite thisSprite = gameObject.GetComponent<SpriteRenderer>().sprite;
				Sprite thatSprite = hits[i].collider.gameObject.GetComponent<SpriteRenderer>().sprite;
				if(thisSprite != thatSprite){
					hits.Remove(hits[i]);
					i--;
				}
			}
		}
	}

	void destroyAll (List<RaycastHit2D> hits)
	{
		for(int i=0;i<hits.Count;i++){
			Destroy(hits[i].collider.gameObject);
			i--;
		}
	}

	public bool isFalling(){
		return falling;
	}


}
