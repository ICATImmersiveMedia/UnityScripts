using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Diagnostics;
using Bespoke.Common;
using Bespoke.Common.Osc;

// uses the Bespoke OSC implementation, so that must be present in your Unity project for this to function correctly

public class StopAllSounds : MonoBehaviour {

	public static int localPort = 10025; // this could be any port number, 10025 is randomly chosen

	public string externalIP = "127.0.0.1"; // the IP of the machine to which you want to send messages
	public int externalPort = 7402; // the port on which the receiving machine will be listening for messages
	public string command;

	private IPEndPoint localEndPoint;
	private IPEndPoint externalEndPoint;

	private List<OscMessage> messagesThisFrame = new List<OscMessage>(); // a list of all the messages you Append during a frame, to be bundled together and sent at the end of the frame

	void Start () {
		// initialize EndPoints
		localEndPoint = new IPEndPoint(IPAddress.Loopback, localPort);
		externalEndPoint = new IPEndPoint(IPAddress.Parse(externalIP), externalPort);
	}

	void OnApplicationQuit () {
		List<object> exampleMessageOne = new List<object> ();
		string exampleMessageOneAddress = command; // the message address is a string that gives an indication of what kind of data is in this message.  it is useful for the receiving party to know what kind of data is being received
		//        exampleMessageOne.Add(pointer.position); // the value you want to send in your message.  could be of almost any type.  safe to stick to boolean, string, int, float, double

		exampleMessageOne.Add (-99); // this would be the X Position Value
		exampleMessageOne.Add (-99); // Z Position Value			

		AppendMessage (exampleMessageOneAddress, exampleMessageOne);
		print ("Clue sound sent");
		SendBundle ();
	}

	private void AppendMessage(string address, List<object> values) {
		OscMessage messageToSend = new OscMessage(localEndPoint, address);
		//        messageToSend.ClearData(); // do i need this?
		foreach (object message in values) {
			messageToSend.Append(message);
		}
		messagesThisFrame.Add(messageToSend);
	}

	//  sends the messages stored in messagesThisFrame as a bundle, then clears messagesThisFrame
	private void SendBundle() {
		OscBundle frameBundle = new OscBundle(localEndPoint);
		foreach (OscMessage message in messagesThisFrame) {
			frameBundle.Append(message);
		}
		//UnityEngine.Debug.Log("sending bundle");
		frameBundle.Send(externalEndPoint);

		messagesThisFrame.Clear();
	}

}