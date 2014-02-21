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
		private bool mFirstComplete = false;
	
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
						Touch touch1 = Input.GetTouch (0);
						Touch touch2 = Input.GetTouch (1);

						Ray ray = Camera.main.ScreenPointToRay (touch1.position);				
						RaycastHit hit1;

						// Something weird happening here where it is calling on both for a short while.
						// Need MOAR DEBUG
						if (mObjectTouched = Physics.Raycast (ray, out hit1)) {
								mFirstComplete = true;
								string gameObjectName = hit1.collider.gameObject.name;
				
								switch (gameObjectName) {
								case "Vegas":
										if (!mFlyThroughRunning) {
												mVegasController.Zoom (touch1, touch2);
										}
										break;
								}
						} else {
								mFirstComplete = true;
						}

						if (!mObjectTouched && mFirstComplete) {
								mFirstComplete = false;
								ray = Camera.main.ScreenPointToRay (touch2.position);				
								RaycastHit hit2;
							
								if (Physics.Raycast (ray, out hit2)) {
										string gameObjectName = hit2.collider.gameObject.name;
								
										switch (gameObjectName) {
										case "Vegas":
												if (!mFlyThroughRunning) {
														mVegasController.Zoom (touch1, touch2);
												}
												break;
										}
								}
						} else {
								mFirstComplete = false;
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
