using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GoldEnhanceLevel
{
    public int hp_level = 0;
    public int atk_level = 0;
    public int MAP_level = 0;
    public int GOD_level = 0;
}

public class CharacterEnhanceLevel
{
    public int hp_level = 0;
    public int atk_level = 0;
    public int MAP_level = 0;
    public int GOD_level = 0;
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

        goldLevel = new GoldEnhanceLevel();
        charLevel = new CharacterEnhanceLevel();

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
                Debug.Log($"ü�� {goldLevel.hp_level}���� {goldLevel.hp_level+1}�� ���׷��̵�");
            }
            else
            {
                Debug.Log("��尡 ���ڶ��ϴ�");
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
                Debug.Log($"���� {goldLevel.atk_level}���� {goldLevel.atk_level + 1}�� ���׷��̵�");
            }
            else
            {
                Debug.Log("��尡 ���ڶ��ϴ�");
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
                Debug.Log($"ü�� {charLevel.hp_level}���� {charLevel.hp_level + 1}�� ���׷��̵�");
            }
            else
            {
                Debug.Log("����Ʈ�� ���ڶ��ϴ�");
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
                Debug.Log($"���� {charLevel.atk_level}���� {charLevel.atk_level + 1}�� ���׷��̵�");
            }
            else
            {
                Debug.Log("����Ʈ�� ���ڶ��ϴ�");
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
            Debug.Log($"ü�� {goldLevel.hp_level}���� {goldLevel.hp_level + 1}�� ���׷��̵�");

            data = goldEnhanceTable.GetData(goldLevel.hp_level);
            //GoldHP.SetText($"��� ��ȭ\nü��\n{data.HP_Gold}");
            GoldHP.text = $"������ ���\n{data.HP_Gold}G";
            HPInfo.text = $"LV.{goldLevel.hp_level}\n{data.GB_HP} ����";
            UpdatePoint();
        }
        else
        {
            Debug.Log("��尡 ���ڶ��ϴ�");
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
            Debug.Log($"���� {goldLevel.atk_level}���� {goldLevel.atk_level + 1}�� ���׷��̵�");

            data = goldEnhanceTable.GetData(goldLevel.atk_level);
            //textGoldAtk.SetText($"��� ��ȭ\n������\n{data.ATK_Gold}");
            GoldAtk.text = $"������ ���\n{data.ATK_Gold}G";
            DMGInfo.text = $"LV.{goldLevel.atk_level}\n{data.GB_ATK} ����";
            UpdatePoint();
        }
        else
        {
            Debug.Log("��尡 ���ڶ��ϴ�");
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
            Debug.Log($"���� {goldLevel.MAP_level}���� {goldLevel.MAP_level + 1}�� ���׷��̵�");

            data = goldEnhanceTable.GetData(goldLevel.MAP_level);
            //GoldHP.SetText($"��� ��ȭ\nü��\n{data.HP_Gold}");
            GoldMAP.text = $"������ ���\n{data.MAP_Gold}G";
            MAPInfo.text = $"LV.{goldLevel.MAP_level}\n{data.GB_MAP} ����";
            UpdatePoint();
        }
        else
        {
            Debug.Log("��尡 ���ڶ��ϴ�");
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
            Debug.Log($"�ŷ� {goldLevel.GOD_level}���� {goldLevel.GOD_level + 1}�� ���׷��̵�");

            data = goldEnhanceTable.GetData(goldLevel.GOD_level);
            //GoldHP.SetText($"��� ��ȭ\nü��\n{data.HP_Gold}");
            GoldGOD.text = $"������ ���\n{data.GOD_Gold}G";
            GODInfo.text = $"LV.{goldLevel.GOD_level}\n{data.GB_GOD} ����";
            UpdatePoint();
        }
        else
        {
            Debug.Log("��尡 ���ڶ��ϴ�");
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
            Debug.Log($"ü�� {charLevel.hp_level}���� {charLevel.hp_level + 1}�� ���׷��̵�");
            HPPointInfo.text = $"LV.{charLevel.hp_level}\n{data.AB_HP} ����";
            UpdatePoint();
        }
        else
        {
            Debug.Log("����Ʈ�� ���ڶ��ϴ�");
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
            Debug.Log($"���� {charLevel.atk_level}���� {charLevel.atk_level + 1}�� ���׷��̵�");
            DMGPointInfo.text = $"LV.{charLevel.atk_level}\n{data.AB_ATK} ����";
            UpdatePoint();
        }
        else
        {
            Debug.Log("����Ʈ�� ���ڶ��ϴ�");
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
            Debug.Log($"���� {charLevel.MAP_level}���� {charLevel.MAP_level + 1}�� ���׷��̵�");
            MAPPointInfo.text = $"LV.{charLevel.MAP_level}\n{data.AB_MAP} ����";
            UpdatePoint();
        }
        else
        {
            Debug.Log("����Ʈ�� ���ڶ��ϴ�");
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
            Debug.Log($"�ŷ� {charLevel.GOD_level}���� {charLevel.GOD_level + 1}�� ���׷��̵�");
            GODPointInfo.text = $"LV.{charLevel.GOD_level}\n{data.AB_GOD} ����";
            UpdatePoint();
        }
        else
        {
            Debug.Log("����Ʈ�� ���ڶ��ϴ�");
        }
    }


    public void UpdatePoint()
    {
        CurrentGold.text = $"��� : {player.status._gold}G";
        CurrentPoint.text = $"������ ����Ʈ : {player.status._levelPoint}";

        
    }
}
