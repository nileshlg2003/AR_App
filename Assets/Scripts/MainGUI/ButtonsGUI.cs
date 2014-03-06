using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System;

public class ButtonsGUI : MonoBehaviour
{
    public enum ButtonFunction
    {
        VegasModel = 1,
        EmpireModel = 2,
        EmpireImageSlider = 3,
        EmpireInfo = 4,
        Panoramic = 5
    }

    #region Fields
    
    // Constants
    private const float BUTTON_WIDTH_SCALE = 0.15f;
    private const float BUTTON_HEIGHT_SCALE = 0.03f;

    // Public properties
    public GUISkin HeaderSkin;
    public GameObject EmpireStateBuilding;
    
    public VegasStrip_Handler VegasStripHandler;
    
    public InfoBoxGUI InfoBoxGUI;
    public ImageSliderGUI ImageSliderGUI;
    public PanoramicGUI PanoramicGUI;
    
    public Texture BoxTexture;
    public Texture ButtonRightBorderTexture;
    public Texture ButtonNoRightBorderTexture;
    public Texture ButtonActiveTexture;

    // Screen params
    private int _W;
    private int _H;
    
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
    private Vector2 _BoxStart;
    private Vector2 _BoxEnd;
    
    // GUI state props
    private ButtonFunction _SelectedFunction;
    private bool _AnimationRunning = true;
    
    private GUIStyle _TempButtonStyle;
    
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
        
        _ButtonWidth = _W * BUTTON_WIDTH_SCALE;
        _ButtonHeight = _H * BUTTON_HEIGHT_SCALE;
        _TopOffset = _H * 0.01f;
        _ButtonOffset = 0.0f;//_H * 0.005f;
        _BounceOffset = _H * 0.005f;
        
        //int padding = Convert.ToInt32(Math.Round(_H * 0.005f, MidpointRounding.AwayFromZero));
        //_BoxPadding = new RectOffset(padding, padding, padding, padding);
        _BoxPadding = new RectOffset(0, 0, 0, 0);
        
        _NumButtons = _ButtonFunctions.Count;
        
        _TempButtonStyle = HeaderSkin.button;
    }

    void Start()
    {
        Init();
    }

    void OnGUI()
    {
        GUI.skin = HeaderSkin;

        GUI.Box(_TopBarBox.rect, "");
        foreach (var button in _ButtonList)
        {
            // Check if button active, last or normal and assign correct texture
            Debug.Log(button.mText + " button isActive = " + button.mActive);
            _TempButtonStyle.normal.background = (Texture2D)(button.mActive ? ButtonActiveTexture : button.mTexture);
            if (GUI.Button(button.mRect.rect, button.mText, _TempButtonStyle))
            {
                HandleButtonClick(button.mFunction);
            }
        }
    }

    #endregion

    #region Button GUI Methods
    
    void Init()
    {
        float midPoint = _W / 2;
        float betweenButtons = (_NumButtons - 1) * _ButtonOffset;
        
        float boxLeftRightPadding = _BoxPadding.left + _BoxPadding.right;
        float boxTopBottomPadding = _BoxPadding.top + _BoxPadding.top;
        
        float totalButtonWidth = _NumButtons * _ButtonWidth;
        float totalBoxWidth = totalButtonWidth + boxLeftRightPadding + betweenButtons;
        float totalBoxHeight = _ButtonHeight + boxTopBottomPadding;
        
        float boxLeftPos = midPoint - (totalBoxWidth / 2);
        float boxTopPos = _TopOffset;
        
        _BoxStart = new Vector2(boxLeftPos, -1 * totalBoxHeight);
        _BoxEnd = new Vector2(boxLeftPos, boxTopPos);
        _TopBarBox = new LTRect(new Rect(_BoxStart.x, _BoxStart.y, totalBoxWidth, totalBoxHeight));
        
        float buttonTop = boxTopPos + _BoxPadding.top;
        
        int count = 0;
        foreach (var kvp in _ButtonFunctions)
        {
            float buttonLeft = boxLeftPos + _BoxPadding.left + count * (_ButtonWidth + _ButtonOffset);
            
            Vector2 start = new Vector2(buttonLeft, -1 * _ButtonHeight);
            Vector2 end = new Vector2(buttonLeft, buttonTop);
            Vector2 bounceEnd = new Vector2(buttonLeft, buttonTop - _BounceOffset);
            
            LTRect rect = new LTRect(new Rect(start.x, start.y, _ButtonWidth, _ButtonHeight));
            Texture texture = count == _NumButtons - 1 ? ButtonNoRightBorderTexture : ButtonRightBorderTexture;
            _ButtonList.Add(new CustomButton(kvp.Key, kvp.Value, rect, start, end, bounceEnd, texture));
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
                    ActivateButton(ButtonFunction.VegasModel);
                    SetupVegasModel();
                    break;
                case ButtonFunction.EmpireModel:
                    ActivateButton(ButtonFunction.EmpireModel);
                    SetupEmpireStateModel();
                    break;
                case ButtonFunction.EmpireImageSlider:
                    ActivateButton(ButtonFunction.EmpireImageSlider);
                    ImageSliderGUI.Show();
                    break;
                case ButtonFunction.EmpireInfo:
                    ActivateButton(ButtonFunction.EmpireInfo);
                    InfoBoxGUI.Show();
                    break;
                case ButtonFunction.Panoramic:
                    ActivateButton(ButtonFunction.Panoramic);
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
    
        LeanTween.move(_TopBarBox, _BoxEnd, 1.0f).setEase(LeanTweenType.easeOutExpo).setDelay(0.5f);
        
        int count = 0;
        foreach (var button in _ButtonList)
        {
            float extraDelay = count * 0.1f;
            if (count == _NumButtons - 1)
            {
                // The last button must signal animation finished
                LeanTween.move(button.mRect, button.mEnd, 0.5f).setEase(LeanTweenType.easeOutBack).setDelay(0.5f + extraDelay)
                    .setOnComplete(completeExpandButtons);
            } else
            {
                LeanTween.move(button.mRect, button.mEnd, 0.5f).setEase(LeanTweenType.easeOutBack).setDelay(0.5f + extraDelay);
            }
            
            count++;
        }
    }
    
    void BounceButton(ButtonFunction selectedFunction)
    {
        CustomButton button = _ButtonList.Where(b => b.mFunction == selectedFunction).First();
        if (!LeanTween.isTweening(button.mRect))
        {
            LeanTween.move(button.mRect, button.mBounceEnd, 0.05f).setEase(LeanTweenType.easeOutCubic)
                .setOnComplete(() => LeanTween.move(button.mRect, button.mEnd, 0.07f).setEase(LeanTweenType.easeOutBounce));
        }
    }

    void ActivateButton(ButtonFunction function)
    {
        foreach (var button in _ButtonList)
        {
            button.mActive = button.mFunction == function;
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
        
        VegasStripHandler.Hide();
        EmpireStateBuilding.SetActive(false);
    }
    
    void SetupVegasModel()
    {
        VegasStripHandler.Show();
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
    
    public Vector2 mStart { get; set; }
    
    public Vector2 mEnd { get; set; }
    
    public Vector2 mBounceEnd { get; set; }
    
    public Texture mTexture { get; set; }
    
    public bool mActive = false;
    
    public CustomButton(ButtonsGUI.ButtonFunction function, string text, LTRect rect, Vector2 start, Vector2 end, Vector2 bounceEnd, Texture texture)
    {
        mFunction = function;
        mText = text;
        mRect = rect;
        mStart = start;
        mEnd = end;
        mBounceEnd = bounceEnd;
        mTexture = texture;
    }
}
