using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public struct GameInfo
{
    public int mainStageMax { get; set; }
    public int subStageMax { get; set; }
    public int mainStageCurr { get; set; }
    public int subStageCurr { get; set; }
}

public class GameManager : MonoBehaviour
{
    

    public static GameManager Instance;
    public GameInfo gameInfo;
    public TextMeshProUGUI stage;
    public TextMeshProUGUI stageLoop;

    public Player player;
    public PlayerSkill playerSkill;
    public bool enterNext = true;

    private int monsterCount = 1;
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
     
    private void Start()
    {
        var data = SaveLoadSystem.AutoLoad() as SaveDataV1;

        player.DataLoadProcess(data);
        playerSkill.DataLoadProcess(data); //여기만 채우면 됨

        SetStageText();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha0))
        {
            player.DataSaveProcess();
            playerSkill.DataSaveProcess();
            var data = new SaveDataV1();

            SaveLoadSystem.AutoSave(data);
            Debug.Log("세이브 완료");
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
            if(gameInfo.subStageMax == 11) //서브 스테이지가 10이면
            {
                //서브 스테이지 0으로 줄이고 메인을 1 증가
                gameInfo.subStageMax = 1;
                gameInfo.mainStageMax++;
            }
        }

        if (!enterNext) return; //스테이지 반복 옵션 켜져있으면 return

        //반복 옵션 꺼져 있으면 다음 스테이지 진행
        gameInfo.subStageCurr++; 
        if (gameInfo.subStageCurr == 11) 
        {
            gameInfo.subStageCurr = 1;
            gameInfo.mainStageCurr++;
        }

        SetStageText();
    }

    //플레이어가 죽으면
    public void PlayerDie()
    {
        //다시 살림

        //한단계 아래로 내림

        if(gameInfo.mainStageCurr == 1 &&  gameInfo.subStageCurr == 1) { return; }

        Debug.Log("스테이지 하락");
        gameInfo.subStageCurr--;
        if (gameInfo.subStageCurr == 0)
        {
            gameInfo.subStageCurr = 10;
            gameInfo.mainStageCurr--;
        }
        SetStageText();

        if (!enterNext) return;
        StageLoopOnOff();
    }

    public void SetStageText()
    {
        if (stage == null) return;
        stage.SetText($"현재 스테이지 : KF {gameInfo.mainStageCurr} - {gameInfo.subStageCurr}");
    }

    public void StageLoopOnOff()
    {
        if(stageLoop == null) return;   
        if(enterNext)
        {
            //다음 스테이지 진입 비활성
            enterNext = !enterNext;
            stageLoop.SetText("반복\nON");
        }
        else
        {
            //다음 스테이지 진입 활성
            enterNext = !enterNext;
            stageLoop.SetText("반복\nOFF");
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene("UI Test");
    }

    public void LevelUp()
    {
        player.GetComponent<PlayerSkill>().UnlockSkill();
    }
}