using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightPulsation : MonoBehaviour {

    public float pulsationSpeed = 0.5f;
    public float pulsationAmount = 2f;

    float startingIntensity;
    Light attachedLight;

    void Start()
    {
        attachedLight = GetComponent<Light>();
        startingIntensity = attachedLight.intensity;
    }

	// Update is called once per frame
	void Update () {
        attachedLight.intensity = startingIntensity + Mathf.Sin(Time.time * pulsationSpeed) * pulsationAmount;
	}
}
