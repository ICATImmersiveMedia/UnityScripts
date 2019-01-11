using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OtherUserDataReceivedEventArgs : EventArgs {

	public int clientIndex { get; set; }
	public Vector3 position { get; set; }
	public float hue { get; set; }
}
