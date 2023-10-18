using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Jobs.LowLevel.Unsafe;
using UnityEngine;

public class PlayerStatusConverter : JsonConverter<PlayerStatus>
{
    public override PlayerStatus ReadJson(JsonReader reader, Type objectType, PlayerStatus existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        var jObj = JObject.Load(reader);
        var data = new PlayerStatus();

        data._gold = (int)jObj["_gold"];
        data._dragon = (int)jObj["_dragon"];
        data._diamond = (int)jObj["_diamond"];
        data._exp = (int)jObj["_exp"];
        data._level = (int)jObj["_level"];
        data._levelPoint = (int)jObj["_levelPoint"];
        data._MAP = (int)jObj["_MAP"];
        data._GOD = (int)jObj["_GOD"];
        data.attackDelay = (float)jObj["attackDelay"];

        var table = DataTableMgr.GetTable<GachaTable>();

        data.e_weapon = table.FindName((string)jObj["e_weapon"]);
        data.e_topArmor = table.FindName((string)jObj["e_topArmor"]);
        data.e_bottomArmor = table.FindName((string)jObj["e_bottomArmor"]);

        return data;
    }

    public override void WriteJson(JsonWriter writer, PlayerStatus value, JsonSerializer serializer)
    {
        writer.WriteStartObject();

        writer.WritePropertyName("_gold");
        writer.WriteValue(value._gold);
        writer.WritePropertyName("_dragon");
        writer.WriteValue(value._dragon);
        writer.WritePropertyName("_diamond");
        writer.WriteValue(value._diamond);
        writer.WritePropertyName("_exp");
        writer.WriteValue(value._exp);
        writer.WritePropertyName("_level");
        writer.WriteValue(value._level);
        writer.WritePropertyName("_levelPoint");
        writer.WriteValue(value._levelPoint);
        writer.WritePropertyName("_MAP");
        writer.WriteValue(value._MAP);
        writer.WritePropertyName("_GOD");
        writer.WriteValue(value._GOD);
        writer.WritePropertyName("attackDelay");
        writer.WriteValue(value.attackDelay);

        writer.WritePropertyName("e_weapon");
        if(value.e_weapon == null)
        {
            writer.WriteValue("null");
        }
        else
        {
            writer.WriteValue(value.e_weapon.Item_Name);
        }

        writer.WritePropertyName("e_topArmor");
        if(value.e_topArmor == null)
        {
            writer.WriteValue("null");
        }
        else
        {
            writer.WriteValue(value.e_topArmor.Item_Name);
        }

        writer.WritePropertyName("e_bottomArmor");
        if(value.e_bottomArmor == null)
        {
            writer.WriteValue("null");
        }
        else
        {
            writer.WriteValue(value.e_bottomArmor.Item_Name);
        }

        writer.WriteEndObject();
    }
}

public class ItemListConverter : JsonConverter<List<Item>>
{
    public override List<Item> ReadJson(JsonReader reader, Type objectType, List<Item> existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        var jObj = JObject.Load(reader);
        var data = new List<Item>();

        var table = DataTableMgr.GetTable<GachaTable>();

        for(int i=0; i<40; i++)
        {
            GachaData item = table.FindName((string)jObj[$"itemName{i}"]);
            int quantity = (int)jObj[$"itemQuantity{i}"];
            int enhance = (int)jObj[$"itemEnhance{i}"];

            data.Add(new Item(item, quantity, enhance));
        }

        return data;
    }

    public override void WriteJson(JsonWriter writer, List<Item> value, JsonSerializer serializer)
    {
        writer.WriteStartObject();

        for(int i=0; i<value.Count; i++)
        {
            writer.WritePropertyName($"itemName{i}");
            writer.WriteValue(value[i].data.Item_Name);
            writer.WritePropertyName($"itemQuantity{i}");
            writer.WriteValue(value[i].quantity);
            writer.WritePropertyName($"itemEnhance{i}");
            writer.WriteValue(value[i].enhance);
        }

        writer.WriteEndObject();
    }
}

public class SkillInfoConverter : JsonConverter<List<SkillInfo>>
{
    public override List<SkillInfo> ReadJson(JsonReader reader, Type objectType, List<SkillInfo> existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        var jObj = JObject.Load(reader);
        var data = new List<SkillInfo>();

        var table = DataTableMgr.GetTable<SkillTable>();
        var costTable = DataTableMgr.GetTable<SkillCostTable>();
        
        for (int i = 0; i < 5; i++)
        {
            SkillInfo info = new SkillInfo();

            info.level = (int)jObj[$"SkillInfoLevel{i}"];
            info.data = table.GetSkillData((int)jObj[$"SkillDataID{i}"]);

            var cost = costTable.GetCostData(i);

            info.maxLevel = cost.maxLevel;
            info.cost = cost.cost;
            info.increaseCost = cost.increaseCost;

            data.Add(info);
        }

        return data;
    }

    public override void WriteJson(JsonWriter writer, List<SkillInfo> value, JsonSerializer serializer)
    {
        writer.WriteStartObject();

        for (int i = 0; i < value.Count; i++)
        {
            writer.WritePropertyName($"SkillInfoLevel{i}");
            writer.WriteValue(value[i].level);
            writer.WritePropertyName($"SkillDataID{i}");
            writer.WriteValue(value[i].data.Skill_ID);
        }

        writer.WriteEndObject();
    }
}
