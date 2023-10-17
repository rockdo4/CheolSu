using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class PlayerInfo : MonoBehaviour
{
    private CharacterTable charTable;

    void Start()
    {
        charTable = DataTableMgr.GetTable<CharacterTable>();

        
        //나중에 로드
    }

    public int CheckLevelUp(Player info)
    {
        var data = charTable.GetData(info.status._level - 1);
        //Debug.Log(data.Char_EXP);

        //레벨업
        if(data.Char_EXP <= info.status._exp)
        {
            info.status._exp -= data.Char_EXP;
            info.status._level++;
            info.status._levelPoint++;
            Debug.Log($"{info.status._level-1}에서 {info.status._level}로 레벨업");

            info.MaxHealth += data.Char_HP;
            info.Damage += data.Char_ATK;
            //data.Char_GOD 신력
            //data.Char_MAP 마력
        }

        return data.Char_EXP;
    }
}
