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
        SetStageText();

        var data = SaveLoadSystem.AutoLoad() as SaveDataV1;

        if (data != null)
        {
            existSaveData = true;
            player.DataLoadProcess(data);
            playerSkill.DataLoadProcess(data);
            upgradeManager.DataLoadProcess(data);
        }
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
            Debug.Log("���̺� �Ϸ�");
        }
    }

    public GameManager()
    {
        if(!existSaveData)
        {
            //���� �����Ͱ� ������
            gameInfo.mainStageMax = 1;
            gameInfo.subStageMax = 1;
            gameInfo.mainStageCurr = 1;
            gameInfo.subStageCurr = 1;
        }
            remainMonster = monsterCount;
    }

    public void MonsterDie(int Drop_ID)
    {
        //���
        var data = DataTableMgr.GetTable<DropTable>().GetMonsterData(Drop_ID);
        player.GetItem(data);

        remainMonster -= 1;
        if (remainMonster > 0) return;

        
        remainMonster = monsterCount;
        Debug.Log("�������� Ŭ����");
        player.StageClear();  //�������� Ŭ����

        if (gameInfo.mainStageMax == gameInfo.mainStageCurr && gameInfo.subStageMax == gameInfo.subStageCurr)
        {
            gameInfo.subStageMax++; 
            if(gameInfo.subStageMax == 11) 
            {
                gameInfo.subStageMax = 1;
                gameInfo.mainStageMax++;
            }
        }

        if (!enterNext) return; 

        gameInfo.subStageCurr++; 
        if (gameInfo.subStageCurr == 11) 
        {
            gameInfo.subStageCurr = 1;
            gameInfo.mainStageCurr++;
        }

        SetStageText();
    }

    public void PlayerDie()
    {

        if(gameInfo.mainStageCurr == 1 &&  gameInfo.subStageCurr == 1) { return; }

        Debug.Log("�������� �϶�");
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
        stage.SetText($"���� �������� : KF {gameInfo.mainStageCurr} - {gameInfo.subStageCurr}");
    }

    public void StageLoopOnOff()
    {
        if(stageLoop == null) return;   
        if(enterNext)
        {
            //���� �������� ���� ��Ȱ��
            enterNext = !enterNext;
            stageLoop.SetText("�ݺ�\nON");
        }
        else
        {
            //���� �������� ���� Ȱ��
            enterNext = !enterNext;
            stageLoop.SetText("�ݺ�\nOFF");
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