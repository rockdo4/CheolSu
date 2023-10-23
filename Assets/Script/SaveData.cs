using System.Collections.Generic;
using UnityEngine;


public abstract class SaveData
{
    public int Version { get; set; }
    public abstract SaveData VersionUp();

    public SaveData()
    {
        
    }
}

public class SaveDataV1 : SaveData
{
    public SaveDataV1()
    {
        Debug.Log("1");
        Version = 1;

        status = Data.instance.status;
        Maxhealth = Data.instance.Maxhealth;
        Damage = Data.instance.Damage;
        itemList = Data.instance.itemList;
        skillInfosList = Data.instance.skillInfosList;
        stageInfo = Data.instance.stageInfo;
        goldEnhance = Data.instance.goldEnhance;
        charEnhance = Data.instance.charEnhance;
    }

    public PlayerStatus status;
    public int Maxhealth;
    public int Damage;
    public List<Item> itemList;
    public List<SkillInfo> skillInfosList;
    public GameInfo stageInfo;
    public GoldEnhanceLevel goldEnhance;
    public CharacterEnhanceLevel charEnhance;

    public override SaveData VersionUp()
    {
        return null;
    }
}