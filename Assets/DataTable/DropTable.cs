using CsvHelper.Configuration;
using CsvHelper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;

public class DropTable : DataTable
{
    public class DropData
    {
        public string Monster_Name { get; set; }
        public string Monster_ID { get; set; }
        public int Drop_ID { get; set; }
        public int Monster_EXP { get; set; }
        public int Monster_Gold { get; set; }
        public int Monster_DGP { get; set; }
    }

    protected List<DropData> m_MonsterList = new List<DropData>();

    public DropTable()
    {
        path = "DropTable";
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
            var records = csv.GetRecords<DropData>();

            foreach (var record in records)
            {
                var temp = new DropData();
                temp = record;
                m_MonsterList.Add(temp);
                Debug.Log(temp.Monster_Name);
            }
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
            Debug.LogError("csv 로드 에러");
        }
    }
}
