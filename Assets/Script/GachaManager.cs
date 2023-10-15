using Rito;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GachaManager : MonoBehaviour
{
    public static GachaManager Instance;

    private WeightedRandomPicker<GachaData> weaponPicker = new WeightedRandomPicker<GachaData>();
    private WeightedRandomPicker<GachaData> armorPicker = new WeightedRandomPicker<GachaData>();
    private GachaTable gachaTable;

    private Player player;

    private void Awake()
    {
        Debug.Log("생성");
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("GachaManager instance already exists, destroy this one");
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        gachaTable = DataTableMgr.GetTable<GachaTable>();

        weaponPicker = new WeightedRandomPicker<GachaData>();
        armorPicker = new WeightedRandomPicker<GachaData>();

        foreach(var data in gachaTable.m_WeaponList)
        {
            weaponPicker.Add(data, data.Item_Random);
        }

        foreach (var data in gachaTable.m_ArmorList)
        {
            armorPicker.Add(data, data.Item_Random);
        }

        foreach (var item in weaponPicker.GetNormalizedItemDictReadonly())
        {
            Debug.Log(item);
        }

    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha9))
        {
            var data = weaponPicker.GetRandomPick();

            if(player != null)
            {
                player.itemList[data].quantity++;
                Debug.Log($"이름 : {data.Item_Name}, 레어도 : {data.Item_Type}, 현재 수량: {player.itemList[data].quantity}");
            }
        }
        if(Input.GetKeyDown(KeyCode.Alpha0))
        {
            var data = armorPicker.GetRandomPick();

            if (player != null)
            {
                player.itemList[data].quantity++;
                Debug.Log($"이름 : {data.Item_Name}, 레어도 : {data.Item_Type}, 현재 수량: {player.itemList[data].quantity}");
            }
        }
    }

    public void SetPlayer(Player p)
    {
        player = p;
    }
}
