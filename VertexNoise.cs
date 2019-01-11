using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VertexNoise : MonoBehaviour {

    public float localDeviationAmount = 100f;
    public float grossDeviationAmount = 0.005f;
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
    }
	
	// Update is called once per frame
	void Update () {

        //if (Time.frameCount % 5 != 0)
        //{
        //    return;
        //}

        Vector3[] currentVertices = mesh.vertices;
        for (int i = 0; i < origVertices.Length; i++)
        {
            // perlin noise, using original position as seed
            Vector3 normalizedVertex = origVertices[i] * localDeviationAmount;
            //normalizedVertex -= new Vector3(-0.5f, -0.5f, -0.5f);
           // normalizedVertex.Normalize();

            float perlinNoiseSeedX = normalizedVertex.x + Time.time * deviationSpeed;
            float perlinNoiseSeedY = normalizedVertex.y + Time.time * deviationSpeed;
            float perlinNoiseSeedZ = normalizedVertex.z + Time.time * deviationSpeed;

            Vector3 perlinNoiseOutput = new Vector3();
            //perlinNoiseOutput.x = (Mathf.PerlinNoise(perlinNoiseSeedX, Time.time * deviationSpeed) - 0.5f) * deviationAmount;
            //perlinNoiseOutput.y = (Mathf.PerlinNoise(perlinNoiseSeedY, Time.time * deviationSpeed) - 0.5f) * deviationAmount;
            //perlinNoiseOutput.z = (Mathf.PerlinNoise(perlinNoiseSeedZ, Time.time * deviationSpeed) - 0.5f) * deviationAmount;
            perlinNoiseOutput.x = (Mathf.PerlinNoise(perlinNoiseSeedX, perlinNoiseSeedX) - 0.5f) * grossDeviationAmount;
            perlinNoiseOutput.y = (Mathf.PerlinNoise(perlinNoiseSeedY, perlinNoiseSeedY) - 0.5f) * grossDeviationAmount;
            perlinNoiseOutput.z = (Mathf.PerlinNoise(perlinNoiseSeedZ, perlinNoiseSeedZ) - 0.5f) * grossDeviationAmount;

            currentVertices[i].x = origVertices[i].x + perlinNoiseOutput.x;
            currentVertices[i].y = origVertices[i].y + perlinNoiseOutput.y;
            currentVertices[i].z = origVertices[i].z + perlinNoiseOutput.z;
        }
        mesh.vertices = currentVertices;
    }
}
