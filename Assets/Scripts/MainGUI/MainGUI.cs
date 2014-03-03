using UnityEngine;
using System.Collections;

public class MainGUI : MonoBehaviour//, ITrackableEventHandler
{
		public GUISkin thisMetalGUISkin;
		//private TrackableBehaviour mTrackableBehaviour;

		private bool ShowMenu = false;
		//private bool MenuCollapsed = false;
		private bool AnimationComplete = false;

		// margin is wall to box
		// padding is box to layout
		private int margin = 10;
		private int padding = 10;
		private int boxHeight = 100;
		private int buttonWidth = 200;
		private int buttonHeight = 80;
	
		private LTRect button1;
		private LTRect button2;

		private Rect menuGroup = new Rect(10,10, Screen.width-10,80); //MENU PANEL

		void Start ()
		{

//				mTrackableBehaviour = gameObject.GetComponent<TrackableBehaviour> ();
//				if (mTrackableBehaviour) {
//						mTrackableBehaviour.RegisterTrackableEventHandler (this);
//				}

				button1 = new LTRect (-2 * (padding + buttonWidth), (padding + buttonHeight), buttonWidth, buttonHeight);
				button2 = new LTRect (-1 * (padding + buttonWidth), (padding + buttonHeight), buttonWidth, buttonHeight);

				LeanTween.move (button1, new Vector2 (padding, (padding + buttonHeight)), 0.5f).setEase (LeanTweenType.easeOutBack);
				LeanTween.move (button2, new Vector2 ((2 * padding + buttonWidth), (padding + buttonHeight)), 0.5f).setEase (LeanTweenType.easeOutBack).setDelay (0.3f);
		}

		public void OnTrackableStateChanged (TrackableBehaviour.Status previousStatus, TrackableBehaviour.Status newStatus)
		{
//				if (newStatus == TrackableBehaviour.Status.DETECTED ||
//						newStatus == TrackableBehaviour.Status.TRACKED) {
//						OnTrackingFound ();
//				} else {
//						OnTrackingLost ();
//				}
		}
	
//		private void OnTrackingFound ()
//		{
//				ShowMenu = true;
//		}
//	
//		private void OnTrackingLost ()
//		{
//				ShowMenu = false;
//		}
	
		void OnGUI ()
		{
//				if (!ShowMenu) {
//						return;
//				}

				GUI.skin = thisMetalGUISkin;

				GUILayout.BeginArea(menuGroup,"box");
				if (GUI.Button(button1.rect, "Empire State 3D")) {
						// Start Async load of AR_App while showing loading scene
						LoadingOptions.mSceneName = "AR_App";
						Application.LoadLevel ("LoadingScene");
				}

				if (GUI.Button (button2.rect, "Vegas 3D")) {
						// more stuff
				}
				GUILayout.EndArea();
				GUI.DragWindow();
		
//				GUILayout.BeginArea (new Rect (margin + padding, margin + padding, Screen.width - (2 * (padding + margin)), boxHeight - (2 * padding)));
//					
//				GUILayout.BeginHorizontal (GUI.skin.GetStyle ("box"));
//					
//				if (GUILayout.Button ("Empire State 3D", GUILayout.Width (buttonWidth))) {
//						// Start Async load of AR_App while showing loading scene
//						LoadingOptions.mSceneName = "AR_App";
//						Application.LoadLevel ("LoadingScene");
//				}
//
//				if (GUILayout.Button ("Vegas 3D", GUILayout.Width (buttonWidth))) {
//						//Application.LoadLevel ("AR_App");
//				}
//					
//				GUILayout.EndHorizontal ();
//
//				GUILayout.EndArea ();
		}
}
