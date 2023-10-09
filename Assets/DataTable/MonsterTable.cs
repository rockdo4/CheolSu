using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CsvHelper;
using System.IO;
using CsvHelper.Configuration;
using System.Globalization;
using Unity.VisualScripting.Antlr3.Runtime;
using System;

public class MonsterTable : DataTable
{
    public class MonsterData
    {
        public string Monster_ID { get; set; }
        public string Monster_Name { get; set; }
        public int Monster_Hp { get; set; }
        public int Masin_Armor { get; set; }
        public int Monster_Atk { get; set; }
        public int Masin_Shield { get; set; }
        public int Monster_Divine { get; set; } 
        public int Monster_Type { get; set; }
        public int Drop_ID { get; set; }
    }

    protected List<MonsterData> m_MonsterList = new List<MonsterData>();

    public MonsterTable()
    {
        path = "MonsterTable";
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
            var records = csv.GetRecords<MonsterData>();

            foreach (var record in records)
            {
                var temp = new MonsterData();
                temp = record;
                m_MonsterList.Add(temp);
                Debug.Log(temp.Monster_Hp);
            }
        }
        catch(Exception ex)
        {
            Debug.Log(ex.Message);
            Debug.LogError("csv 로드 에러");
        }
        
    }

    public MonsterData GetMonsterData(int num)
    {
        if(num < 0 || num >= m_MonsterList.Count)
        {
            return null;
        }
        return m_MonsterList[num];
    }
}
