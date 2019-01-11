using System.Collections;
using System.Collections.Generic;
using UnityStandardAssets.ImageEffects;
using UnityEngine;

public class SSAOAnimator : MonoBehaviour {

    //public float maxRadius = 0.1f;

    public float radiusChangeSpeed = 0.0002f;

    ScreenSpaceAmbientOcclusion ssao;

	// Use this for initialization
	void Start () {
        ssao = GetComponent<ScreenSpaceAmbientOcclusion>();

    }
	
	// Update is called once per frame
	void Update () {

        if (ssao.m_Radius >= 1.0f || ssao.m_Radius < .059)
        {
            radiusChangeSpeed = -radiusChangeSpeed;
        }
        ssao.m_Radius += radiusChangeSpeed;
    }
}
