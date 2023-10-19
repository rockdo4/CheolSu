using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GoldEnhanceLevel
{
    public int hp_level { get; set; } = 0;
    public int atk_level { get; set; } = 0;
    public int MAP_level { get; set; } = 0;
    public int GOD_level { get; set; } = 0;
}

public class CharacterEnhanceLevel
{
    public int hp_level { get; set; } = 0;
    public int atk_level { get; set; } = 0;
    public int MAP_level { get; set; } = 0;
    public int GOD_level { get; set; } = 0;
}

public class UpgradeManager : MonoBehaviour
{
    public Player player;
    private GoldEnhanceTable goldEnhanceTable;
    private CharacterEnhanceTable enhanceTable;

    private GoldEnhanceLevel goldLevel;
    private CharacterEnhanceLevel charLevel;

    public Text GoldHP;
    public Text GoldAtk;
    public Text GoldMAP;
    public Text GoldGOD;
    public Text CurrentGold;
    public Text CurrentPoint;

    public Text HPInfo;
    public Text DMGInfo;
    public Text MAPInfo;
    public Text GODInfo;

    public Text HPPointInfo;
    public Text DMGPointInfo;
    public Text MAPPointInfo;
    public Text GODPointInfo;

    void Start()
    {
        enhanceTable = DataTableMgr.GetTable<CharacterEnhanceTable>();
        goldEnhanceTable = DataTableMgr.GetTable<GoldEnhanceTable>();

        if(!GameManager.existSaveData)
        {
            goldLevel = new GoldEnhanceLevel();
            charLevel = new CharacterEnhanceLevel();
        }

        UpdatePoint();
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePoint();
    }


    #region
    public void Upgrade()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("1");
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
    #endregion 

    public void GoldUpgradeHealth()
    {
        var data = goldEnhanceTable.GetData(goldLevel.hp_level);
        if (data == null) return;

        if (data.HP_Gold <= player.status._gold)
        {
            player.status._gold -= data.HP_Gold;
            player.MaxHealth += data.GB_HP;
            goldLevel.hp_level++;
            Debug.Log($"체력 {goldLevel.hp_level}에서 {goldLevel.hp_level + 1}로 업그레이드");

            data = goldEnhanceTable.GetData(goldLevel.hp_level);
            //GoldHP.SetText($"골드 강화\n체력\n{data.HP_Gold}");
            GoldHP.text = $"레벨업 비용\n{data.HP_Gold}G";
            HPInfo.text = $"LV.{goldLevel.hp_level}\n{data.GB_HP} 증가";
            UpdatePoint();
        }
        else
        {
            Debug.Log("골드가 모자랍니다");
        }
    }
    public void GoldUpgradeDamage()
    {
        var data = goldEnhanceTable.GetData(goldLevel.atk_level);
        if (data == null) return;

        if (data.ATK_Gold <= player.status._gold)
        {
            player.status._gold -= data.ATK_Gold;
            player.Damage += data.GB_ATK;
            goldLevel.atk_level++;
            Debug.Log($"공격 {goldLevel.atk_level}에서 {goldLevel.atk_level + 1}로 업그레이드");

            data = goldEnhanceTable.GetData(goldLevel.atk_level);
            //textGoldAtk.SetText($"골드 강화\n데미지\n{data.ATK_Gold}");
            GoldAtk.text = $"레벨업 비용\n{data.ATK_Gold}G";
            DMGInfo.text = $"LV.{goldLevel.atk_level}\n{data.GB_ATK} 증가";
            UpdatePoint();
        }
        else
        {
            Debug.Log("골드가 모자랍니다");
        }
    }
    public void GoldUpgradeMAP()
    {
        var data = goldEnhanceTable.GetData(goldLevel.MAP_level);
        if (data == null) return;

        if (data.MAP_Gold <= player.status._gold)
        {
            player.status._gold -= data.MAP_Gold;
            player.status._MAP += data.GB_MAP;
            goldLevel.MAP_level++;
            Debug.Log($"마력 {goldLevel.MAP_level}에서 {goldLevel.MAP_level + 1}로 업그레이드");

            data = goldEnhanceTable.GetData(goldLevel.MAP_level);
            //GoldHP.SetText($"골드 강화\n체력\n{data.HP_Gold}");
            GoldMAP.text = $"레벨업 비용\n{data.MAP_Gold}G";
            MAPInfo.text = $"LV.{goldLevel.MAP_level}\n{data.GB_MAP} 증가";
            UpdatePoint();
        }
        else
        {
            Debug.Log("골드가 모자랍니다");
        }
    }

