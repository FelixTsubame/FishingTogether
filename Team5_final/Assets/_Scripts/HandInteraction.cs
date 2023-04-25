using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using VRTK;

public class HandInteraction : MonoBehaviour
{
    public enum HandType{
        LeftHand = 0,
        RightHand = 1
    }
    public HandType handType;
    public CharacterInteraction characterInteraction;
    private GameObject touchObject;
    private VRTK_ControllerEvents controllerEvents;
    private bool _pickUp = false;

    void Start()
    {
        //Assign the VR TK Controller Event (left or right hand)
        if(this.handType == HandType.LeftHand){
            controllerEvents = DeviceManager.Instance.leftHand.GetComponent<VRTK_ControllerEvents>();
        }
        else{
            controllerEvents = DeviceManager.Instance.rightHand.GetComponent<VRTK_ControllerEvents>();
        }
    }

    void Update()
    {
        if(this.touchObject != null) checkGrabObject(this.touchObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(characterInteraction.playerGameObject != null){
            if(characterInteraction.playerGameObject.GetComponent<PhotonView>().IsMine){      
                //Check if the user touch the grabbable object 
                if(other.CompareTag("Grabbable")){
                    this.touchObject = other.gameObject;
                }
                //Check if the user touch any avatar's hand
                if(other.CompareTag("AvatarHand")){
                    //Check whether the hand is the same user's other hand, if it is, then nothing happens
                    if(other.gameObject.GetComponent<HandInteraction>().characterInteraction == this.characterInteraction) return;
                    //If it is other users' hand, then activate the highfive effect
                    Vector3 pos = (this.transform.position + other.gameObject.transform.position) / 2.0f;
                    this.characterInteraction.playerGameObject.GetComponent<PlayerVisualEffect>().activateHighFiveEffect(pos);
                }
            } 
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Grabbable")){
            this.touchObject = null;
        }
    }
    private void checkGrabObject(GameObject touchedObject){
        //If the user activate the grab action, then grab the object
        if(controllerEvents.triggerPressed){
            pickObject(touchedObject);
        }
        if(!controllerEvents.triggerPressed && this._pickUp){
            dropObject(touchedObject);
        }
    }
    private void pickObject(GameObject other){
        //Pick up the object
        other.GetComponent<PhotonView>().RequestOwnership();
        other.transform.position = this.transform.position;
        this.GetComponent<FixedJoint>().connectedBody = other.GetComponent<Rigidbody>();
        this._pickUp = true;
    }

    private void dropObject(GameObject other){
        this.GetComponent<FixedJoint>().connectedBody = null;
        //Assign the velocity and the angular velocity from the controller
        VRTK_VelocityEstimator velocityEstimator = controllerEvents.gameObject.GetComponent<VRTK_VelocityEstimator>();
        other.GetComponent<Rigidbody>().velocity = velocityEstimator.GetVelocityEstimate();
        other.GetComponent<Rigidbody>().angularVelocity = velocityEstimator.GetAngularVelocityEstimate();
        this._pickUp = false;
    }

}
