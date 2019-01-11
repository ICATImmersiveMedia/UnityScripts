using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneShotProbability : MonoBehaviour {

    private AudioSource dsp = null;
    private GameObject CamObject = null;
    private float maxDist = 0f;
    private float rnd = 0f;

    public bool alreadyPlayed = false; // LATER: make this private
    public int probability = 5;
    public float radius = 0.75f; // Radius of the overall audible range in which to assess whether the sound should happen

    // DEBUG
    public int count = 0;

	// Use this for initialization
	private void Start () {
        dsp = GetComponent<AudioSource>();
        CamObject = GameObject.Find("Cameras");
        maxDist = dsp.maxDistance;

        dsp.loop = false;
        dsp.playOnAwake = false;
        dsp.Stop();
	}

    // Update is called once per frame
    private void Update()
    {
        if (!alreadyPlayed)
        {

            float dist = Vector3.Distance(CamObject.transform.position, transform.position);
            if (dist < dsp.maxDistance * radius)
            {
                count++;
                rnd = Random.value * 1000f;
                if (rnd < probability)
                {
                    dsp.Play();
                    alreadyPlayed = true;
                }
            }
        }
    }

    public void Reset()
    {
        alreadyPlayed = false;
    }
}
