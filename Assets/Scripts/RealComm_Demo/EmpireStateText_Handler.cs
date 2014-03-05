using UnityEngine;
using System.Collections;

public class EmpireStateText_Handler : MonoBehaviour {

	void Start () {
	
	}
	
	void Update () {
        if (this.gameObject.activeSelf) {
            this.gameObject.transform.LookAt(Camera.main.transform);
        }
	}
}
