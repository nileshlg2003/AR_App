using UnityEngine;
using System.Collections;


/*
 * This class should be attached to the Vegas ImageTarget. It is the event controller for
 * all that happens in the Vegas 'scene'.
 */
public class VegasSceneController : MonoBehaviour, ITrackableEventHandler, VegasController.CompleteListener
{
	#region Fields
	
		public ExclamationMarkController mExclamationMarkController;
		public VegasController mVegasController;

		private GameObject mVegasImageTarget;
		private TrackableBehaviour mTrackableBehaviour;

		private bool mFlyThroughRunning = false;
		private bool mObjectTouched = false;
		private static bool mFirstComplete = false;
	
	#endregion

	#region Events

		// Use this for initialization
		void Start ()
		{
				mVegasImageTarget = gameObject;

				mTrackableBehaviour = mVegasImageTarget.GetComponent<TrackableBehaviour> ();
				if (mTrackableBehaviour) {
						mTrackableBehaviour.RegisterTrackableEventHandler (this);
				}
		}
	
		// Update is called once per frame
		void Update ()
		{
				if (Input.touchCount == 1) {
						Touch touch = Input.GetTouch (0);			

						Ray ray = Camera.main.ScreenPointToRay (touch.position);				
						RaycastHit hit;
						
						if (Physics.Raycast (ray, out hit)) {
								string gameObjectName = hit.collider.gameObject.name;
			
								switch (gameObjectName) {
								case "ExclamationMark":
										if (touch.phase == TouchPhase.Ended) {
						
												mExclamationMarkController.Hide ();
												mVegasController.StartFlyThrough (this);
												mFlyThroughRunning = true;
										}
										break;
								case "Vegas":
										if (!mFlyThroughRunning) {
												mVegasController.Drag (touch, ray);
										}
										break;
								}
						}
				} else if (Input.touchCount == 2) {
						// Get both touches
						Touch touch1 = Input.GetTouch (0);
						Touch touch2 = Input.GetTouch (1);

						// Check if ray1 hit and get its object
						Ray ray1 = Camera.main.ScreenPointToRay (touch1.position);
						RaycastHit hit1;
						bool ray1hit = false;
						string gameObjectName1 = null;
						if (ray1hit = Physics.Raycast (ray1, out hit1)) {
								gameObjectName1 = hit1.collider.gameObject.name;
						}

						// Check if ray2 hit and get its object
						Ray ray2 = Camera.main.ScreenPointToRay (touch2.position);
						RaycastHit hit2;
						bool ray2hit = false;
						string gameObjectName2 = null;
						if (ray2hit = Physics.Raycast (ray2, out hit2)) {
								gameObjectName2 = hit2.collider.gameObject.name;
						}

						// If either hit and their object was vegas, do the zoom.
						if ((ray1hit && gameObjectName1 == "Vegas") ||
								(ray2hit && gameObjectName2 == "Vegas")) {
								mVegasController.Zoom (touch1, touch2);
						}
				}
		}

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

		public void OnFlyThroughComplete ()
		{
				mFlyThroughRunning = false;
		}
	
	#endregion
	
				#region Public Methods
		
				#endregion
		
	#region Private Methods
		
		private void OnTrackingFound ()
		{
				mExclamationMarkController.StartBounce ();
		}
	
		private void OnTrackingLost ()
		{
				mExclamationMarkController.Reset ();
				mVegasController.Reset ();
		}

	#endregion
}
