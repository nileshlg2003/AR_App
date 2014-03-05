using UnityEngine;
using System.Collections;

public class MirageTextController : MonoBehaviour
{
	#region Fields
	
		private GameObject mMirageText;
	
		private Vector3 mDefaultPosition;
		private Quaternion mDefaultRotation;
		private Vector3 mDefaultScale;
	
	#endregion
	
	#region Events
	
		void Start ()
		{
				mMirageText = gameObject;
				mDefaultPosition = mMirageText.transform.position;
				mDefaultRotation = mMirageText.transform.rotation;
				mDefaultScale = mMirageText.transform.localScale;
				//mMirageText.SetActive (false);
		}
	
		void Update ()
		{
				// Only update is active
				if (mMirageText.activeSelf) {
						mMirageText.transform.LookAt (Camera.main.transform);
				}
		}
	
	#endregion
	
	#region Public Methods
	
		public void Reset ()
		{
				mMirageText.SetActive (false);
				mMirageText.transform.position = mDefaultPosition;
				mMirageText.transform.rotation = mDefaultRotation;
				mMirageText.transform.localScale = mDefaultScale;
		}
	
		public void Show ()
		{
				// Set the transform before it becomes visible
				mMirageText.transform.LookAt (Camera.main.transform);
				mMirageText.SetActive (true);
		}
	
	#endregion
	
	#region Private Methods
	
	#endregion
}
