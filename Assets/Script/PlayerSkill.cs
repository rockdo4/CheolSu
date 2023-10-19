using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public struct SkillInfo
{
    public int maxLevel;
    public int level;
    public int cost;
    public float increaseCost;
    public SkillData data;

    public const int Skill_Tier1 = 50;
    public const int Skill_Tier2 = 100;
    public const int Skill_Tier3 = 201;
}

public class PlayerSkill : MonoBehaviour
{
    private Player player;

    private SkillInfo magmaInfo;
    private SkillInfo explosionInfo;
    private SkillInfo exerciseInfo;
    private SkillInfo fenceInfo;
    private SkillInfo desireInfo;

    public List<Text> magmaText;
    public List<Text> explosionText;
    public List<Text> exerciseText;
    public List<Text> fenceText;
    public List<Text> desireText;

    public List<Button> button;
    public List<Image> activeButton;


    private SkillTable skillTable;

    private bool autoSkill = false;
    public Transform magmaPos;

    private float magmaCoolTime;
    private float explosionCoolTime;
    public TextMeshProUGUI magmaTimer;
    public TextMeshProUGUI explosionTimer;
    public TextMeshProUGUI auto;

    private void Start()
    {
        SkillCostTable skillCostTable = DataTableMgr.GetTable<SkillCostTable>();
        
        player = GetComponentInParent<Player>();
        skillTable = DataTableMgr.GetTable<SkillTable>();

        if(!GameManager.existSaveData)
        {
            var magma = skillCostTable.GetCostData(0);
            magmaInfo.data = skillTable.GetSkillData(11000101);
            magmaInfo.maxLevel = magma.maxLevel;
            magmaInfo.cost = magma.cost;
            magmaInfo.increaseCost = magma.increaseCost;
            magmaInfo.level = 1;

            var explosion = skillCostTable.GetCostData(1);
            explosionInfo.data = skillTable.GetSkillData(11000201);
            explosionInfo.maxLevel = explosion.maxLevel;
            explosionInfo.cost = explosion.cost;
            explosionInfo.increaseCost = explosion.increaseCost;
            explosionInfo.level = 1;

            var exercise = skillCostTable.GetCostData(2);
            exerciseInfo.data = skillTable.GetSkillData(21000101);
            exerciseInfo.maxLevel = exercise.maxLevel;
            exerciseInfo.cost = exercise.cost;
            exerciseInfo.increaseCost = exercise.increaseCost;
            exerciseInfo.level = 1;

            var fence = skillCostTable.GetCostData(3);
            fenceInfo.data = skillTable.GetSkillData(21000102);
            fenceInfo.maxLevel = fence.maxLevel;
            fenceInfo.cost = fence.cost;
            fenceInfo.increaseCost = fence.increaseCost;
            fenceInfo.level = 1;

            var desire = skillCostTable.GetCostData(4);
            desireInfo.data = skillTable.GetSkillData(21000103);
            desireInfo.maxLevel = desire.maxLevel;
            desireInfo.cost = desire.cost;
            desireInfo.increaseCost = desire.increaseCost;
            desireInfo.level = 1;
        }
        else
        {
            if (SkillInfo.Skill_Tier1 <= magmaInfo.level && magmaInfo.level < SkillInfo.Skill_Tier2)
            {
                magmaInfo.data = skillTable.GetSkillData(11000102);
            }
            else if (SkillInfo.Skill_Tier2 <= magmaInfo.level && magmaInfo.level < SkillInfo.Skill_Tier3)
            {
                magmaInfo.data = skillTable.GetSkillData(11000103);
            }

            if (SkillInfo.Skill_Tier1 <= explosionInfo.level && explosionInfo.level < SkillInfo.Skill_Tier2)
            {
                explosionInfo.data = skillTable.GetSkillData(11000202);
            }
            else if (SkillInfo.Skill_Tier2 <= explosionInfo.level && explosionInfo.level < SkillInfo.Skill_Tier3)
            {
                explosionInfo.data = skillTable.GetSkillData(11000203);
            }
        }
        

        magmaCoolTime = 0;
        explosionCoolTime = 0;
        //데이터 있으면 로드
        //아래에 추가 해야함

        LockSkill();
    }

