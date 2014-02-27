using UnityEngine;
using System.Collections;

public class PanoStart : MonoBehaviour {

	public Material material1;

	void Start() {
		RenderSettings.skybox = material1;
	}
}
