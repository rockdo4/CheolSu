using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GachaManager : MonoBehaviour
{
    public static GachaManager Instance;

    private WeightedRandomPicker<GachaData> weaponPicker = new WeightedRandomPicker<GachaData>();
    private WeightedRandomPicker<GachaData> armorPicker = new WeightedRandomPicker<GachaData>();
    private GachaTable gachaTable;

    private Player player;

    public GridLayoutGroup gachaList;
    public GameObject[] weaponList;
    public GameObject[] armorList;
    public Text[] weaponCount;
    public Text[] armorCount;

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

        //foreach (var item in weaponPicker.GetNormalizedItemDictReadonly())
        //{
        //    Debug.Log(item);
        //}


        UpdateWeaponCount();
        UpdateArmorCount();
    }

    private void Update()
    {
    }
    public void WeaponGacha()
    {
        var data = weaponPicker.GetRandomPick();

        if (player == null) return;

        if (!player.itemList[data].unlock) 
            player.itemList[data].unlock = true;

        player.itemList[data].quantity++;
        Debug.Log($"이름 : {data.Item_Name}, 레어도 : {data.Item_Type}, 현재 수량: {player.itemList[data].quantity}");

        var item = Instantiate(weaponList[data.Item_ID - 1]);
        item.transform.SetParent(gachaList.transform, false);
        UpdateWeaponCount();
    }

    public void WeaponGacha11()
    {
        for (int i = 0; i <11; i++)
        {
            WeaponGacha();
        }
        UpdateWeaponCount();
    }

    public void ArmorGacha()
    {
        var data = armorPicker.GetRandomPick();

        if (player == null) return;
        if (!player.itemList[data].unlock)
            player.itemList[data].unlock = true;

        player.itemList[data].quantity++;
        Debug.Log($"이름 : {data.Item_Name}, 레어도 : {data.Item_Type}, 현재 수량: {player.itemList[data].quantity}");

        var item = Instantiate(armorList[data.Item_ID - 1]);
        item.transform.SetParent(gachaList.transform, false);
        UpdateArmorCount();
    }

    public void ArmorGacha11()
    {
        for(int i=0; i<11; i++)
        {
            ArmorGacha();
        }
        UpdateArmorCount();
    }

    public void SetPlayer(Player p)
    {
        player = p;
    }

    public void ClearGachaList()
    {
        foreach(Transform child in gachaList.transform)
        {
            Destroy(child.gameObject);
        }
    }

    public void UpdateWeaponCount()
    {
        foreach (var data in gachaTable.m_WeaponList)
        {
            var item = player.itemList[data];

            if(item == null) continue;

            weaponCount[data.Item_ID - 1].text = $"{item.quantity}";
        }
    }

    public void UpdateArmorCount()
    {
        foreach (var data in gachaTable.m_ArmorList)
        {
            var item = player.itemList[data];

            if (item == null) continue;

            armorCount[data.Item_ID - 1].text = $"{item.quantity}";
        }
    }
}
