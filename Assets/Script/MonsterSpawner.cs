using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    public List<GameObject> MonsterPrefabList;

    private MonsterTable monsterTable;

    public static MonsterSpawner Instance;
    private void Awake()
    {
        Debug.Log("»ý¼º");
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("MonsterSpawner instance already exists, destroy this one");
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        monsterTable = DataTableMgr.GetTable<MonsterTable>();
        SummonMonster();
    }

    // Update is called once per frame
    private void Update()
    {
        
    }

    public Creature SummonMonster()
    {
        int mainStage = GameManager.Instance.gameInfo.mainStageCurr;
        int subStage = GameManager.Instance.gameInfo.subStageCurr;

        var stage = ((mainStage - 1) * 2) + subStage / 10;

        var monster = Instantiate(MonsterPrefabList[stage], transform.position, Quaternion.identity);
        var info = monster.GetComponent<Monster>();
        //Debug.Log($"KF{mainStage} - {subStage}, {info.ID}, {info.Name}, {info.MaxHealth}, {info.Damage}");
        return info;
    }
}