    public void GoldUpgradeGOD()
    {
        var data = goldEnhanceTable.GetData(goldLevel.GOD_level);
        if (data == null) return;

        if (data.GOD_Gold <= player.status._gold)
        {
            player.status._gold -= data.GOD_Gold;
            player.status._GOD += data.GB_GOD;
            goldLevel.GOD_level++;
            Debug.Log($"신력 {goldLevel.GOD_level}에서 {goldLevel.GOD_level + 1}로 업그레이드");

            data = goldEnhanceTable.GetData(goldLevel.GOD_level);
            //GoldHP.SetText($"골드 강화\n체력\n{data.HP_Gold}");
            GoldGOD.text = $"레벨업 비용\n{data.GOD_Gold}G";
            GODInfo.text = $"LV.{goldLevel.GOD_level}\n{data.GB_GOD} 증가";
            UpdatePoint();
        }
        else
        {
            Debug.Log("골드가 모자랍니다");
        }
    }

    public void PointUpgradeHealth()
    {
        var data = enhanceTable.GetData(charLevel.hp_level);
        if (data == null) return;

        if (player.status._levelPoint > 0)
        {
            player.status._levelPoint--;
            player.MaxHealth += data.AB_HP;
            charLevel.hp_level++;
            Debug.Log($"체력 {charLevel.hp_level}에서 {charLevel.hp_level + 1}로 업그레이드");
            HPPointInfo.text = $"LV.{charLevel.hp_level}\n{data.AB_HP} 증가";
            UpdatePoint();
        }
        else
        {
            Debug.Log("포인트가 모자랍니다");
        }
    }
    public void PointUpgradeDamage()
    {
        var data = enhanceTable.GetData(charLevel.atk_level);
        if (data == null) return;

        if (player.status._levelPoint > 0)
        {
            player.status._levelPoint--;
            player.Damage += data.AB_ATK;
            charLevel.atk_level++;
            Debug.Log($"공격 {charLevel.atk_level}에서 {charLevel.atk_level + 1}로 업그레이드");
            DMGPointInfo.text = $"LV.{charLevel.atk_level}\n{data.AB_ATK} 증가";
            UpdatePoint();
        }
        else
        {
            Debug.Log("포인트가 모자랍니다");
        }
    }

    public void PointUpgradeMAP()
    {
        var data = enhanceTable.GetData(charLevel.MAP_level);
        if (data == null) return;

        if (player.status._levelPoint > 0)
        {
            player.status._level--;
            player.status._MAP += data.AB_MAP;
            charLevel.MAP_level++;
            Debug.Log($"마력 {charLevel.MAP_level}에서 {charLevel.MAP_level + 1}로 업그레이드");
            MAPPointInfo.text = $"LV.{charLevel.MAP_level}\n{data.AB_MAP} 증가";
            UpdatePoint();
        }
        else
        {
            Debug.Log("포인트가 모자랍니다");
        }
    }

    public void PointUpgradeGOD()
    {
        var data = enhanceTable.GetData(charLevel.GOD_level);
        if (data == null) return;

        if (player.status._levelPoint > 0)
        {
            player.status._levelPoint--;
            player.status._MAP += data.AB_GOD;
            charLevel.GOD_level++;
            Debug.Log($"신력 {charLevel.GOD_level}에서 {charLevel.GOD_level + 1}로 업그레이드");
            GODPointInfo.text = $"LV.{charLevel.GOD_level}\n{data.AB_GOD} 증가";
            UpdatePoint();
        }
        else
        {
            Debug.Log("포인트가 모자랍니다");
        }
    }


    public void UpdatePoint()
    {
        CurrentGold.text = $"골드 : {player.status._gold}G";
        CurrentPoint.text = $"레벨업 포인트 : {player.status._levelPoint}";
    }

    public void DataSaveProcess()
    {
        Data.instance.goldEnhance = goldLevel;
        Data.instance.charEnhance = charLevel;
    }

    public void DataLoadProcess(SaveDataV1 data)
    {
        goldLevel = data.goldEnhance;
        charLevel = data.charEnhance;
    }
}
