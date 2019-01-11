using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	private static Vector3 resetRotation = new Vector3 (0, 0, 0);

    public float cameraSpeed;

	void Update()
    {
        if (Input.GetKey(KeyCode.LeftArrow)) {
            transform.Rotate(new Vector3(0, -cameraSpeed, 0));
        }

        if (Input.GetKey(KeyCode.RightArrow)) {
            transform.Rotate(new Vector3(0, cameraSpeed, 0));
        }

		if (Input.GetKey (KeyCode.UpArrow)) {
			transform.Rotate (new Vector3 (-cameraSpeed, 0, 0));
		}

		if (Input.GetKey (KeyCode.DownArrow)) {
			transform.Rotate (new Vector3 (cameraSpeed, 0, 0));
		}

		if (Input.GetKeyDown (KeyCode.Return)) {
			transform.eulerAngles = resetRotation;
		}
    }

}
