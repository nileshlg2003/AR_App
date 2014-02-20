using UnityEngine;
using System.Collections;

public class PinchZoomHandler : MonoBehaviour {
	private Transform mBuildingTransform = null;
	private float touchDelta = 0.0F;
	private Vector2 prevDist = new Vector2(0,0);
	private Vector2 curDist = new Vector2(0,0);
	private int vertOrHorzOrientation = 0; //this tells if the two fingers to each other are oriented horizontally or vertically, 1 for vertical and -1 for horizontal
	
	// Use this for initialization
	void Start () 
	{
		GameObject building = GameObject.Find("EmpireState_fbx");
		
		if (building != null)
		{
			Debug.Log("building found");
			mBuildingTransform = building.transform;
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		// Pinch Input
		if (Input.touchCount == 2 && 
		    Input.GetTouch(0).phase == TouchPhase.Moved && 
		    Input.GetTouch(1).phase == TouchPhase.Moved) 
		{
			//current distance between finger touches
			curDist = Input.GetTouch(0).position - Input.GetTouch(1).position; 
			
			//difference in previous locations using delta positions
			prevDist = ((Input.GetTouch(0).position - Input.GetTouch(0).deltaPosition) - (Input.GetTouch(1).position - Input.GetTouch(1).deltaPosition)); 
			
			touchDelta = curDist.magnitude - prevDist.magnitude;
			
			if ((Input.GetTouch(0).position.x - Input.GetTouch(1).position.x) > (Input.GetTouch(0).position.y - Input.GetTouch(1).position.y))
			{
				vertOrHorzOrientation = -1; 
			}
			if ((Input.GetTouch(0).position.x - Input.GetTouch(1).position.x) < (Input.GetTouch(0).position.y - Input.GetTouch(1).position.y))
			{
				vertOrHorzOrientation = 1;
			}
			
			if (touchDelta < 0)
			{
				float oldScale = mBuildingTransform.localScale.x;
				float newScale = oldScale / 1.1f;
				mBuildingTransform.localScale = new Vector3(newScale, newScale, newScale);
			}			
			else
			{
				float oldScale = mBuildingTransform.localScale.x;
				float newScale = oldScale * 1.1f;
				mBuildingTransform.localScale = new Vector3(newScale, newScale, newScale);
			}
		}
	}
}
