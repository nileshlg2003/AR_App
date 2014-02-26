using UnityEngine;
using System.Collections;

public class VegasButtonsSceneController : MonoBehaviour, ITrackableEventHandler
{
	
		private GameObject mVegasImageTarget;
		private TrackableBehaviour mTrackableBehaviour;

		// Use this for initialization
		void Start ()
		{
	
		}
	
		// Update is called once per frame
		void Update ()
		{
	
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



		private void OnTrackingFound ()
		{

		}

		private void OnTrackingLost ()
		{
		
		}
}
