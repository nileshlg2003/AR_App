using UnityEngine;
using System.Collections;

public class ExclamationMarkController : MonoBehaviour
{
	#region Fields

		private GameObject mExclamationMark;
		private Hashtable mBounceTable = new Hashtable ();
		private Vector3 mDefaultPosition;
		private Quaternion mDefaultRotation;
		private Vector3 mDefaultScale;
	
	#endregion
	
	#region Events
	
		// Use this for initialization
		void Start ()
		{
				mExclamationMark = gameObject;

				// Can't set transform directly, so need to save all parts separately
				mDefaultPosition = mExclamationMark.transform.position;
				mDefaultRotation = mExclamationMark.transform.rotation;
				mDefaultScale = mExclamationMark.transform.localScale;

				InitialiseBounceTable ();
		}
	
	#endregion
	
	#region Public Methods

		public void StartBounce ()
		{
				iTween.MoveTo (mExclamationMark, mBounceTable);
		}

		public void Hide ()
		{
				mExclamationMark.SetActive (false);
		}

		public void Reset ()
		{
				mExclamationMark.transform.position = mDefaultPosition;
				mExclamationMark.transform.rotation = mDefaultRotation;
				mExclamationMark.transform.localScale = mDefaultScale;
				mExclamationMark.SetActive (true);
				iTween.Stop (mExclamationMark);
		}

	#endregion
	
	#region Private Methods

		private void InitialiseBounceTable ()
		{
				Vector3 point = mDefaultPosition;
				point.y += 300;
				mBounceTable.Add ("position", point);
				mBounceTable.Add ("time", 1);
				mBounceTable.Add ("looptype", iTween.LoopType.pingPong);
				mBounceTable.Add ("easetype", iTween.EaseType.easeInOutQuad);
		}

	#endregion
}
