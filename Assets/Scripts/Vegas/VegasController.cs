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
		private GameObject mVegasContainer;

		private Hashtable mFlyThroughTable = new Hashtable ();
		private static bool mFlyThroughTableInitialized = false;

		private Vector3 mDefaultVegasPosition;
		private Quaternion mDefaultVegasRotation;
		private Vector3 mDefaultVegasContainerScale;

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
				mVegasContainer = GameObject.Find ("VegasContainer");

				mTargetPlane = new Plane (mVegasImageTarget.transform.up, mVegasImageTarget.transform.position);

				mVegas.SetActive (false);
		
				// Can't set transform directly, so need to save all parts separately
				mDefaultVegasPosition = mVegas.transform.position;
				mDefaultVegasRotation = mVegas.transform.rotation;
				mDefaultVegasContainerScale = mVegasContainer.transform.localScale;
		}
	
		// Update is called once per frame
		// void Update ()
		// {
		//		
		// }

		void FlyThroughComplete ()
		{
				Debug.Log ("Logan - VegasController - FlyThroughComplete");
				mListener.OnFlyThroughComplete ();
		}
	
	#endregion
	
	#region Public Methods

		// IEnumerator
		public void StartFlyThrough (CompleteListener listener)
		{
				Debug.Log ("Logan - VegasController - StartFlyThrough");
				mListener = listener;

				mVegas.SetActive (true);
				if (!mFlyThroughTableInitialized) {
						InitialiseFlyThroughTable ();
				}

				iTween.MoveTo (mVegas, mFlyThroughTable);
				//yield return new WaitForSeconds (mFlyThroughTime);
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
						mVegasContainer.transform.position += planePoint - mLastPlanePoint;
						mLastPlanePoint = planePoint;
				}
		}

		public void Zoom (Touch touch1, Touch touch2)
		{
				Vector2 curDist = touch1.position - touch2.position;
				Vector2 prevDist = ((touch1.position - touch1.deltaPosition) - (touch2.position - touch2.deltaPosition));
				float touchDelta = curDist.magnitude - prevDist.magnitude;
				//Debug.Log ("Logan - touchDelta " + touchDelta.ToString ());

				if (touchDelta < 0) {
						float oldScale = mVegasContainer.transform.localScale.x;
						float newScale = oldScale / 1.1f;
						mVegasContainer.transform.localScale = new Vector3 (newScale, newScale, newScale);
				} else if (touchDelta > 0) {
						float oldScale = mVegasContainer.transform.localScale.x;
						float newScale = oldScale * 1.1f;
						mVegasContainer.transform.localScale = new Vector3 (newScale, newScale, newScale);
				}
		}
	
		public void Reset ()
		{
				Debug.Log ("Logan - VegasController - Reset");
				mVegas.transform.position = mDefaultVegasPosition;
				mVegas.transform.rotation = mDefaultVegasRotation;
				mVegasContainer.transform.localScale = mDefaultVegasContainerScale;
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
				//startPoint.y += mVegas.renderer.bounds.size.y / 2;
				// TODO: Get it to stop somewhere
				//mFlyThroughTable.Add ("position", startPoint);

				mFlyThroughTable.Add ("path", iTweenPath.GetPath ("FlyThroughPath"));
				mFlyThroughTable.Add ("time", mFlyThroughTime);

				// circ/quart/cubic/quint
				mFlyThroughTable.Add ("easetype", iTween.EaseType.easeOutCubic);
				mFlyThroughTable.Add ("looptype", iTween.LoopType.none);
				mFlyThroughTable.Add ("orienttopath", true);
				mFlyThroughTable.Add ("axis", "y");
				mFlyThroughTable.Add ("movetopath", false);

				mFlyThroughTable.Add ("oncomplete", "FlyThroughComplete");
				mFlyThroughTableInitialized = true;
		}
	
	#endregion
}
