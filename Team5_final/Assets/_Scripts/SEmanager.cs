using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using VRTK;

public class SEmanager : MonoBehaviour
{
	private VRTK_ControllerEvents controllerEvents;
    private GameObject rightHand;
	public AudioSource[] soundFX;

    private int waterSEcount;
    // Start is called before the first frame update
    void Start()
    {
        waterSEcount = 0;
        rightHand = GameObject.Find("RightHand");
    }

    // Update is called once per frame
    void Update()
    {
        if(waterSEcount > 0){
            if(waterSEcount == 10){
                soundFX[2].Play();
                waterSEcount = 0;
            }else{
                ++waterSEcount;
            }
        }
        controllerEvents = rightHand.gameObject.GetComponent<VRTK_ControllerEvents>();
        if(controllerEvents.buttonOnePressed){
            soundFX[0].Play();       
            if(waterSEcount == 0){
                waterSEcount = 1;
            }
        }

    }    
}
