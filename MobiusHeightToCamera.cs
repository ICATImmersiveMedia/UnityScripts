using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobiusHeightToCamera : MonoBehaviour {

    public GameObject myCamera;

    // the distance from the mobius mesh to the center of the transform on the Y axis
    public float mobiusYOffset = -1.7f;

    public void Start()
    {
        UserManager.instance.NewUserCreatedEvent += InitializeMobiusHeightToNewUser;
    }

    public void InitializeMobiusHeightToNewUser(object sender, NewUserIndexReceivedEventArgs e)
    {
        //transform.localPosition = new Vector3(transform.localPosition.x, myCamera.transform.position.y + mobiusYOffset, transform.localPosition.z);
        Debug.Log("initialized height of mobius strip to user's height");
    }
}
