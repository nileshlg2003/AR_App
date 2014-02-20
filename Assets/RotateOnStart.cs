using UnityEngine;
using System.Collections;

public class RotateOnStart : MonoBehaviour {

	float rotSpeed = 30; // degrees per second

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		gameObject.transform.Rotate(0, rotSpeed * Time.deltaTime, 0, Space.World);
	}
}
