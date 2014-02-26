using UnityEngine;
using System.Collections;

public class VirtualTourButtonController : MonoBehaviour
{
	#region Fields
	
		private GameObject mVirtualTourButton;
	
	#endregion
	
	#region Events

		void Start ()
		{
				Debug.Log ("Logan - Virtual Button - Start");
				mVirtualTourButton = gameObject;
				Hide ();
		}

		void Update ()
		{
				// Only update is active
				if (mVirtualTourButton.activeSelf) {
						//Debug.Log ("Logan - Virtual Button - Update and LookAt");
						mVirtualTourButton.transform.LookAt (Camera.main.transform);
				}
		}
	
	#endregion
	
	#region Public Methods
	
		public void Hide ()
		{
				//Debug.Log ("Logan - Virtual Button - Hide");
				mVirtualTourButton.SetActive (false);
		}
	
		public void Show ()
		{
				//Debug.Log ("Logan - Virtual Button - Show");
				// Set the transform before it becomes visible
				mVirtualTourButton.transform.LookAt (Camera.main.transform);
				mVirtualTourButton.SetActive (true);
		}
	
	#endregion
	
	#region Private Methods
	
	#endregion
}
