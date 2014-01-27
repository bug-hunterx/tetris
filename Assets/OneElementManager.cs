using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using AssemblyCSharp;


public class OneElementManager : MonoBehaviour, ITouchableObject{

	public int uniqueID;

	private bool falling = true;
	private Rigidbody2D myRigidBody;

	private float size;
	private float verticalSpeed;

	public LayerMask obstacles;

	private float delta = 0.05f;


	private float screenWidth = Screen.width;
	private float maxX;
	private float minX;


	//const vertical velocity
	private Vector2 vertVelocity;

	//touch or mouse input delta for last frame
	public float touchDistance;

	// Use this for initialization
	void Start () {
		myRigidBody = GetComponent<Rigidbody2D>();
		size = transform.localScale.x;

		//vertical speed. 20 - just coefficient //TODO what is mesure of speed? 
		verticalSpeed = Screen.height/10;

		vertVelocity = new Vector2(0, -1*verticalSpeed);
		myRigidBody.velocity = vertVelocity;

		maxX = Screen.width/4-size/4;
		minX = -Screen.width/4 + size/4;

	}
	
	// Update is called once per frame
	void Update () {

		moveIfNecessary();
		//checkForNeightbors();	

	}

	private void moveIfNecessary(){
		stopOrStartCubeIfNecessary();
		if(falling){
			//moveByKeyboard();

			moveByMouseOrTouch();

		}
	}

	void moveByKeyboard ()
	{
		//Vector3 position = transform.position;
		if(Input.GetKey(KeyCode.LeftArrow) ){
			myRigidBody.velocity = -100*Vector2.right + vertVelocity;
			//position.x -= size/2;
			//transform.position = position;
		}else if(Input.GetKey(KeyCode.RightArrow) ){
			myRigidBody.velocity = 100*Vector2.right + vertVelocity;
			//position.x += size/2;
			//transform.position = position;
		}else if(Input.GetKey(KeyCode.DownArrow)){
			//				//get coordinates of top edge of object below
			//				RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector3.down, Mathf.Infinity, obstacles);
			//				Vector3 startPos = transform.position;
			//				Vector3 endPos = new Vector3(transform.position.x, hit.point.y + size/4, 0);
			//				transform.position = Vector3.Lerp(startPos, endPos, moveDownTime);
			myRigidBody.velocity = 10*vertVelocity;
			
		}else{
			myRigidBody.velocity = vertVelocity;
		}
	}

	void moveByMouseOrTouch ()
	{
		if(touchDistance!=0){

			float newXCoordinate = getNewCoordinateRegardingObstacles(touchDistance);
			transform.position = new Vector3 (newXCoordinate, transform.position.y, transform.position.z);
			touchDistance=0;
		}

	}

	//calculate real distance on which cube can be moved. cubes can't overlap each other.
	private float getNewCoordinateRegardingObstacles (float touchDistance)
	{
		float newPosition = transform.position.x + touchDistance;
		//check if new position overlaping obstacle
		RaycastHit2D hit = Physics2D.Linecast(
			transform.position, 
			transform.position + new Vector3(newPosition, 0, 0), 
			obstacles );
		if(hit){
			if(touchDistance>0){
				return hit.transform.position.x - size/2;
			}else{
				return hit.transform.position.x + size/2;
			}
		//check for screen edges
		}else if(newPosition<minX){
			return minX;
		}else if(newPosition>maxX){
			return maxX;
		}else{
			return newPosition;
		}
	}

	private void stopOrStartCubeIfNecessary(){
		if(isStayOnGroundOrOtherCube()){
			falling = false;
			myRigidBody.velocity = new Vector2(0,0);
			gameObject.layer = LayerMask.NameToLayer ("PlacedCubes");
			//as cube stops it became kinematic
			myRigidBody.isKinematic = true;
		}else{
			falling = true;
			myRigidBody.isKinematic = false;
			myRigidBody.velocity = new Vector2(0, -1*verticalSpeed);
			gameObject.layer = LayerMask.NameToLayer ("FallingCubes");
		}
	}

	private bool isStayOnGroundOrOtherCube(){
		return getObjectTouchedBottom();
	}

	private bool isOnLeftEdgeOfScreen(){
		float currentX = transform.position.x;
		//is on left edge of screen
		if(currentX<=(-Screen.width/4 + size/4)){
			return true;
		}
		return getObjectTouchedLeft();
	}

	private bool isOnRightEdgeOfScreen(){
		float currentX = transform.position.x;

		if(currentX>=(Screen.width/4-size/4)){
			return true;
		}
		return getObjectTouchedRight();
	}

