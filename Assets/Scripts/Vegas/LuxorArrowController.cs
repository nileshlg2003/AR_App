using UnityEngine;
using System.Collections;

public class LuxorArrowController : MonoBehaviour
{
    #region Fields
    
    private GameObject mLuxorArrow;
    
    private Vector3 mDefaultPosition;
    private Quaternion mDefaultRotation;
    private Vector3 mDefaultScale;
    
    #endregion
    
    #region Events
    
    void Start()
    {
        mLuxorArrow = gameObject;
        mDefaultPosition = mLuxorArrow.transform.position;
        mDefaultRotation = mLuxorArrow.transform.rotation;
        mDefaultScale = mLuxorArrow.transform.localScale;
        mLuxorArrow.SetActive(true);
    }
    
    void Update()
    {
        mLuxorArrow.transform.LookAt(Camera.main.transform);
    }
    
    #endregion
    
    #region Public Methods
    
    public void Reset()
    {
        mLuxorArrow.SetActive(false);
        mLuxorArrow.transform.position = mDefaultPosition;
        mLuxorArrow.transform.rotation = mDefaultRotation;
        mLuxorArrow.transform.localScale = mDefaultScale;
    }
    
    public void Show()
    {
        // Set the transform before it becomes visible
        mLuxorArrow.transform.LookAt(Camera.main.transform);
        mLuxorArrow.SetActive(true);
    }
    
    #endregion
    
    #region Private Methods
    
    #endregion
}
