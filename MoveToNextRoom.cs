using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToNextRoom : MonoBehaviour {

    public static float distanceBetweenRooms = 5.0f;

	private static int initialRoom = 0;

	public int numberOfRooms;
	public LevelTransition staticCamera;
	public WinScreen winScreen;
	public Transform wand;
    public Transform playerHat;
	public GameObject sendCueLocation;
    public GameObject sendCueLocation2;
    public GameObject sendCueLocation3;

	public static LevelTransition staticCameraCopy;
	public static WinScreen winScreenCopy;
	public static Transform wandCopy;
    public static Transform playerHatCopy;
	public static GameObject sendCueLocationCopy;
    public static GameObject sendCueLocationCopy2;

	private static bool transitioning;
	private static int currentRoom;
	private static int numberOfRoomsCopy;

    void Start()
    {
        staticCameraCopy = staticCamera;
		winScreenCopy = winScreen;
		wandCopy = wand;
        playerHatCopy = playerHat;
		sendCueLocationCopy = sendCueLocation;
        sendCueLocationCopy2 = sendCueLocation2;

		transitioning = false;
		currentRoom = initialRoom;
		numberOfRoomsCopy = numberOfRooms;
    }

	void Update () {
		if (transitioning && staticCameraCopy.IsFinished ()) {
			Vector3 tempWandPos = wandCopy.position;
			tempWandPos.y -= distanceBetweenRooms;
			wandCopy.position = tempWandPos;

			Vector3 tempHatPos = playerHatCopy.position;
			tempHatPos.y -= distanceBetweenRooms;
			playerHatCopy.position = tempHatPos;

			WandRigidBodyController.yOffset -= distanceBetweenRooms;

			staticCamera.Halt ();
			transitioning = false;
			staticCameraCopy.IncrementInitialPos ();
			currentRoom++;

			if (currentRoom == 2) {
				sendCueLocationCopy.GetComponent<SendCueLocation> ().startClue = true;
			}

            if (currentRoom == 3)
            {
                sendCueLocationCopy2.GetComponent<SendCueLocation>().startClue = true;
            }
        }
	}

    public static void MoveRooms ()
    {
        //Vector3 tempCamPos = staticCameraCopy.transform.position;
        //tempCamPos.y -= distanceBetweenRooms;
		//staticCameraCopy.transform.position = tempCamPos;
		if (currentRoom == numberOfRoomsCopy && !WinScreen.executing) {
			winScreenCopy.Reset ();
			TimeUpdater.running = false;
		} else if(!WinScreen.executing) {
			transitioning = true;
			staticCameraCopy.SetIsMoving (true);
		}

		if (currentRoom == 2) {
			sendCueLocationCopy.GetComponent<SendCueLocation> ().startClue = false;
		}

        if (currentRoom == 3)
        {
            sendCueLocationCopy2.GetComponent<SendCueLocation>().startClue = false;
        }
		//Vector3 tempWandPos = wandCopy.position;
		//tempWandPos.y -= distanceBetweenRooms;
		//wandCopy.position = tempWandPos;

        //Vector3 tempHatPos = playerHatCopy.position;
        //tempHatPos.y -= distanceBetweenRooms;
        //playerHatCopy.position = tempHatPos;

        //WandRigidBodyController.yOffset -= distanceBetweenRooms;
    }

	public static void MoveToMainRoom () {
		Vector3 tempCamPos = staticCameraCopy.transform.position;
		tempCamPos.y = 7.5f;
		staticCameraCopy.transform.position = tempCamPos;
		staticCameraCopy.transform.eulerAngles = new Vector3 (0, 0, 0);

		Vector3 tempHatPos = playerHatCopy.transform.position;
		tempHatPos.y = 6.5f;
		playerHatCopy.transform.position = tempHatPos;

		Vector3 tempWandPos = wandCopy.position;
		tempWandPos.y = 7.5f;
		wandCopy.position = tempWandPos;
		wandCopy.eulerAngles = new Vector3 (0, 0, 0);

		WandRigidBodyController.yOffset = 5f;

		staticCameraCopy.ResetInitialPos ();
		currentRoom = initialRoom;
	}

	public static int GetCurrentRoom() {
		return currentRoom;
	}
    
}
