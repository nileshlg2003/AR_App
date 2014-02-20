using UnityEngine;
using System.Collections;

public class DragRotationHandler : MonoBehaviour {

	private Transform mBuildingTransform = null;
	private float touchDelta = 0.0F;
	private float curPos = 0.0F;
	private float prevPos = 0.0F;
	private int vertOrHorzOrientation = 0; //this tells if the two fingers to each other are oriented horizontally or vertically, 1 for vertical and -1 for horizontal

	// Use this for initialization
	void Start () {
		GameObject gameObject = this.gameObject;

		if (gameObject != null)
		{
			Debug.Log("Game Object found");
			mBuildingTransform = gameObject.transform;
		}
	}
	
	// Update is called once per frame
	void Update () {
		// Drag Input
		if (Input.touchCount == 1 && 
	         Input.GetTouch(0).phase == TouchPhase.Moved) 
		{
			//current distance between finger touches
			curPos = Input.GetTouch(0).position.x; 
			prevPos = Input.GetTouch(0).deltaPosition.x; 
			
			touchDelta = curPos - prevPos;
			
			//this code is for x-axis rotation
			if(Input.GetTouch(0).deltaPosition.x < 0|| 
			   Input.GetTouch(0).deltaPosition.x > 0)
			{
				float tiltAroundY = Input.GetAxis("Horizontal") * 15.0F; // 15 = angle of rotation
				var target1 = Quaternion.Euler(tiltAroundY, 0.0F, 0.0F);
				mBuildingTransform.rotation = Quaternion.Slerp(mBuildingTransform.rotation, target1,Time.deltaTime);
				mBuildingTransform.Rotate(0.0F, Input.GetTouch(0).deltaPosition.x * 1.0F, 0.0F, Space.World);
			}
		}
	}
}