    private void Update()
    {
        if(magmaCoolTime > 0)
        {
            magmaTimer.gameObject.SetActive(true);
            magmaCoolTime -= Time.deltaTime;
            magmaTimer.SetText($"{Mathf.RoundToInt(magmaCoolTime)}");
        }
        else
        {
            magmaTimer.gameObject.SetActive(false);
        }

        if(explosionCoolTime > 0)
        {
            explosionTimer.gameObject.SetActive(true);
            explosionCoolTime -= Time.deltaTime;
            explosionTimer.SetText($"{Mathf.RoundToInt(explosionCoolTime)}");
        }
        else
        {
            explosionTimer.gameObject.SetActive(false);
        }

        if(autoSkill)
        {
            if (player.enemy == null) return;

            if(magmaCoolTime <= 0)  ActiveMagma();
            if(explosionCoolTime <= 0) ActiveExplosion();
        }
    }

    public void ActiveMagma()
    {
        if (player.status._level < magmaInfo.data.Skill_LearnLevel) return;
        if(magmaCoolTime > 0)
        {
            Debug.Log("쿨타임");
            return;
        }

        var obj = ObjectPoolManager.instance.GetGo("SkillEffect");
        obj.GetComponent<SkillEffect>().SkillMagma(player, magmaInfo, magmaPos);

        magmaCoolTime = magmaInfo.data.Skill_Cool;
    }

    public void ActiveExplosion()
    {
        if (player.status._level < explosionInfo.data.Skill_LearnLevel) return;
        if (player.enemy == null)
        {
            Debug.Log("대상이 없음");
            return;
        }
        if (explosionCoolTime > 0)
        {
            Debug.Log("쿨타임");
            return;
        }

        var obj = ObjectPoolManager.instance.GetGo("SkillEffect");
        obj.GetComponent<SkillEffect>().SkillExplosion(player, explosionInfo);

        explosionCoolTime = explosionInfo.data.Skill_Cool;
    }

    public void UpgradeMagma()
    {
        if(magmaInfo.level >= magmaInfo.maxLevel)
        {
            Debug.Log("최대 레벨");
            return;
        }

        var cost = magmaInfo.cost * Mathf.Pow(magmaInfo.increaseCost, magmaInfo.level - 1);

        if(player.status._gold < (int)cost)
        {
            Debug.Log("골드 모자람");
            return;
        }

        player.status._gold -= (int)cost;


        magmaInfo.level++;
        Debug.Log(("업그레이드", "현재 레벨: " + magmaInfo.level));

        if (magmaInfo.level == SkillInfo.Skill_Tier1)
        {
            magmaInfo.data = skillTable.GetSkillData(11000102);
        }
        else if(magmaInfo.level == SkillInfo.Skill_Tier2)
        {
            magmaInfo.data = skillTable.GetSkillData(11000103);
        }

        float ratio;
        if(magmaInfo.level < SkillInfo.Skill_Tier1)
        {
            ratio = magmaInfo.data.Skill_Dmg + (magmaInfo.data.Skill_LevelUpDmgIncrease * (magmaInfo.level - 1));
        }
        else if(magmaInfo.level < SkillInfo.Skill_Tier2)
        {
            ratio = magmaInfo.data.Skill_Dmg + (magmaInfo.data.Skill_LevelUpDmgIncrease * (magmaInfo.level - SkillInfo.Skill_Tier1));
        }
        else
        {
            ratio = magmaInfo.data.Skill_Dmg + (magmaInfo.data.Skill_LevelUpDmgIncrease * (magmaInfo.level - SkillInfo.Skill_Tier2));
        }   

        magmaText[0].text = $"LV. {magmaInfo.level}";
        magmaText[1].text = $"비용: {Mathf.RoundToInt(magmaInfo.cost * Mathf.Pow(magmaInfo.increaseCost, magmaInfo.level - 1))}G";
        magmaText[2].text = $"김철수가 용암의 힘을 조종해 적을 공격한다.\r\n강화 레벨에 따라 스킬이 강력해 진다.\r\n\r\n" +
            $"공격력의 {Mathf.RoundToInt(ratio * 100)}% 로 1회 타격한다.";
    }

