using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentEnchantManager : MonoBehaviour
{
    public Player player;
    public Dictionary<GachaData, Item> list;
    public GachaTable table;

    public Button[] equipButton;
    public Button[] enchantButton;


    private void Awake()
    {
        list = player.itemList;
        table = DataTableMgr.GetTable<GachaTable>();
    }

    private void Start()
    {
        for (int i = 0; i < enchantButton.Length; i++)
        {
            equipButton[i].onClick.AddListener(() => EnchantEquipment(i));
        }

        for (int i = 0; i < equipButton.Length; i++)
        {
            equipButton[i].onClick.AddListener(() => EquipItem(i));
        }
    }

    private void Update()
    {
        
    }

    private void EnchantEquipment(int num)
    {
        var data = table.GetWeaponData(num); //아이템 번호 받아서 찾음
        var item = list[data]; //그걸로 딕셔너리에서 뽑아옴

        if (!item.unlock) return; //언락 안됐으면 리턴

        if (player.status._gold < item.data.Item_Gold + (item.data.Item_LevelUp_Gold * item.enhance)) return;
        player.status._gold -= item.data.Item_Gold;
        item.enhance++;

        switch (item.data.Item_Kind)
        {
            case 1:
                if(player.status.e_weapon == item.data)
                {
                    EquipItem(num);
                }
                break;
            case 2:
                if(player.status.e_topArmor == item.data)
                {
                    EquipItem(num);
                }
                break;
            case 3:
                if (player.status.e_bottomArmor == item.data)
                {
                    EquipItem(num);
                }
                break;
        }
    }

    private void EquipItem(int num)
    {
        if (num >= 20) num -= 20;

        var data = table.GetWeaponData(num); //아이템 번호 받아서 찾음
        var item = list[data]; //그걸로 딕셔너리에서 뽑아옴

        if (!item.unlock) return; //언락 안됐으면 리턴

        switch (item.data.Item_Kind) //아이템 종류
        {
            case 1:
                var e_weapon = player.status.e_weapon;
                if (e_weapon != null)
                {
                    player.Damage -= e_weapon.Item_ATK + (e_weapon.Item_LevelUp_ATKUP * list[e_weapon].enhance);
                }
                player.Damage += item.data.Item_ATK + (item.data.Item_LevelUp_ATKUP * item.enhance);

                break;
            case 2:
                var e_top = player.status.e_topArmor;
                if(e_top != null)
                {
                    player.MaxHealth -= e_top.Item_HP + (e_top.Item_LevelUp_HPUP * list[e_top].enhance);
                }
                player.MaxHealth += item.data.Item_HP + (item.data.Item_LevelUp_HPUP * item.enhance);

                break;
            case 3:
                var e_bottom = player.status.e_bottomArmor;
                if(e_bottom != null)
                {
                    player.status._MAP -= e_bottom.Item_MAP + (e_bottom.Item_LevelUP_MAPUP * list[e_bottom].enhance);
                    player.status._GOD -= e_bottom.Item_GOD + (e_bottom.Item_LevelUP_GODUP * list[e_bottom].enhance);
                }
                player.status._MAP += item.data.Item_MAP + (item.data.Item_LevelUP_MAPUP * item.enhance);
                player.status._GOD += item.data.Item_GOD + (item.data.Item_LevelUP_GODUP * item.enhance);

                break;
        }
    }
}
