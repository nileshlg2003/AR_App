using UnityEngine;
using System.Collections;
using System;

public class InfoBoxGUI : MonoBehaviour
{
    public bool ShowInfoBox;
    public GUISkin GUISkin;
    public Texture OBLogo;
    public Texture ButtonTexture;
    public Texture ButtonActiveTexture;

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

    private Vector2 _InfoBoxStart;
    private Vector2 _InfoBoxEnd;
    
    // GUI Styles for Text
    private GUIStyle _BoldHeaderStyle;
    private GUIStyle _LargeTextStyle;

    private bool _AnimationRunning = true;

    void Awake()
    {
        _Depth = 1;
        
        _W = Screen.width;
        _H = Screen.height;
        
        _InfoBoxWidth = _W * 0.8f;
        _InfoBoxHeight = _H * 0.8f;
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
            GUILayout.BeginArea(_InfoBoxBox.rect, GUI.skin.box);
            _InfoBoxScrollPosition = GUILayout.BeginScrollView(_InfoBoxScrollPosition, GUILayout.Width(_InfoBoxWidth), GUILayout.Height(_InfoBoxHeight));
            // GUILayout.BeginScrollView(_InfoBoxScrollPosition);

            GUILayout.Label(OBLogo);
            GUILayout.Label("We passionately believe in constantly striving for doing things a better way.\nWe do this by challenging firmly held beliefs and conventions in ourselves and everyone around us every day. \n\n", _LargeTextStyle);
            GUILayout.Label("What we do \n\n", _BoldHeaderStyle);
            GUILayout.Label("Real Estate software solutions, soup to nuts. Identification of opportunities to increase revenue, decrease cost and reduce risk through to supporting the resulting systems, processes and people in your organization.\n\n"
                + "Our experienced team is ready to bring their knowledge to your organization, to help you run a successful project, from start to finish. We thrive on getting involved from the initial business case, working with you and the business to develop the approach and objectives for a project. \n\n"
                + "We understand real estate, we bring industry knowledge, systems thinking, and innovative technological approaches to the table. We speak to the technical detail required by your IT Department, and just as easily understand the business challenges and language of the rest of the people in your organization. Bridging these worlds, is what Open Box is about. Doing so with the great technology in a fast, innovative and efficient way. \nWe understand your business, your pain points, and will be your partner through the \n\n");
            GUILayout.Label("We Know Commercial Real Estate: \n\n", _BoldHeaderStyle);
            GUILayout.Label("For the past 12 years we have been working with some of the leading names in the global commercial real estate industry. As a result we have been privileged to have been involved in defining industry best practice for not only the use of technology, but also business processes.\n"
                + "We understand the difference between the different sectors, whether your commercial property is retail, office, residential or industrial. We understand the industry cycles, as well as the past, present and likely future challenges and opportunities related to each of these. \n"
                + "With clients around the world we have a unique global perspective. \n"
                + "We speak your business language - one that is highly specific to the real estate industry. \n"
                + "All of this means that we immediately understand where you are today, and can partner in assisting you to get to where you want to be in the future. \n\n");
            GUILayout.Label("We believe that if you get great people together in the right environment and set them a challenge, \ninspirational things will result.  \n\n", _LargeTextStyle);
            GUILayout.Label("Contact Us  \n\n", _BoldHeaderStyle);
            GUILayout.Label("info@openboxsoftware.com | www.openboxsoftware.com | +27 32 713 9300");
            
            GUILayout.EndScrollView();
            GUILayout.EndArea();
        }
    }
    
    void Init()
    {
        float infoBoxLeft = (_W / 2) - (_InfoBoxWidth / 2);
        float infoBoxTop = (_H / 2) - (_InfoBoxHeight / 2);
        
        _InfoBoxStart = new Vector2(infoBoxLeft, _H);
        _InfoBoxEnd = new Vector2(infoBoxLeft, infoBoxTop);
        
        _InfoBoxBox = new LTRect(new Rect(_InfoBoxStart.x, _InfoBoxStart.y, _InfoBoxWidth, _InfoBoxHeight));

        // Set GUIStyles
        _BoldHeaderStyle = new GUIStyle();
        _BoldHeaderStyle.fontSize = 12;
        _BoldHeaderStyle.fontStyle = FontStyle.Bold;

        _LargeTextStyle = new GUIStyle();
        _LargeTextStyle.fontSize = 16;
        _LargeTextStyle.fontStyle = FontStyle.Bold;
    }
    
    public void Show()
    {
        _Depth = 1;
        
        ShowInfoBox = _AnimationRunning = true;
        
        Action completeShowInfoBox = () => {
            _AnimationRunning = false; };
        
        if (!LeanTween.isTweening(_InfoBoxBox))
        {
            LeanTween.move(_InfoBoxBox, _InfoBoxEnd, 0.35f).setEase(LeanTweenType.easeOutCirc)
                .setOnComplete(completeShowInfoBox);
        }
    }
    
    public void Hide()
    {
        _Depth = 2;
        
//        // TODO: Better logic around animation timings etc
//        if (!LeanTween.isTweening(_InfoBoxBox))
//        {
//            Action completeHideInfoBox = () => 
//            {
//                _AnimationRunning = false;
//                ShowInfoBox = false;
//            };
//            
//            LeanTween.move(_InfoBoxBox, _InfoBoxStart, 0.4f).setEase(LeanTweenType.easeInCubic)
//                .setOnComplete(completeHideInfoBox);
//        }
        
        _InfoBoxBox.y = _InfoBoxStart.y;
        ShowInfoBox = false;
    }
}
