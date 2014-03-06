using UnityEngine;
using System.Collections;
using System;
using System.Text;

public class InfoBoxGUI : MonoBehaviour
{
    public bool ShowInfoBox;
    public GUISkin InfoBoxSkin;
    
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
    
    private string _TextToDisplay;

    void Awake()
    {
        _Depth = 1;
        
        _W = Screen.width;
        _H = Screen.height;
        
        _InfoBoxWidth = _W * 0.75f;
        _InfoBoxHeight = _H * 0.75f;
        
        _TextToDisplay = CreateTextToDisplay();
    }

    void Start()
    {
        Init();
    }
	
    void OnGUI()
    {
        GUI.skin = InfoBoxSkin;
        GUI.depth = _Depth;
    
        if (ShowInfoBox)
        {
            // TODO: Change background of are to be something more awesome
            GUI.Box(_InfoBoxBox.rect, "");
            GUILayout.BeginArea(_InfoBoxBox.rect);
            _InfoBoxScrollPosition = GUILayout.BeginScrollView(_InfoBoxScrollPosition, GUILayout.Width(_InfoBoxWidth), GUILayout.Height(_InfoBoxHeight));
            
            GUILayout.Label(_TextToDisplay);
            
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

    string CreateTextToDisplay()
    {
        StringBuilder b = new StringBuilder();
        
        string header = string.Format("<size={0}><b>Historical Timeline</b></size>", 50);
        string body = @"<b>1799</b>: The City of New York sold a virgin tract (now bounded by Broadway and Sixth Avenue on the west, Madison Avenue on the east, 33rd Street on the south and 36th Street on the north) to John Thompson for $2,600. He farmed it.
<b>1825</b>: Thompson sold the farm to Charles Lawton for $10,000.
<b>1827</b>: William Backhouse Astor, the second son of John Jacob Astor, bought the farm for $20,500 as an investment.
<b>1859</b>: John Jacob Astor, Jr. erected a mansion on the northwest corner of 33rd Street and Fifth Avenue.
<b>1862</b>: John Jacob, Jr.'s younger brother, William Backhouse Astor, built his mansion next door at the southwest corner of 34th Street and Fifth Avenue.
<b>1893</b>: William Waldorf Astor, son of John Jacob Astor, Jr., razed his inherited mansion and erected the Waldorf Hotel on the corner of Fifth Avenue and 33rd Street.
<b>1897</b>: Mrs. William Backhouse Astor, sister-in-law of John Jacob, Jr., allowed her mansion at 34th Street and Fifth Avenue to be razed, and the Astoria Hotel was erected on the site. The new complex was known as the Waldorf-Astoria Hotel.
<b>1928</b>: The Waldorf-Astoria Hotel was sold to Bethlehem Engineering Corporation for an estimated $20 million.";
        
        b.AppendLine(header);
        b.AppendLine(body);
        
        return b.ToString();
    }
}
