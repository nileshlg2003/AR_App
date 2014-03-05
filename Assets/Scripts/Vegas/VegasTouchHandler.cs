using UnityEngine;
using System.Collections;

public class VegasTouchHandler : MonoBehaviour, ITrackableEventHandler
{
	#region Fields

		// Globals
		private GameObject _imageTarget = null;
		private GameObject _vegasObject = null;
		private Plane _targetPlane;
		private Vector3 _defaultVegasPosition;
		private Quaternion _defaultVegasRotation;
		private Vector3 _defaultVegasScale;
		private TrackableBehaviour _mTrackableBehaviour;
		private bool _isVegasAtOrigin = true;
		private bool _isVegasAtDefaultScale = true;

		// Dragging variables
		private Transform _pickedObject = null;
		private Vector3 _lastPlanePoint;

		// Zooming variables
		private float touchDelta = 0.0F;
		private Vector2 prevDist = new Vector2 (0, 0);
		private Vector2 curDist = new Vector2 (0, 0);
		//private float curPos = 0.0F;
		//private float prevPos = 0.0F;

	#endregion

	#region MonoBehaviour implementation

		// Use this for initialization
		void Start ()
		{			
				_imageTarget = GameObject.Find("VegasStripTarget");
				if (_imageTarget != null) {
						_targetPlane = new Plane (_imageTarget.transform.up, _imageTarget.transform.position);
						_mTrackableBehaviour = _imageTarget.GetComponent<TrackableBehaviour> ();
			
						if (_mTrackableBehaviour) {
								Debug.Log ("mTrackableBehaviour eventHandler registered...");
								_mTrackableBehaviour.RegisterTrackableEventHandler (this);
						}
				}

				_vegasObject = GameObject.Find("VegasContainer");
				if (_vegasObject != null) {
						_defaultVegasPosition = _vegasObject.transform.position;
						_defaultVegasRotation = _vegasObject.transform.rotation;
						_defaultVegasScale = _vegasObject.transform.localScale;
				}
				
		}
	
		// Update is called once per frame
		void Update ()
		{
            if (this.gameObject.activeSelf)
            {
                // Check that the image target has been set, the model exists
                if (_imageTarget != null && _vegasObject != null)
                {
                    if (Input.touchCount == 1)
                    {
                        // We are moving the object on the plane
                        DragObject(Input.GetTouch(0));
                        _isVegasAtOrigin = false;
                    } else if (Input.touchCount == 2 &&
                        (Input.GetTouch(0).phase == TouchPhase.Moved || Input.GetTouch(1).phase == TouchPhase.Moved))
                    {
                        // We are pinch zooming
                        PinchZoomObject(Input.GetTouch(0), Input.GetTouch(1));
                        _isVegasAtDefaultScale = false;
                    }
                }
            }
		}

	#endregion
	
	#region ITrackableEventHandler implementation
	
		public void OnTrackableStateChanged (TrackableBehaviour.Status previousStatus, TrackableBehaviour.Status newStatus)
		{
				Debug.Log ("OnTrackableStateChanged = " + newStatus.ToString ());
				if (newStatus == TrackableBehaviour.Status.DETECTED ||
						newStatus == TrackableBehaviour.Status.TRACKED) {
						OnTrackingFound ();
				} else {
						OnTrackingLost ();
				}
		}
	
	#endregion

	#region Private Methods

		private void PinchZoomObject (Touch touch1, Touch touch2)
		{
				//current distance between finger touches
				curDist = touch1.position - touch2.position;
				Debug.Log ("touch1 = " + touch1.position);
				Debug.Log ("touch2 = " + touch2.position);
				Debug.Log ("curDist = " + curDist.ToString ());
		
				//difference in previous locations using delta positions
				prevDist = ((touch1.position - touch1.deltaPosition) - (touch2.position - touch2.deltaPosition)); 
		
				touchDelta = curDist.magnitude - prevDist.magnitude;
		
//				if ((Input.GetTouch (0).position.x - Input.GetTouch (1).position.x) > (Input.GetTouch (0).position.y - Input.GetTouch (1).position.y)) {
//						vertOrHorzOrientation = -1; 
//				}
//				if ((Input.GetTouch (0).position.x - Input.GetTouch (1).position.x) < (Input.GetTouch (0).position.y - Input.GetTouch (1).position.y)) {
//						vertOrHorzOrientation = 1;
//				}
		
				if (touchDelta < 0) {
						float oldScale = _vegasObject.transform.localScale.x;
						float newScale = oldScale / 1.1f;
						_vegasObject.transform.localScale = new Vector3 (newScale, newScale, newScale);
				} else {
						float oldScale = _vegasObject.transform.localScale.x;
						float newScale = oldScale * 1.1f;
						_vegasObject.transform.localScale = new Vector3 (newScale, newScale, newScale);
				}
		}

		private void DragObject (Touch touch)
		{
				//Gets the ray at position where the screen is touched
				Ray ray = Camera.main.ScreenPointToRay (touch.position);
		
				// Debug.DrawRay (ray.origin, ray.direction * 10, Color.yellow);
		
				//Gets the position of ray along plane
				float dist = 0.0f;
		
				//Intersects ray with the plane. Sets dist to distance along the ray where intersects
				_targetPlane.Raycast (ray, out dist);
		
				//Returns point dist along the ray.
				Vector3 planePoint = ray.GetPoint (dist);
		
				//Debug.Log("Point=" + planePoint);
				//If ray intersects collider, set pickedObject to transform of collider object
				if (touch.phase == TouchPhase.Began) {
						// If finger touch began
			
						//Struct used to get info back from a raycast
						RaycastHit hit = new RaycastHit ();
			
						if (Physics.Raycast (ray, out hit)) {
								// If ray intersects with collider, set pickedObject to transform of collider
								_pickedObject = hit.transform;
								_lastPlanePoint = planePoint;
						} else {
								// Else, clear pickedObject
								_pickedObject = null;
						}
			
						//Move Object when finger moves after object selected.
				} else if (touch.phase == TouchPhase.Moved) {
						// Else, we are moving
						if (_pickedObject != null) {
								// If there is a pickedObject, move it along the plane
								_pickedObject.position += planePoint - _lastPlanePoint;
								_lastPlanePoint = planePoint;
						}
				} else if (touch.phase == TouchPhase.Ended) {
						// Else, we have ended, therefore clear pickedObject
						_pickedObject = null;
				}
		}

		private void OnTrackingFound ()
		{
				// do something
		}

		private void OnTrackingLost ()
		{
				if (_vegasObject != null && !_isVegasAtDefaultScale) {
						_vegasObject.transform.localScale = _defaultVegasScale;
				}

				// Reset to original position and rotation when tracking lost
				if (_vegasObject != null && !_isVegasAtOrigin) {
						_vegasObject.transform.position = _defaultVegasPosition;
						_vegasObject.transform.rotation = _defaultVegasRotation;
						_isVegasAtOrigin = true;
				}
		}

	#endregion
}