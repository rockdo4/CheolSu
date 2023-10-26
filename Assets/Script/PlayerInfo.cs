
using UnityEngine;


public class PlayerInfo : MonoBehaviour
{
    private CharacterTable charTable;

    void Start()
    {
        charTable = DataTableMgr.GetTable<CharacterTable>();

        
        //���߿� �ε�
    }

    public int CheckLevelUp(Player info)
    {
        var data = charTable.GetData(info.status._level - 1);
        if(data == null)
        {
            return -1;
        }
        //Debug.Log(data.Char_EXP);

        //������
        if(data.Char_EXP <= info.status._exp)
        {
            info.status._exp -= data.Char_EXP;
            info.status._level++;
            info.status._levelPoint++;
            Debug.Log($"{info.status._level-1}���� {info.status._level}�� ������");

            info.MaxHealth += data.Char_HP;
            info.Damage += data.Char_ATK;
            info.status._MAP += data.Char_MAP;
            info.status._GOD += data.Char_GOD;

            GameManager.Instance.LevelUp();
        }

        return data.Char_EXP;
    }
}
