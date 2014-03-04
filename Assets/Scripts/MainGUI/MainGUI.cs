using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

public class MainGUI : MonoBehaviour
{
		public GUISkin thisMetalGUISkin;

		private bool ShowBottomBar = false;

		private float ButtonWidth = 150.0f;
		private float ButtonHeight = 80.0f;
		private float Spacing = 5.0f;

		private bool modelPicked = false;
		private bool showList = false;
		// private bool functionPicked = false;

		private GUIStyle listStyle;
		private GUIContent[] modelTypeList;
		private int selectedModelType = 0;

		void Start ()
		{
				modelTypeList = GUIState.ModelTypes.Select (mt => new GUIContent (mt.Value)).ToArray ();

				listStyle = new GUIStyle ();
				listStyle.normal.textColor = Color.white;
				var tex = new Texture2D (2, 2);
				var colors = new Color[4];
				for (int i = 0; i < 4; i++) {
						colors [i] = Color.white;
				}

				tex.SetPixels (colors);
				tex.Apply ();
				listStyle.hover.background = tex;
				listStyle.onHover.background = tex;
				listStyle.padding.left = listStyle.padding.right = listStyle.padding.top = listStyle.padding.bottom = 4;

				if (GUIState.mSelectedModelType == GUIState.ModelType.None) {
						// Hide the bottom bar
						ShowBottomBar = false;
				} else if (GUIState.mSelectedModelType == GUIState.ModelType.NewYorkSkyline || GUIState.mSelectedModelType == GUIState.ModelType.Vegas) {
						// Show New York Funcs
						ShowBottomBar = true;
				}
		}
	
		void OnGUI ()
		{
				ButtonsVerion ();
				DropDownVersion ();
		}

		private void DropDownVersion ()
		{
				if (Popup.List (new Rect (50, 100, 100, 20), ref showList, ref selectedModelType, new GUIContent (modelTypeList [selectedModelType]), modelTypeList, listStyle)) {
						modelPicked = true;
				}
//				if (modelPicked) {
//						GUI.Label (Rect (50, 70, 400, 20), "You picked " + list [listEntry].text + "!");
//				}
		}

		private void ButtonsVerion ()
		{
				GUI.skin = thisMetalGUISkin;
		
				GUILayout.BeginArea (new Rect (10, 10, 2 * ButtonWidth + 2 * GUI.skin.box.padding.left + GUI.skin.box.padding.right, ButtonHeight));
		
				GUILayout.BeginHorizontal (GUI.skin.GetStyle ("box"));
		
				if (GUILayout.Button ("Empire State 3D", GUILayout.Width (ButtonWidth))) {
						// Start Async load of AR_App while showing loading scene
						//						LoadingOptions.mSceneName = "AR_App";
						//						Application.LoadLevel ("LoadingScene");
						ShowBottomBar = true;
						GUIState.mSelectedModelType = GUIState.ModelType.NewYorkSkyline;
				}
		
				if (GUILayout.Button ("Vegas 3D", GUILayout.Width (ButtonWidth))) {
						//Application.LoadLevel ("AR_App");
						ShowBottomBar = true;
						GUIState.mSelectedModelType = GUIState.ModelType.Vegas;
				}
		
				GUILayout.EndHorizontal ();
		
				GUILayout.EndArea ();
		
				if (ShowBottomBar) {
						if (GUIState.mSelectedModelType == GUIState.ModelType.NewYorkSkyline) {
				
								// These values should all be stored and worked with as variables, time constraints etc...
								GUILayout.BeginArea (new Rect (10, 25 + GUI.skin.button.fixedHeight, 2 * ButtonWidth + 2 * GUI.skin.box.padding.left + GUI.skin.box.padding.right, ButtonHeight));
				
								GUILayout.BeginHorizontal (GUI.skin.GetStyle ("box"));
				
								if (GUILayout.Button ("Empire Function 1", GUILayout.Width (ButtonWidth))) {
								}
				
								if (GUILayout.Button ("Empire Function 2", GUILayout.Width (ButtonWidth))) {
								}
				
								GUILayout.EndHorizontal ();
				
								GUILayout.EndArea ();
						} else if (GUIState.mSelectedModelType == GUIState.ModelType.Vegas) {
				
								GUILayout.BeginArea (new Rect (10, 25 + GUI.skin.button.fixedHeight, 2 * ButtonWidth + 2 * GUI.skin.box.padding.left + GUI.skin.box.padding.right, ButtonHeight));
				
								GUILayout.BeginHorizontal (GUI.skin.GetStyle ("box"));
				
								if (GUILayout.Button ("Vegas Function 1", GUILayout.Width (ButtonWidth))) {
								}
				
								if (GUILayout.Button ("Vegas Function 2", GUILayout.Width (ButtonWidth))) {
								}
				
								GUILayout.EndHorizontal ();
				
								GUILayout.EndArea ();
						}
				}
		}
}
