using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class DeviceManager : MonoBehaviour
{
    public static DeviceManager Instance;
    public Transform HMD;
    public Transform leftHand;
    public Transform rightHand;
    // Start is called before the first frame update
    void Start()
    {
        DeviceManager.Instance = this;
    }

    // Update is called once per frame
    void Update()
    {   
        //For IDVR Demo
        //Check the controller button effects
        checkControllerState();
    }

    void checkControllerState(){
        //The controller Event can be checked here: https://vrtoolkit.readme.io/docs/vrtk_controllerevents-1
        //Left Hand 
        VRTK_ControllerEvents leftHandControl = leftHand.gameObject.GetComponent<VRTK_ControllerEvents>();
        if(leftHandControl.triggerPressed){
            print("Left Hand TriggerPressed");
        }
        if(leftHandControl.gripPressed){
            print("Left Hand GripPressed");
        }
        if(leftHandControl.touchpadTouched){
            Vector2 pos = leftHandControl.GetTouchpadAxis();
            print("left hand touch pad pressed at " + pos);
        }

        //Right Hand 
        VRTK_ControllerEvents rightHandControl = rightHand.gameObject.GetComponent<VRTK_ControllerEvents>();
        if(rightHandControl.triggerPressed){
            print("Right Hand TriggerPressed");
        }
        if(rightHandControl.gripPressed){
            print("Right Hand GripPressed");
        }
        if(rightHandControl.touchpadTouched){
            Vector2 pos = leftHandControl.GetTouchpadAxis();
            print("Right hand touch pad pressed at " + pos);
        }
    }
}
