using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelUIManager : MonoBehaviour
{
    public Text levelText;
    public Text expText;
    public Text fishnum;
    public int level;
    public int exp;
    public int fishes;

    // Start is called before the first frame update
    void Start()
    {
        fishes = 0;
        level = 1;
        exp = 0;
        fishnum.text = "Fish x " + fishes;
        levelText.text = "Level: " + level;
        expText.text = "EXP: " + exp + "/" + (10*level);
    }

    public void fish_update(int num)
    {
        fishnum.text = "Fish x " + num;
    }

    // Update is called once per frame
    public void Exp_Up(){
        exp++;
        if(exp>10*level)
        {
            exp = exp-10*level;
            level++;
        }
        levelText.text = "Level: " + level;
        expText.text = "EXP: " + exp + "/" + (10*level);
    }
}
