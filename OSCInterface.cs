using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Diagnostics;
using Bespoke.Common;
using Bespoke.Common.Osc;

public class OSCInterface : MonoBehaviour {

    public int localPort = 9101;
   
    public string externalIP = "127.0.0.1"; 

    // the IP of the machine to which you want to send messages
    
    public int externalPort = 9100; // the port on which the receiving machine will be listening for messages

    public OtherUserParticleEmissionControl otherUserParticleEmissionControl;

    private Thread receivingThread; // thread opened to receive data as it comes in
    private UdpClient receivingClient;
    private byte[] bytePacket;
    private IPEndPoint receivingEndPoint;
    private volatile bool dataReceived; // used in update every frame to determine if new data has been received since the last frame

    private IPEndPoint localEndPoint;
    private IPEndPoint externalEndPoint;

    private List<OscMessage> messagesThisFrame = new List<OscMessage>(); // a list of all the messages you Append during a frame, to be bundled together and sent at the end of the frame

    // When a new user initialization message is received, classes that register methods to this event will be triggered
    public event EventHandler<NewUserIndexReceivedEventArgs> NewUserIndexReceivedEvent;
    protected virtual void OnNewUserIndexReceivedEvent(NewUserIndexReceivedEventArgs e)
    {
        EventHandler<NewUserIndexReceivedEventArgs> handler = NewUserIndexReceivedEvent;
        if(handler != null)
        {
            handler(this, e);
        }
    }

    public event EventHandler<OtherUserDataReceivedEventArgs> OtherUserDataReceivedEvent;
    protected virtual void OnOtherUserDataReceivedEvent(OtherUserDataReceivedEventArgs e)
    {
        EventHandler<OtherUserDataReceivedEventArgs> handler = OtherUserDataReceivedEvent;
        if(handler != null)
        {
            handler(this, e);
        }
    }

    void Start () {
        localEndPoint = new IPEndPoint(IPAddress.Loopback, localPort);
        externalEndPoint = new IPEndPoint(IPAddress.Parse(externalIP), externalPort);

        receivingThread = new Thread(new ThreadStart(ReceiveData));
        receivingThread.IsBackground = true;
        receivingThread.Start();

        OscPacket.UdpClient = new UdpClient();  // what does this actually do?
    }
	
    void Update () {
        if (dataReceived)
        {
            dataReceived = false;
            FrameParser(OscPacket.FromByteArray(receivingEndPoint, bytePacket));
        }

        // when you're done appending messages, call SendBundle to send all the messages together in a bundle
        SendBundle();
    }

    /************************************************************************/
    /******************************* SENDING ********************************/
    /************************************************************************/

    public void AppendMessage(string address, List<object> values) {      
        OscMessage messageToSend = new OscMessage(localEndPoint, address);       
        messageToSend.ClearData(); // do i need this?       
        foreach (object message in values) {           
            messageToSend.Append(message);      
        }
        
        messagesThisFrame.Add(messageToSend);
    
    }

    
    //  sends the messages stored in messagesThisFrame as a bundle, then clears messagesThisFrame  
    private void SendBundle() { 
    	//UnityEngine.Debug.Log("sending bundle");  
        OscBundle frameBundle = new OscBundle(externalEndPoint);    
         foreach (OscMessage message in messagesThisFrame) {         
             //LogMessage(message);           
             frameBundle.Append(message);       
         }
    
        //UnityEngine.Debug.Log("sending bundle");   
        frameBundle.Send(externalEndPoint);    
        messagesThisFrame.Clear();
    }

    /************************************************************************/
    /******************************* RECEIVING ******************************/
    /************************************************************************/
    private void ReceiveData()
    {
        receivingClient = new UdpClient(localPort);
        receivingClient.Client.ReceiveTimeout = 500;
        while (true)
        {
            try
            {
                IPEndPoint anyIP = new IPEndPoint(IPAddress.Any, 0);
                bytePacket = receivingClient.Receive(ref anyIP);
                receivingEndPoint = anyIP;
                dataReceived = true;
            }
            catch (Exception err)
            {
                SocketException sockErr = (SocketException)err;
                if (sockErr.ErrorCode != 10060)
                {
                    UnityEngine.Debug.Log("Error receiving packet: " + sockErr.ToString());
                }
            }
        }
    }

