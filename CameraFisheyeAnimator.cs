using System.Collections;
using System.Collections.Generic;
using UnityStandardAssets.ImageEffects;
using UnityEngine;

public class CameraFisheyeAnimator : MonoBehaviour {

    public float maxDistortion = 0.1f;

    public float xDistortionSpeed = 0.0005f;
    public float yDistortionSpeed = 0.0005f;

    Fisheye fisheye;

	// Use this for initialization
	void Start () {
        fisheye = GetComponent<Fisheye>();
    }
	
	// Update is called once per frame
	void Update () {
		if (fisheye.strengthX > maxDistortion || fisheye.strengthX < - maxDistortion) {
            xDistortionSpeed = -xDistortionSpeed;
		}
        fisheye.strengthX += xDistortionSpeed;

		if (fisheye.strengthX > maxDistortion || fisheye.strengthX < -maxDistortion) {
            yDistortionSpeed = -yDistortionSpeed;
		}
        fisheye.strengthX += yDistortionSpeed;
	
	}
}
