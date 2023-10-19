using CsvHelper.Configuration;
using CsvHelper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;


public class SkillCostData
{
    public int maxLevel { get; set; }
    public int cost { get; set; }
    public float increaseCost { get; set; }
}
public class SkillCostTable : DataTable
{
    protected List<SkillCostData> m_SkillCostList = new List<SkillCostData>();

    public SkillCostTable()
    {
        path = "SkillCostTable";
        Load();
    }

    public override void Load()
    {
        var csvData = Resources.Load<TextAsset>(path);

        TextReader reader = new StringReader(csvData.text);

        var csvConfiguration = new CsvConfiguration(CultureInfo.InvariantCulture);
        csvConfiguration.HasHeaderRecord = false; // CSV ���Ͽ� ����� �ִ� ���

        var csv = new CsvReader(reader, csvConfiguration);

        try
        {
            var records = csv.GetRecords<SkillCostData>();

            foreach (var record in records)
            {
                var temp = new SkillCostData();
                temp = record;
                m_SkillCostList.Add(temp);
            }
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
            Debug.LogError("csv �ε� ����");
        }
    }

    public SkillCostData GetCostData(int idx)
    {
        var data = m_SkillCostList[idx];
        if(data == null)
        {
            return null;
        }
        return data;
    }
}
