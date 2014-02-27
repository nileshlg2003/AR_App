using UnityEngine;
using System.Collections;

public class NYSceneHandler : MonoBehaviour, ITrackableEventHandler {

	private TrackableBehaviour mTrackableBehaviour;
	private GameObject nyImageTarget;
	private GameObject empireStateBuilding;
	private GameObject galleryGO;
	private GameObject historyPlaneGO;
	private GameObject historyTextGO;

	//private Vector3 defaultPosition;
	private Quaternion defaultRotation;
	private Vector3 defaultScale;

	void Start () {
		this.nyImageTarget = this.gameObject;
		this.empireStateBuilding = GameObject.Find("EmpireState_fbx");
		this.galleryGO = GameObject.Find("ImageSlider");
		this.historyPlaneGO = GameObject.Find("HistoryPlane");
		this.historyTextGO = GameObject.Find("HistoryText");

		this.RecordDefaults();

		this.mTrackableBehaviour = this.nyImageTarget.GetComponent<TrackableBehaviour>();
		if (this.mTrackableBehaviour) {			
			this.mTrackableBehaviour.RegisterTrackableEventHandler (this);
		}
	}
	
	void Update () {
	
	}

	public void OnTrackableStateChanged(TrackableBehaviour.Status previousStatus, TrackableBehaviour.Status newStatus)
	{
		Debug.Log ("OnTrackableStateChanged = " + newStatus.ToString ());
		if (newStatus == TrackableBehaviour.Status.DETECTED ||
		    newStatus == TrackableBehaviour.Status.TRACKED) {
			this.OnTrackingFound();
		} else {
			this.OnTrackingLost();
		}
	}

	private void OnTrackingFound ()
	{
	}
	
	private void OnTrackingLost ()
	{
		this.Reset();
	}

	private void Reset()
	{
		//this.empireStateBuilding.transform.position = this.defaultPosition;
		this.empireStateBuilding.transform.rotation = this.defaultRotation;
		this.empireStateBuilding.transform.localScale = this.defaultScale;
		this.galleryGO.SetActive(false);
		this.historyPlaneGO.SetActive(true);
		this.historyTextGO.SetActive(true);
	}

	private void RecordDefaults() {
		// Can't set transform directly, so need to save all parts separately
		//this.defaultPosition = this.empireStateBuilding.transform.position;
		this.defaultRotation = this.empireStateBuilding.transform.rotation;
		this.defaultScale = this.empireStateBuilding.transform.localScale;
	}
}
