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

    public void SummonMonster()
    {
        int mainStage = GameManager.Instance.gameInfo.mainStageCurr;
        int subStage = GameManager.Instance.gameInfo.subStageCurr;
        int currentStage = ((mainStage - 1) * 10) + subStage;

        var monsterData = monsterTable.GetMonsterData(currentStage);

        var monster = Instantiate(MonsterPrefabList[mainStage], transform.position, Quaternion.identity);
        var info = monster.GetComponent<Monster>();
        //Debug.Log($"KF{mainStage} - {subStage}, {info.ID}, {info.Name}, {info.MaxHealth}, {info.Damage}");
    }
}