using UnityEngine;
using System.Collections;

public class LuxorTextController : MonoBehaviour
{
	#region Fields
	
		private GameObject mLuxorText;
	
		private Vector3 mDefaultPosition;
		private Quaternion mDefaultRotation;
		private Vector3 mDefaultScale;
	
	#endregion
	
	#region Events
	
		void Start ()
		{
				mLuxorText = gameObject;
				mDefaultPosition = mLuxorText.transform.position;
				mDefaultRotation = mLuxorText.transform.rotation;
				mDefaultScale = mLuxorText.transform.localScale;
				//mLuxorText.SetActive (false);
		}
	
		void Update ()
		{
				// Only update is active
				if (mLuxorText.activeSelf) {
						mLuxorText.transform.LookAt (Camera.main.transform);
				}
		}
	
	#endregion
	
	#region Public Methods
	
		public void Reset ()
		{
				mLuxorText.SetActive (false);
				mLuxorText.transform.position = mDefaultPosition;
				mLuxorText.transform.rotation = mDefaultRotation;
				mLuxorText.transform.localScale = mDefaultScale;
		}
	
		public void Show ()
		{
				// Set the transform before it becomes visible
				mLuxorText.transform.LookAt (Camera.main.transform);
				mLuxorText.SetActive (true);
		}
	
	#endregion
	
	#region Private Methods
	
	#endregion
}
