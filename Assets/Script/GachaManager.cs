using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GachaManager : MonoBehaviour
{
    public static GachaManager Instance;

    

    private GachaSystem<GachaData> weaponPicker = new GachaSystem<GachaData>();
    private GachaSystem<GachaData> armorPicker = new GachaSystem<GachaData>();
    private GachaTable gachaTable;

    private Player player;

    public GridLayoutGroup gachaList;
    public GameObject[] weaponList;
    public GameObject[] armorList;
    public Text[] weaponCount;
    public Text[] armorCount;

    private bool isCoroutine = false;

    private void Awake()
    {
        Debug.Log("생성");
        if (Instance == null)
        {
            Instance = this;

            gachaTable = DataTableMgr.GetTable<GachaTable>();

            weaponPicker = new GachaSystem<GachaData>();
            armorPicker = new GachaSystem<GachaData>();
        }
        else
        {
            Debug.LogWarning("GachaManager instance already exists, destroy this one");
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        foreach(var data in gachaTable.m_WeaponList)
        {
            weaponPicker.Add(data, (double)data.Item_Random);
        }

        foreach (var data in gachaTable.m_ArmorList)
        {
            armorPicker.Add(data, (double)data.Item_Random);
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
        if (player.status._dragon < 50) return;
        player.status._dragon -= 50;
        var data = weaponPicker.GetGacha();
        Debug.Log(data.Item_Name);

        if (player == null) return;

        if (!player.itemList[data].unlock)
        {
            player.itemList[data].unlock = true;
            EquipmentEnchantManager.Instance.UnlockWeapon(data);
        }

        player.itemList[data].quantity++;
        //Debug.Log($"이름 : {data.Item_Name}, 레어도 : {data.Item_Type}, 현재 수량: {player.itemList[data].quantity}");

        var item = Instantiate(weaponList[data.Item_ID - 1]);
        item.transform.SetParent(gachaList.transform, false);
        UpdateWeaponCount();
    }

    IEnumerator WeaponCoroutine(int count)
    {

        isCoroutine = true;
        while (count > 0)
        {
            WeaponGacha();
            count--;
            yield return new WaitForSeconds(0.02f);
        }
        UpdateWeaponCount();
        isCoroutine = false;
    }
    public void WeaponGacha11()
    {
        if (player.status._dragon < 500) return;
        if (!isCoroutine)
        {
            StartCoroutine(WeaponCoroutine(11));
        }
    }

    public void ArmorGacha()
    {
        if (player.status._dragon < 50) return;
        player.status._dragon -= 50;

        var data = armorPicker.GetGacha();

        if (player == null) return;

        if (!player.itemList[data].unlock)
        {
            player.itemList[data].unlock = true;
            EquipmentEnchantManager.Instance.UnlockArmor(data);
        }

        player.itemList[data].quantity++;
        Debug.Log($"이름 : {data.Item_Name}, 레어도 : {data.Item_Type}, 현재 수량: {player.itemList[data].quantity}");

        var item = Instantiate(armorList[data.Item_ID - 1]);
        item.transform.SetParent(gachaList.transform, false);
        UpdateArmorCount();
    }

    IEnumerator ArmorCoroutine(int count)
    {
        isCoroutine = true;
        while (count > 0)
        {
            ArmorGacha();
            count--;
            yield return new WaitForSeconds(0.02f);
        }
        UpdateArmorCount();
        isCoroutine = false;
    }

    public void ArmorGacha11()
    {
        if (player.status._dragon < 500) return;      
        if (isCoroutine) return;
        StartCoroutine(ArmorCoroutine(11));
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
