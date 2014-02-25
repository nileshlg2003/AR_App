using UnityEngine;
using System.Collections;

public class AirplaneHandler : MonoBehaviour {

	public bool playAnimation = true;

	void Update () {
		if (this.playAnimation) {
			iTween.MoveTo(this.gameObject, iTween.Hash(
				"path", iTweenPath.GetPath("FlightPath"),
				//"orienttopath", true,
				"time", 10,
				"lookTime", 0.2,
				"easetype", iTween.EaseType.linear,
      			"looptype", iTween.LoopType.loop));
			playAnimation = false;
		}
	}
}
