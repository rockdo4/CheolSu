using System.Collections.Generic;
using UnityEngine;


public abstract class SaveData
{
    public int Version { get; set; }
    public abstract SaveData VersionUp();
}

public class SaveDataV1 : SaveData
{
    public SaveDataV1()
    {
        Version = 1;

        status = Data.instance.status;
        Maxhealth = Data.instance.Maxhealth;
        Damage = Data.instance.Damage;
        itemList = Data.instance.itemList;
        skillInfosList = Data.instance.skillInfosList;
        stageInfo = Data.instance.stageInfo;
    }

    public PlayerStatus status;
    public int Maxhealth;
    public int Damage;
    public List<Item> itemList;
    public List<SkillInfo> skillInfosList;
    public GameInfo stageInfo;

    public override SaveData VersionUp()
    {
        return null;
    }
}