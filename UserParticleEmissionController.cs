using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class UserParticleEmissionController : MonoBehaviour {

    [HideInInspector]
    public static UserParticleEmissionController instance { get; private set; }

    public ICATEmpaticaBLEClient empaticaClient;
    public OSCInterface oscInterface;

    public Text text_RemappedGSR;
    public Text text_AdjustedGSR;

    Vector3 positionLastFrame = new Vector3();

    DateTime originalDateTime;

    Color color = new Color(0, 255, 0);

    string filePath = "Assets/Resources/test.txt";

    bool bEmpaticaDataCalibrated = false;
   	List<float> calibrationGSRValues = new List<float>();
   	float averageCalibrationGSRValue = 0;

   	float hue = .6f;

    private void Awake()
    {
        instance = this;
    }

    void Start () {
        originalDateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
    }
	
	void Update () {
        //EmitParticlesWithEmpatica();
        EmitParticlesWithoutEmpatica();
	}

    void EmitParticlesWithEmpatica(){
        float gsr = empaticaClient.currentGSR;
        float remappedgsr = ExtensionMethods.Map(gsr, 0, 1, 0, 1);
        //Debug.Log("remapped gsr: " + remappedgsr);
        text_RemappedGSR.text = "remapped GSR: " + remappedgsr;
        float heartrate = empaticaClient.currentHeartrate;

        if(!bEmpaticaDataCalibrated){
            if(remappedgsr > 0 && remappedgsr < 10){ // filter out garbage values
                calibrationGSRValues.Add(remappedgsr);
            }

            if(calibrationGSRValues.Count > 200){
                float total = 0;
                for(int i = 0; i < calibrationGSRValues.Count; i++){
                    total += calibrationGSRValues[i];
                }
                averageCalibrationGSRValue = total/calibrationGSRValues.Count;
                bEmpaticaDataCalibrated = true;
            }
        }
        else{
            if(remappedgsr > 0 && remappedgsr < 10){ // filter out garbage values
                //Debug.Log("averageCalibrationGSRValue: " + averageCalibrationGSRValue);
                float adjustedgsr = Mathf.Abs(remappedgsr * .8f - averageCalibrationGSRValue * .8f); // adjust so that values at baseline are 0. the scalars artifiically magnify the differences
                //Debug.Log("adjustedgsr: " + adjustedgsr);
                
                hue = adjustedgsr += 0.6f; // offset so that 0 baseline is blue
                hue = ExtensionMethods.MapWrap(hue, 0.0f, 1.0f);
                //Debug.Log("hue: " + hue);
                text_AdjustedGSR.text = "hue: " + hue;

                color = Color.HSVToRGB(hue, 1, 1);
                color = new Color(color.r, color.g, color.b, .5f);
            }
        }

        if(transform.position != new Vector3(0.0f, 0.0f, 0.0f)){
            if(Vector3.Distance(positionLastFrame, transform.position) > .005f)
            {
                if (UserManager.instance.userIndex >= 0)
                {
                    //Debug.Log("emitting particles");
                    ParticleSystem.MainModule main = GetComponent<ParticleSystem>().main;
                    main.startColor = color;
                    GetComponent<ParticleSystem>().Emit(2);
                    SendUserDataToServer();
                }
            }
        }

        positionLastFrame = transform.position;
    }

    void EmitParticlesWithoutEmpatica(){

        hue += Time.deltaTime / 350f;
        color = Color.HSVToRGB(hue, 1, 1);
        color = new Color(color.r, color.g, color.b, .5f);

        if(transform.position != new Vector3(0.0f, 0.0f, 0.0f)){
            if(Vector3.Distance(positionLastFrame, transform.position) > .005f)
            {
                //if (UserManager.instance.userIndex >= 0)
                //{
                    //Debug.Log("emitting particles");
                    ParticleSystem.MainModule main = GetComponent<ParticleSystem>().main;
                    main.startColor = color;
                    GetComponent<ParticleSystem>().Emit(3);
                //}
            }
        }

        positionLastFrame = transform.position;
    }
    
    void SendUserDataToServer()
    {
        //Debug.Log("sending user data to server");
        double currentDateTimeMilliseconds = DateTime.Now.ToUniversalTime().Subtract(originalDateTime).TotalMilliseconds;
        List<object> oscMessage = new List<object>();
        string oscMessageAddress = "/UserDataToServer/";
        
        oscMessage.Add(UserManager.instance.clientIndex);
        oscMessage.Add(UserManager.instance.userIndex);
        oscMessage.Add(currentDateTimeMilliseconds);
        oscMessage.Add(transform.position.x);
        oscMessage.Add(transform.position.y);
        oscMessage.Add(transform.position.z);
        oscMessage.Add(hue);
        oscInterface.AppendMessage(oscMessageAddress, oscMessage);
    }
}
