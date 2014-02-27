using UnityEngine;
using System.Collections;

public class PanoramicButtonHandler : MonoBehaviour {

	public Camera panoCamera;
	public Camera ARCamera;
	public Material panoramicMaterial;

	public Texture2D buttonNormalTexture = null;
	public Texture2D buttonPressedTexture = null;

	public bool showPanoramic = false;

	void Start () {
		this.panoCamera.enabled = showPanoramic;
	}
	
	void Update () {
		
	}

	public void selectGUIButtons() {
		this.gameObject.renderer.material.mainTexture = buttonPressedTexture;
	}
	
	public void DeselectGUIButtons() {
		this.gameObject.renderer.material.mainTexture = buttonNormalTexture;
	}

	public void Show() {
		this.showPanoramic = true;

		RenderSettings.skybox = panoramicMaterial;

		this.panoCamera.enabled = true;
		this.ARCamera.enabled = false;
	}
}
