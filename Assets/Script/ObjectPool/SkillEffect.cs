using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillEffect : PoolAble
{
    [SerializeField]
    private List<ParticleSystem> magma;
    [SerializeField]
    private List<ParticleSystem> explosion;

    public void SkillMagma(Player player, SkillInfo info, Transform pos)
    {
        switch (info.level)
        {
            case < SkillInfo.Skill_Tier1:
                magma[0].transform.position = pos.position;
                magma[0].Stop();
                magma[0].Play();
                Invoke("Release", 5f);
                break;
            case < SkillInfo.Skill_Tier2:
                magma[1].transform.position = pos.position;
                magma[1].Stop();
                magma[1].Play();
                Invoke("Release", 5f);
                break;
            case < SkillInfo.Skill_Tier3:
                magma[2].transform.position = pos.position;
                magma[2].Stop();
                magma[2].Play();
                Invoke("Release", 5f);
                break;
            default:
                break;
        }
    }

    public void SkillExplosion(Player player, SkillInfo info)
    {
        switch (info.level)
        {
            case < SkillInfo.Skill_Tier1:
                explosion[0].transform.position = player.enemy.transform.position;
                explosion[0].Stop();
                explosion[0].Play();
                Invoke("Release", 5f);
                break;
            case < SkillInfo.Skill_Tier2:
                explosion[1].transform.position = player.enemy.transform.position;
                explosion[1].Stop();
                explosion[1].Play();
                Invoke("Release", 5f);
                break;
            case < SkillInfo.Skill_Tier3:
                explosion[2].transform.position = player.enemy.transform.position;
                explosion[2].Stop();
                explosion[2].Play();
                Invoke("Release", 5f);
                break;
            default:
                break;
        }
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

    private void StopAllExcept(ParticleSystem obj)
    { 
        foreach(var particle in magma)
        {
            if (particle == obj) continue;
            particle.Stop();
        }
        foreach (var particle in explosion)
        {
            if (particle == obj) continue;
            particle.Stop();
        }
    }

    private void Release()
    {
        Debug.Log("¹ÝÈ¯");
        Pool.Release(this.gameObject);
    }
}
