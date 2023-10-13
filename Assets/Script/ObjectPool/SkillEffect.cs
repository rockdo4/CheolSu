using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillEffect : PoolAble
{
    [SerializeField]
    private List<ParticleSystem> magma;
    [SerializeField]
    private List<ParticleSystem> explosion;

    public SkillInfo currentSkill;
    public Player player;


    private void Awake()
    {
        foreach(ParticleSystem p in magma)
        {
            p.gameObject.SetActive(false);
        }
        foreach(ParticleSystem p in explosion)
        {
            p.gameObject.SetActive(false);
        }
    }

    public void SkillMagma(Player player, SkillInfo info, Transform pos)
    {
        currentSkill = info;
        this.player = player;

        switch (info.level)
        {
            case < SkillInfo.Skill_Tier1:
                ActiveSkill(player, info, pos, magma[0]);
                break;
            case < SkillInfo.Skill_Tier2:
                ActiveSkill(player, info, pos, magma[1]);
                break;
            case < SkillInfo.Skill_Tier3:
                ActiveSkill(player, info, pos, magma[2]);
                break;
            default:
                break;
        }
    }

    public void SkillExplosion(Player player, SkillInfo info)
    {
        currentSkill = info;
        this.player = player;

        switch (info.level)
        {
            case < SkillInfo.Skill_Tier1:
                ActiveSkill(player, info, player.enemy.transform, explosion[0]);
                break;
            case < SkillInfo.Skill_Tier2:
                ActiveSkill(player, info, player.enemy.transform, explosion[1]);
                break;
            case < SkillInfo.Skill_Tier3:
                ActiveSkill(player, info, player.enemy.transform, explosion[2]);
                break;
            default:
                break;
        }
    }

    private void ActiveSkill(Player player, SkillInfo info, Transform pos, ParticleSystem p)
    {
        
        p.gameObject.SetActive(true);
        p.transform.position = pos.transform.position;
        p.Stop();
        p.Play();

        StartCoroutine(Release(p));
    }

    IEnumerator TakeDot(Creature enemy, int damage, int count)
    {
        while (count > 0)
        {
            yield return new WaitForSeconds(0.2f);
            enemy.TakeDamage(damage);
            count--;
        }
        yield return null;
    }

    IEnumerator Release(ParticleSystem p)
    {
        yield return new WaitForSeconds(1);
        p.gameObject.SetActive(false);
        Pool.Release(this.gameObject);
    }
}
