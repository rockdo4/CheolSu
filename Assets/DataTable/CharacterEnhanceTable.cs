using CsvHelper.Configuration;
using CsvHelper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;

public class CharacterEnhanceTable : DataTable
{
    public class CharacterEnhanceData
    {
        public int AB_Level { get; set; }
        public int AB_HP { get; set; }
        public int AB_ATK { get; set; }
        public int AB_MAP { get; set; }
        public int AB_GOD { get; set; }
    }

    protected List<CharacterEnhanceData> m_CharacterEnhanceData = new List<CharacterEnhanceData>();

    public CharacterEnhanceTable()
    {
        path = "CharacterEnhanceTable";
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
            var records = csv.GetRecords<CharacterEnhanceData>();

            foreach (var record in records)
            {
                var temp = new CharacterEnhanceData();
                temp = record;
                m_CharacterEnhanceData.Add(temp);
            }
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
            Debug.LogError("csv 로드 에러");
        }

    }

    public CharacterEnhanceData GetData(int num)
    {
        if (num < 0 || num >= m_CharacterEnhanceData.Count)
        {
            return null;
        }
        return m_CharacterEnhanceData[num];
    }
}
