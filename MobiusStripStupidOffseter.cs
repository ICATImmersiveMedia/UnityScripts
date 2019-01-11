using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobiusStripStupidOffseter : MonoBehaviour {

	public GameObject movingCamera;
	Vector3 origPosition;

	// Use this for initialization
	void Start () {
		origPosition = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = new Vector3(origPosition.x,
			origPosition.y - movingCamera.transform.position.x / 6.5f,
			origPosition.z);
	}
}
