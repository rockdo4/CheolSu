using CsvHelper.Configuration;
using CsvHelper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;
using Unity.VisualScripting;

enum SKillType
{
    Active = 1,
    Passive = 2,
}

public class SkillData
{
    public string Skill_Name { get; set; }
    public int Skill_ID { get; set; }
    public int Skill_Type { get; set; }
    public int Skill_LearnLevel { get; set; }
    public int Skill_Cool { get; set; }
    public float Skill_Dmg { get; set; }
    public float Skill_LevelUpDmgIncrease { get; set; }
    public float Skill_Heal { get; set; }
    public float Skill_LevelUpHealIncrease { get; set; }
    public int Skill_ATKUp { get; set; }
    public int Skill_LevelUpATKUpIncrease { get; set; }
    public float Skill_ATKSpeed { get; set; }
    public float Skill_LevelUpATKSpeedIncrease { get; set; }
    public int Skill_MAP { get; set; }
    public int Skill_LevelUpMAPIncrease { get; set; }
    public int Skill_GOD { get; set; }
    public int Skill_LevelUpGODIncrease { get; set; }
}

public class SkillTable : DataTable
{
    protected Dictionary<int, SkillData> m_SkillTableDictionary = new Dictionary<int, SkillData>();

    public SkillTable()
    {
        path = "SkillTable";
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
            var records = csv.GetRecords<SkillData>();

            foreach (var record in records)
            {
                Debug.Log(record.Skill_Name);
                //var temp = new SkillData();
                //temp = record;
                //m_SkillTableDictionary.Add(temp.Skill_ID, temp);
            }
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
            Debug.LogError("csv 로드 에러");
        }
    }

    public SkillData GetSkillData(int ID)
    {
        var data = m_SkillTableDictionary[ID];
        return data;
    }
}
