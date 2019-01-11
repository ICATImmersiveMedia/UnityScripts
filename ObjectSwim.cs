using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSwim : MonoBehaviour {

    public float deviationAmount = 1f;
    public float deviationSpeed = .1f;

    Vector3 origPosition;

	// Use this for initialization
	void Start () {
        origPosition = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        float perlinNoiseSeedX = origPosition.x;
        float perlinNoiseSeedY = origPosition.y;
        float perlinNoiseSeedZ = origPosition.z;

        Vector3 newPosition = new Vector3();

        newPosition.x = origPosition.x + (Mathf.PerlinNoise(perlinNoiseSeedX, Time.time * deviationSpeed) - 0.5f) * deviationAmount;
        newPosition.y = origPosition.y + (Mathf.PerlinNoise(perlinNoiseSeedY, Time.time * deviationSpeed) - 0.5f) * deviationAmount;
        newPosition.z = origPosition.z + (Mathf.PerlinNoise(perlinNoiseSeedZ, Time.time * deviationSpeed) - 0.5f) * deviationAmount;

        transform.position = newPosition;
    }
}
