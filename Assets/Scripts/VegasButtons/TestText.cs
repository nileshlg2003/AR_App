using UnityEngine;
using System.Collections;

public class TestText : MonoBehaviour
{
		private GameObject mTestText;
	
		// Use this for initialization
		void Start ()
		{
				mTestText = gameObject;
		}
	
		// Update is called once per frame
		void Update ()
		{
				mTestText.transform.LookAt (Camera.main.transform);

				if (Input.touchCount == 1) {
						Ray ray = Camera.main.ScreenPointToRay (Input.GetTouch (0).position);				
						RaycastHit hit;
			
						if (Physics.Raycast (ray, out hit)) {
								if (hit.collider.gameObject.name == "TestHolder") {
										Application.OpenURL ("http://www.mirage.com/");
								}
						}
				}
		}
}
