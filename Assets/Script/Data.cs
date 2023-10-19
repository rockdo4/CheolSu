using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Data : MonoBehaviour
{
    public static Data instance;

    public Data()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }


    public PlayerStatus status;
    public int Maxhealth { get; set; }
    public int Damage { get; set; }
    public List<Item> itemList = new List<Item>();
    public List<SkillInfo> skillInfosList = new List<SkillInfo>();
    public GameInfo stageInfo { get; set; }
}