using CsvHelper.Configuration;
using CsvHelper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;

public class GoldEnhanceTable : DataTable
{
    public class GoldEnhanceData
    {
        public int GB_Level { get; set; }
        public int GB_HP { get; set; }
        public int HP_Gold { get; set; }
        public int GB_ATK { get; set; }
        public int ATK_Gold { get; set; }
        public int GB_MAP { get; set; }
        public int MAP_Gold { get; set; }
        public int GB_GOD { get; set; }
        public int GOD_Gold { get; set; }
    }

    protected List<GoldEnhanceData> m_GoldEnhanceData = new List<GoldEnhanceData>();

    public GoldEnhanceTable()
    {
        path = "GoldEnhanceTable";
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
            var records = csv.GetRecords<GoldEnhanceData>();

            foreach (var record in records)
            {
                var temp = new GoldEnhanceData();
                temp = record;
                m_GoldEnhanceData.Add(temp);
                //Debug.Log(temp.Monster_ID);
            }
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
            Debug.LogError("csv 로드 에러");
        }

    }

    public GoldEnhanceData GetData(int num)
    {
        if (num < 0 || num >= m_GoldEnhanceData.Count)
        {
            return null;
        }
        return m_GoldEnhanceData[num];
    }
}
