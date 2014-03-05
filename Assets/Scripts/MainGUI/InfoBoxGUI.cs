using UnityEngine;
using System.Collections;
using System;

public class InfoBoxGUI : MonoBehaviour
{
    public bool ShowInfoBox;
    public GUISkin GUISkin;
    
    // Screen params
    private int _W;
    private int _H;
    
    private int _Depth;
    
    // Info Box GUI props
    private float _InfoBoxWidth;
    private float _InfoBoxHeight;
    private LTRect _InfoBoxBox;
    private Vector2 _InfoBoxScrollPosition;
    private float _InfoBoxDeltaHeight;
    
    private bool _AnimationRunning = true;

    void Awake()
    {
        _Depth = 1;
        
        _W = Screen.width;
        _H = Screen.height;
        
        _InfoBoxWidth = _W * 0.75f;
        _InfoBoxHeight = _H * 0.75f;
    }

    void Start()
    {
        Init();
    }
	
    void OnGUI()
    {
        GUI.skin = GUISkin;
        GUI.depth = _Depth;
    
        if (ShowInfoBox)
        {
            // TODO: Change background of are to be something more awesome
            GUI.Box(_InfoBoxBox.rect, "");
            GUILayout.BeginArea(_InfoBoxBox.rect);
            _InfoBoxScrollPosition = GUILayout.BeginScrollView(_InfoBoxScrollPosition, GUILayout.Width(_InfoBoxWidth), GUILayout.Height(_InfoBoxHeight));
            
            GUILayout.Label("BLAH BLAHDFAGFH ADSLGHADGLHADGF BLAH BLAHDFAGFH ADSLGHADGLHADGF BLAH BLAHDFAGFH ADSLGHADGLHADGF BLAH BLAHDFAGFH ADSLGHADGLHADGF BLAH BLAHDFAGFH ADSLGHADGLHADGF BLAH BLAHDFAGFH ADSLGHADGLHADGF BLAH BLAHDFAGFH ADSLGHADGLHADGF BLAH BLAHDFAGFH ADSLGHADGLHADGF BLAH BLAHDFAGFH ADSLGHADGLHADGF BLAH BLAHDFAGFH ADSLGHADGLHADGF BLAH BLAHDFAGFH ADSLGHADGLHADGF BLAH BLAHDFAGFH ADSLGHADGLHADGF BLAH BLAHDFAGFH ADSLGHADGLHADGF BLAH BLAHDFAGFH ADSLGHADGLHADGF BLAH BLAHDFAGFH ADSLGHADGLHADGF BLAH BLAHDFAGFH ADSLGHADGLHADGF BLAH BLAHDFAGFH ADSLGHADGLHADGF BLAH BLAHDFAGFH ADSLGHADGLHADGF BLAH BLAHDFAGFH ADSLGHADGLHADGF BLAH BLAHDFAGFH ADSLGHADGLHADGF ");
            
            GUILayout.EndScrollView();
            GUILayout.EndArea();
        }
    }
    
    void Init()
    {
        float infoBoxLeft = (_W / 2) - (_InfoBoxWidth / 2);
        float infoBoxTop = (_H / 2) - (_InfoBoxHeight / 2);
        _InfoBoxDeltaHeight = _W - infoBoxTop;
        _InfoBoxBox = new LTRect(new Rect(infoBoxLeft, infoBoxTop + _InfoBoxDeltaHeight, _InfoBoxWidth, _InfoBoxHeight));
    }
    
    public void Show()
    {
        _Depth = 1;
        
        ShowInfoBox = _AnimationRunning = true;
        
        Action completeShowInfoBox = () => {
            _AnimationRunning = false; };
        
        if (!LeanTween.isTweening(_InfoBoxBox))
        {
            LeanTween.move(_InfoBoxBox, new Vector2(_InfoBoxBox.rect.x, _InfoBoxBox.y - _InfoBoxDeltaHeight), 0.6f).setEase(LeanTweenType.easeOutBack)
                .setOnComplete(completeShowInfoBox);
        }
    }
    
    public void Hide()
    {
        _Depth = 2;
        
        // TODO: Better logic around animation timings etc
        if (!LeanTween.isTweening(_InfoBoxBox))
        {
            Action completeHideInfoBox = () => 
            {
                _AnimationRunning = false;
                ShowInfoBox = false;
            };
            
            LeanTween.move(_InfoBoxBox, new Vector2(_InfoBoxBox.rect.x, _InfoBoxBox.y + _InfoBoxDeltaHeight), 0.4f).setEase(LeanTweenType.easeInCubic)
                .setOnComplete(completeHideInfoBox);
        }
    }
}
