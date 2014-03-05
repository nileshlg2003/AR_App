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

    // Public properties
    public GUISkin mGUISkin;
    public Texture mTexture;
    public Texture Image1;
    public Texture Image2;
    public Texture Image3;
    
    private IList<Texture> mImages;
    
    // Screen params
    private int w;
    private int h;
    
    // Header GUI props
    private int nButtons;
    private float mButtonWidth;
    private float mButtonHeight;
    private float mTopOffset;
    private float mButtonOffset;
    private RectOffset mBoxPadding;
    private LTRect mTopBarBox;
    
    // Image Slider GUI props
    private float mImageSliderWidth;
    private float mImageSliderHeight;
    private float mSliderButtonOffset;
    private LTRect mImageSliderBox;
    private LTRect mPrevButton;
    private LTRect mNextButton;
    private int mSelectedImageIndex;
    
    // GUI state props
    private ButtonFunction mSelectedFunction;
    
    // Show/Hide bools
    private bool mShowImageSlider;
    
    // Lists etc.
    private static IList<KeyValuePair<ButtonFunction, LTRect>> mButtons = new List<KeyValuePair<ButtonFunction, LTRect>>();
    private static Dictionary<ButtonFunction, string> mButtonFunctions = new Dictionary<ButtonFunction, string>()
    {
        {ButtonFunction.VegasModel, "Vegas Model"},
        {ButtonFunction.EmpireModel, "Empire Model"},
        {ButtonFunction.EmpireImageSlider, "Empire Image Slider"},
        {ButtonFunction.EmpireInfo, "Empire Info"}
    };

    #endregion

    #region Events

    void Awake()
    {
        // Weird stuff was happening when these were being initialized at
        // declaration time, like some values were just not getting set
        w = Screen.width;
        h = Screen.height;

        mButtonWidth = 150.0f;
        mButtonHeight = 30.0f;
        mTopOffset = 10.0f;
        mButtonOffset = 5.0f;
        mBoxPadding = new RectOffset(5, 5, 5, 5);

        // TODO: Hard code or variable?
        float temp = w < h ? w : h;
        mImageSliderWidth = temp / 2;
        mImageSliderHeight = temp / 2;
        mSliderButtonOffset = 30.0f;

        mSelectedImageIndex = 0;
        mShowImageSlider = false;

        nButtons = mButtonFunctions.Count;
    }

    void Start()
    {
        InitButtonsGUI();
        InitSlider();
    }

    void OnGUI()
    {
        GUI.skin = mGUISkin;

        ButtonsGUI();
        ImageSliderGUI();
    }

    #endregion

    #region GUI Methods

    void ButtonsGUI()
    {
        GUI.Box(mTopBarBox.rect, "");
        foreach (var button in mButtons)
        {
            if (GUI.Button(button.Value.rect, mButtonFunctions [button.Key]))
            {
                HandleButtonClick(button.Key);
            }
        }
    }

    void HandleButtonClick(ButtonFunction selectedFunction)
    {
        // Only run code if changing function
        if (mSelectedFunction != selectedFunction)
        {
            mSelectedFunction = selectedFunction;

            // Reset all to hidden, then display the one we want, easier to manage
            Reset();

            switch (selectedFunction)
            {
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

    void InitButtonsGUI()
    {
        if (mGUISkin != null)
        {
            mButtonWidth = mGUISkin.button.fixedWidth;
            mButtonHeight = mGUISkin.button.fixedHeight;
            mBoxPadding = mGUISkin.box.padding;
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
        mTopBarBox = new LTRect(new Rect(boxLeftPos, boxTopPos, totalBoxWidth, totalBoxHeight));

        float buttonTop = boxTopPos + mBoxPadding.top;
        int count = 0;
        foreach (var kvp in mButtonFunctions)
        {
            float buttonLeft = boxLeftPos + mBoxPadding.left + count * (mButtonWidth + mButtonOffset);
            LTRect button = new LTRect(new Rect(buttonLeft, buttonTop, mButtonWidth, mButtonHeight));
            mButtons.Add(new KeyValuePair<ButtonFunction, LTRect>(kvp.Key, button));
            count++;
        }
    }

    void ExpandButtons()
    {

    }

    #endregion

    #region Image Slider

    void ImageSliderGUI()
    {
        if (mShowImageSlider)
        {
            GUI.DrawTexture(mImageSliderBox.rect, mTexture);
            GUI.DrawTexture(mImageSliderBox.rect, mImages [mSelectedImageIndex]);
            if (GUI.Button(mPrevButton.rect, "Prev"))
            {
                mSelectedImageIndex--;
                if (mSelectedImageIndex < 0)
                {
                    mSelectedImageIndex = 0;
                }
            }
            if (GUI.Button(mNextButton.rect, "Next"))
            {
                mSelectedImageIndex++;
                if (mSelectedImageIndex > mImages.Count - 1)
                {
                    mSelectedImageIndex = mImages.Count - 1;
                }
            }
        }
    }

    void InitSlider()
    {
        // TODO: This can be refactored when we figure out how to load from a zip file or folder etc.
        mImages = new List<Texture>()
		{
			Image1,
			Image2,
			Image3
		};

        float sliderLeft = (w / 2) - (mImageSliderWidth / 2);
        float sliderTop = (h / 2) - (mImageSliderHeight / 2);
        mImageSliderBox = new LTRect(new Rect(sliderLeft, sliderTop, mImageSliderWidth, mImageSliderHeight));

        float buttonTop = h / 2 + mButtonHeight / 2;
        float prevButtonLeft = sliderLeft - (mSliderButtonOffset + mButtonWidth);
        float nextButtonLeft = sliderLeft + mImageSliderWidth + mSliderButtonOffset;
        mPrevButton = new LTRect(new Rect(prevButtonLeft, buttonTop, mButtonWidth, mButtonHeight));
        mNextButton = new LTRect(new Rect(nextButtonLeft, buttonTop, mButtonWidth, mButtonHeight));

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
    
    void Reset()
    {		
        // Hide all elements here
        mShowImageSlider = false;
    }
    
    #endregion
}
