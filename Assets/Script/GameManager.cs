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
    public static bool existSaveData = false;

    public static GameManager Instance;
    public GameInfo gameInfo;
    public TextMeshProUGUI stage;
    public TextMeshProUGUI stageLoop;

    public Player player;
    public PlayerSkill playerSkill;
    public UpgradeManager upgradeManager;
    public StageChanger changer;
    public BGM bgm;

    public bool enterNext = true;

    private int monsterCount = 1;
    private int remainMonster;

    private void Awake()
    {
        var data = SaveLoadSystem.AutoLoad() as SaveDataV1;
        if(data != null) existSaveData = true;

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
        SetStageText();

        var data = SaveLoadSystem.AutoLoad() as SaveDataV1;

        Debug.Log("테스트");
        if (data != null)
        {
            existSaveData = true;
            player.DataLoadProcess(data);
            playerSkill.DataLoadProcess(data);
            upgradeManager.DataLoadProcess(data);
			SetStageText();
		}
        bgm.ChangeBGM(gameInfo.mainStageCurr);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {

            player.DataSaveProcess();
            playerSkill.DataSaveProcess();
            upgradeManager.DataSaveProcess();
            var data = new SaveDataV1();

            SaveLoadSystem.AutoSave(data);
            Debug.Log("세이브 완료");
        }
    }

    public GameManager()
    {
        if(!existSaveData)
        {
            //저장 데이터가 없으면
            gameInfo.mainStageMax = 1;
            gameInfo.subStageMax = 1;
            gameInfo.mainStageCurr = 1;
            gameInfo.subStageCurr = 1;
        }
            remainMonster = monsterCount;
    }

    public void MonsterDie(int Drop_ID)
    {
        //드랍
        var data = DataTableMgr.GetTable<DropTable>().GetMonsterData(Drop_ID);
        player.GetItem(data);

        remainMonster -= 1;
        if (remainMonster > 0) return;

        
        remainMonster = monsterCount;
        Debug.Log("스테이지 클리어");
        player.StageClear();  //스테이지 클리어

        if (gameInfo.mainStageMax == gameInfo.mainStageCurr && gameInfo.subStageMax == gameInfo.subStageCurr)
        {
            gameInfo.subStageMax++; 
            if(gameInfo.subStageMax == 11) 
            {
                gameInfo.subStageMax = 1;
                gameInfo.mainStageMax++;

                
            }
        }

		player.DataSaveProcess();
		playerSkill.DataSaveProcess();
		upgradeManager.DataSaveProcess();
		var sdata = new SaveDataV1();

		SaveLoadSystem.AutoSave(sdata);
		Debug.Log("세이브 완료");

		if (!enterNext) return; 

        gameInfo.subStageCurr++; 
        if (gameInfo.subStageCurr == 11) 
        {
            gameInfo.subStageCurr = 1;
            gameInfo.mainStageCurr++;
            changer.SetStage(gameInfo.mainStageCurr - 1);
            bgm.ChangeBGM(gameInfo.mainStageCurr);
        }

        SetStageText();
	}

    public void PlayerDie()
    {

        if(gameInfo.mainStageCurr == 1 &&  gameInfo.subStageCurr == 1) { return; }

        Debug.Log("스테이지 하락");
        gameInfo.subStageCurr--;
        if (gameInfo.subStageCurr == 0)
        {
            gameInfo.subStageCurr = 10;
            gameInfo.mainStageCurr--;

            changer.SetStage(gameInfo.mainStageCurr - 1);
            bgm.ChangeBGM(gameInfo.mainStageCurr);
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