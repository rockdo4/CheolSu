using UnityEngine;

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

    private SkillTable skillTable;

    public Transform magmaPos;


    private void Start()
    {
        player = GetComponentInParent<Player>();
        skillTable = DataTableMgr.GetTable<SkillTable>();

        magmaInfo.data = skillTable.GetSkillData(11000101);
        explosionInfo.data = skillTable.GetSkillData(11000201);
        exerciseInfo.data = skillTable.GetSkillData(21000101);
        fenceInfo.data = skillTable.GetSkillData(21000102);
        desireInfo.data = skillTable.GetSkillData(21000103);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            ActiveMagma();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ActiveExplosion();
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            
        }
    }

    public void ActiveMagma()
    {
        var obj = ObjectPoolManager.instance.GetGo("SkillEffect");
        obj.GetComponent<SkillEffect>().SkillMagma(player, magmaInfo, magmaPos);
    }

    public void ActiveExplosion()
    {
        var obj = ObjectPoolManager.instance.GetGo("SkillEffect");
        obj.GetComponent<SkillEffect>().SkillExplosion(player, explosionInfo);
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
        Debug.Log("업그레이드");

        if (magmaInfo.level == SkillInfo.Skill_Tier1)
        {
            magmaInfo.data = skillTable.GetSkillData(11000102);
        }
        else if(magmaInfo.level == SkillInfo.Skill_Tier2)
        {
            magmaInfo.data = skillTable.GetSkillData(11000103);
        }
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
        Debug.Log("업그레이드");

        if (explosionInfo.level == SkillInfo.Skill_Tier1)
        {
            explosionInfo.data = skillTable.GetSkillData(11000102);
        }
        else if (explosionInfo.level == SkillInfo.Skill_Tier2)
        {
            explosionInfo.data = skillTable.GetSkillData(11000103);
        }
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

        player.status._gold -= (int)cost;
        exerciseInfo.level++;
        Debug.Log("업그레이드");
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

        player.status._gold -= (int)cost;
        fenceInfo.level++;
        Debug.Log("업그레이드");
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

        player.status._gold -= (int)cost;
        desireInfo.level++;
        Debug.Log("업그레이드");
    }
}
