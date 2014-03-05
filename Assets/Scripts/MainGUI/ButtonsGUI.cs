using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System;

public class ButtonsGUI : MonoBehaviour
{
    // Comes from iPad 2
    public const float DEFAULT_DPI = 132.0f;

    public enum ButtonFunction
    {
        VegasModel = 1,
        EmpireModel = 2,
        EmpireImageSlider = 3,
        EmpireInfo = 4,
        Panoramic = 5
    }

    #region Fields

    // Public properties
    public GUISkin GUISkin;
    public GameObject EmpireStateBuilding;
    public GameObject VegasStrip;
    
    public InfoBoxGUI InfoBoxGUI;
    public ImageSliderGUI ImageSliderGUI;
    public PanoramicGUI PanoramicGUI;
    

    // Screen params
    private int _W;
    private int _H;
    private float _Scale;
    
    // Header GUI props
    private int _NumButtons;
    private float _ButtonWidth;
    private float _ButtonHeight;
    private float _TopOffset;
    private float _ButtonOffset;
    private RectOffset _BoxPadding;
    private LTRect _TopBarBox;
    private float _BoxDeltaHeight;
    private float _ButtonDeltaHeight;
    private float _BounceOffset;
    
    
    // GUI state props
    private ButtonFunction _SelectedFunction;
    private bool _AnimationRunning = true;
    
    // Show/Hide bools
    
    // Lists etc.
    private static IList<CustomButton> _ButtonList = new List<CustomButton>();
    private static Dictionary<ButtonFunction, string> _ButtonFunctions = new Dictionary<ButtonFunction, string>()
    {
        {ButtonFunction.VegasModel, "Vegas Model"},
        {ButtonFunction.EmpireModel, "Empire Model"},
        {ButtonFunction.EmpireImageSlider, "Empire Image Slider"},
        {ButtonFunction.EmpireInfo, "Empire Info"},
        {ButtonFunction.Panoramic, "Panoramic"}
    };

    #endregion

    #region Events

    void Awake()
    {
        // Weird stuff was happening when these were being initialized at
        // declaration time, like some values were just not getting set
        _W = Screen.width;
        _H = Screen.height;
        _Scale = Screen.dpi > 0 ? Screen.dpi / DEFAULT_DPI : 1.0f;
        
        _ButtonWidth = _W * 0.15f;
        _ButtonHeight = _H * 0.025f;
        _TopOffset = _H * 0.01f;
        _ButtonOffset = _H * 0.005f;
        _BounceOffset = _H * 0.005f;
        
        int padding = Convert.ToInt32(Math.Round(_H * 0.005f, MidpointRounding.AwayFromZero));
        _BoxPadding = new RectOffset(padding, padding, padding, padding);
        
        _NumButtons = _ButtonFunctions.Count;
    }

    void Start()
    {
        Init();
    }

    void OnGUI()
    {
        GUI.skin = GUISkin;

        GUI.Box(_TopBarBox.rect, "");
        foreach (var button in _ButtonList)
        {
            if (GUI.Button(button.mRect.rect, button.mText))
            {
                HandleButtonClick(button.mFunction);
            }
        }
    }

    #endregion

    #region Button GUI Methods
    
    void Init()
    {
        if (GUISkin != null)
        {
            _ButtonWidth = GUISkin.button.fixedWidth;
            _ButtonHeight = GUISkin.button.fixedHeight;
            _BoxPadding = GUISkin.box.padding;
        }
        
        float midPoint = _W / 2;
        float betweenButtons = (_NumButtons - 1) * _ButtonOffset;
        float boxLeftRightPadding = _BoxPadding.left + _BoxPadding.right;
        float boxTopBottomPadding = _BoxPadding.top + _BoxPadding.top;
        float totalButtonWidth = _NumButtons * _ButtonWidth;
        float totalBoxWidth = totalButtonWidth + boxLeftRightPadding + betweenButtons;
        float totalBoxHeight = _ButtonHeight + boxTopBottomPadding;
        float boxLeftPos = midPoint - (totalBoxWidth / 2);
        float boxTopPos = _TopOffset;
        _BoxDeltaHeight = _TopOffset + totalBoxHeight;
        _TopBarBox = new LTRect(new Rect(boxLeftPos, boxTopPos - _BoxDeltaHeight, totalBoxWidth, totalBoxHeight));
        
        float buttonTop = boxTopPos + _BoxPadding.top;
        _ButtonDeltaHeight = buttonTop + _ButtonHeight;
        int count = 0;
        foreach (var kvp in _ButtonFunctions)
        {
            float buttonLeft = boxLeftPos + _BoxPadding.left + count * (_ButtonWidth + _ButtonOffset);
            LTRect rect = new LTRect(new Rect(buttonLeft, buttonTop - _ButtonDeltaHeight, _ButtonWidth, _ButtonHeight));
            _ButtonList.Add(new CustomButton(kvp.Key, kvp.Value, rect));
            count++;
        }
        
        ExpandButtons();
        Reset();
    }

