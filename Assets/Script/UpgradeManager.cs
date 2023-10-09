using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldEnhanceLevel
{
    public int hp_level = 0;
    public int atk_level = 0;
}

public class CharacterEnhanceLevel
{
    public int hp_level = 0;
    public int atk_level = 0;
}

public class UpgradeManager : MonoBehaviour
{
    public Player player;
    private GoldEnhanceTable goldEnhanceTable;
    private CharacterEnhanceTable enhanceTable;
    private GoldEnhanceLevel goldLevel;
    private CharacterEnhanceLevel charLevel;

    void Start()
    {
        enhanceTable = DataTableMgr.GetTable<CharacterEnhanceTable>();
        goldEnhanceTable = DataTableMgr.GetTable<GoldEnhanceTable>();

        goldLevel = new GoldEnhanceLevel();
        charLevel = new CharacterEnhanceLevel();
    }

    // Update is called once per frame
    void Update()
    {
        Upgrade();
    }

    public void Upgrade()
    {
        if(Input.GetKeyDown(KeyCode.F1))
        {
            var data = goldEnhanceTable.GetData(goldLevel.hp_level);

            if(data.HP_Gold <= player.status._gold)
            {
                player.status._gold -= data.HP_Gold;
                player.MaxHealth += data.GB_HP;
                goldLevel.hp_level++;
                Debug.Log($"체력 {goldLevel.hp_level}에서 {goldLevel.hp_level+1}로 업그레이드");
            }
            else
            {
                Debug.Log("골드가 모자랍니다");
            }
        }

        if (Input.GetKeyDown(KeyCode.F2))
        {
            var data = goldEnhanceTable.GetData(goldLevel.atk_level);

            if (data.ATK_Gold <= player.status._gold)
            {
                player.status._gold -= data.ATK_Gold;
                player.Damage += data.ATK_Gold;
                goldLevel.atk_level++;
                Debug.Log($"공격 {goldLevel.atk_level}에서 {goldLevel.atk_level + 1}로 업그레이드");
            }
            else
            {
                Debug.Log("골드가 모자랍니다");
            }
        }

        if (Input.GetKeyDown(KeyCode.F3))
        {
            var data = enhanceTable.GetData(charLevel.hp_level);

            if (player.status._levelPoint > 0)
            {
                player.status._levelPoint--;
                player.Damage += data.AB_HP;
                charLevel.hp_level++;
                Debug.Log($"체력 {charLevel.hp_level}에서 {charLevel.hp_level + 1}로 업그레이드");
            }
            else
            {
                Debug.Log("포인트가 모자랍니다");
            }
        }

        if (Input.GetKeyDown(KeyCode.F4))
        {
            var data = enhanceTable.GetData(charLevel.atk_level);

            if (player.status._levelPoint > 0)
            {
                player.status._levelPoint--;
                player.Damage += data.AB_ATK;
                charLevel.atk_level++;
                Debug.Log($"공격 {charLevel.atk_level}에서 {charLevel.atk_level + 1}로 업그레이드");
            }
            else
            {
                Debug.Log("포인트가 모자랍니다");
            }
        }
    }
}
