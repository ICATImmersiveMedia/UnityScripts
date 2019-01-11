using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputController : MonoBehaviour {

	private static Vector3 resetRotation = new Vector3 (0, 0, 0);

	public float dotSpeed;
	
	void Update () {
		float horizontalDirection = 0;
		float verticalDirection = 0;

		if (Input.GetKey (KeyCode.A)) {
			horizontalDirection--;
		}

		if (Input.GetKey (KeyCode.D)) {
			horizontalDirection++;
		}

		if (Input.GetKey (KeyCode.W)) {
			verticalDirection--;
		}

		if (Input.GetKey (KeyCode.S)) {
			verticalDirection++;
		}

		transform.Rotate (new Vector3 (dotSpeed * verticalDirection, dotSpeed * horizontalDirection, 0));

		if (Input.GetKeyDown (KeyCode.Space)) {
			transform.eulerAngles = resetRotation;
		}
	}

}
