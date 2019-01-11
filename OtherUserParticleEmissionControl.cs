using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OtherUserParticleEmissionControl : MonoBehaviour {

    public MotionCaptureStreamingReceiver mocapReceiver;
    public OSCInterface oscInterface;
    public int otherClientIndex = -1;
    public string otherClientRBName = "";

    Vector3 positionLastFrame = new Vector3();
    Color particleColor = new Color(0, 0, 0, 0);

    float localHue = .6f;

	void Start () {
		oscInterface.OtherUserDataReceivedEvent += EmitOtherUserParticles;

        // tell the MocapReceiver to call the GetWandPosition method from this script every time it gets new data for the rigidbody named in the first argument
        mocapReceiver.RegisterRigidBodyDelegate (otherClientRBName, SetTransform);
	}

    void Update(){
        /*
        particleColor = new Color(localHue, 1, 1);
        localHue += Random.Range(-.001f, .001f);
        if(localHue < 0){

            localHue = 1;
        }
        else if(localHue >1){
            localHue = 0;
        }
        */
    }

    public void EmitOtherUserParticles(object sender, OtherUserDataReceivedEventArgs e)
    {
        //UnityEngine.Debug.Log("EmitOtherUserParticles.. received client index: " +e.clientIndex);
        if(otherClientIndex == e.clientIndex){
            //UnityEngine.Debug.Log("setting other users hue: " + e.hue);
            particleColor = new Color(e.hue, 1, 1);
            particleColor = Color.HSVToRGB(particleColor.r, particleColor.g, particleColor.b);
        }
    }

    public void SetOtherUserParticlesAutomatically()
    {
        localHue += Time.deltaTime / 350f;
        particleColor = new Color(localHue, 1, 1);
        particleColor = Color.HSVToRGB(particleColor.r, particleColor.g, particleColor.b);
    }

    // this is the method that MocapReceiver will call when it gets new data for the rigidbody you named
    public void SetTransform(Vector3 position, Quaternion rotation){
        transform.position = position;

        if(transform.position != new Vector3(0.0f, 0.0f, 0.0f)){
            if(Vector3.Distance(positionLastFrame, transform.position) > .005f)
            {
                //UnityEngine.Debug.Log("emitting other users particles");
                ParticleSystem.MainModule main = GetComponent<ParticleSystem>().main;
                main.startColor = particleColor;
                GetComponent<ParticleSystem>().Emit(3);
            }
        }
        positionLastFrame = transform.position;
    }
}
