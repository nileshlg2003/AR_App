using UnityEngine;
using System.Collections;

public class MirageHolderRotate : MonoBehaviour
{
		private GameObject mMirageHolder;

		// Use this for initialization
		void Start ()
		{
				mMirageHolder = gameObject;
				Reset ();
		}
	
		// Update is called once per frame
		void Update ()
		{
				mMirageHolder.transform.LookAt (Camera.main.transform);
		}

		public void Show ()
		{
				mMirageHolder.SetActive (true);
		}

		public void Reset ()
		{
				mMirageHolder.SetActive (false);
		}
}
