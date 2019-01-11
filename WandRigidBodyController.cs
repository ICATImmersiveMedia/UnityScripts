using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WandRigidBodyController : MonoBehaviour {

    public static float yOffset = 5f;

    public MotionCaptureStreamingReceiver receiver;
    public string id;

	void Start ()
    {
		receiver.RegisterRigidBodyDelegate(id, AssignWandData);
	}
    
	public void AssignWandData(Vector3 position, Quaternion rotation)
    {
        transform.position = new Vector3(position.x, position.y + yOffset, position.z);
        transform.rotation = rotation;
    }

}
