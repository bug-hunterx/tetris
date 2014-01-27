using UnityEngine;
using System.Collections;
using AssemblyCSharp;
using System;

public class TouchControll : MonoBehaviour
{

		ITouchableObject catchedTouchableObject;
		private float radius = 0.1f;
		Vector2 touchPositionInLastFrame;
		public LayerMask touchableObjects;

		// Use this for initialization
		void Start ()
		{

		}

		// Update is called once per frame
		void Update ()
		{

			// If we click the mouse - find clicked object
			if (Input.GetMouseButtonDown (0)) { 
					//Debug.Log ("Mouse button down");

					Vector2 touchPosition = Camera.main.ScreenToWorldPoint (Input.mousePosition);
					//Debug.Log ("Mouse down. Position =  " + touchPosition);
					Collider2D[] colliders = Physics2D.OverlapCircleAll (touchPosition, radius, touchableObjects);

					//Debug.Log ("Hit " + colliders.Length + " objects");

					if (colliders.Length > 0) {    
						// If we hit several objects process only first one
						
						catchedTouchableObject = (ITouchableObject) colliders[0].gameObject.GetComponentInChildren(typeof(ITouchableObject));
						touchPositionInLastFrame = touchPosition;
						
						//Debug.Log ("Object catched = " + catchedTouchableObject);
						//Debug.Log ("mouse position = " + touchPositionInLastFrame);
                		
                

					}
		
			} else if (Input.GetMouseButton (0) && catchedTouchableObject != null) {
					//Debug.Log ("Mouse pressed");
					//move object on X axis
					Vector2 touchPosition = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			catchedTouchableObject.handleDrag(touchPosition - touchPositionInLastFrame);
					touchPositionInLastFrame = touchPosition;

			} else if (Input.GetMouseButtonUp (0)) {
					//Debug.Log ("Mouse up");
					catchedTouchableObject = null;     
		
			}

			
		}


}
