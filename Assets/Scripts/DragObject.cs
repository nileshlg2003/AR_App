using UnityEngine;
using System.Collections;

public class DragObject : MonoBehaviour
{
	public GameObject imageTarget = null;
	public GUIText message = null;
	private Transform pickedObject = null;
	private Vector3 lastPlanePoint;

	// Use this for initialization
	void Start ()
	{
	}

	// Update is called once per frame
	void Update ()
	{
		Debug.Log("In update function");

		Plane targetPlane = new Plane(imageTarget.transform.up, imageTarget.transform.position);

		//message.text = "";
		foreach (Touch touch in Input.touches)
		{
			Debug.Log("Inside touch: " + touch.ToString());
			//Gets the ray at position where the screen is touched
			Ray ray = Camera.main.ScreenPointToRay(touch.position);

			// Debug.DrawRay (ray.origin, ray.direction * 10, Color.yellow);

			//Gets the position of ray along plane
			float dist = 0.0f;

			//Intersects ray with the plane. Sets dist to distance along the ray where intersects
			targetPlane.Raycast(ray, out dist);

			//Returns point dist along the ray.
			Vector3 planePoint = ray.GetPoint(dist);

			//Debug.Log("Point=" + planePoint);
			 //If ray intersects collider, set pickedObject to transform of collider object
			if (touch.phase == TouchPhase.Began)
			{
				// If finger touch began

				//Struct used to get info back from a raycast
				RaycastHit hit = new RaycastHit();

				if (Physics.Raycast(ray, out hit))
				{
					// If ray intersects with collider, set pickedObject to transform of collider
					pickedObject = hit.transform;
					lastPlanePoint = planePoint;
				}
				else
				{
					// Else, clear pickedObject
					pickedObject = null;
				}

				//Move Object when finger moves after object selected.
			}
			else if (touch.phase == TouchPhase.Moved)
			{
				// Else, we are moving
				if (pickedObject != null)
				{
					// If there is a pickedObject, move it along the plane
					pickedObject.position += planePoint - lastPlanePoint;
					lastPlanePoint = planePoint;
				}
			}
			else if (touch.phase == TouchPhase.Ended)
			{
				// Else, we have ended, therefore clear pickedObject
				pickedObject = null;
			}
		}
	}
}