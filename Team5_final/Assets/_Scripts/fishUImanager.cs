using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fishUImanager : MonoBehaviour
{
	private GameObject[] fish_img_list;
	private int nowShowing;
    private int fish_type;
    // Start is called before the first frame update
    void Start()
    {
        fish_type = 2;
        fish_img_list = GameObject.FindGameObjectsWithTag("fishImg");
        for(int i=0; i<fish_type; ++i){
        	SpriteRenderer fish = fish_img_list[i].GetComponent<SpriteRenderer>();
        	Color c = fish.color;
        	c.a = 0;
        	fish.color = c;
        }
    }

    public void show(int i){
    	SpriteRenderer fish = fish_img_list[i].GetComponent<SpriteRenderer>();
    	Color c = fish.color;
    	c.a = 1.0f;
    	fish.color = c;
    	nowShowing = i;
    }

    public void hide(){
    	if(nowShowing == -1){
    		return;
    	}
    	SpriteRenderer fish = fish_img_list[nowShowing].GetComponent<SpriteRenderer>();
    	Color c = fish.color;
    	c.a = 0;
    	fish.color = c;
    	nowShowing = -1;
    }

    public void setPos(Vector3 pos, Quaternion angle){
    	transform.position = pos;
    	transform.rotation = angle;
    }
}
