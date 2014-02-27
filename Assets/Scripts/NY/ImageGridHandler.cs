using UnityEngine;
using System.Collections;

public class ImageGridHandler : MonoBehaviour {

	public string _imagePath = "Assets/Textures/NY_Images";	
	public Texture2D image1;
	public Texture2D image2;
	public Texture2D image3;
//	public Texture2D image4;
//	public Texture2D image5;
//	public Texture2D image6;
//	public Texture2D image7;
//	public Texture2D image8;
//	public Texture2D image9;
//	public Texture2D image10;
	public PrevButtonHandler prevButton;
	public NextButtonHandler nextButton;

	private Texture2D[] imagesArray;
	private int currentImageIndex = -1;
	private GameObject imagePanel;

	private void Start () {
		this.imagesArray = new Texture2D[3];
		this.imagePanel = GameObject.Find("ImageViewer");
	}

	public void NextImage() {
		this.nextButton.playAnimation = true; // Automatically stops after first loop

		if (this.currentImageIndex < this.imagesArray.Length) {
			this.currentImageIndex++;
		} else {
			this.currentImageIndex = 0;
		}

		this.imagePanel.transform.renderer.material.mainTexture = this.imagesArray[this.currentImageIndex] as Texture2D;
	}

	public void PrevImage() {
		this.prevButton.playAnimation = true; // Automatically stops after first loop

		if (this.currentImageIndex > 0) {
			this.currentImageIndex--;
		} else {
			this.currentImageIndex = this.imagesArray.Length - 1;
		}
		
		this.imagePanel.transform.renderer.material.mainTexture = this.imagesArray[this.currentImageIndex] as Texture2D;
	}

	public void ShowImages() {
		if (this.imagesArray != null && this.imagesArray.Length > 0){
		
			this.currentImageIndex = 0;
			this.imagePanel.transform.renderer.material.mainTexture = this.imagesArray[this.currentImageIndex] as Texture2D;
		}
	}

	public void LoadImages() {
		imagesArray[0] = image1;
		imagesArray[1] = image2;
		imagesArray[2] = image3;
//		imagesArray[3] = image4;
//		imagesArray[4] = image5;
//		imagesArray[5] = image6;
//		imagesArray[6] = image7;
//		imagesArray[7] = image8;
//		imagesArray[8] = image9;
//		imagesArray[9] = image10;
	}
}
