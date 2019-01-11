using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//[ExecuteInEditMode]
public class SpectatorController : MonoBehaviour {

    //public Camera myCamera;
    public bool bUseMomentum;

    [HideInInspector]
    public float yElevationBoost = 0;

    public float movementSpeed = 0.25f;

    public float mouseSensitivityX = 3F;
    public float mouseSensitivityY = 3F;

    public float mouseSmoothWithMomentum = 10;

    public bool bRunInBackground = false;
    public bool bHideCursor = false;

    float myGlobalRotationX = 0f;
    float myGlobalRotationY = 0F;

    bool bToggleOne = false;
    bool bToggleTwo = false;

    Rigidbody rb;

    float smoothXAxis;
    float smoothYAxis;

    float minimumX = -360F;
    float maximumX = 360F;

    float minimumY = -180F;
    float maximumY = 180F;

    float rotationY = 0F;


	Vector3 unscaledLocalPositionLastFrame;

    void Start() {
        myGlobalRotationX = GetComponent<Transform>().eulerAngles.y;
        myGlobalRotationY = GetComponent<Transform>().eulerAngles.x;
		unscaledLocalPositionLastFrame = GetComponent<Transform> ().localPosition;

        if (bUseMomentum) {
            rb = gameObject.AddComponent<Rigidbody>();
            rb.drag = 2;
            rb.angularDrag = 0;
            rb.mass = 0.05f;
            rb.useGravity = false;
        }
    }

    void Update() {
        Application.runInBackground = bRunInBackground;
        Cursor.visible = !bHideCursor;

        if (Input.GetKeyDown("return")) {
            bToggleOne = !bToggleOne;
        }

        if (!bToggleOne) {
            float moveStrafe = Input.GetAxis("Horizontal");
            float moveForwardBack = Input.GetAxis("Vertical");
            float moveUpDown = Input.GetAxis("Jump");

			//transform.localPosition = unscaledLocalPositionLastFrame;

            Vector3 movement = new Vector3(moveStrafe, moveUpDown, moveForwardBack);
            movement *= movementSpeed;

            if (bUseMomentum) {
                rb.AddRelativeForce(movement);
            }
            else {
                transform.Translate(movement);
            }



			unscaledLocalPositionLastFrame = transform.localPosition;

            // Method two stores the current rotation from last frame in global variables, adds in the input multiplied by the sensitivity, then assigns that back to the current rotation
            myGlobalRotationX += Input.GetAxis("Mouse X") * mouseSensitivityX;
            myGlobalRotationY += Input.GetAxis("Mouse Y") * mouseSensitivityY;

            if (bUseMomentum) {
                smoothXAxis = Mathf.Lerp(smoothXAxis, Input.GetAxis("Mouse X"), Time.deltaTime * mouseSmoothWithMomentum);
                smoothYAxis = Mathf.Lerp(smoothYAxis, Input.GetAxis("Mouse Y"), Time.deltaTime * mouseSmoothWithMomentum);

                float rotationX = transform.localEulerAngles.y + smoothXAxis * mouseSensitivityX;

                rotationY += smoothYAxis * mouseSensitivityY;
                rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);

                transform.localEulerAngles = new Vector3(-rotationY, rotationX, 0);
            }
            else {
                transform.localEulerAngles = new Vector3(-myGlobalRotationY, myGlobalRotationX, 0);
            }
        }



/*
        SendCameraTransformViaOSC sender = GetComponent<SendCameraTransformViaOSC>();
        List<object> msg = new List<object>();
        msg.Add(transform.localPosition.x);
        msg.Add(transform.localPosition.y);
        msg.Add(transform.localPosition.z);
        msg.Add(transform.localRotation.x);
        msg.Add(transform.localRotation.y);
        msg.Add(transform.localRotation.z);
        msg.Add(transform.localRotation.w);
        sender.AppendMessage("/OSCCameraController/transform", msg);
 */

    }




}


