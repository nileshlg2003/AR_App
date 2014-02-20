using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GalleryGUIButton : MonoBehaviour {

	public Texture2D buttonNormalTexture = null;
	public Texture2D buttonPressedTexture = null;
	public ImageGridHandler imageSlider;

	private GameObject imageGrid;
	
	private void Start () {		
		this.gameObject.transform.renderer.material.mainTexture = buttonNormalTexture;

		this.imageGrid = GameObject.Find("ImageSlider");
		this.imageGrid.SetActive(false);
	}

	public void selectGUIButtons() {
		this.gameObject.renderer.material.mainTexture = buttonPressedTexture;
		
		this.imageGrid.SetActive(true);
		this.imageSlider.LoadImages();
		this.imageSlider.ShowImages();
	}
	
	public void DeselectGUIButtons() {
		this.gameObject.renderer.material.mainTexture = buttonNormalTexture;
		
		this.imageGrid.SetActive(false);
	}
}
