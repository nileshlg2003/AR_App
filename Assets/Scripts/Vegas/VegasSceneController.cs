using UnityEngine;
using System.Collections;


/*
 * This class should be attached to the Vegas ImageTarget. It is the event controller for
 * all that happens in the Vegas 'scene'.
 */
public class VegasSceneController : MonoBehaviour, ITrackableEventHandler, VegasController.CompleteListener
{
	#region Fields

		public VirtualTourButtonController mVirtualTourButtonController;
		public VegasController mVegasController;
		public MirageTextController mMirageTextController;
		public MirageArrowController mMirageArrowController;
		public LuxorTextController mLuxorTextController;
		public LuxorArrowController mLuxorArrowController;
		

		//public ExclamationMarkController mExclamationMarkController;
		//public MirageArrowBounce mMirageArrowBounce;
		//public LuxorArrowBounce mLuxorArrowBounce;
		//public VenetianArrowBounce mVenetianArrowBounce;
		//public MirageHolderRotate mMirageHolderRotate;
		//public TestText mTestText;

		private GameObject mVegasImageTarget;
		private TrackableBehaviour mTrackableBehaviour;

		private bool mFlyThroughRunning = false;
	
	#endregion

	#region Events

		// Use this for initialization
		void Start ()
		{
				Debug.Log ("Logan - Scene Controller - Start");
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
								case "VirtualTourButton":
										if (touch.phase == TouchPhase.Ended) {
												Debug.Log ("Logan - Scene Controller - Update and Touch VirtualTourButton");
												mVirtualTourButtonController.Hide ();
												mFlyThroughRunning = true;
												mVegasController.StartFlyThrough (this);
										}
										break;
								case "VegasModel":
										if (!mFlyThroughRunning) {
												Debug.Log ("Logan - Scene Controller - Update and Drag Vegas");
												mVegasController.Drag (touch, ray);
										}
										break;
//								case "TestHolder":
//										if (!mFlyThroughRunning) {
//												Application.OpenURL ("http://www.mirage.com/");
//										}
//										break;
								}
						}
				} else if (Input.touchCount == 2) {
						// Get both touches
						Touch touch1 = Input.GetTouch (0);
						Touch touch2 = Input.GetTouch (1);

						// Check if ray1 hit and get its object name
						RaycastHit hit1;
						string gameObjectName1 = null;
						Ray ray1 = Camera.main.ScreenPointToRay (touch1.position);
						if (Physics.Raycast (ray1, out hit1)) {
								gameObjectName1 = hit1.collider.gameObject.name;
						}

						// Check if ray2 hit and get its object name
						RaycastHit hit2;
						string gameObjectName2 = null;
						Ray ray2 = Camera.main.ScreenPointToRay (touch2.position);
						if (Physics.Raycast (ray2, out hit2)) {
								gameObjectName2 = hit2.collider.gameObject.name;
						}

						// If either hit and their object was vegas, do the zoom.
						if (gameObjectName1 == "VegasModel" || gameObjectName2 == "VegasModel") {
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
				mMirageTextController.Show ();
				mMirageArrowController.Show ();
				mLuxorTextController.Show ();
				mLuxorArrowController.Show ();

				//mVenetianArrowBounce.StartBounce ();
				//Invoke ("mLuxorArrowBounce.StartBounce", 0.2f);
				//Invoke ("mMirageArrowBounce.StartBounce", 0.5f);
				
				//mMirageHolderRotate.Show ();
				//mTestText.Show ();
		}
	
	#endregion
	
				#region Public Methods
		
				#endregion
		
	#region Private Methods
		
		private void OnTrackingFound ()
		{
				Debug.Log ("Logan - Scene Controller - OnTrackingFound");
				mVirtualTourButtonController.Show ();
				//mExclamationMarkController.StartBounce ();
		}
	
		private void OnTrackingLost ()
		{
				Debug.Log ("Logan - Scene Controller - OnTrackingLost");
				mVirtualTourButtonController.Hide ();

				// Reset vegas controller first otherwise the others reset weirdly
				mVegasController.Reset ();

				mMirageTextController.Reset ();
				mMirageArrowController.Reset ();
				mLuxorTextController.Reset ();
				mLuxorArrowController.Reset ();


				//mExclamationMarkController.Reset ();
				//mMirageArrowBounce.Reset ();
				//mVenetianArrowBounce.Reset ();
				//mLuxorArrowBounce.Reset ();
				//mTestText.Reset ();
				//mMirageHolderRotate.Reset ();
		}

	#endregion
}
