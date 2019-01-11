using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneShotControl : MonoBehaviour {

    public OneShotProbability[] children = null;
    public OneShotProbabilityMove[] childrenMove = null;

    // Use this for initialization
    void Start () {
        children = GetComponentsInChildren<OneShotProbability>();
        childrenMove = GetComponentsInChildren<OneShotProbabilityMove>();
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Space))
            Reset();
    }

    public void Reset() {
        foreach (OneShotProbability sound in children)
            sound.Reset();
        foreach (OneShotProbabilityMove sound in childrenMove)
            sound.Reset();
    }
}
