using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class ImageSliderGUI : MonoBehaviour
{
    public GUISkin GUISkin;
    public Texture SliderBackground;
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
    private float _SliderButtonOffset;
    private LTRect _ImageSliderBox;
    private LTRect _PrevButton;
    private LTRect _NextButton;
    private float _SliderDeltaHeight;
    private float _SliderPrevButtonDeltaWidth;
    private float _SliderNextButtonDeltaWidth;
    
    private int _SelectedImageIndex;
    
    private IList<Texture> _Images;
    
    private bool _AnimationRunning = true;
    
    void Awake()
    {
        _Depth = 1;
        
        _W = Screen.width;
        _H = Screen.height;
        
        // Percentage of screen
        _ButtonWidth = _W * ButtonsGUI.BUTTON_WIDTH_SCALE;
        _ButtonHeight = _H * ButtonsGUI.BUTTON_HEIGHT_SCALE;
        
        // TODO: Hard code or variable?
        float temp = _W < _H ? _W : _H;
        _ImageSliderWidth = temp / 2;
        _ImageSliderHeight = temp / 2;
        _SliderButtonOffset = _H * 0.0375f;
        
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
        GUI.skin = GUISkin;
        GUI.depth = _Depth;
        
        if (ShowImageSlider)
        {
            GUI.DrawTexture(_ImageSliderBox.rect, SliderBackground);
            GUI.DrawTexture(_ImageSliderBox.rect, _Images [_SelectedImageIndex]);
            if (GUI.Button(_PrevButton.rect, "Prev"))
            {
                if (_SelectedImageIndex > 0)
                {
                    _SelectedImageIndex--;
                } else
                {
                    _SelectedImageIndex = _Images.Count - 1;
                }
            }
            if (GUI.Button(_NextButton.rect, "Next"))
            {
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
        _SliderDeltaHeight = _W - sliderTop;
        _ImageSliderBox = new LTRect(new Rect(sliderLeft, sliderTop + _SliderDeltaHeight, _ImageSliderWidth, _ImageSliderHeight));
        
        float buttonTop = _H / 2 + _ButtonHeight / 2;
        float prevButtonLeft = sliderLeft - (_SliderButtonOffset + _ButtonWidth);
        float nextButtonLeft = sliderLeft + _ImageSliderWidth + _SliderButtonOffset;
        _SliderPrevButtonDeltaWidth = prevButtonLeft + _ButtonWidth;
        _SliderNextButtonDeltaWidth = _W - nextButtonLeft;
        _PrevButton = new LTRect(new Rect(prevButtonLeft - _SliderPrevButtonDeltaWidth, buttonTop, _ButtonWidth, _ButtonHeight));
        _NextButton = new LTRect(new Rect(nextButtonLeft + _SliderNextButtonDeltaWidth, buttonTop, _ButtonWidth, _ButtonHeight));
        
    }
    
    public void Show()
    {
        _Depth = 1;
        
        ShowImageSlider = _AnimationRunning = true;
        
        Action completeShowSlider = () => {
            _AnimationRunning = false; };
        
        if (!LeanTween.isTweening(_ImageSliderBox) && !LeanTween.isTweening(_PrevButton) && !LeanTween.isTweening(_NextButton))
        {
            LeanTween.move(_ImageSliderBox, new Vector2(_ImageSliderBox.rect.x, _ImageSliderBox.y - _SliderDeltaHeight), 0.6f).setEase(LeanTweenType.easeOutBack)
                .setOnComplete(completeShowSlider);
            
            LeanTween.move(_PrevButton, new Vector2(_PrevButton.rect.x + _SliderPrevButtonDeltaWidth, _PrevButton.rect.y), 0.4f).setEase(LeanTweenType.easeOutBack).setDelay(0.2f);
            LeanTween.move(_NextButton, new Vector2(_NextButton.rect.x - _SliderNextButtonDeltaWidth, _NextButton.rect.y), 0.4f).setEase(LeanTweenType.easeOutBack).setDelay(0.2f);
        }
    }
    
    public void Hide()
    {
        _Depth = 2;
        
        // TODO: Better logic around animation timings etc
        if (!LeanTween.isTweening(_ImageSliderBox) && !LeanTween.isTweening(_PrevButton) && !LeanTween.isTweening(_NextButton))
        {
            Action completeHideSlider = () => 
            {
                _AnimationRunning = false;
                ShowImageSlider = false;
            };
            
            LeanTween.move(_ImageSliderBox, new Vector2(_ImageSliderBox.rect.x, _ImageSliderBox.y + _SliderDeltaHeight), 0.4f).setEase(LeanTweenType.easeInCubic)
                .setOnComplete(completeHideSlider);
            
            LeanTween.move(_PrevButton, new Vector2(_PrevButton.rect.x - _SliderPrevButtonDeltaWidth, _PrevButton.rect.y), 0.3f).setEase(LeanTweenType.easeInCubic).setDelay(0.1f);
            LeanTween.move(_NextButton, new Vector2(_NextButton.rect.x + _SliderNextButtonDeltaWidth, _NextButton.rect.y), 0.3f).setEase(LeanTweenType.easeInCubic).setDelay(0.1f);
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
