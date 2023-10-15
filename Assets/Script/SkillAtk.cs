
using System.Collections;
using UnityEngine;


public class SkillAtk : MonoBehaviour
{
    private BoxCollider bc;
    private Rigidbody rb;
    private bool Once = false;
    private SkillInfo skillInfo;
    private Player player;
    private ParticleSystem ps;
    private ParticleSystem.Particle[] pList;

    private void OnEnable()
    {
        Once = false;
        skillInfo = GetComponentInParent<SkillEffect>().currentSkill;

        if(skillInfo.data.Skill_ID == 11000102 || skillInfo.data.Skill_ID == 11000103)
        {
            rb.velocity = Vector3.zero;
            rb.AddForce(400, 0, 0);
        }
    }
    private void Awake()
    {
        bc = GetComponent<BoxCollider>();
        rb = GetComponentInParent<Rigidbody>();
        ps = GetComponent<ParticleSystem>();
        player = GetComponentInParent<SkillEffect>().player;
    }

    private void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (Once) return;

        if (other.gameObject.tag != "Enemy") return;
        
        Once = true;
        var target = other.GetComponent<Monster>();

        DamageProgress(target);
    }

    private void DamageProgress(Monster target)
    {
        var damage = player.Damage;
        float ratio;
        var info = skillInfo.data;

        switch(skillInfo.data.Skill_ID)
        {
            case 11000101:
                ratio = info.Skill_Dmg + (info.Skill_LevelUpDmgIncrease * (skillInfo.level - 1));
                Debug.Log((ratio, damage));
                target.TakeDamage((int)ratio * damage);

                break;
            case 11000102:
                ratio = info.Skill_Dmg + (info.Skill_LevelUpDmgIncrease * (skillInfo.level - SkillInfo.Skill_Tier1));
                Debug.Log((ratio, damage));
                target.TakeDamage((int)ratio * damage);

                break;
            case 11000103:
                ratio = info.Skill_Dmg + (info.Skill_LevelUpDmgIncrease * (skillInfo.level - SkillInfo.Skill_Tier2));
                Debug.Log((ratio, damage));
                target.TakeDamage((int)ratio * damage);
                target.StartCoroutine(DotDamage(target, 10));

                break;
            case 11000201:
                ratio = info.Skill_Dmg + (info.Skill_LevelUpDmgIncrease * (skillInfo.level - 1));
                Debug.Log((ratio, damage));
                target.TakeDamage((int)ratio * damage);

                break;
            case 11000202:
                ratio = info.Skill_Dmg + (info.Skill_LevelUpDmgIncrease * (skillInfo.level - SkillInfo.Skill_Tier1));
                Debug.Log((ratio, damage));
                target.TakeDamage((int)ratio * damage);
                target.StartCoroutine(DotDamage(target, 10));

                break;
            case 11000203:
                ratio = info.Skill_Dmg + (info.Skill_LevelUpDmgIncrease * (skillInfo.level - SkillInfo.Skill_Tier2));
                Debug.Log((ratio, damage));
                target.TakeDamage((int)ratio * damage);
                target.StartCoroutine(DotDamage(target, 10));

                break;
        }
    }

    IEnumerator DotDamage(Monster target, int count)
    {
        while(count > 0)
        {
            yield return new WaitForSeconds(0.2f);
            if(target.dead)
            {
                count = 0;
                yield return null;
            }
            target.TakeDamage(player.Damage);
            count--;
        }
    }
}
