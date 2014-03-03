using UnityEngine;
using System.Collections;

public class SceneGUI_Handler : MonoBehaviour {

	public GUISkin GUISkin;

	//Panel 2 MenuRect2 = The panel 2. Box2 rect = the button
	private bool menu2RectState = false;
	private bool box2RectState = false;

	private Rect menu2Rect = new Rect(-1000,184,600,400); //MENU PANEL
	private Rect box2Rect = new Rect(235,280,120,50);  //MENU BUTTON
		
	//Panel 1 Menu1Rect = The panel. Boxrect = the button
	private bool menu1RectState = false;
	private bool boxRectState = false;

	//Panel
	private Rect menu1Rect = new Rect(212,184,600,400); //MENU PANEL
	private Rect initialPositionMenu = new Rect(212,184,600,400); //MENU PANEL
	private Rect activePositionMenu = new Rect(1200,184,600,400); //MENU PANEL

	//Button
	private Rect boxRect = new Rect(235,280,120,50);  //MENU BUTTON
	private Rect initialPosition = new Rect(235,280,120,30); //MENU BUTTON
	private Rect activePosition = new Rect(235,280,120,50);//MENU BUTTON


	void Start () {

	}

	//Panel 2
	void MoveMenu2 (Rect newCoordinates){
		menu2Rect=newCoordinates;
	}
	
	void MoveBox2 (Rect newCoordinates){
		box2Rect=newCoordinates;
	}
	
	//Panel 1
	void MoveMenu1 (Rect newCoordinates){
		menu1Rect=newCoordinates;
	}
	
	void MoveBox (Rect newCoordinates){
		boxRect=newCoordinates;
	}

	void OnGUI()
	{
		//GUISkin for Group
		GUI.skin = GUISkin;
		
		GUI.BeginGroup(menu2Rect, "","Window");
		if(GUI.Button(box2Rect,"","Button")) 
		{    
			if(menu2RectState)
			{
				iTween.ValueTo(gameObject,iTween.Hash("from",menu2Rect,"to",initialPositionMenu,"onupdate","MoveMenu2","easetype","easeinoutback")); 
			}
			else
			{
				iTween.ValueTo(gameObject,iTween.Hash("from",menu2Rect,"to",activePositionMenu,"onupdate","MoveMenu2","easetype","easeinoutback"));
			}

			menu2RectState = !menu2RectState;
			
			if(boxRectState)
			{
				iTween.ValueTo(gameObject,iTween.Hash("from",box2Rect,"to",initialPosition,"onupdate","Movebox2","easetype","easeinoutback")); 
			}
			else
			{
				iTween.ValueTo(gameObject,iTween.Hash("from",box2Rect,"to",activePosition,"onupdate","Movebox2","easetype","easeinoutback"));
			}

			box2RectState = !box2RectState;
		}
		
		GUI.EndGroup();
		
		GUI.BeginGroup(menu1Rect, "","Window");
		
		if(GUI.Button(boxRect,"","Button")) 
		{    
			if(menu1RectState)
			{
				iTween.ValueTo(gameObject,iTween.Hash("from",menu1Rect,"to",initialPositionMenu,"onupdate","MoveMenu1","easetype","easeinoutback")); 
			}
			else
			{
				iTween.ValueTo(gameObject,iTween.Hash("from",menu1Rect,"to",activePositionMenu,"onupdate","MoveMenu1","easetype","easeinoutback"));
			}

			menu1RectState = !menu1RectState;
			
			if(boxRectState)
			{
				iTween.ValueTo(gameObject,iTween.Hash("from",boxRect,"to",initialPosition,"onupdate","Movebox","easetype","easeinoutback")); 
			}
			else
			{
				iTween.ValueTo(gameObject,iTween.Hash("from",boxRect,"to",activePosition,"onupdate","Movebox","easetype","easeinoutback"));
			}

			boxRectState = !boxRectState;
		}
		
		GUI.EndGroup();
	}
}
