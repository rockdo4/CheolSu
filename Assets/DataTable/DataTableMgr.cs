using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CsvHelper;

public static class DataTableMgr
{
    private static Dictionary<System.Type, DataTable> tables = new Dictionary<System.Type, DataTable>();

    static DataTableMgr()
    {
        tables.Clear();
        //static 생성자 처음 접근할 때 실행됨

        var monsterTable = new MonsterTable();
        var dropTable = new DropTable();
        var characterLevelTable = new CharacterTable();
        var characterEnhanceTable = new CharacterEnhanceTable();
        var goldEnhanceTable = new GoldEnhanceTable();
        var skillTable = new SkillTable();
        var skillCostTable = new SkillCostTable();

        tables.Add(typeof(MonsterTable), monsterTable);
        tables.Add(typeof(DropTable), dropTable);
        tables.Add(typeof(CharacterTable), characterLevelTable);
        tables.Add(typeof(CharacterEnhanceTable), characterEnhanceTable);
        tables.Add(typeof(GoldEnhanceTable), goldEnhanceTable);
        tables.Add(typeof(SkillTable), skillTable);
        tables.Add(typeof(SkillCostTable), skillCostTable);
    }

    public static T GetTable<T>() where T : DataTable
    {
        var id = typeof(T);
        if (!tables.ContainsKey(id))
        {
            return null;
        }
        return tables[id] as T;
    }
}
