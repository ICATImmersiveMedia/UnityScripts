using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animation2D : MonoBehaviour {

	public float durationInSeconds;
	public bool looping;
	public Texture originalTexture;
	public int numberOfTextures;
	public string baseFilePath;

	private Texture[] textures;
	private bool running;
	private int currentTexture;
	private float timePassed;
	private bool finished;

	void Start () {
		Debug.Log ("START BEING CALLED!!!" + numberOfTextures);
		textures = new Texture[numberOfTextures];

		for (int i = 0; i < numberOfTextures; i++) {
			string zeroes;

			if (i < 10) {
				zeroes = "0000";
			} else if (i < 100) {
				zeroes = "000";
			} else if (i < 1000) {
				zeroes = "00";
			} else if (i < 10000) {
				zeroes = "0";
			} else {
				zeroes = "";
			}

			Debug.Log (baseFilePath + "_" + zeroes + i + ".png");
			Debug.Log ("CHECKPOINT 1 REACHED");
			Debug.Log ("File path: " + baseFilePath);
			Debug.Log ("i = " + i);
			textures[i] = (Texture)Resources.Load (baseFilePath + "_" + zeroes + i);
			if (textures [i] == null) {
				Debug.Log ("Texture " + i + " in Start is NULL");
			}
		}
	}

	public float GetTimePassed () {
		return timePassed;
	}

	public void Begin () {
		finished = false;
		running = true;
		currentTexture = 0;
		timePassed = 0;
		GetComponent<Renderer> ().material.SetTexture ("_MainTex", textures[currentTexture]);
	}

	public void Halt () {
		finished = false;
		running = false;
		GetComponent<Renderer> ().material.SetTexture ("_MainTex", originalTexture);
	}

	public bool IsFinished () {
		return finished;
	}

	void Update () {
		if (running) {
			timePassed += Time.deltaTime;

			if (timePassed >= durationInSeconds) {
				if (looping) {
					Begin ();
				} else {
					running = false;
				}

				finished = true;
				return;
			}

			currentTexture = (int)(textures.Length * (timePassed / durationInSeconds));
			if (textures [currentTexture] == null) {
				Debug.Log ("Texture " + currentTexture + " is NULL");
			}
			GetComponent<Renderer> ().material.SetTexture ("_MainTex", textures[currentTexture]);
		}
	}
}
