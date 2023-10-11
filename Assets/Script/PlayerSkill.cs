using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Skill
{
    public int maxLevel;
    public int level;
    public int cost;
    public float increaseCost;
    public SkillData data;
}

public class PlayerSkill : MonoBehaviour
{
    private Player player;

    private const int Skill_Tier1 = 50;
    private const int Skill_Tier2 = 100;
    private const int Skill_Tier3 = 201;

    [SerializeField]
    private List<ParticleSystem> magma;
    private Skill magmaInfo;

    [SerializeField]
    private List<ParticleSystem> explosion;
    private Skill explosionInfo;

    private SkillTable skillTable;
    

    private void Start()
    {
        player = GetComponentInParent<Player>();
        skillTable = DataTableMgr.GetTable<SkillTable>();

        magmaInfo.data = skillTable.GetSkillData(11000101);
        //explosionInfo.data = skillTable.GetSkillData(11000201);
    }

    public void UseMagma(Creature enemy, int damage)
    {
        switch(magmaInfo.level)
        {
            case < Skill_Tier1:
                MagmaTier1(enemy, damage);
                break;
            case < Skill_Tier2:
                MagmaTier2(enemy, damage);
                break;
            case < Skill_Tier3:
                MagmaTier3(enemy, damage);
                break;
            default: 
                break;
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            MagmaTier1(default, default);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            MagmaTier2(default, default);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            MagmaTier3(default, default);
        }
    }

    public void UseExplosion()
    {

    }

    public void MagmaTier1(Creature enemy, int damage)
    {
        var effect = Instantiate(magma[0], player.transform.position, Quaternion.identity);
        effect.Stop();
        effect.Play();
    }

    public void MagmaTier2(Creature enemy, int damage)
    {
        var effect = Instantiate(magma[1], player.transform.position, Quaternion.identity);
        effect.Stop();
        effect.Play();
    }

    public void MagmaTier3(Creature enemy, int damage)
    {
        var effect = Instantiate(magma[2], player.transform.position, Quaternion.identity);
        effect.Stop();
        effect.Play();
    }

    public void ExplosionTier1()
    {
        explosion[0].Play();
    }

    public void ExplosionTier2()
    {
        explosion[1].Play();
    }

    public void ExplosionTier3()
    {
        explosion[2].Play();
    }

    IEnumerator TakeDot(Creature enemy, int damage, int count)
    {
        while(count > 0)
        {
            yield return new WaitForSeconds(0.2f);
            enemy.TakeDamage(damage);
            count--;
        }
        yield return null;
    }
}
