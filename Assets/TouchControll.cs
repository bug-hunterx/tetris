using UnityEngine;
using System.Collections;

public class TouchControll : MonoBehaviour
{

		Transform objectTransform = null;
		float offsetX;
		private float radius = 0.1f;
		Vector2 mousePositionInLastFrame;
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
						Debug.Log ("Mouse button down");

						Vector2 touchPosition = Camera.main.ScreenToWorldPoint (Input.mousePosition);
						Debug.Log ("Mouse down. Position =  " + touchPosition);
						Collider2D[] colliders = Physics2D.OverlapCircleAll (touchPosition, radius, touchableObjects);

						Debug.Log ("Hit " + colliders.Length + " objects");

						if (colliders.Length > 0) {    
								// If we hit several objects process only first one
								objectTransform = colliders [0].transform;   
								mousePositionInLastFrame = touchPosition;

								Debug.Log ("Object position = " + objectTransform.position);
								Debug.Log ("mouse position = " + mousePositionInLastFrame);

						}
			
				} else if (Input.GetMouseButton (0) && objectTransform != null) {
						Debug.Log ("Mouse pressed");
						//move object on X axis
						Vector2 touchPosition = Camera.main.ScreenToWorldPoint (Input.mousePosition);
						offsetX = mousePositionInLastFrame.x - touchPosition.x;
			//TODO send offsetX value to oneElementManager and let him check for borders and everything
						Debug.Log ("OffsetX = " + offsetX);
						objectTransform.position = new Vector3 (objectTransform.position.x - offsetX, objectTransform.position.y, objectTransform.position.z);
						mousePositionInLastFrame = touchPosition;

				} else if (Input.GetMouseButtonUp (0)) {
						Debug.Log ("Mouse up");
						objectTransform = null;     // Let go of the object.
			
				}

			
		}


}
