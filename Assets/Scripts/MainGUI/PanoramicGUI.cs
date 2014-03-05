using UnityEngine;
using System.Collections;

public class PanoramicGUI : MonoBehaviour
{
    // Panoramic Related
    public Camera PanoCamera;
    public Camera ARCamera;
    public Material PanoramicMaterial;
    public bool ShowPanoramic = false;
    
    // Use this for initialization
    void Start()
    {
        PanoCamera.enabled = ShowPanoramic;
    }
    
    public void Show()
    {
        ShowPanoramic = true;
        
        RenderSettings.skybox = PanoramicMaterial;
        
        PanoCamera.enabled = true;
        ARCamera.enabled = false;
    }
    
    public void Hide()
    {
        ShowPanoramic = false;
        
        PanoCamera.enabled = false;
        ARCamera.enabled = true;
    }
}
