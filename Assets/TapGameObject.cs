using UnityEngine;
using System.Collections;

public class TapGameObject : MonoBehaviour {

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		foreach (iPhoneTouch touch in iPhoneInput.touches) {
			// Construct a ray from the current touch coordinates				
			Ray ray = Camera.main.ScreenPointToRay(touch.position);				
			RaycastHit hit;

			if (Physics.Raycast(ray, out hit, 5000)) {
				//Transform transform = hit.collider.gameObject.transform;
				//transform.Rotate(0, 30 * Time.deltaTime, 0, Space.World);
			}
		}
	}
}
