using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using System;

public class TestGUI : MonoBehaviour
{
		public GUISkin thisMetalGUISkin;

		public GUIStyle style;
		public Texture texture;

		private int w;
		private int h;

		private LTRect ModelTypeButton;
		private LTRect DropDownBox;
		private LTRect EmpireButton;
		private LTRect VegasButton;

		private LTRect FunctionTypeButton;
		private Rect EmpireBoxRect;
		private Rect VegasBoxRect;
		private LTRect SlideInBox;
		private LTRect FunctionButton1;
		private LTRect FunctionButton2;

		private float Padding = 5.0f;
		private float ButtonWidth = 150.0f;
		private float ButtonHeight = 30.0f;

		private IList<string> ModelTypeList;
		private IList<string> EmpireFunctionTypeList;
		private IList<string> VegasFunctionTypeList;

		private bool ShowDropDown = false;
		private bool DropDownAnimationRunning = true;

		private bool ShowSlideIn = false;
		private bool SlideInAnimationRunning = true;

		private bool ModelSelected {
				get {
						return GUIState.mSelectedModelType != GUIState.ModelType.None;
				}
		}
		
		private Dictionary<GUIState.FunctionType, string> FunctionTypes {
				get {
						if (GUIState.mSelectedModelType == GUIState.ModelType.NewYorkSkyline) {
								return GUIState.EmpireFunctionTypes;
						} else {
								return GUIState.VegasFunctionTypes;
						}
				}
		}

		// Use this for initialization
		void Start ()
		{
				w = Screen.width;
				h = Screen.height;

				// Model type init
				ModelTypeList = GUIState.ModelTypes.Select (mt => mt.Value).ToList ();

				ModelTypeButton = new LTRect (new Rect (-1 * (Padding + ButtonWidth), Padding, ButtonWidth, ButtonHeight));
				DropDownBox = new LTRect (new Rect (Padding, (2 * Padding) + ButtonHeight, ButtonWidth, 0));
		
				EmpireButton = new LTRect (new Rect (-1 * (Padding + ButtonWidth), (3 * Padding) + ButtonHeight, ButtonWidth, ButtonHeight));
				VegasButton = new LTRect (new Rect (-1 * (Padding + ButtonWidth), (4 * Padding) + (2 * ButtonHeight), ButtonWidth, ButtonHeight));

				// Function type init
				EmpireFunctionTypeList = GUIState.EmpireFunctionTypes.Select (ft => ft.Value).ToList ();
				VegasFunctionTypeList = GUIState.VegasFunctionTypes.Select (ft => ft.Value).ToList ();
			
				FunctionTypeButton = new LTRect (new Rect (w + (Padding + ButtonWidth), Padding, ButtonWidth, ButtonHeight));
				
				//EmpireBoxRect = new Rect(Padding, )
				SlideInBox = new LTRect (new Rect (w - (4 * Padding + 2 * ButtonWidth), Padding - 2, 3 * Padding + 2 * ButtonWidth, 0));

				FunctionButton1 = new LTRect (new Rect (w + Padding, Padding, ButtonWidth, ButtonHeight));
				FunctionButton2 = new LTRect (new Rect (w + Padding, Padding, ButtonWidth, ButtonHeight));
				
				// Initial animations
				LeanTween.move (ModelTypeButton, new Vector2 (Padding, Padding), 1.0f).setEase (LeanTweenType.easeOutBack).setOnComplete (() => DropDownAnimationRunning = false);
		}

		void OnGUI ()
		{
				//GUI.skin = thisMetalGUISkin;
				HandleModelTypeDropDown ();
				HandleFunctionTypeDropDown ();
		
		
				if ((ShowDropDown && Event.current.type == EventType.mouseDown && !DropDownBox.rect.Contains (Event.current.mousePosition)) ||
						(ShowSlideIn && Event.current.type == EventType.mouseDown && !SlideInBox.rect.Contains (Event.current.mousePosition))) {
						CollapseAll ();
				}
		}

		void HandleFunctionTypeDropDown ()
		{
				if (ModelSelected) {
						if (!ShowSlideIn) {
								if (GUI.Button (FunctionTypeButton.rect, FunctionTypes [GUIState.mSelectedFunctionType])) {
										if (!SlideInAnimationRunning) {
												if (!ShowSlideIn) {
														ExpandSlideIn ();
												} 
										}
								}
						} else {
								GUI.Box (SlideInBox.rect, texture);
								if (GUI.Button (FunctionButton1.rect, FunctionTypes [GUIState.FunctionType.Panoramic])) {
										if (!SlideInAnimationRunning) {
												GUIState.mSelectedFunctionType = GUIState.FunctionType.Panoramic;
												CollapseAll ();
										}
								}
								if (GUI.Button (FunctionButton2.rect, FunctionTypes [GUIState.FunctionType.Model3D])) {
										if (!SlideInAnimationRunning) {
												GUIState.mSelectedFunctionType = GUIState.FunctionType.Model3D;
												//ExpandSlideIn ();
												CollapseAll ();
										}
								}
						}
				}
		}

