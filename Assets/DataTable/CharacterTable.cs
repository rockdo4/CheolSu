using CsvHelper.Configuration;
using CsvHelper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;

public class CharacterTable : DataTable
{
    public class CharacterLevelData
    {
        public int Char_ID { get; set; }
        public int Char_Level { get; set; }
        public int Char_HP { get; set; }
        public int Char_EXP { get; set; }
        public int Char_ATK { get; set; }
        public int Char_MAP { get; set; }
        public int Char_GOD { get; set; }
    }

    protected List<CharacterLevelData> m_CharacterLevelList = new List<CharacterLevelData>();

    public CharacterTable()
    {
        path = "CharacterTable";
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
            var records = csv.GetRecords<CharacterLevelData>();

            foreach (var record in records)
            {
                var temp = new CharacterLevelData();
                temp = record;
                m_CharacterLevelList.Add(temp);
                //Debug.Log(temp.Monster_ID);
            }
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
            Debug.LogError("csv 로드 에러");
        }

    }

    public CharacterLevelData GetData(int num)
    {
        if (num < 0 || num >= m_CharacterLevelList.Count)
        {
            return null;
        }
        return m_CharacterLevelList[num];
    }
}
