using UnityEngine;
using System.Collections;

public class EmpireStateBuilding_Demo: MonoBehaviour {
	
	private float touchDelta = 0.0F;
	private float curPos = 0.0F;
	private float prevPos = 0.0F;	
	private Vector3 defaultScale;
	private Vector2 prevDist = new Vector2 (0, 0);
	private Vector2 curDist = new Vector2 (0, 0);
	private Quaternion defaultRotation;
	
	void Start () {
	}
	
	void Update () {
		if (Input.touchCount == 1 &&
		    Input.GetTouch (0).phase == TouchPhase.Moved) {
			
			Ray ray = Camera.main.ScreenPointToRay (Input.GetTouch(0).position);				
			RaycastHit hit;
			
			if (Physics.Raycast (ray, out hit)) {
				string gameObjectName = hit.collider.gameObject.name;
				
				if(gameObjectName.Equals("EmpireStateBuilding"))
				{
					this.RotateGameObject(hit.collider.gameObject.transform);
				}
			}
		} else if (Input.touchCount == 2 && 
		           Input.GetTouch(0).phase == TouchPhase.Moved && 
		           Input.GetTouch(1).phase == TouchPhase.Moved) {
			
			Touch touch1 = Input.GetTouch(0);
			Touch touch2 = Input.GetTouch(1);
			
			RaycastHit hit1;
			string gameObjectName1 = null;
			Ray ray1 = Camera.main.ScreenPointToRay (touch1.position);
			
			RaycastHit hit2;
			string gameObjectName2 = null;
			Ray ray2 = Camera.main.ScreenPointToRay (touch2.position);
			
			if (Physics.Raycast (ray1, out hit1)) {
				gameObjectName1 = hit1.collider.gameObject.name;
			}
			
			if (Physics.Raycast (ray2, out hit2)) {
				gameObjectName2 = hit2.collider.gameObject.name;
			}
			
			// Check which gameObject was hit and apply scaling
			if (gameObjectName1.Equals("EmpireStateBuilding") || 
			    gameObjectName2.Equals("EmpireStateBuilding")) {
				this.ScaleGameObject(this.gameObject.transform);
			}
		}
	}
	
	private void RotateGameObject(Transform buildingTransform) 
	{
		//current distance between finger touches
		curPos = Input.GetTouch (0).position.x; 
		prevPos = Input.GetTouch (0).deltaPosition.x; 
		
		touchDelta = curPos - prevPos;
		
		//this code is for x-axis rotation
		if (Input.GetTouch (0).deltaPosition.x < 0 || 
		    Input.GetTouch (0).deltaPosition.x > 0) {
			float tiltAroundY = Input.GetAxis ("Horizontal") * 15.0F; // 15 = angle of rotation
			var target1 = Quaternion.Euler (tiltAroundY, 0.0F, 0.0F);
			buildingTransform.rotation = Quaternion.Slerp (buildingTransform.rotation, target1, Time.deltaTime);
			buildingTransform.Rotate (0.0F, Input.GetTouch (0).deltaPosition.x * 1.0F, 0.0F, Space.World);
		}
	}
	
	private void ScaleGameObject(Transform buildingTransform)
	{
		curDist = Input.GetTouch (0).position - Input.GetTouch (1).position; 
		prevDist = ((Input.GetTouch (0).position - Input.GetTouch (0).deltaPosition) - (Input.GetTouch (1).position - Input.GetTouch (1).deltaPosition)); 
		touchDelta = curDist.magnitude - prevDist.magnitude;
		
		if (touchDelta < 0) {
			float oldScale = buildingTransform.localScale.x;
			float newScale = oldScale / 1.1f;
			if(newScale > 0.009) {
				buildingTransform.localScale = new Vector3 (newScale, newScale, newScale);
			}
		} else {
			float oldScale = buildingTransform.localScale.x;
			float newScale = oldScale * 1.1f;
			if(newScale < 0.02) {
				buildingTransform.localScale = new Vector3 (newScale, newScale, newScale);
			}
		}
	}
}