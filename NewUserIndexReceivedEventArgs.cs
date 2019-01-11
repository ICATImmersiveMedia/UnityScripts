using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class NewUserIndexReceivedEventArgs : EventArgs
{
    public int newUserIndex { get; set; }
}