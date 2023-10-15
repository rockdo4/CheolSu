using CsvHelper.Configuration;
using CsvHelper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;

public class GachaData
{
    public string Item_Name { get; set; }
    public int Item_ID { get; set; }
    public int Item_Kind { get; set; }
    public int Item_Type { get; set; }
    public int Item_ATK { get;set; }
    public int Item_LevelUp_ATKUP { get; set; }
    public int Item_HP { get; set; }
    public int Item_LevelUp_HPUP { get; set; }
    public int Item_MAP { get; set; }
    public int Item_LevelUP_MAPUP { get; set; }
    public int Item_GOD { get; set; }
    public int Item_LevelUP_GODUP { get; set; }
    public int Item_Gold { get; set; }
    public int Item_LevelUp_Gold { get; set; }
    public float Item_Random { get; set; }
}

public class GachaTable : DataTable
{

    internal List<GachaData> m_WeaponList = new List<GachaData>();
    internal List<GachaData> m_ArmorList = new List<GachaData>();

    public GachaTable()
    {
        path = "GachaTable";
        Load();
    }

    public override void Load()
    {
        var csvData = Resources.Load<TextAsset>(path);

        TextReader reader = new StringReader(csvData.text);

        var csvConfiguration = new CsvConfiguration(CultureInfo.InvariantCulture);
        csvConfiguration.HasHeaderRecord = true; // CSV 파일에 헤더가 있는 경우

        var csv = new CsvReader(reader, csvConfiguration);

        try
        {
            var records = csv.GetRecords<GachaData>();

            foreach (var record in records)
            {
                var temp = new GachaData();
                temp = record;

                if(temp.Item_Kind == 1)
                {
                    m_WeaponList.Add(temp);
                }
                else
                {
                    m_ArmorList.Add(temp);
                }
            }
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
            Debug.LogError("csv 로드 에러");
        }

    }

    public GachaData GetWeaponData(int num)
    {
        if (num < 0 || num >= m_WeaponList.Count)
        {
            return null;
        }
        return m_WeaponList[num];
    }

    public GachaData GetArmorData(int num)
    {
        if (num < 0 || num >= m_ArmorList.Count)
        {
            return null;
        }
        return m_ArmorList[num];
    }
}
