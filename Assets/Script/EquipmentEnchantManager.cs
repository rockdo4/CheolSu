using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class EquipmentEnchantManager : MonoBehaviour
{
    [System.Serializable]
    private class EnchantInfo
    {
        public Text NameEnchant;
        public Text ItemInfo;
        public Text Cost;
        public Button equipButton;
        public Button enchantButton;
    }

    [SerializeField]
    private EnchantInfo[] enchantInfos;

    public Player player;
    public Dictionary<GachaData, Item> list;
    public GachaTable table;

    public Button[] equipButton;
    public Button[] enchantButton;

    public static EquipmentEnchantManager Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        list = player.itemList;
        table = DataTableMgr.GetTable<GachaTable>();
    }

    private void Start()
    {
        for (int i = 0; i < enchantInfos.Length; i++)
        {
            var temp = i;
            enchantInfos[temp].equipButton.onClick.AddListener(() => EquipItem(temp));
            enchantInfos[temp].enchantButton.onClick.AddListener(() => EnchantEquipment(temp));
        }

        for(int i=0; i<enchantInfos.Length; i++)
        {
            GachaData item = null;

            if(i < 20)
            {
                item = table.GetWeaponData(i);
            }
            else if(i >= 20)
            {
                item = table.GetArmorData(i - 20);
            }

            //if (list[item].quantity > 0)
            //{
            //    Unlock(i);
            //}
            //else
            //{
            //    Lock(i);
            //}

            //enchantInfos[i].NameEnchant.text = $"{list[item].data.Item_Name} +{list[item].enhance}";
        }
    }

    private void Update()
    {
        
    }

    private void EnchantEquipment(int num)
    {
        Debug.Log(Time.time);
        GachaData data = null;
        if (num < 20)
        {
            data = table.GetWeaponData(num); //아이템 번호 받아서 찾음
        }
        else if (num >= 20)
        {
            data = table.GetArmorData(num - 20);
        }

        var item = list[data]; //그걸로 딕셔너리에서 뽑아옴

        if (!item.unlock) return; //언락 안됐으면 리턴

        if (player.status._gold < item.data.Item_Gold + (item.data.Item_LevelUp_Gold * item.enhance)) return;
        if (item.quantity < 1) return;

        item.quantity--;
        player.status._gold -= item.data.Item_Gold + (item.data.Item_LevelUp_Gold * item.enhance);
        item.enhance++;

        enchantInfos[num].NameEnchant.text = $"{item.data.Item_Name} +{item.enhance}";
        enchantInfos[num].Cost.text = $"비용: {item.data.Item_Gold + (item.data.Item_LevelUp_Gold * item.enhance)}G";


        switch (item.data.Item_Kind)
        {
            case 1:
                enchantInfos[num].ItemInfo.text = $"\n\n장착 효과\n\n공격력 {item.data.Item_ATK + (item.data.Item_LevelUp_ATKUP * item.enhance)}증가";

                if(player.status.e_weapon == item.data)
                {
                    player.Damage += item.data.Item_LevelUp_ATKUP;
                }
                break;
            case 2:
                enchantInfos[num].ItemInfo.text = $"\n\n장착 효과\n\n체력 {item.data.Item_HP + (item.data.Item_LevelUp_HPUP * item.enhance)}증가";

                if (player.status.e_topArmor == item.data)
                {
                    player.MaxHealth += item.data.Item_LevelUp_HPUP;
                }
                break;
            case 3:
                enchantInfos[num].ItemInfo.text = $"\n\n장착 효과\n\n" +
                    $"마력 {item.data.Item_MAP + (item.data.Item_LevelUP_MAPUP * item.enhance)}증가\n" +
                    $"신력 {item.data.Item_GOD + (item.data.Item_LevelUP_GODUP * item.enhance)}증가";

                if (player.status.e_bottomArmor == item.data)
                {
                    player.status._MAP += item.data.Item_MAP;
                    player.status._GOD += item.data.Item_GOD;
                }
                break;
        }
    }

    private void EquipItem(int num)
    {
        GachaData data = null;
        if (num < 20)
        {
            data = table.GetWeaponData(num); //아이템 번호 받아서 찾음
        }
        else if (num >= 20)
        {
            data = table.GetArmorData(num - 20);
        }

        var item = list[data]; //그걸로 딕셔너리에서 뽑아옴
        if (!item.unlock) 
        {
            Debug.Log("언락 안됨");
            return; //언락 안됐으면 리턴
        }

        switch (item.data.Item_Kind) //아이템 종류
        {
            case 1:
                var e_weapon = player.status.e_weapon;
                if (e_weapon != null)
                {
                    player.Damage -= e_weapon.Item_ATK + (e_weapon.Item_LevelUp_ATKUP * list[e_weapon].enhance);
                }
                player.Damage += item.data.Item_ATK + (item.data.Item_LevelUp_ATKUP * item.enhance);
                player.status.e_weapon = item.data;

                break;
            case 2:
                var e_top = player.status.e_topArmor;
                if(e_top != null)
                {
                    player.MaxHealth -= e_top.Item_HP + (e_top.Item_LevelUp_HPUP * list[e_top].enhance);
                }
                player.MaxHealth += item.data.Item_HP + (item.data.Item_LevelUp_HPUP * item.enhance);
                player.status.e_topArmor = item.data;

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
                player.status.e_bottomArmor = item.data;

                break;
        }
    }

    public void Unlock(int num)
    {
        enchantInfos[num].enchantButton.gameObject.SetActive(true);
        enchantInfos[num].equipButton.gameObject.SetActive(true);
    }

    public void UnlockArmor(GachaData data)
    {
        var index = table.m_ArmorList.FindIndex(x => data.Item_Name == x.Item_Name);
        index += 20;

        enchantInfos[index].enchantButton.gameObject.SetActive(true);
        enchantInfos[index].equipButton.gameObject.SetActive(true);
    }

    public void UnlockWeapon(GachaData data)
    {
        var index = table.m_WeaponList.FindIndex(x => data.Item_Name == x.Item_Name);

        enchantInfos[index].enchantButton.gameObject.SetActive(true);
        enchantInfos[index].equipButton.gameObject.SetActive(true);
    }

    public void Lock(int num)
    {
        enchantInfos[num].enchantButton.gameObject.SetActive(false);
        enchantInfos[num].equipButton.gameObject.SetActive(false);
    }
}
