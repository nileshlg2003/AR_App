using UnityEngine;
using System.Collections;

public class Loading : MonoBehaviour
{
		public Texture mTexture;
		//private AsyncOperation mAsyncOperation;
		//private float mProgress;
		private bool mGUILoaded = false;

		// Use this for initialization
		void Start ()
		{
				
				//mAsyncOperation = Application.LoadLevelAsync (LoadingOptions.mSceneName);
		}
	
		// Update is called once per frame
		void Update ()
		{
				if (mGUILoaded) {
						Application.LoadLevel (LoadingOptions.mSceneName);
				}
				//mProgress = mAsyncOperation.progress;
				// Do something here
		}

		void OnGUI ()
		{
				GUI.DrawTexture (new Rect (0, 0, Screen.width, Screen.height), mTexture, ScaleMode.ScaleToFit, true, 2.0f);
				mGUILoaded = true;
		}
}
