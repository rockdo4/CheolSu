using UnityEngine;

public class GameManager : MonoBehaviour
{
    public struct GameInfo
    {
        public int mainStageMax { get; set; }
        public int subStageMax { get; set; }
        public int mainStageCurr { get; set; }
        public int subStageCurr { get; set; }
    }

    public static GameManager Instance;
    public GameInfo gameInfo;

    public Player player;
    public bool enterNext = true;

    private int monsterCount = 2;
    private int remainMonster;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("GameManager instance already exists, destroy this one");
            Destroy(gameObject);
        }
    }

    public GameManager()
    {
        if(true)
        {
            //저장 데이터가 없으면
            gameInfo.mainStageMax = 1;
            gameInfo.subStageMax = 1;
            gameInfo.mainStageCurr = 1;
            gameInfo.subStageCurr = 1;
            remainMonster = monsterCount;
        }
        else
        {
            //저장 데이터가 있으면 데이터 로드
        }
    }

    public void MonsterDie(int Drop_ID)
    {
        //플레이어 한테 보상
        var data = DataTableMgr.GetTable<DropTable>().GetMonsterData(Drop_ID);
        player.GetItem(data);

        remainMonster -= 1; //남은 몹 수 감소
        if (remainMonster > 0) return; //잡아야 될 몹이 아직 남아있으면 수만 줄이고 return

        //다 잡으면 다시 채우고
        remainMonster = monsterCount;
        Debug.Log("스테이지 클리어");
        player.StageClear();  //스테이지 클리어하면 캐릭터 체력 초기화

        //현재 스테이지가 최대 스테이지랑 똑같으면
        if (gameInfo.mainStageMax == gameInfo.mainStageCurr && gameInfo.subStageMax == gameInfo.subStageCurr)
        {
            gameInfo.subStageMax++; //서브 스테이지 1 올림
            if(gameInfo.subStageMax == 10) //서브 스테이지가 10이면
            {
                //서브 스테이지 0으로 줄이고 메인을 1 증가
                gameInfo.subStageMax = 1;
                gameInfo.mainStageMax++;
            }
        }

        if (!enterNext) return; //스테이지 반복 옵션 켜져있으면 return

        //반복 옵션 꺼져 있으면 다음 스테이지 진행
        gameInfo.subStageCurr++; 
        if (gameInfo.subStageCurr == 10) 
        {
            gameInfo.subStageCurr = 1;
            gameInfo.mainStageCurr++;
        }
    }

    //플레이어가 죽으면
    public void PlayerDie()
    {
        //다시 살림

        //한단계 아래로 내림
        Debug.Log("스테이지 하락");
        gameInfo.subStageCurr--;
        if (gameInfo.subStageCurr == 0)
        {
            gameInfo.subStageCurr = 9;
            gameInfo.mainStageCurr--;
        }
    }
}