    // Process Data Frame OscBundle
    private void FrameParser(OscPacket packet)
    {
        //LogPacket(packet); //Used for debugging to see all received packets
        if (packet.IsBundle)
        {
            foreach (OscMessage message in ((OscBundle)packet).Messages)
            {
                if (String.Compare(message.Address, "/NewUserIndexCreated/") == 0)
                {
                    int clientIndex = (int)message.Data[0];
                    int newUserIndex = (int)message.Data[1];
                    UnityEngine.Debug.Log("OSCInterface received new user index from server: " + newUserIndex);
                    // call all methods that have been registered to listen for it
                    NewUserIndexReceivedEventArgs args = new NewUserIndexReceivedEventArgs();
                    args.newUserIndex = newUserIndex;
                    OnNewUserIndexReceivedEvent(args);

                    List<object> oscMessage = new List<object>();
		            string oscMessageAddress = "/NewUserIndexReceived/";
		            oscMessage.Add(newUserIndex);
		            AppendMessage(oscMessageAddress, oscMessage);

                }
                if (String.Compare(message.Address, "/UserDataToClients/") == 0)
                {
                	
                    int clientIndex = (int)message.Data[0];
                    UnityEngine.Debug.Log("received user data from server from client Index: " + clientIndex);
                    int userIndex = (int)message.Data[1];
                    double timeInMS = (double)message.Data[2];
                    float positionX = (float)message.Data[3];
                    float positionY = (float)message.Data[4];
                    float positionZ = (float)message.Data[5];
                    float hue = (float)message.Data[6];

                    // call all methods that have been registered to listen for it
                    OtherUserDataReceivedEventArgs args = new OtherUserDataReceivedEventArgs();
                    args.clientIndex = clientIndex;
                    args.position = new Vector3(positionX, positionY, positionZ);
                    args.hue = hue;
                    OnOtherUserDataReceivedEvent(args);

                    // FIX THIS TO FIRE AN EVENT WHEN PARTICLE DATA FOR OTHER USERS ARE RECEIVED
                    //if (clientIndex != newUserCreator.clientIndex)
                    //{
                    //    otherUserParticleEmissionControl.EmitOtherUserParticles(new Vector3(positionX, positionY, positionZ));
                    //}
                    //UnityEngine.Debug.Log("received other position: " + positionX + ", " + positionY + ", " + positionZ);
                }
            }
        }
        else
        { // if the packet is not a bundle and is just one message
            if (String.Compare(((OscMessage)packet).Address, "/ExampleOSCAddressOfMessage/") == 0)
            {
                //string theStringMessageIKnowIsBeingSent = (string)(OscBundle)packet[0].Messages.Data[0];
                //UnityEngine.Debug.Log("received example message string: " + theStringMessageIKnowIsBeingSent);
            }
        }
    }


    // Log OscMessage or OscBundle
    private static void LogPacket(OscPacket packet)
    {
        if (packet.IsBundle)
        {
            foreach (OscMessage message in ((OscBundle)packet).Messages)
            {
                LogMessage(message);
            }
        }
        else
        {
            LogMessage((OscMessage)packet);
        }
    }

    // Log OscMessage
    private static void LogMessage(OscMessage message)
    {
        StringBuilder s = new StringBuilder();
        s.Append(message.Address);
        for (int i = 0; i < message.Data.Count; i++)
        {
            s.Append(" ");
            if (message.Data[i] == null)
            {
                s.Append("Nil");
            }
            else
            {
                s.Append(message.Data[i] is byte[] ? BitConverter.ToString((byte[])message.Data[i]) : message.Data[i].ToString());
            }
        }
        UnityEngine.Debug.Log(s);
    }
}