    public void UpgradeExplosion()
    {
        if (explosionInfo.level >= explosionInfo.maxLevel)
        {
            Debug.Log("최대 레벨");
            return;
        }

        var cost = explosionInfo.cost * Mathf.Pow(explosionInfo.increaseCost, explosionInfo.level - 1);

        if (player.status._gold < (int)cost)
        {
            Debug.Log("골드 모자람");
            return;
        }

        player.status._gold -= (int)cost;
        explosionInfo.level++;
        Debug.Log(("업그레이드", "현재 레벨: " + explosionInfo.level));

        if (explosionInfo.level == SkillInfo.Skill_Tier1)
        {
            explosionInfo.data = skillTable.GetSkillData(11000202);
        }
        else if (explosionInfo.level == SkillInfo.Skill_Tier2)
        {
            explosionInfo.data = skillTable.GetSkillData(11000203);
        }

        float ratio;
        if (explosionInfo.level < SkillInfo.Skill_Tier1)
        {
            ratio = explosionInfo.data.Skill_Dmg + (explosionInfo.data.Skill_LevelUpDmgIncrease * (explosionInfo.level - 1));
        }
        else if (explosionInfo.level < SkillInfo.Skill_Tier2)
        {
            ratio = explosionInfo.data.Skill_Dmg + (explosionInfo.data.Skill_LevelUpDmgIncrease * (explosionInfo.level - SkillInfo.Skill_Tier1));
        }
        else
        {
            ratio = explosionInfo.data.Skill_Dmg + (explosionInfo.data.Skill_LevelUpDmgIncrease * (explosionInfo.level - SkillInfo.Skill_Tier2));
        }

        explosionText[0].text = $"LV. {explosionInfo.level}";
        explosionText[1].text = $"비용: {Mathf.RoundToInt(explosionInfo.cost * Mathf.Pow(explosionInfo.increaseCost, explosionInfo.level - 1))}G";
        explosionText[2].text = $"김철수가 적의 내면에 있는 용암을 폭발시킨다.\r\n강화 레벨에 따라 스킬이 강력해 진다.\r\n\r\n" +
            $"현재 공격력의 {Mathf.RoundToInt(ratio * 100)}% 만큼 적을 1회 타격한다.";
    }

    public void UpgradeExercise()
    {
        if (exerciseInfo.level >= exerciseInfo.maxLevel)
        {
            Debug.Log("최대 레벨");
            return;
        }

        var cost = exerciseInfo.cost * Mathf.Pow(exerciseInfo.increaseCost, exerciseInfo.level - 1);

        if (player.status._gold < (int)cost)
        {
            Debug.Log("골드 모자람");
            return;
        }

        player.Damage += exerciseInfo.data.Skill_LevelUpATKUpIncrease;

        player.status._gold -= (int)cost;
        exerciseInfo.level++;
        Debug.Log("업그레이드");

        exerciseText[0].text = $"LV. {exerciseInfo.level}";
        exerciseText[1].text = $"비용: {Mathf.RoundToInt(exerciseInfo.cost * Mathf.Pow(exerciseInfo.increaseCost, exerciseInfo.level - 1))}G";
        exerciseText[2].text = $"김철수가 틈틈히 운동을 하여 영구 공격력이\n2 만큼 증가한다.";
    }

    public void UpgradeFence()
    {
        if (fenceInfo.level >= fenceInfo.maxLevel)
        {
            Debug.Log("최대 레벨");
            return;
        }

        var cost = fenceInfo.cost * Mathf.Pow(fenceInfo.increaseCost, fenceInfo.level - 1);

        if (player.status._gold < (int)cost)
        {
            Debug.Log("골드 모자람");
            return;
        }
        player.status.attackDelay -= fenceInfo.data.Skill_LevelUpATKSpeedIncrease;

        player.status._gold -= (int)cost;
        fenceInfo.level++;
        Debug.Log("업그레이드");

        fenceText[0].text = $"LV. {fenceInfo.level}";
        fenceText[1].text = $"비용: {Mathf.RoundToInt(fenceInfo.cost * Mathf.Pow(fenceInfo.increaseCost, fenceInfo.level - 1))}G";
        fenceText[2].text = $"김철수가 칼질을 연습하여 공격속도가 \r\n0.5% 만큼 증가한다.";
    }

