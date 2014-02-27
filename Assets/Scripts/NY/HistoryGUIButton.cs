using UnityEngine;
using System.Collections;

public class HistoryGUIButton : MonoBehaviour {

	public Texture2D buttonNormalTexture = null;
	public Texture2D buttonPressedTexture = null;

	private GameObject historyText;
	private GameObject backPlane;
	
	void Start () {
		this.gameObject.transform.renderer.material.mainTexture = buttonPressedTexture;

		this.historyText = GameObject.Find("HistoryText");
		this.backPlane = GameObject.Find("HistoryPlane");

//		this.historyText.SetActive(false);
//		this.backPlane.SetActive(false);
	}

	public void selectGUIButtons() {
		this.gameObject.renderer.material.mainTexture = buttonPressedTexture;

		this.historyText.SetActive(true);
		this.backPlane.SetActive(true);
	}

	public void DeselectGUIButtons() {
		this.gameObject.renderer.material.mainTexture = buttonNormalTexture;

		this.historyText.SetActive(false);
		this.backPlane.SetActive(false);
	}
}