	//all these method will return null if there is no such cube
	public RaycastHit2D getObjectTouchedBottom(){
		float currentX = transform.position.x;
		float currentY = transform.position.y;
		float z =0;
		RaycastHit2D hitLeftCorner = Physics2D.Linecast(
			new Vector3(currentX - size/4f + delta, currentY - size/4f, z), 
			new Vector3(currentX - size/4f + delta, currentY - size/4f - delta, z), 
			obstacles );
		if(hitLeftCorner){
			return hitLeftCorner;
		}
		RaycastHit2D hitRightCorner = Physics2D.Linecast(
			new Vector3(currentX + size/4f - delta, currentY - size/4f, z), 
			new Vector3(currentX + size/4f - delta, currentY - size/4f - delta, z), 
			obstacles );
		if(hitRightCorner){
			return hitRightCorner;
		}
		//return null wraped into 
		return hitRightCorner;
	}

	public RaycastHit2D getObjectTouchedTop(){
		float currentX = transform.position.x;
		float currentY = transform.position.y;
		float z =0;
		RaycastHit2D hitLeftCorner = Physics2D.Linecast(
			new Vector3(currentX - size/4f + delta, currentY + size/4f, z), 
			new Vector3(currentX - size/4f + delta, currentY + size/4f + delta, z), 
			obstacles );
		if(hitLeftCorner){
			return hitLeftCorner;
		}
		RaycastHit2D hitRightCorner = Physics2D.Linecast(
			new Vector3(currentX + size/4f - delta, currentY + size/4f, z), 
			new Vector3(currentX + size/4f - delta, currentY + size/4f + delta, z), 
			obstacles );
		if(hitRightCorner){
			return hitRightCorner;
		}
		//return null wraped into 
		return hitRightCorner;
	}

	
	public RaycastHit2D getObjectTouchedLeft(){
		float currentX = transform.position.x;
		float currentY = transform.position.y;
		float z =0;
		//run linecast from left-bottom corner horizontaly
		RaycastHit2D hit = Physics2D.Linecast(
			new Vector3(currentX - size/4f, currentY - size/4f, z), 
			new Vector3(currentX - size/4f - delta, currentY - size/4f, z), 
			obstacles );
		//Debug.Log("Touch smth on left is " + (bool) hit);
		return hit;
	}

	public RaycastHit2D getObjectTouchedRight(){
		float currentX = transform.position.x;
		float currentY = transform.position.y;
		float z =0;
		//chech from right-bottom corner
		RaycastHit2D hit = Physics2D.Linecast(
			new Vector3(currentX + size/4f, currentY - size/4f, z), 
			new Vector3(currentX + size/4f + delta, currentY - size/4f, z), 
			obstacles );
		//Debug.Log("Touch smth on right is " + (bool) hit);
		return hit;
	}



//	//If this box is not failing and touch more then two other boxes with same texture - delte them all and increase score
//	void checkForNeightbors ()
//	{
//		if(!falling){
//			List<RaycastHit2D> hits = new List<RaycastHit2D>();
//			RaycastHit2D down = getObjectTouchedBottom();
//			RaycastHit2D left = getObjectTouchedLeft();
//			RaycastHit2D right = getObjectTouchedRight();
//			hits.Add(down);
//			hits.Add(left);
//			hits.Add(right);
//			filterListBySprite(hits);
//			if(hits.Count>=2){
//				Debug.Log("One cube has " + hits.Count + " neightbirs with same texture");
//				destroyAll(hits);
//				Destroy(gameObject);
//			}
//		}
//	}
//
//	void filterListBySprite (List<RaycastHit2D> hits)
//	{
//		for(int i=0;i<hits.Count;i++){
//			if(!hits[i]){
//				hits.Remove(hits[i]);
//				i--;
//			}else{
//				Sprite thisSprite = gameObject.GetComponent<SpriteRenderer>().sprite;
//				Sprite thatSprite = hits[i].collider.gameObject.GetComponent<SpriteRenderer>().sprite;
//				if(thisSprite != thatSprite){
//					hits.Remove(hits[i]);
//					i--;
//				}
//			}
//		}
//	}
//
//	void destroyAll (List<RaycastHit2D> hits)
//	{
//		for(int i=0;i<hits.Count;i++){
//			Destroy(hits[i].collider.gameObject);
//			i--;
//		}
//	}

	public bool isFalling(){
		return falling;
	}


	#region ITouchableObject implementation
	public void handleTouch (Vector2 touchCoordinates)
	{
		throw new NotImplementedException ();
	}
	public void handleDrag (Vector2 distance)
	{
		//move horizontaly only
		touchDistance = distance.x;
	}
	#endregion
}
