using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class HeaderGUI : MonoBehaviour
{
		private enum ButtonFunction
		{
				VegasModel = 1,
				EmpireModel = 2,
				EmpireImageSlider = 3,
				EmpireInfo = 4
		}

	#region Fields

		public GUISkin mGUISkin;
		public Texture mTexture;

		public Texture Image1;
		public Texture Image2;
		public Texture Image3;

		private IList<Texture> mImages;

		private int w;
		private int h;
		private int nButtons;

		private float mButtonWidth = 150.0f;
		private float mButtonHeight = 30.0f;

		private float mTopOffset = 10.0f;
		private float mButtonOffset = 5.0f;

		private RectOffset mBoxPadding = new RectOffset (5, 5, 5, 5);
		private RectOffset mButtonPadding = new RectOffset (5, 5, 5, 5);

		private LTRect mTopBarBox;
		private static IList<KeyValuePair<ButtonFunction, LTRect>> mButtons = new List<KeyValuePair<ButtonFunction, LTRect>> ();

		private static Dictionary<ButtonFunction, string> mButtonFunctions = new Dictionary<ButtonFunction, string> ()
		{
			{ButtonFunction.VegasModel, "Vegas Model"},
			{ButtonFunction.EmpireModel, "Empire Model"},
			{ButtonFunction.EmpireImageSlider, "Empire Image Slider"},
			{ButtonFunction.EmpireInfo, "Empire Info"}
		};

		private float mImageSliderWidth = 150.0f;
		private float mImageSliderHeight = 150.0f;
		private float mSliderButtonOffset = 30.0f;
		private LTRect mImageSliderBox;
		private LTRect mPrevButton;
		private LTRect mNextButton;
		private int mSelectedImageIndex = 0;
		private bool mShowImageSlider = false;

		private ButtonFunction mSelectedFunction;

	#endregion

	#region Events

		// Use this for initialization
		void Start ()
		{
				InitGUI ();
				InitSlider ();
		}

		void OnGUI ()
		{
				GUI.skin = mGUISkin;

				GUI.Box (mTopBarBox.rect, "");
				foreach (var button in mButtons) {
						if (GUI.Button (button.Value.rect, mButtonFunctions [button.Key])) {
								HandleButtonClick (button.Key);
						}
				}

				ImageSliderGUI ();
		}

	#endregion

	#region GUI Methods

		void HandleButtonClick (ButtonFunction selectedFunction)
		{
				// Only run code if changing function
				if (mSelectedFunction != selectedFunction) {
						mSelectedFunction = selectedFunction;

						// Reset all to hidden, then display the one we want, easier to manage
						Reset ();
			
						switch (selectedFunction) {
						case ButtonFunction.VegasModel:
								//Debug.Log (mButtonFunctions [ButtonFunction.VegasModel]);
								break;
						case ButtonFunction.EmpireModel:
								//Debug.Log (mButtonFunctions [ButtonFunction.EmpireModel]);
								break;
						case ButtonFunction.EmpireImageSlider:
								//Debug.Log (mButtonFunctions [ButtonFunction.EmpireImageSlider]);
								mShowImageSlider = true;
								break;
						case ButtonFunction.EmpireInfo:
								//Debug.Log (mButtonFunctions [ButtonFunction.EmpireInfo]);
								break;
						default:
								break;
						}
				}
		}
	
		void InitGUI ()
		{
				w = Screen.width;
				h = Screen.height;
				nButtons = mButtonFunctions.Count;

				if (mGUISkin != null) {
						mButtonWidth = mGUISkin.button.fixedWidth;
						mButtonHeight = mGUISkin.button.fixedHeight;
						mBoxPadding = mGUISkin.box.padding;
						mButtonPadding = mGUISkin.button.padding;
				}

				float midPoint = w / 2;
				float betweenButtons = (nButtons - 1) * mButtonOffset;
				float boxLeftRightPadding = mBoxPadding.left + mBoxPadding.right;
				float boxTopBottomPadding = mBoxPadding.top + mBoxPadding.top;
				float totalButtonWidth = nButtons * mButtonWidth;
				float totalBoxWidth = totalButtonWidth + boxLeftRightPadding + betweenButtons;
				float totalBoxHeight = mButtonHeight + boxTopBottomPadding;
				float boxLeftPos = midPoint - (totalBoxWidth / 2);
				float boxTopPos = mTopOffset;
				mTopBarBox = new LTRect (new Rect (boxLeftPos, boxTopPos, totalBoxWidth, totalBoxHeight));

				float buttonTop = boxTopPos + mBoxPadding.top;
				int count = 0;
				foreach (var kvp in mButtonFunctions) {
						float buttonLeft = boxLeftPos + mBoxPadding.left + count * (mButtonWidth + mButtonOffset);
						LTRect button = new LTRect (new Rect (buttonLeft, buttonTop, mButtonWidth, mButtonHeight));
						mButtons.Add (new KeyValuePair<ButtonFunction, LTRect> (kvp.Key, button));
						count++;
				}
		}

	#endregion

	#region Image Slider

		void ImageSliderGUI ()
		{
				if (mShowImageSlider) {
						GUI.DrawTexture (mImageSliderBox.rect, mTexture);
						GUI.DrawTexture (mImageSliderBox.rect, mImages [mSelectedImageIndex]);
						if (GUI.Button (mPrevButton.rect, "Prev")) {
								mSelectedImageIndex--;
								if (mSelectedImageIndex < 0) {
										mSelectedImageIndex = 0;
								}
						}
						if (GUI.Button (mNextButton.rect, "Next")) {
								mSelectedImageIndex++;
								if (mSelectedImageIndex > mImages.Count - 1) {
										mSelectedImageIndex = mImages.Count - 1;
								}
						}
				}
		}

		void InitSlider ()
		{
				// TODO: This can be refactored when we figure out how to load from a zip file or folder etc.
				mImages = new List<Texture> ()
				{
					Image1,
					Image2,
					Image3
				};

				// Weird shit going down here, things are coming up 0 when they should have a value :?
				float sliderLeft = (w / 2) - (mButtonWidth / 2);
				float sliderTop = (h / 2) - (mButtonWidth / 2);
				Debug.Log ("mImageSliderWidth" + mButtonWidth);
				Debug.Log ("mImageSliderHeight" + mButtonWidth);
				Debug.Log ("mSliderButtonOffset" + mSliderButtonOffset);
				mImageSliderBox = new LTRect (new Rect (sliderLeft, sliderTop, mButtonWidth, mButtonWidth));

				float buttonTop = h / 2 + mButtonHeight / 2;
//				float prevButtonLeft = sliderLeft - (mSliderButtonOffset + mButtonWidth);
//				float nextButtonLeft = sliderLeft + mButtonWidth + mSliderButtonOffset;
				float prevButtonLeft = sliderLeft - (30.0f + mButtonWidth);
				float nextButtonLeft = sliderLeft + mButtonWidth + 30.0f;
				mPrevButton = new LTRect (new Rect (prevButtonLeft, buttonTop, mButtonWidth, mButtonHeight));
				mNextButton = new LTRect (new Rect (nextButtonLeft, buttonTop, mButtonWidth, mButtonHeight));
		}

//		IEnumerator LoadFiles ()
//		{
//				Texture2D tex;
//
//				string filePath = Path.Combine (Application.streamingAssetsPath, "Images.zip");
//				
//				if (filePath.Contains ("://")) {
//			WWW www = new WWW(filePath);
//			yield return www;
//			tex = www.texture;
//				} else {
//			result = System.IO.File.(filePath);
//		Resources.LoadAssetAtPath(filePath)
//				}
//				yield return new WaitForSeconds (2.0f);
//				Debug.Log ("Logan - After yield");
//		}

	#endregion

	#region General Methods

		void Reset ()
		{
				// Do hiding of all elements here
				mShowImageSlider = false;
		}

	#endregion
}
