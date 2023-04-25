using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using VRTK;

public class rodPosition : MonoBehaviour
{
    private VRTK_ControllerEvents controllerEvents;
	private GameObject rod;
	private GameObject righthand;
    private bool rod_animation;
    private int rotate_count;
    private Vector3 angle, angle_back;
    private Quaternion original_angle;

    // Start is called before the first frame update
    void Start()
    {
        angle = new Vector3(0.0f, 20.0f, 20.0f);
        angle_back = new Vector3(0.0f, -20.0f, -20.0f);
        rotate_count = 0;
        rod_animation = false;
        rod = GameObject.Find("fishingRod");
        righthand = GameObject.Find("RightHand");
    }

    // Update is called once per frame
    void Update()
    {
        controllerEvents = righthand.gameObject.GetComponent<VRTK_ControllerEvents>();
        if(rod_animation == true){
            Vector3 righthand_position = righthand.transform.position;
            rod.transform.position = new Vector3(righthand_position.x, righthand_position.y, righthand_position.z);
            if(rotate_count < 2){
                rod.gameObject.transform.Rotate(angle);
                ++rotate_count;
            }else if(rotate_count < 4){
                rod.gameObject.transform.Rotate(angle_back);
                ++rotate_count;
            }else{
                rotate_count = 0;
                rod_animation = false;
                rod.transform.rotation = original_angle;
            }
        }else{
            if(controllerEvents.buttonOnePressed){
                rod_animation = true;
                original_angle = rod.transform.rotation;
            }else{
            	Vector3 righthand_position = righthand.transform.position;
                rod.transform.position = new Vector3(righthand_position.x, righthand_position.y, righthand_position.z);
                rod.transform.eulerAngles = new Vector3(righthand.transform.eulerAngles.x, righthand.transform.eulerAngles.y+10, righthand.transform.eulerAngles.z+30);
            }
        }
    }
}
