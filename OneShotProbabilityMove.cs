using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneShotProbabilityMove : MonoBehaviour {

    private AudioSource dsp = null;
    private GameObject CamObject = null;
    private float maxDist = 0f;
    private float rnd = 0f;

    public Vector3 detect = new Vector3(0.0f, 0.0f, 0.0f); // proximity trigger point
    public Vector3 start = new Vector3(0.0f, 0.0f, 0.0f); // starting point for movement
    public Vector3 velocity = new Vector3(0.0f, 0.0f, 0.0f);
    public bool alreadyPlayed = false; // LATER: make this private
    public int probability = 5;
    public float radius = 0.75f; // Radius of the overall audible range in which to assess whether the sound should happen (as a ratio of the overall maxDistance)


    // DEBUG
    public float radiusRatio = 0f;
    public float dist = 0f;
    public int count = 0;
    //public bool playing = false;

	// Use this for initialization
	private void Start () {
        dsp = GetComponent<AudioSource>();
        CamObject = GameObject.Find("Cameras");
        maxDist = dsp.maxDistance;
        detect = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        radiusRatio = dsp.maxDistance * radius;

        dsp.loop = false;
        dsp.playOnAwake = false;
        dsp.Stop();
	}

    // Update is called once per frame
    private void Update()
    {
        dist = Vector3.Distance(CamObject.transform.position, transform.position); //LATER: to optimize, move this under !alreadyPlayed check
        if (!alreadyPlayed)
        {
            if (dist < dsp.maxDistance * radius)
            {
                count++;
                rnd = Random.value * 1000f;
                if (rnd < probability)
                {
                    transform.position = start;
                    dsp.Play();
                    alreadyPlayed = true;
                }
            }
        }
    }

    private void FixedUpdate()
    {
        if (dsp.isPlaying)
        {
            //playing = true;
            this.transform.Translate(velocity);
        }
        else
        {
            //playing = false;
            transform.position = detect;
        }
    }

    public void Reset()
    {
        alreadyPlayed = false;
    }
}
