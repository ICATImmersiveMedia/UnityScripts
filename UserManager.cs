using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserManager : MonoBehaviour {

	public OneShotControl oneShotControl;
    public OSCInterface oscInterface;
    public int clientIndex = 0;     // the index of this machine
    public int userIndex = -1;   // the index received from the server that indicates the user number as assigned by the server.  every person that puts on the backpack should get their own user index

    public Text text_UserNumber;

    //Static singleton property.
    public static UserManager instance { get; private set; }

    void Awake()
    {
        // Save singleton instance.
        instance = this;
    }

    void Start() {
        oscInterface.NewUserIndexReceivedEvent += InitializeNewUser;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            List<object> oscMessage = new List<object>();
            string oscMessageAddress = "/RequestNewUserIndex/";
            oscMessage.Add(clientIndex);
            oscInterface.AppendMessage(oscMessageAddress, oscMessage);
        }
    }

    public void InitializeNewUser(object sender, NewUserIndexReceivedEventArgs e)
    {
        Debug.Log("User Manager initializing a new user with index: " + e.newUserIndex);
        userIndex = e.newUserIndex;

        text_UserNumber.text = "userIndex: " + userIndex;

        oneShotControl.Reset(); // change this to subscribe to event instead of direct reference

        // call all methods that have been registered to listen for it
        NewUserIndexReceivedEventArgs args = new NewUserIndexReceivedEventArgs();
        args.newUserIndex = userIndex;
        OnNewUserCreatedEvent(args);
    }

    public event EventHandler<NewUserIndexReceivedEventArgs> NewUserCreatedEvent;
    protected virtual void OnNewUserCreatedEvent(NewUserIndexReceivedEventArgs e)
    {
        EventHandler<NewUserIndexReceivedEventArgs> handler = NewUserCreatedEvent;
        if (handler != null)
        {
            handler(this, e);
        }
    }
}
