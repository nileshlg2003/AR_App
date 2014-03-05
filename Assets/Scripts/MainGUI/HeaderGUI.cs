using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class HeaderGUI : MonoBehaviour
{
    public enum ButtonFunction
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
    public GameObject mEmpireStateBuilding;
    public GameObject mVegasStrip;
    private IList<Texture> mImages;

    // Panoramic Related
    public Camera panoCamera;
    public Camera ARCamera;
    public Material panoramicMaterial;
    public bool showPanoramic = false;

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
    private float mBoxDeltaHeight;
    private float mButtonDeltaHeight;
    private float mBounceOffset;
    
    // Image Slider GUI props
    private float mImageSliderWidth;
    private float mImageSliderHeight;
    private float mSliderButtonOffset;
    private LTRect mImageSliderBox;
    private LTRect mPrevButton;
    private LTRect mNextButton;
    private int mSelectedImageIndex;
    
    // Info Box GUI props
    private float mInfoBoxWidth;
    private float mInfoBoxHeight;
    private LTRect mInfoBoxBox;
    private Vector2 mInfoBoxScrollPosition;
    
    // GUI state props
    private ButtonFunction mSelectedFunction;
    
    // Show/Hide bools
    private bool mShowImageSlider;
    private bool mShowInfoBox;
    
    // Lists etc.
    private static IList<CustomButton> mButtons = new List<CustomButton>();
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
        mBounceOffset = 3.0f;
        mBoxPadding = new RectOffset(5, 5, 5, 5);

        // TODO: Hard code or variable?
        float temp = w < h ? w : h;
        mImageSliderWidth = temp / 2;
        mImageSliderHeight = temp / 2;
        mSliderButtonOffset = 30.0f;
        
        mInfoBoxWidth = w * 0.75f;
        mInfoBoxHeight = h * 0.75f;

        mSelectedImageIndex = 0;
        mShowImageSlider = false;

        nButtons = mButtonFunctions.Count;
    }

    void Start()
    {
        panoCamera.enabled = showPanoramic;

        InitButtonsGUI();
        InitSlider();
        InitInfoBox();
        Reset();
    }

    void OnGUI()
    {
        GUI.skin = mGUISkin;

        ButtonsGUI();
        ImageSliderGUI();
        InfoBoxGUI();

        if (showPanoramic) {
            var buttonX = (Screen.width - 100) / 2.0f;
            var buttonY = (Screen.height - 50) / 2.0f;
            
            if (GUI.Button (new Rect (buttonX, buttonY, 100, 50), "Back")) {
                HidePanoramic();
            }
        }
    }

    #endregion

    #region GUI Methods

    void ButtonsGUI()
    {
        GUI.Box(mTopBarBox.rect, "");
        foreach (var button in mButtons)
        {
            if (GUI.Button(button.mRect.rect, button.mText))
            {
                HandleButtonClick(button.mFunction);
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
            BounceButton(selectedFunction);
            switch (selectedFunction)
            {
                case ButtonFunction.VegasModel:
                    SetupVegasModel();
                    break;
                case ButtonFunction.EmpireModel:
                    SetupEmpireStateModel();
                    break;
                case ButtonFunction.EmpireImageSlider:
                    //Debug.Log (mButtonFunctions [ButtonFunction.EmpireImageSlider]);
                    mShowImageSlider = true;
                    break;
                case ButtonFunction.EmpireInfo:
                    mShowInfoBox = true;
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
        mBoxDeltaHeight = mTopOffset + totalBoxHeight;
        mTopBarBox = new LTRect(new Rect(boxLeftPos, boxTopPos - mBoxDeltaHeight, totalBoxWidth, totalBoxHeight));

        float buttonTop = boxTopPos + mBoxPadding.top;
        mButtonDeltaHeight = buttonTop + mButtonHeight;
        int count = 0;
        foreach (var kvp in mButtonFunctions)
        {
            float buttonLeft = boxLeftPos + mBoxPadding.left + count * (mButtonWidth + mButtonOffset);
            LTRect rect = new LTRect(new Rect(buttonLeft, buttonTop - mButtonDeltaHeight, mButtonWidth, mButtonHeight));
            mButtons.Add(new CustomButton(kvp.Key, kvp.Value, rect));
            count++;
        }
        
        ExpandButtons();
    }

    void ExpandButtons()
    {
        LeanTween.move(mTopBarBox, new Vector2(mTopBarBox.rect.x, mTopBarBox.rect.y + mBoxDeltaHeight), 1.0f).setEase(LeanTweenType.easeOutExpo).setDelay(0.5f);
        
        int count = 0;
        foreach (var button in mButtons)
        {
            float extraDelay = count * 0.1f;
            LeanTween.move(button.mRect, new Vector2(button.mRect.rect.x, button.mRect.rect.y + mButtonDeltaHeight), 0.5f).setEase(LeanTweenType.easeOutBack).setDelay(0.5f + extraDelay);
            count++;
        }
    }
    
    void BounceButton(ButtonFunction selectedFunction)
    {
        LTRect buttonRect = mButtons.Where(b => b.mFunction == selectedFunction).Select(b => b.mRect).First();
        if (!LeanTween.isTweening(buttonRect))
        {
            LeanTween.move(buttonRect, new Vector2(buttonRect.x, buttonRect.y - mButtonOffset), 0.1f).setEase(LeanTweenType.easeOutCubic)
                .setOnComplete(() => LeanTween.move(buttonRect, new Vector2(buttonRect.x, buttonRect.y + mButtonOffset), 0.5f).setEase(LeanTweenType.easeOutBounce));
        }
    }

    #endregion

    #region Image Slider

    void ImageSliderGUI()
    {
        if (mShowImageSlider)
        {
            GUI.DrawTexture(mImageSliderBox.rect, mTexture);
            GUI.DrawTexture(mImageSliderBox.rect, mImages[mSelectedImageIndex]);
            if (GUI.Button(mPrevButton.rect, "Prev"))
            {
                if (mSelectedImageIndex > 0) {
                    mSelectedImageIndex--;
                } else {
                    mSelectedImageIndex = mImages.Count-1;
                }
            }
            if (GUI.Button(mNextButton.rect, "Next"))
            {
                if (mSelectedImageIndex < mImages.Count-1) {
                    mSelectedImageIndex++;
                } else {
                    mSelectedImageIndex = 0;
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
    
    #region Info Box GUI

    void InfoBoxGUI()
    {
        if (mShowInfoBox)
        {
            // TODO: Change background of are to be something more awesome
            GUI.Box(mInfoBoxBox.rect, "");
            GUILayout.BeginArea(mInfoBoxBox.rect);
            mInfoBoxScrollPosition = GUILayout.BeginScrollView(mInfoBoxScrollPosition, GUILayout.Width(mInfoBoxWidth), GUILayout.Height(mInfoBoxHeight));
            
            GUILayout.Label("BLAH BLAHDFAGFH ADSLGHADGLHADGF BLAH BLAHDFAGFH ADSLGHADGLHADGF BLAH BLAHDFAGFH ADSLGHADGLHADGF BLAH BLAHDFAGFH ADSLGHADGLHADGF BLAH BLAHDFAGFH ADSLGHADGLHADGF BLAH BLAHDFAGFH ADSLGHADGLHADGF BLAH BLAHDFAGFH ADSLGHADGLHADGF BLAH BLAHDFAGFH ADSLGHADGLHADGF BLAH BLAHDFAGFH ADSLGHADGLHADGF BLAH BLAHDFAGFH ADSLGHADGLHADGF BLAH BLAHDFAGFH ADSLGHADGLHADGF BLAH BLAHDFAGFH ADSLGHADGLHADGF BLAH BLAHDFAGFH ADSLGHADGLHADGF BLAH BLAHDFAGFH ADSLGHADGLHADGF BLAH BLAHDFAGFH ADSLGHADGLHADGF BLAH BLAHDFAGFH ADSLGHADGLHADGF BLAH BLAHDFAGFH ADSLGHADGLHADGF BLAH BLAHDFAGFH ADSLGHADGLHADGF BLAH BLAHDFAGFH ADSLGHADGLHADGF BLAH BLAHDFAGFH ADSLGHADGLHADGF ");
            
            GUILayout.EndScrollView();
            GUILayout.EndArea();
        }
    }
    
    void InitInfoBox()
    {
        float infoBoxLeft = (w / 2) - (mInfoBoxWidth / 2);
        float infoBoxTop = (h / 2) - (mInfoBoxHeight / 2);
        mInfoBoxBox = new LTRect(new Rect(infoBoxLeft, infoBoxTop, mInfoBoxWidth, mInfoBoxHeight));
    }
    
    #endregion

    #region General Methods
    
    void Reset()
    {		
        // Hide all elements here
        mShowImageSlider = false;
        mShowInfoBox = false;
        mVegasStrip.SetActive(false);
        mEmpireStateBuilding.SetActive(false);
    }
    
    void SetupVegasModel()
    {
        mVegasStrip.SetActive(true);
    }
    
    void SetupEmpireStateModel()
    {
        mEmpireStateBuilding.SetActive(true);     
    }
    
    void ShowPanoramic() {
        showPanoramic = true;
        
        RenderSettings.skybox = panoramicMaterial;
        
        panoCamera.enabled = true;
        ARCamera.enabled = false;
    }
    
    void HidePanoramic() {
        showPanoramic = false;
        
        panoCamera.enabled = false;
        ARCamera.enabled = true;
    }

    #endregion
}

class CustomButton
{
    public HeaderGUI.ButtonFunction mFunction { get; set; }
    
    public string mText { get; set; }
    
    public LTRect mRect { get; set; }
    
    public bool mTweening = false;
    
    public CustomButton(HeaderGUI.ButtonFunction function, string text, LTRect rect)
    {
        mFunction = function;
        mText = text;
        mRect = rect;
    }
}
