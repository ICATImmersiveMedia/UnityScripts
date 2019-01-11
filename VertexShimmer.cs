using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VertexShimmer : MonoBehaviour {

    public float deviationAmount = 1f;
    public float deviationSpeed = .1f;

    Mesh mesh;
    Vector3[] origVertices;

    float randomSeed;

    float updateEveryXFrames = 5;
    int randomDoThisFrame;

    // Use this for initialization
    void Start () {
        // get the vertices for the mesh
        mesh = GetComponent<MeshFilter>().mesh;
        origVertices = mesh.vertices;

        // get a random seed for the perlin noise for this mesh
        randomSeed = Random.Range(-10, 10);

        // get a random number for determining which frame to update THIS mesh, if we're doing update every x frames
        randomDoThisFrame = Random.Range(0, (int)updateEveryXFrames);

        // throw in a little bit of noise to the deviation speed so that not everything hits the 0 mark at the same time
        deviationSpeed += deviationSpeed * Random.Range(-.1f, .1f);
    }
	
	// Update is called once per frame
	void Update () {

        if (Time.frameCount % 5 != 0)
        {
            return;
        }

        Vector3[] currentVertices = mesh.vertices;
        for (int i = 0; i < origVertices.Length; i++)
        {
            // move current vertex position within random distance
            // total randomness
            //currentVertices[i] += new Vector3(Random.Range(-.01f, .01f), Random.Range(-.01f, .01f), Random.Range(-.01f, .01f));

            // perlin noise, using array index as seed
            //currentVertices[i].x = origVertices[i].x + (Mathf.PerlinNoise(Time.time * deviationSpeed, i) - 0.5f) * deviationAmount;
            //currentVertices[i].y = origVertices[i].y + (Mathf.PerlinNoise(Time.time * deviationSpeed, i - randomSeed) - 0.5f) * deviationAmount;
            //currentVertices[i].z = origVertices[i].z + (Mathf.PerlinNoise(Time.time * deviationSpeed, i + randomSeed) - 0.5f) * deviationAmount;

            // perline noise, using original position as seed
            float perlinNoiseSeedX = origVertices[i].x;
            float perlinNoiseSeedY = origVertices[i].y;
            float perlinNoiseSeedZ = origVertices[i].z;

            currentVertices[i].x = origVertices[i].x + (Mathf.PerlinNoise(perlinNoiseSeedX, Time.time * deviationSpeed) - 0.5f) * deviationAmount;
            currentVertices[i].y = origVertices[i].y + (Mathf.PerlinNoise(perlinNoiseSeedY, Time.time * deviationSpeed) - 0.5f) * deviationAmount;
            currentVertices[i].z = origVertices[i].z + (Mathf.PerlinNoise(perlinNoiseSeedZ, Time.time * deviationSpeed) - 0.5f) * deviationAmount;

            // clamp vertex position to within a distance from the original
            //currentVertices[i].x = Mathf.Clamp(currentVertices[i].x, origVertices[i].x - .1f * maxDeviation, origVertices[i].x + .1f * maxDeviation);
            //currentVertices[i].y = Mathf.Clamp(currentVertices[i].y, origVertices[i].y - .1f * maxDeviation, origVertices[i].y + .1f * maxDeviation);
            //currentVertices[i].z = Mathf.Clamp(currentVertices[i].z, origVertices[i].z - .1f * maxDeviation, origVertices[i].z + .1f * maxDeviation);

        }
        mesh.vertices = currentVertices;
    }
}
