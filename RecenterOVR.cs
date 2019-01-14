using UnityEngine;
using System.Collections;

public class RecenterOVR : MonoBehaviour {

	void Start(){

		//UnityEngine.XR.XRDevice.fovZoomFactor = 0.2f;
	}

	// Update is called once per frame
	void Update () {

UnityEngine.XR.XRDevice.fovZoomFactor = 1f;;
		if(Input.GetKeyDown("space")){
			Debug.Log("recentering OVR rotation");
			UnityEngine.XR.InputTracking.Recenter();
		}
	}
}
