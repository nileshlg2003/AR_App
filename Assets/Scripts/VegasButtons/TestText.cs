using UnityEngine;
using System.Collections;

public class TestText : MonoBehaviour
{
		private GameObject mTestText;
	
		// Use this for initialization
		void Start ()
		{
				mTestText = gameObject;
				Reset ();
		}
	
		// Update is called once per frame
		void Update ()
		{
				mTestText.transform.LookAt (Camera.main.transform);
		}

		public void Show ()
		{
				mTestText.SetActive (true);
		}

		public void Reset ()
		{
				mTestText.SetActive (false);
		}
}
