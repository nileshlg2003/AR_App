using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class ImageSliderGUI : MonoBehaviour
{
    private enum ButtonType
    {
        Previous = 0,
        Next = 1
    }

    private const float BUTTON_WIDTH_SCALE = 0.05f;
    
    public GUISkin ImageSliderSkin;
    public Texture SliderBackground;
    public Texture PrevTexture;
    public Texture NextTexture;
    public Texture Image1;
    public Texture Image2;
    public Texture Image3;
    public bool ShowImageSlider;
    
    // Screen params
    private int _W;
    private int _H;
    
    private int _Depth;
    
    private float _ButtonWidth;
    private float _ButtonHeight;
    
    // Image Slider GUI props
    private float _ImageSliderWidth;
    private float _ImageSliderHeight;
    private float _SliderButtonOffsetBetween;
    private float _SliderButtonOffsetTop;
    private LTRect _ImageSliderBox;
    private LTRect _PrevButton;
    private LTRect _NextButton;
    private float _ButtonBounceOffset;
    
    private Vector2 _SliderStart;
    private Vector2 _SliderEnd;
    private Vector2 _PrevButtonStart;
    private Vector2 _PrevButtonEnd;
    private Vector2 _PrevButtonBounceEnd;
    private Vector2 _NextButtonStart;
    private Vector2 _NextButtonEnd;
    private Vector2 _NextButtonBounceEnd;
    
    private RectOffset _BackgroundPadding;
    
    private int _SelectedImageIndex;
    
    private IList<Texture> _Images;
    
    private bool _AnimationRunning = true;
    
    void Awake()
    {
        _Depth = 1;
        
        _W = Screen.width;
        _H = Screen.height;
        
        int padding = Convert.ToInt32(Math.Round(_H * 0.01f, MidpointRounding.AwayFromZero));
        _BackgroundPadding = new RectOffset(padding, padding, padding, padding);
        
        // Percentage of screen (square buttons)
        _ButtonWidth = _W * BUTTON_WIDTH_SCALE;
        _ButtonHeight = _ButtonWidth;
        
        // TODO: Hard code or variable?
        float temp = _W < _H ? _W : _H;
        _ImageSliderWidth = temp * 0.8f;
        _ImageSliderHeight = temp * 0.7f;
        _SliderButtonOffsetTop = _H * 0.0375f;
        _SliderButtonOffsetBetween = _W * 0.2f;
        
        _ButtonBounceOffset = _W * 0.01f;
        
        _SelectedImageIndex = 0;
        
        ShowImageSlider = false;
    }
    
    // Use this for initialization
    void Start()
    {
        Init();
    }
	
    // Update is called once per frame
    void OnGUI()
    {
        GUI.skin = ImageSliderSkin;
        GUI.depth = _Depth;
        
        if (ShowImageSlider)
        {
            GUI.DrawTexture(_BackgroundPadding.Add(_ImageSliderBox.rect), SliderBackground);
            GUI.DrawTexture(_ImageSliderBox.rect, _Images [_SelectedImageIndex]);
            if (GUI.Button(_PrevButton.rect, PrevTexture))
            {
                BounceButton(ButtonType.Previous);
                if (_SelectedImageIndex > 0)
                {
                    _SelectedImageIndex--;
                } else
                {
                    _SelectedImageIndex = _Images.Count - 1;
                }
            }
            if (GUI.Button(_NextButton.rect, NextTexture))
            {
                BounceButton(ButtonType.Next);
                if (_SelectedImageIndex < _Images.Count - 1)
                {
                    _SelectedImageIndex++;
                } else
                {
                    _SelectedImageIndex = 0;
                }
            }
        }
    }
    
    void Init()
    {
        // TODO: This can be refactored when we figure out how to load from a zip file or folder etc.
        _Images = new List<Texture>()
        {
            Image1,
            Image2,
            Image3
        };
        
        float sliderLeft = (_W / 2) - (_ImageSliderWidth / 2);
        float sliderTop = (_H / 2) - (_ImageSliderHeight / 2);
        
        _SliderStart = new Vector2(sliderLeft, _H);
        _SliderEnd = new Vector2(sliderLeft, sliderTop);
        _ImageSliderBox = new LTRect(new Rect(_SliderStart.x, _SliderStart.y, _ImageSliderWidth, _ImageSliderHeight));
        
        float buttonTop = sliderTop + _ImageSliderHeight + _SliderButtonOffsetTop;
        float prevButtonLeft = sliderLeft - _BackgroundPadding.left;
        float nextButtonLeft = sliderLeft + _ImageSliderWidth + _BackgroundPadding.right - _ButtonWidth;
        
        _PrevButtonStart = new Vector2(-1 * _ButtonWidth, buttonTop);
        _PrevButtonEnd = new Vector2(prevButtonLeft, buttonTop);
        _PrevButtonBounceEnd = new Vector2(_PrevButtonEnd.x - _ButtonBounceOffset, _PrevButtonEnd.y);
        _NextButtonStart = new Vector2(_W, buttonTop);
        _NextButtonEnd = new Vector2(nextButtonLeft, buttonTop);
        _NextButtonBounceEnd = new Vector2(_NextButtonEnd.x + _ButtonBounceOffset, _NextButtonEnd.y);
        
        _PrevButton = new LTRect(new Rect(_PrevButtonStart.x, _PrevButtonStart.y, _ButtonWidth, _ButtonHeight));
        _NextButton = new LTRect(new Rect(_NextButtonStart.x, _NextButtonStart.y, _ButtonWidth, _ButtonHeight));
    }
    
    public void Show()
    {
        _Depth = 1;
        
        ShowImageSlider = _AnimationRunning = true;
        
        Action completeShowSlider = () => {
            _AnimationRunning = false; };
            
        if (!LeanTween.isTweening(_ImageSliderBox) && !LeanTween.isTweening(_PrevButton) && !LeanTween.isTweening(_NextButton))
        {
            LeanTween.move(_ImageSliderBox, _SliderEnd, 0.4f).setEase(LeanTweenType.easeOutCirc);
            
            LeanTween.move(_PrevButton, _PrevButtonEnd, 0.4f).setEase(LeanTweenType.easeOutCirc);
            LeanTween.move(_NextButton, _NextButtonEnd, 0.4f).setEase(LeanTweenType.easeOutCirc)
                .setOnComplete(completeShowSlider);
        }
        
    }
    
    public void Hide()
    {
        _Depth = 2;
        
        // TODO: Better logic around animation timings etc
//        if (!LeanTween.isTweening(_ImageSliderBox) && !LeanTween.isTweening(_PrevButton) && !LeanTween.isTweening(_NextButton))
//        {
//            Action completeHideSlider = () => 
//            {
//                _AnimationRunning = false;
//                ShowImageSlider = false;
//            };
//            
//            LeanTween.move(_ImageSliderBox, new Vector2(_ImageSliderBox.rect.x, _ImageSliderBox.y + _SliderDeltaHeight), 0.3f).setEase(LeanTweenType.easeInCubic)
//                .setOnComplete(completeHideSlider);
//            
//            LeanTween.move(_PrevButton, new Vector2(_PrevButton.rect.x - _SliderPrevButtonDeltaWidth, _PrevButton.rect.y), 0.2f).setEase(LeanTweenType.easeInCubic);
//            LeanTween.move(_NextButton, new Vector2(_NextButton.rect.x + _SliderNextButtonDeltaWidth, _NextButton.rect.y), 0.2f).setEase(LeanTweenType.easeInCubic);
//        }

        _ImageSliderBox.y = _SliderStart.y;
        _PrevButton.x = _PrevButtonStart.x;
        _NextButton.x = _NextButtonStart.x;
        ShowImageSlider = false;
    }
    
    void BounceButton(ButtonType type)
    {
        LTRect buttonRect;
        Vector2 endPosition;
        
        // This is start of bounce, i.e. end of button
        Vector2 startPosition;
        float offset;
        switch (type)
        {
            case ButtonType.Previous:
                buttonRect = _PrevButton;
                endPosition = _PrevButtonBounceEnd;
                startPosition = _PrevButtonEnd;
                break;
            case ButtonType.Next:
                buttonRect = _NextButton;
                endPosition = _NextButtonBounceEnd;
                startPosition = _NextButtonEnd;
                break;
            default:
                buttonRect = _NextButton;
                endPosition = _NextButtonBounceEnd;
                startPosition = _NextButtonEnd;
                break;
        }
    
        if (!LeanTween.isTweening(buttonRect))
        {
            LeanTween.move(buttonRect, endPosition, 0.1f).setEase(LeanTweenType.easeOutCubic)
                .setOnComplete(() => LeanTween.move(buttonRect, startPosition, 0.2f).setEase(LeanTweenType.easeOutBack));
        }
    }
    
    //      IEnumerator LoadFiles ()
    //      {
    //              Texture2D tex;
    //
    //              string filePath = Path.Combine (Application.streamingAssetsPath, "Images.zip");
    //              
    //              if (filePath.Contains ("://")) {
    //          WWW www = new WWW(filePath);
    //          yield return www;
    //          tex = www.texture;
    //              } else {
    //          result = System.IO.File.(filePath);
    //      Resources.LoadAssetAtPath(filePath)
    //              }
    //              yield return new WaitForSeconds (2.0f);
    //              Debug.Log ("Logan - After yield");
    //      }
}
