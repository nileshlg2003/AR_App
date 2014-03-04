using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HeaderGUI : MonoBehaviour
{
		private enum ButtonFunction
		{
				VegasModel = 1,
				EmpireModel = 2,
				EmpireImageSlider = 3,
				EmpireInfo = 4
		}

		public GUISkin mGUISkin;
		public Texture mTexture;

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

		// Use this for initialization
		void Start ()
		{
				w = Screen.width;
				h = Screen.height;
				Debug.Log (mButtonFunctions);
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

		void OnGUI ()
		{
				GUI.skin = mGUISkin;

				GUI.Box (mTopBarBox.rect, "");
				foreach (var button in mButtons) {
						if (GUI.Button (button.Value.rect, mButtonFunctions [button.Key])) {
								HandleButtonClick (button.Key);
						}
				}
		}

		void HandleButtonClick (ButtonFunction key)
		{
				switch (key) {
				case ButtonFunction.VegasModel:
						Debug.Log (mButtonFunctions [ButtonFunction.VegasModel]);
						break;
				case ButtonFunction.EmpireModel:
						Debug.Log (mButtonFunctions [ButtonFunction.EmpireModel]);
						break;
				case ButtonFunction.EmpireImageSlider:
						Debug.Log (mButtonFunctions [ButtonFunction.EmpireImageSlider]);
						break;
				case ButtonFunction.EmpireInfo:
						Debug.Log (mButtonFunctions [ButtonFunction.EmpireInfo]);
						break;
				default:
						break;
				}
		}
}
