using UnityEngine;
using System.Collections;

public class MirageArrowBounce : MonoBehaviour
{
	#region Fields
	
		private GameObject mArrow;
		private Hashtable mBounceTable = new Hashtable ();
		private Vector3 mDefaultPosition;
		private Quaternion mDefaultRotation;
		private Vector3 mDefaultScale;
	
	#endregion
	
	#region Events
	
		// Use this for initialization
		void Start ()
		{
				mArrow = gameObject;
		
				// Can't set transform directly, so need to save all parts separately
				mDefaultPosition = mArrow.transform.position;
				mDefaultRotation = mArrow.transform.rotation;
				mDefaultScale = mArrow.transform.localScale;
		
				InitialiseBounceTable ();
				Reset ();

		}
	
	#endregion
	
	#region Public Methods
	
		public void StartBounce ()
		{
				mArrow.SetActive (true);
				iTween.MoveTo (mArrow, mBounceTable);
		}
	
		public void Hide ()
		{
				mArrow.SetActive (false);
		}
	
		public void Reset ()
		{
//				mArrow.transform.position = mDefaultPosition;
//				mArrow.transform.rotation = mDefaultRotation;
//				mArrow.transform.localScale = mDefaultScale;
				mArrow.SetActive (false);
				iTween.Stop (mArrow);
		}
	
	#endregion
	
	#region Private Methods

		// IEnumerator
		private void InitialiseBounceTable ()
		{
				//yield return new WaitForSeconds (0.3f);
				Vector3 point = mDefaultPosition;
				point.y += 75;
				mBounceTable.Add ("position", point);
				mBounceTable.Add ("time", 0.75);
				mBounceTable.Add ("looptype", iTween.LoopType.pingPong);
				mBounceTable.Add ("easetype", iTween.EaseType.easeInOutQuad);
		}

#endregion
}
