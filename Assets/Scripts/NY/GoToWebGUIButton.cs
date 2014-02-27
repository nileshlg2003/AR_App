using UnityEngine;
using System.Collections;

public class GoToWebGUIButton : MonoBehaviour {
	
	public Texture2D buttonNormalTexture = null;
	public Texture2D buttonPressedTexture = null;
	
	void Start () {
		this.gameObject.transform.renderer.material.mainTexture = buttonNormalTexture;
	}
	
	void Update () {
	}
	
	public void selectGUIButtons() {
		this.gameObject.renderer.material.mainTexture = buttonPressedTexture;
		Application.OpenURL("http://www.esbnyc.com");
	}
	
	public void DeselectGUIButtons() {
		this.gameObject.renderer.material.mainTexture = buttonNormalTexture;
	}
}
