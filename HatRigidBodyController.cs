using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HatRigidBodyController : MonoBehaviour {

	public MotionCaptureStreamingReceiver receiver;
	public string id;

	void Start ()
	{
		receiver.RegisterRigidBodyDelegate(id, AssignHatData);
	}

	public void AssignHatData(Vector3 position, Quaternion rotation)
	{
		transform.position = new Vector3(position.x, transform.position.y, position.z);
	}

}