		void HandleModelTypeDropDown ()
		{
				if (GUI.Button (ModelTypeButton.rect, GUIState.ModelTypes [GUIState.mSelectedModelType])) {
						if (!DropDownAnimationRunning) {
								if (!ShowDropDown) {
										ExpandDropDown ();
								} else {
										CollapseDropDown ();
								}
						}
				}
		
				if (ShowDropDown) {
						GUI.Box (DropDownBox.rect, texture);
						if (GUI.Button (EmpireButton.rect, GUIState.ModelTypes [GUIState.ModelType.NewYorkSkyline])) {
								// Close drop down
								if (!DropDownAnimationRunning) {
										GUIState.mSelectedModelType = GUIState.ModelType.NewYorkSkyline;
										ExpandSlideIn ();
										//CollapseDropDown ();
								}
						}
			
						if (GUI.Button (VegasButton.rect, GUIState.ModelTypes [GUIState.ModelType.Vegas])) {
								// Close drop down
								if (!DropDownAnimationRunning) {
										GUIState.mSelectedModelType = GUIState.ModelType.Vegas;
										ExpandSlideIn ();
										//CollapseDropDown ();
								}
						}
				}
		
				if (ShowDropDown && Event.current.type == EventType.mouseDown && !DropDownBox.rect.Contains (Event.current.mousePosition)) {
						CollapseDropDown ();
				}
		}

		void CollapseAll ()
		{
				CollapseDropDown ();
				CollapseSlideIn ();
		}

	#region Slide In Expand/Collapse Methods

		void ShowFunctionTypeButton ()
		{
				LeanTween.move (FunctionTypeButton, new Vector2 (w - (Padding + ButtonWidth), Padding), 0.3f).setEase (LeanTweenType.easeOutBack);
		}

		void HideFunctionTypeButton ()
		{
				LeanTween.move (FunctionTypeButton, new Vector2 (w + (Padding + ButtonWidth), Padding), 0.3f).setEase (LeanTweenType.easeInBack);
		}

		void ExpandSlideIn ()
		{
				HideFunctionTypeButton ();
				ShowSlideIn = true;
				SlideInAnimationRunning = true;
				//LeanTween.move (FunctionTypeButton, new Vector2 (w - (Padding + ButtonWidth), Padding), 0.6f).setEase (LeanTweenType.easeOutBack);
				LeanTween.scale (SlideInBox, new Vector2 (2 * Padding + 2 * ButtonWidth, ButtonHeight + 4), 0.8f).setEase (LeanTweenType.easeOutExpo).setOnComplete (() => CompleteCollapseSlideIn ());
				LeanTween.move (FunctionButton1, new Vector2 (w - (2 * Padding + 2 * ButtonWidth), Padding), 0.4f).setEase (LeanTweenType.easeOutExpo)
			.setOnComplete (() => LeanTween.move (FunctionButton2, new Vector2 (w - (Padding + ButtonWidth), Padding), 0.3f).setEase (LeanTweenType.easeOutExpo));
		}

		void CollapseSlideIn ()
		{
				SlideInAnimationRunning = true;
				LeanTween.move (FunctionButton2, new Vector2 (w + Padding, Padding), 0.3f).setEase (LeanTweenType.easeInBack)
			.setOnComplete (() => ChainSlideIn ());
		}

		void ChainSlideIn ()
		{
				// TODO: Change this
				LeanTween.move (FunctionButton1, new Vector2 (w + Padding, Padding), 0.4f).setEase (LeanTweenType.easeInBack);
				LeanTween.scale (SlideInBox, new Vector2 (2 * Padding + 2 * ButtonWidth, 0), 0.3f).setDelay (0.2f).setEase (LeanTweenType.easeInBack).setOnComplete (() => CompleteExpandSlideIn ());
		}

		void CompleteCollapseSlideIn ()
		{
				SlideInAnimationRunning = false;
				ShowFunctionTypeButton ();
		}

		void CompleteExpandSlideIn ()
		{
				ShowSlideIn = SlideInAnimationRunning = false;
		}

	#endregion

	#region DropDown Expand/Collapse Methods

		void ExpandDropDown ()
		{
				ShowDropDown = true;
				DropDownAnimationRunning = true;
				LeanTween.scale (DropDownBox, new Vector2 (ButtonWidth, (3 * Padding) + (2 * ButtonHeight)), 1.3f).setEase (LeanTweenType.easeOutExpo);
				LeanTween.move (EmpireButton, new Vector2 (Padding, (3 * Padding) + ButtonHeight), 0.7f).setEase (LeanTweenType.easeOutBack).setDelay (0.2f);
				LeanTween.move (VegasButton, new Vector2 (Padding, (4 * Padding) + (2 * ButtonHeight)), 0.7f).setEase (LeanTweenType.easeOutBack).setDelay (0.3f).setOnComplete (() => CompleteExpandDropDown ());
		}

		void CollapseDropDown ()
		{
				DropDownAnimationRunning = true;
				LeanTween.scale (DropDownBox, new Vector2 (ButtonWidth, 0), 0.7f).setEase (LeanTweenType.easeInBack).setDelay (0.1f).setOnComplete (() => CompleteCollapseDropDown ());
				LeanTween.move (EmpireButton, new Vector2 (-1 * (Padding + ButtonWidth), (3 * Padding) + ButtonHeight), 0.5f).setEase (LeanTweenType.easeInBack).setDelay (0.1f);
				LeanTween.move (VegasButton, new Vector2 (-1 * (Padding + ButtonWidth), (4 * Padding) + (2 * ButtonHeight)), 0.5f).setEase (LeanTweenType.easeInBack);
		}

		void CompleteExpandDropDown ()
		{
				DropDownAnimationRunning = false;
		}

		void CompleteCollapseDropDown ()
		{
				ShowDropDown = DropDownAnimationRunning = false;
		}

	#endregion
}
