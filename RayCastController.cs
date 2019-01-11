using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class RayCastController : MonoBehaviour {

	public static int level1ObjectsTotal = 1;
	public static int level2ObjectsTotal = 1;
	public static int level3ObjectsTotal = 3;

	public static int level1ObjectsPassed = 0;
	public static int level2ObjectsPassed = 0;
	public static int level3ObjectsPassed = 0;

    private static Vector3 idlePos = new Vector3(99, 99, 99);
    private static Vector3 northRotation = new Vector3(0, 0, 0);
    private static Vector3 southRotation = new Vector3(0, 180, 0);
    private static Vector3 eastRotation = new Vector3(0, 90, 0);
    private static Vector3 westRotation = new Vector3(0, -90, 0);
	private static Vector3 floorRotation = new Vector3 (90, 0, 0);
	private static Vector3 ceilingRotation = new Vector3(-90, 0, 0);
    private static float offset = 0.05f;

    public Transform pointer;
    public Transform cam;
	public Animation2D wandPointerAnimation;
	public SendCueLocation sendStartButton;

	public Material startButtonMaterial;
	public Texture pressedButtonTexture;
	public Texture unpressedButtonTexture;

    public Transform northWall;
    public Transform southWall;
    public Transform eastWall;
    public Transform westWall;

	private bool animationPlaying;
	private bool startButtonHeld;

	void Start() {
		//pointerPlayer.loopPointReached += PlayerDone;
		animationPlaying = false;
		startButtonHeld = false;
		startButtonMaterial.SetTexture ("_MainTex", unpressedButtonTexture);
	}

	void Update () {
        RaycastHit hit;

		if (Physics.Raycast (transform.position, transform.TransformDirection (Vector3.forward), out hit, 9999)) {
			GameObject obj = hit.transform.gameObject;
			Debug.Log (obj.tag);

			if (obj.tag == "North") {
				pointer.eulerAngles = northRotation;
				Debug.Log ("North Wall Hit!");
			} else if (obj.tag == "South") {
				pointer.eulerAngles = southRotation;
				Debug.Log ("South Wall Hit!");
			} else if (obj.tag == "East") {
				pointer.eulerAngles = eastRotation;
				Debug.Log ("East Wall Hit!");
			} else if (obj.tag == "West") {
				pointer.eulerAngles = westRotation;
				Debug.Log ("West Wall Hit!");
			} else if (obj.tag == "Floor") {
				pointer.eulerAngles = floorRotation;
				Debug.Log ("Floor Hit!");
			} else if (obj.tag == "Ceiling") {
				pointer.eulerAngles = ceilingRotation;
				Debug.Log ("Ceiling Hit!");
			}
            
			Vector3 toCamera = (cam.position - hit.point).normalized;
			pointer.position = hit.point + toCamera * offset;

			bool hittingTarget = obj.tag == "Button" || obj.tag == "SoundEmitter" || obj.tag == "StartButton";

			if (!animationPlaying && hittingTarget) {
				//pointerPlayer.enabled = true;
				//pointerPlayer.Play ();
				animationPlaying = true;
				wandPointerAnimation.Begin ();

				if (obj.tag == "StartButton") {
					startButtonMaterial.SetTexture ("_MainTex", pressedButtonTexture);
					sendStartButton.startClue = true;
				}
			} else if(animationPlaying && !hittingTarget) {
				//pointerPlayer.Stop ();
				//pointerPlayer.enabled = false;
				animationPlaying = false;
				wandPointerAnimation.Halt ();

				startButtonMaterial.SetTexture ("_MainTex", unpressedButtonTexture);
				sendStartButton.startClue = false;
			}

			if (wandPointerAnimation.IsFinished ()) {
				int currRoom = MoveToNextRoom.GetCurrentRoom ();

				if (obj.tag == "Button" || obj.tag == "SoundEmitter") {
					obj.GetComponent<ChangeRoomObject> ().Disable ();

					if (currRoom == 1) {
						level1ObjectsPassed++;
					} else if (currRoom == 2) {
						level2ObjectsPassed++;
					} else if (currRoom == 3) {
						level3ObjectsPassed++;
					}
				}

				bool room0Passed = currRoom == 0;
				bool room1Passed = currRoom == 1 && level1ObjectsPassed == level1ObjectsTotal;
				bool room2Passed = currRoom == 2 && level2ObjectsPassed == level2ObjectsTotal;
				bool room3Passed = currRoom == 3 && level3ObjectsPassed == level3ObjectsTotal;

				if (room0Passed || room1Passed || room2Passed || room3Passed) {
					ContinueToNextRoom ();
				}
			}
		}
	}

	//void PlayerDone (VideoPlayer player) {
	//	Debug.Log ("NEXT ROOM!");
    //    MoveToNextRoom.MoveRooms();
	//}

	private void ContinueToNextRoom () {
		Debug.Log ("NEXT ROOM!");
		TimeUpdater.running = true;
		MoveToNextRoom.MoveRooms();
		wandPointerAnimation.Halt ();
		wandPointerAnimation.Begin ();
	}

}
