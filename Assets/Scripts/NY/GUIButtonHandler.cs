﻿using UnityEngine;
using System.Collections;

public class GUIButtonHandler : MonoBehaviour {

	public GoToWebGUIButton goToWebGUIButton;
	public GalleryGUIButton galleryGUIButton;
	public HistoryGUIButton historyGUIButton;
	public PanoramicButtonHandler panoramicGUIButton;

	void Start () {
	}
	
	void Update () {
		foreach (Touch touch in Input.touches) {
			if(Input.touchCount == 1) {
				if(Input.GetTouch(0).phase == TouchPhase.Ended) {
					
					Ray ray = Camera.main.ScreenPointToRay(touch.position);				
					RaycastHit hit;
					
					if (Physics.Raycast(ray, out hit, 5000)) {
						string gameObjectName = hit.collider.gameObject.name;

						switch (gameObjectName) {
							case "Gallery":
								galleryGUIButton.selectGUIButtons();								
								historyGUIButton.DeselectGUIButtons();
								goToWebGUIButton.DeselectGUIButtons();
								panoramicGUIButton.DeselectGUIButtons();
								break;
							case "History":
								historyGUIButton.selectGUIButtons();								
								galleryGUIButton.DeselectGUIButtons();
								goToWebGUIButton.DeselectGUIButtons();
								panoramicGUIButton.DeselectGUIButtons();
								break;
							case "Website":
								goToWebGUIButton.selectGUIButtons();
								historyGUIButton.DeselectGUIButtons();
								galleryGUIButton.DeselectGUIButtons();
								panoramicGUIButton.DeselectGUIButtons();
							break;
							case "NextButton":
								galleryGUIButton.imageSlider.NextImage();
								break;
							case "PrevButton":
								galleryGUIButton.imageSlider.PrevImage();
								break;
							case "Panoramic":
								panoramicGUIButton.selectGUIButtons();
								goToWebGUIButton.DeselectGUIButtons();
								historyGUIButton.DeselectGUIButtons();
								galleryGUIButton.DeselectGUIButtons();
								panoramicGUIButton.Show();
								break;
						}
					}
				}
			}
		}
	}
}
