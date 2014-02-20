using UnityEngine;
using System.Collections;

public class ImageGridHandler : MonoBehaviour {

	public string _imagePath = "Assets/Textures/NY_Images";	
	public Texture2D image1;
	public Texture2D image2;
	public Texture2D image3;

	private Texture2D[] imagesArray;

	private void Start () {
		imagesArray = new Texture2D[3];
	}

	private void OnGUI(){
	}

	public void ShowImages() {
		if (imagesArray != null && imagesArray.Length > 0){

			GameObject imagePanel = GameObject.Find("ImageViewer");

			imagePanel.transform.renderer.material.mainTexture = imagesArray[0] as Texture2D;

//			for (int i = 0; i < imagesArray.Length; i++){
//					GUI.DrawTexture(new Rect(0,0,Screen.width, Screen.height), imagesArray[0], ScaleMode.ScaleToFit, true);
//			}
		}
	}

	public void LoadImages() {
//		Object[] textures = Resources.LoadAll(_imagePath, typeof(Texture2D));
//		imagesArray = new Texture2D[textures.Length];
//		
//		for (int i = 0; i < textures.Length; i++){
//			imagesArray[i] = (Texture2D)textures[i];
//		}
		imagesArray[0] = image1;
		imagesArray[1] = image2;
		imagesArray[2] = image3;
	}
}
