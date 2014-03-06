﻿using UnityEngine;
using System.Collections;
using System;

public class InfoBoxGUI : MonoBehaviour
{
    public bool ShowInfoBox;
    public GUISkin GUISkin;
    public Texture OBLogo;

    // Screen params
    private int _W;
    private int _H;
    
    private int _Depth;

    // Info Box GUI props
    private float _InfoBoxWidth;
    private float _InfoBoxHeight;
    private LTRect _InfoBoxBox;
    private Vector2 _InfoBoxScrollPosition;
    
    private Vector2 _InfoBoxStart;
    private Vector2 _InfoBoxEnd;

    // GUI Styles for Text
    private GUIStyle _NormalTextStyle;
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

            GUILayout.Label(OBLogo);
            GUILayout.Label("\nWe passionately believe in constantly striving for doing things a better way.\nWe do this by challenging firmly held beliefs and conventions in ourselves and everyone around us every day. \n\n", _LargeTextStyle);
            GUILayout.Label("What we do \n\n", _BoldHeaderStyle);
            GUILayout.Label("Real Estate software solutions, soup to nuts. Identification of opportunities to increase revenue, decrease cost and reduce risk \nthrough to supporting the resulting systems, processes and people in your organization.\n\n"
                            + "Our experienced team is ready to bring their knowledge to your organization, to help you run a successful project, \nfrom start to finish. We thrive on getting involved from the initial business case, working with you and the business to develop the \napproach and objectives for a project. \n\n"
                            + "We understand real estate, we bring industry knowledge, systems thinking, and innovative technological approaches to the table. \nWe speak to the technical detail required by your IT Department, and just as easily understand the business challenges and \nlanguage of the rest of the people in your organization. Bridging these worlds, is what Open Box is about.  \nDoing so with the great technology in a fast, innovative and efficient way. We understand your business, your pain points,  \nand will be your partner through the \n\n",_NormalTextStyle);
            GUILayout.Label("We Know Commercial Real Estate: \n\n", _BoldHeaderStyle);
            GUILayout.Label("For the past 12 years we have been working with some of the leading names in the global commercial real estate industry. \nAs a result we have been privileged \nto have been involved in defining industry best practice for not only the use of technology, but also business processes.\n"
                            + "We understand the difference between the different sectors, whether your commercial property is retail, office, residential or \nindustrial. We understand the industry cycles, `as well as the past, present and likely future challenges and opportunities \nrelated to each of these."
                            + "With clients around the world we have a unique global perspective. \n"
                            + "We speak your business language - one that is highly specific to the real estate industry. \n"
                            + "All of this means that we immediately understand where you are today, and can partner in assisting you to get to where you  \nwant to be in the future. \n\n",_NormalTextStyle);
            GUILayout.Label("We believe that if you get great people together in the right environment and set them a challenge, \ninspirational things will result.  \n\n", _LargeTextStyle);
            GUILayout.Label("Contact Us  \n\n", _BoldHeaderStyle);

            GUILayout.Label("info@openboxsoftware.com | www.openboxsoftware.com | +27 32 713 9300",_NormalTextStyle);
            GUILayout.BeginHorizontal();

            GUILayout.Button("info@openboxsoftware.com", GUILayout.Width(200), GUILayout.Height(30));
            GUILayout.Button("www.openboxsoftware.com", GUILayout.Width(200), GUILayout.Height(30));
            //GUILayout.Label(" | +27 32 713 9300", _NormalTextStyle);

            GUILayout.EndHorizontal();

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
        _NormalTextStyle = new GUIStyle();
        _NormalTextStyle.fontSize = 20;

        _BoldHeaderStyle = new GUIStyle();
        _BoldHeaderStyle.fontSize = 21;
        _BoldHeaderStyle.fontStyle = FontStyle.Bold;

        _LargeTextStyle = new GUIStyle();
        _LargeTextStyle.fontSize = 22;
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
        
        // TODO: Better logic around animation timings etc
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
