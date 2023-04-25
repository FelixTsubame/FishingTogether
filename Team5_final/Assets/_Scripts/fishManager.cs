using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;
using Photon.Pun;
using Photon.Realtime;
using RootMotion.FinalIK;

public class fishManager : MonoBehaviour
{

	private VRTK_ControllerEvents controllerEvents;
    private VRTK_ControllerEvents controllerEventsleft;
    private VRTK_ControllerEvents controllerEventshead;
    private GameObject rightHand;
    private GameObject leftHand;
    public GameObject headSet;
	public AudioSource[] soundFX;
	private bool waiting;
	private int waiting_time, catching_count, reeling_count;
	private int base_waiting_time;
	private string status;
    public CharacterInteraction characterInteraction;

    private fishUImanager fum;
    private int fish_show_count;
    public int fish_num;
    private LevelUIManager LUIM;

    public bool give;
    public bool give_release;

    // Start is called before the first frame update
    void Start()
    {
        status = "idle";
    	waiting = false;
    	waiting_time = 0;
        catching_count = 0;
        reeling_count = 0;
    	base_waiting_time = 100;
        fish_show_count = 0;
        fish_num = 0;
        give = false;
        give_release = true;
        rightHand = GameObject.Find("RightHand");
        leftHand = GameObject.Find("LeftHand");
        headSet = GameObject.Find("HeadSet");

        fum = GameObject.FindObjectOfType<fishUImanager>();
        LUIM = GameObject.FindObjectOfType<LevelUIManager>();
    }

    // Update is called once per frame
    void Update()
    {
        controllerEvents = rightHand.gameObject.GetComponent<VRTK_ControllerEvents>();
        controllerEventsleft = leftHand.gameObject.GetComponent<VRTK_ControllerEvents>();
    	if(status == "waiting"){
    		++waiting_time;
    		bool catched = fishChance(waiting_time, base_waiting_time, 0.03f);
    		if(catched){
    			status = "catching";
    			waiting_time = 0;
    			soundFX[0].Play();
    			print("Catched!!!!!!!!!!!");
    		}
    	}else if(status == "catching"){
            if(catching_count > 300){
                status = "idle";
                soundFX[1].Play();
                catching_count = 0;
            }else{
                ++catching_count;
            }
            if(controllerEvents.buttonTwoPressed){
                status = "reeling";
                catching_count = 0;
            }
        }else if(status == "reeling"){
            if(reeling_count % 30 == 0){
                soundFX[2].Play();
            }
            ++reeling_count;
            if(reeling_count > 200){
                status = "success";
                reeling_count = 0;
                soundFX[2].Stop();
            }
        }else if(status == "success"){
            soundFX[3].Play();
            fish_num++;
            status = "idle";

            int catch_type = (Random.Range(0.0f, 1.0f) < 0.5) ? 0 : 1;
            print(catch_type);
            fum.show(catch_type);
            fum.setPos(rightHand.transform.position, rightHand.transform.rotation);
            ++fish_show_count;
        }else if(status == "idle"){
            if(fish_show_count > 0){
                if(fish_show_count == 100){
                    fum.hide();
                    fish_show_count = 0;
                }else{
                    ++fish_show_count;
                }
                return;
            }
            if(controllerEvents.buttonOnePressed){
                status = "waiting";
            }

        }
        if((!controllerEventsleft.buttonOnePressed)&(!give_release))
        {
            give_release = true;
        }
        if(controllerEventsleft.buttonOnePressed&give_release)
        {
            give = true;
            fish_num--;
            LUIM.Exp_Up();
            give_release = false;
        }
        LUIM.fish_update(fish_num);
        GetComponent<PhotonView>().RPC("_fishExchange", RpcTarget.All, headSet.transform.position, give);
        give = false;
    }

    private bool fishChance(int time, int base_time, float hitChance){
    	if(time > base_time){
	    	float chance = Random.Range(0.0f, 1.0f);
	    	if(chance < hitChance){
	    		return true;
	    	}else{
	    		return false;
	    	}
    	}
    	return false;
    }

    [PunRPC]
    public void _fishExchange(Vector3 position, bool take)
    {
        if(take)
        {
            fish_num++;
            this.characterInteraction.playerGameObject.GetComponent<PlayerVisualEffect>().activateHighFiveEffect(headSet.transform.position);    
        }
        if(give)
        {
            this.characterInteraction.playerGameObject.GetComponent<PlayerVisualEffect>().activateHighFiveEffect(position);    
        }
    }
}