    void HandleButtonClick(ButtonFunction selectedFunction)
    {
        // Only run code if not animating and only when changing function
        if (!_AnimationRunning && _SelectedFunction != selectedFunction)
        {
            _SelectedFunction = selectedFunction;

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
                    ImageSliderGUI.Show();
                    break;
                case ButtonFunction.EmpireInfo:
                    InfoBoxGUI.Show();
                    break;
                case ButtonFunction.Panoramic:
                    PanoramicGUI.Show();
                    break;
                default:
                    break;
            }
        }
    }

    void ExpandButtons()
    {
        Action completeExpandButtons = () => {
            _AnimationRunning = false; };
    
        LeanTween.move(_TopBarBox, new Vector2(_TopBarBox.rect.x, _TopBarBox.rect.y + _BoxDeltaHeight), 1.0f).setEase(LeanTweenType.easeOutExpo).setDelay(0.5f);
        
        int count = 0;
        foreach (var button in _ButtonList)
        {
            float extraDelay = count * 0.1f;
            if (count == _NumButtons - 1)
            {
                // The last button must signal animation finished
                LeanTween.move(button.mRect, new Vector2(button.mRect.rect.x, button.mRect.rect.y + _ButtonDeltaHeight), 0.5f).setEase(LeanTweenType.easeOutBack).setDelay(0.5f + extraDelay)
                    .setOnComplete(completeExpandButtons);
            } else
            {
                LeanTween.move(button.mRect, new Vector2(button.mRect.rect.x, button.mRect.rect.y + _ButtonDeltaHeight), 0.5f).setEase(LeanTweenType.easeOutBack).setDelay(0.5f + extraDelay);
            }
            
            count++;
        }
    }
    
    void BounceButton(ButtonFunction selectedFunction)
    {
        LTRect buttonRect = _ButtonList.Where(b => b.mFunction == selectedFunction).Select(b => b.mRect).First();
        if (!LeanTween.isTweening(buttonRect))
        {
            LeanTween.move(buttonRect, new Vector2(buttonRect.x, buttonRect.y - _ButtonOffset), 0.1f).setEase(LeanTweenType.easeOutCubic)
                .setOnComplete(() => LeanTween.move(buttonRect, new Vector2(buttonRect.x, buttonRect.y + _ButtonOffset), 0.5f).setEase(LeanTweenType.easeOutBounce));
        }
    }

    #endregion

    #region General Methods
    
    void Reset()
    {		
        // Hide all elements here
        if (ImageSliderGUI.ShowImageSlider)
        {
            ImageSliderGUI.Hide();
        }
        
        if (InfoBoxGUI.ShowInfoBox)
        {
            InfoBoxGUI.Hide();
        }
        
        if (PanoramicGUI.ShowPanoramic)
        {
            PanoramicGUI.Hide();
        }
        
        VegasStrip.SetActive(false);
        EmpireStateBuilding.SetActive(false);
    }
    
    void SetupVegasModel()
    {
        VegasStrip.SetActive(true);
    }
    
    void SetupEmpireStateModel()
    {
        EmpireStateBuilding.SetActive(true);     
    }

    #endregion
}

class CustomButton
{
    public ButtonsGUI.ButtonFunction mFunction { get; set; }
    
    public string mText { get; set; }
    
    public LTRect mRect { get; set; }
    
    public bool mTweening = false;
    
    public CustomButton(ButtonsGUI.ButtonFunction function, string text, LTRect rect)
    {
        mFunction = function;
        mText = text;
        mRect = rect;
    }
}
