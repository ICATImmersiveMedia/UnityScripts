using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeRoomObject : MonoBehaviour {

	public void Disable () {
		if (gameObject.tag == "Button") {
			gameObject.tag = "DisabledButton";
			gameObject.GetComponent<Renderer> ().material.color = new Color (0, 1, 0);
		} else if (gameObject.tag == "SoundEmitter") {
			gameObject.tag = "DisabledSoundEmitter";
			gameObject.GetComponent<SendCueLocation> ().startClue = false;
		} else {
			Debug.Log ("Trying to disable an invalid object");
		}
	}

	public void Enable () {
		if (gameObject.tag == "DisabledButton") {
			gameObject.tag = "Button";
			gameObject.GetComponent<Renderer> ().material.color = new Color (1, 0, 0);
		} else if (gameObject.tag == "DisabledSoundEmitter") {
			gameObject.tag = "SoundEmitter";
			gameObject.GetComponent<SendCueLocation> ().startClue = true;
		} else {
			Debug.Log ("Trying to enable an invalid object");
		}
	}

}
