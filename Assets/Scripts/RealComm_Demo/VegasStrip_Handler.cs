using UnityEngine;
using System.Collections;

public class VegasStrip_Handler : MonoBehaviour
{
	
	#region Fields
	
    // Globals
    public GameObject ImageTarget;
    
    private GameObject _VegasObject;
    
    private Plane _TargetPlane;
    private Vector3 _DefaultVegasPosition;
    private Quaternion _DefaultVegasRotation;
    private Vector3 _DefaultVegasScale;
    private TrackableBehaviour _TrackableBehaviour;
	
    // Dragging variables
    private Vector3 _LastPlanePoint;
	
	#endregion
	
	#region MonoBehaviour implementation
    
    // Use this for initialization
    void Start()
    {
        Init();
    }
	
    // Update is called once per frame
    void Update()
    {
        // Was incorrectly blocking the Began phase which was causing the drag to not get reinitialized correctly (.phase != TouchPhase.Moved)
        if (Input.touchCount == 1)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);             
            RaycastHit hit;
                
            if (Physics.Raycast(ray, out hit))
            {
                // We are moving the object on the plane
                DragObject(Input.GetTouch(0));
            }
        } else if (Input.touchCount == 2)
        {
            // We are pinch zooming
            PinchZoomObject(Input.GetTouch(0), Input.GetTouch(1));
        }
    }
	
	#endregion
    
    #region Public Methods
    
    public void Show()
    {
        _VegasObject.SetActive(true);
    }
    
    public void Hide()
    {
        _VegasObject.SetActive(false);
        Reset();
    }
    
    #endregion
	
	#region Private Methods

    void Init()
    {
        _VegasObject = gameObject;
        
        // These must come before the trackable behaviour, because it calls OnTrackingFound when it is initialized
        _DefaultVegasPosition = _VegasObject.transform.position;
        _DefaultVegasRotation = _VegasObject.transform.rotation;
        _DefaultVegasScale = _VegasObject.transform.localScale;
        
        _TargetPlane = new Plane(ImageTarget.transform.up, ImageTarget.transform.position);
    }
	
    private void Reset()
    {
        _VegasObject.transform.localScale = _DefaultVegasScale;
        _VegasObject.transform.position = _DefaultVegasPosition;
        _VegasObject.transform.rotation = _DefaultVegasRotation;
    }
    
    private void PinchZoomObject(Touch touch1, Touch touch2)
    {
        Vector2 curDist = touch1.position - touch2.position;
        Vector2 prevDist = ((touch1.position - touch1.deltaPosition) - (touch2.position - touch2.deltaPosition));
        float touchDelta = curDist.magnitude - prevDist.magnitude;
        
        if (touchDelta < 0)
        {
            float oldScale = _VegasObject.transform.localScale.x;
            float newScale = oldScale / 1.1f;
            _VegasObject.transform.localScale = new Vector3(newScale, newScale, newScale);
        } else if (touchDelta > 0)
        {
            float oldScale = _VegasObject.transform.localScale.x;
            float newScale = oldScale * 1.1f;
            _VegasObject.transform.localScale = new Vector3(newScale, newScale, newScale);
        }
    }
	
    private void DragObject(Touch touch)
    {
        //Gets the ray at position where the screen is touched
        Ray ray = Camera.main.ScreenPointToRay(touch.position);
		
        //Gets the position of ray along plane
        float dist = 0.0f;
        
        //Intersects ray with the plane. Sets dist to distance along the ray where intersects
        _TargetPlane.Raycast(ray, out dist);
        
        //Returns point dist along the ray.
        Vector3 planePoint = ray.GetPoint(dist);
        
        if (touch.phase == TouchPhase.Began)
        {
            // If finger touch began
            _LastPlanePoint = planePoint;
        } else if (touch.phase == TouchPhase.Moved)
        {
            // Else, we are moving
            _VegasObject.transform.position += planePoint - _LastPlanePoint;
            _LastPlanePoint = planePoint;
        }
    }
	
	#endregion
}
