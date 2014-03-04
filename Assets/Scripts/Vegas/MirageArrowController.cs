using UnityEngine;
using System.Collections;

public class MirageArrowController : MonoBehaviour
{
	#region Fields
	
		private GameObject mMirageArrow;
	
		private Vector3 mDefaultPosition;
		private Quaternion mDefaultRotation;
		private Vector3 mDefaultScale;
	
	#endregion
	
	#region Events
	
		void Start ()
		{
				mMirageArrow = gameObject;
				mDefaultPosition = mMirageArrow.transform.position;
				mDefaultRotation = mMirageArrow.transform.rotation;
				mDefaultScale = mMirageArrow.transform.localScale;
				mMirageArrow.SetActive (false);
		}
	
		void Update ()
		{
				// Only update is active
//				if (mMirageArrow.activeSelf) {
//						mMirageArrow.transform.LookAt (Camera.main.transform);
//				}
		}
	
	#endregion
	
	#region Public Methods
	
		public void Reset ()
		{
				mMirageArrow.SetActive (false);
				mMirageArrow.transform.position = mDefaultPosition;
				mMirageArrow.transform.rotation = mDefaultRotation;
				mMirageArrow.transform.localScale = mDefaultScale;
		}
	
		public void Show ()
		{
				// Set the transform before it becomes visible
				mMirageArrow.transform.LookAt (Camera.main.transform);
				mMirageArrow.SetActive (true);
		}
	
	#endregion
	
	#region Private Methods
	
	#endregion
}
