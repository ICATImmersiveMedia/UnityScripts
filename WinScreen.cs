using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinScreen : MonoBehaviour {

	public static bool executing = false;

	public float cameraAcceleration;
	public float maxSpeed;
	public float shakeFactor;
	public float maxShakeHeight;
	public float timeBetweenBlinks;
	public GameObject sendEarthquake;
	public Canvas canvas;
    public Animation2D winAnimation;
    public Animation2D fadeAnimation;
	public Animation2D titleAnimation;
    //public Animation2D fadeAnimationLeft;
    //public Animation2D fadeAnimationRight;
    //public Animation2D fadeAnimationFront;
    //public Animation2D fadeAnimationBack;
	public Transform cam;
    //public GameObject winAnimationObject;
    //public GameObject fadeAnimationObject;
    public SendCueLocation soundEmitter1;
    public SendCueLocation soundEmitter2;
	public ChangeRoomObject[] disabledRoomObjects;

	private float camSpeed;
	private float rotX;
	private float rotY;
	private bool shakingUp;
	private bool fadeRunning;
	private float currentTime;

	void Start () {
		titleAnimation.Begin ();
	}

	public void Reset () {
		executing = true;
		shakingUp = false;
		camSpeed = 0;
		rotX = 0;
		rotY = 0;
		currentTime = 0;
		fadeRunning = false;
        //winAnimationRenderer.enabled = true;
        //fadeAnimationRenderer.enabled = true;

        winAnimation.Begin();
    }

	void Update () {
		if (executing) {
			camSpeed += cameraAcceleration;

			if (camSpeed > maxSpeed) {
				camSpeed = maxSpeed;
			}

			rotY += camSpeed;

			if (shakingUp) {
				rotX += shakeFactor;
			} else {
				rotX -= shakeFactor;
			}

			if (shakingUp && rotX >= maxShakeHeight) {
				rotX = maxShakeHeight;
				shakingUp = false;
			} else if (!shakingUp && rotX <= -maxShakeHeight) {
				rotX = -maxShakeHeight;
				shakingUp = true;
			}

			cam.transform.eulerAngles = new Vector3 (rotX, rotY, 0);

			if (winAnimation.GetTimePassed () >= winAnimation.durationInSeconds - fadeAnimation.durationInSeconds / 2 && !fadeRunning) {
				//fadeAnimationLeft.Begin();
				//fadeAnimationRight.Begin();
				//fadeAnimationFront.Begin();
				//fadeAnimationBack.Begin();
				fadeAnimation.Begin ();
				fadeRunning = true;
			}

			currentTime += Time.deltaTime;

			if (currentTime >= timeBetweenBlinks) {
				canvas.enabled = !canvas.enabled;
				currentTime = 0;
			}

			TimeUpdater.running = false;

			if (winAnimation.IsFinished ()) {
				winAnimation.Halt ();
				executing = false;
				canvas.enabled = true;
				RayCastController.level1ObjectsPassed = 0;
				RayCastController.level2ObjectsPassed = 0;
				RayCastController.level3ObjectsPassed = 0;
				titleAnimation.Halt ();
				titleAnimation.Begin ();
				MoveToNextRoom.MoveToMainRoom ();
				TimeUpdater.ResetCounter ();
				//SceneManager.LoadScene (SceneManager.GetActiveScene ().name);

				for (int i = 0; i < disabledRoomObjects.Length; i++) {
					disabledRoomObjects [i].Enable ();
				}

                soundEmitter1.startClue = false;
                soundEmitter2.startClue = false;
			}
		}

		sendEarthquake.GetComponent<SendCueLocation> ().startClue = executing;
	}

}
