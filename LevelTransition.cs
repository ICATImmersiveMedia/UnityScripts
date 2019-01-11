using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTransition : MonoBehaviour {

	public float duration;
	public SendCueLocation sendElevator;

	private float initialPos;
	private bool isMoving;
	private bool isFinished;
	private float timeMoving;

	void Start () {
		initialPos = 7.5f;
		Halt ();
	}

	void Update () {
		if (isMoving) {
			timeMoving += Time.deltaTime;

			if (timeMoving > duration) {
				timeMoving = duration;
				isFinished = true;
			}

			Vector3 tempPos = transform.position;
			tempPos.y = initialPos - ((timeMoving / duration) * MoveToNextRoom.distanceBetweenRooms);
			transform.position = tempPos;
		}

		sendElevator.startClue = isMoving;
	}

	public void Halt () {
		isMoving = false;
		isFinished = false;
		timeMoving = 0;
	}

	public void IncrementInitialPos () {
		initialPos -= MoveToNextRoom.distanceBetweenRooms;
	}

	public void SetIsMoving (bool isMoving) {
		this.isMoving = isMoving;
	}

	public bool IsFinished () {
		return isFinished;
	}

	public void ResetInitialPos () {
		initialPos = 7.5f;
	}

}
