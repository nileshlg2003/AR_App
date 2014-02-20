using UnityEngine;
using System.Collections;

public class CameraZoomPinch : MonoBehaviour
{
		private Transform mBuildingTransform = null;
		private float touchDelta = 0.0F;
		private Vector2 prevDist = new Vector2 (0, 0);
		private Vector2 curDist = new Vector2 (0, 0);
		private float curPos = 0.0F;
		private float prevPos = 0.0F;
		//private int vertOrHorzOrientation = 0; //this tells if the two fingers to each other are oriented horizontally or vertically, 1 for vertical and -1 for horizontal

		// Use this for initialization
		void Start ()
		{
				GameObject building = GameObject.Find ("EmpireState_fbx");

				if (building != null) {
						Debug.Log ("building found");
						mBuildingTransform = building.transform;
				}
		}
	
		// Update is called once per frame
		void Update ()
		{
				// Pinch Input
				if (Input.touchCount == 2 && 
						Input.GetTouch (0).phase == TouchPhase.Moved && 
						Input.GetTouch (1).phase == TouchPhase.Moved) {
						//current distance between finger touches
						curDist = Input.GetTouch (0).position - Input.GetTouch (1).position; 

						//difference in previous locations using delta positions
						prevDist = ((Input.GetTouch (0).position - Input.GetTouch (0).deltaPosition) - (Input.GetTouch (1).position - Input.GetTouch (1).deltaPosition)); 

						touchDelta = curDist.magnitude - prevDist.magnitude;
			
//						if ((Input.GetTouch (0).position.x - Input.GetTouch (1).position.x) > (Input.GetTouch (0).position.y - Input.GetTouch (1).position.y)) {
//								vertOrHorzOrientation = -1; 
//						}
//						if ((Input.GetTouch (0).position.x - Input.GetTouch (1).position.x) < (Input.GetTouch (0).position.y - Input.GetTouch (1).position.y)) {
//								vertOrHorzOrientation = 1;
//						}
			
						if (touchDelta < 0) {
								float oldScale = mBuildingTransform.localScale.x;
								float newScale = oldScale / 1.1f;
								mBuildingTransform.localScale = new Vector3 (newScale, newScale, newScale);
						} else {
								float oldScale = mBuildingTransform.localScale.x;
								float newScale = oldScale * 1.1f;
								mBuildingTransform.localScale = new Vector3 (newScale, newScale, newScale);
						}
				}
		// Drag Input
		else if (Input.touchCount == 1 && 
						Input.GetTouch (0).phase == TouchPhase.Moved) {
						//current distance between finger touches
						curPos = Input.GetTouch (0).position.x; 
						prevPos = Input.GetTouch (0).deltaPosition.x; 
			
						touchDelta = curPos - prevPos;

						//this code is for x-axis rotation
						if (Input.GetTouch (0).deltaPosition.x < 0 || 
								Input.GetTouch (0).deltaPosition.x > 0) {
								float tiltAroundY = Input.GetAxis ("Horizontal") * 15.0F; // 15 = angle of rotation
								var target1 = Quaternion.Euler (tiltAroundY, 0.0F, 0.0F);
								mBuildingTransform.rotation = Quaternion.Slerp (mBuildingTransform.rotation, target1, Time.deltaTime);
								mBuildingTransform.Rotate (0.0F, Input.GetTouch (0).deltaPosition.x * 1.0F, 0.0F, Space.World);
						}
				}
		}
}