    public void UpgradeDesire()
    {
        if (desireInfo.level >= desireInfo.maxLevel)
        {
            Debug.Log("최대 레벨");
            return;
        }

        var cost = desireInfo.cost * Mathf.Pow(desireInfo.increaseCost, desireInfo.level - 1);

        if (player.status._gold < (int)cost)
        {
            Debug.Log("골드 모자람");
            return;
        }

        player.status._MAP += desireInfo.data.Skill_LevelUpMAPIncrease;
        player.status._GOD += desireInfo.data.Skill_LevelUpGODIncrease;

        player.status._gold -= (int)cost;
        desireInfo.level++;
        Debug.Log("업그레이드");

        desireText[0].text = $"LV. {desireInfo.level}";
        desireText[1].text = $"비용: {Mathf.RoundToInt(desireInfo.cost * Mathf.Pow(desireInfo.increaseCost, desireInfo.level - 1))}G";
        desireText[2].text = $"김철수가 마신에게 기도하여 마력과 신력이 \r\n각각 5 증가한다.";
    }

    public void AutoOnOff()
    {
        autoSkill = !autoSkill;
        if (autoSkill)
            auto.SetText("Auto\nOn");
        else
            auto.SetText("Auto\nOFF");
    }

    public void UnlockSkill()
    {
        if(player.status._level == magmaInfo.data.Skill_LearnLevel)
        {
            button[0].gameObject.SetActive(true);
            activeButton[0].color = Color.white;
        }
        else if (player.status._level == explosionInfo.data.Skill_LearnLevel)
        {
            button[1].gameObject.SetActive(true);
            activeButton[1].color = Color.white;
        }
        else if (player.status._level == exerciseInfo.data.Skill_LearnLevel)
        {
            button[2].gameObject.SetActive(true);
        }
        else if (player.status._level == fenceInfo.data.Skill_LearnLevel)
        {
            button[3].gameObject.SetActive(true);
        }
        else if (player.status._level == desireInfo.data.Skill_LearnLevel)
        {
            button[4].gameObject.SetActive(true);
        }
    }

    public void LockSkill()
    {
        if (player.status._level < magmaInfo.data.Skill_LearnLevel)
        {
            button[0].gameObject.SetActive(false);
            activeButton[0].color = Color.gray;
        }

        if (player.status._level < explosionInfo.data.Skill_LearnLevel)
        {
            button[1].gameObject.SetActive(false);
            activeButton[1].color = Color.gray;
        }

        if (player.status._level < exerciseInfo.data.Skill_LearnLevel)
            button[2].gameObject.SetActive(false);

        if (player.status._level < fenceInfo.data.Skill_LearnLevel)
            button[3].gameObject.SetActive(false);

        if (player.status._level < desireInfo.data.Skill_LearnLevel)
            button[4].gameObject.SetActive(false);
    }

    public void DataSaveProcess()
    {
        Data.instance.skillInfosList.Clear();

        Data.instance.skillInfosList.Add(magmaInfo);
        Data.instance.skillInfosList.Add(explosionInfo);
        Data.instance.skillInfosList.Add(exerciseInfo);
        Data.instance.skillInfosList.Add(fenceInfo);    
        Data.instance.skillInfosList.Add(desireInfo);
    }

    public void DataLoadProcess(SaveDataV1 data)
    {
        magmaInfo = data.skillInfosList[0];
        explosionInfo = data.skillInfosList[1];
        exerciseInfo = data.skillInfosList[2];
        fenceInfo = data.skillInfosList[3];
        desireInfo = data.skillInfosList[4];
    }
}
