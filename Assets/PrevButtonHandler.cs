using UnityEngine;
using System.Collections;

public class PrevButtonHandler : MonoBehaviour {

	public bool playAnimation = false;

	void Update () {
		if(this.playAnimation) {
			iTween.MoveTo(gameObject, iTween.Hash("path", iTweenPath.GetPath("PrevButtonPressed"), "time" , 2, "speed", 500));
			this.playAnimation = false;
		}
	}
}
