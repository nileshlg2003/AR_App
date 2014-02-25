using UnityEngine;
using System.Collections;

public class VegasController : MonoBehaviour
{
		public interface CompleteListener
		{
				void OnFlyThroughComplete ();
		}


	#region Fields

		public GameObject mVegasImageTarget;
		public CompleteListener mListener;

		private GameObject mVegas;

		private Hashtable mFlyThroughTable = new Hashtable ();

		private Vector3 mDefaultPosition;
		private Quaternion mDefaultRotation;
		private Vector3 mDefaultScale;

		private int mFlyThroughTime = 10;
	
		// Dragging variables
		private Vector3 mLastPlanePoint;
		private Plane mTargetPlane;
	
	#endregion
	
	#region Events
	
		// Use this for initialization
		void Start ()
		{
				mVegas = gameObject;

				mTargetPlane = new Plane (mVegasImageTarget.transform.up, mVegasImageTarget.transform.position);

				mVegas.SetActive (false);
		
				// Can't set transform directly, so need to save all parts separately
				mDefaultPosition = mVegas.transform.position;
				mDefaultRotation = mVegas.transform.rotation;
				mDefaultScale = mVegas.transform.localScale;

				InitialiseFlyThroughTable ();
		}
	
		// Update is called once per frame
		// void Update ()
		// {
		//		
		// }

		void FlyThroughComplete ()
		{
				mListener.OnFlyThroughComplete ();
		}
	
	#endregion
	
	#region Public Methods
	
		public IEnumerable StartFlyThrough (CompleteListener listener)
		{
				mListener = listener;

				mVegas.SetActive (true);
				iTween.MoveTo (mVegas, mFlyThroughTable);
				yield return mFlyThroughTime;
		}

		public void Drag (Touch touch, Ray ray)
		{
				//Gets the position of ray along plane
				float dist = 0.0f;
		
				//Intersects ray with the plane. Sets dist to distance along the ray where intersects
				mTargetPlane.Raycast (ray, out dist);
		
				//Returns point dist along the ray.
				Vector3 planePoint = ray.GetPoint (dist);

				if (touch.phase == TouchPhase.Began) {
						// If finger touch began
						mLastPlanePoint = planePoint;
			
						//Move Object when finger moves after object selected.
				} else if (touch.phase == TouchPhase.Moved) {
						// Else, we are moving
						mVegas.transform.position += planePoint - mLastPlanePoint;
						mLastPlanePoint = planePoint;
				}
		}

		public void Zoom (Touch touch1, Touch touch2)
		{
				Vector2 curDist = touch1.position - touch2.position;
				Vector2 prevDist = ((touch1.position - touch1.deltaPosition) - (touch2.position - touch2.deltaPosition));
				float touchDelta = curDist.magnitude - prevDist.magnitude;
				Debug.Log ("Logan - touchDelta " + touchDelta.ToString ());

				if (touchDelta < 0) {
						float oldScale = mVegas.transform.localScale.x;
						float newScale = oldScale / 1.1f;
						mVegas.transform.localScale = new Vector3 (newScale, newScale, newScale);
				} else if (touchDelta > 0) {
						float oldScale = mVegas.transform.localScale.x;
						float newScale = oldScale * 1.1f;
						mVegas.transform.localScale = new Vector3 (newScale, newScale, newScale);
				}
		}
	
		public void Reset ()
		{
				mVegas.transform.position = mDefaultPosition;
				mVegas.transform.rotation = mDefaultRotation;
				mVegas.transform.localScale = mDefaultScale;
				mVegas.SetActive (false);
				iTween.Stop (mVegas);
		}
	
	#endregion
	
	#region Private Methods
	
		private void InitialiseFlyThroughTable ()
		{
				// Get half the z value of Vegas to stop at the beginning of the model
				Vector3 startPoint = mVegasImageTarget.transform.position;
				//Debug.Log ("Logan - " + ((mVegas.renderer.bounds.size.z / 2) / mVegas.transform.localScale.z).ToString ());
				//startPoint.y += (mVegas.renderer.bounds.size.y / 2) / mVegas.transform.localScale.y;
				startPoint.y += mVegas.renderer.bounds.size.y / 2;
				mFlyThroughTable.Add ("position", startPoint);
				mFlyThroughTable.Add ("time", mFlyThroughTime);
				mFlyThroughTable.Add ("easetype", iTween.EaseType.easeOutCirc);
				mFlyThroughTable.Add ("oncomplete", "FlyThroughComplete");
		}
	
	#endregion
}
