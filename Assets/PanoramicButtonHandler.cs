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
	
	void OnGUI() {
		if (this.showPanoramic) {
			var buttonX = (Screen.width - 100) / 2.0f;
			var buttonY = (Screen.height - 50) / 2.0f;

			if (GUI.Button (new Rect (buttonX, buttonY, 100, 50), "Back")) {
				this.Hide ();
			}
		}
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

	public void Hide() {
		this.showPanoramic = false;
		
		this.panoCamera.enabled = false;
		this.ARCamera.enabled = true;
	}
}
