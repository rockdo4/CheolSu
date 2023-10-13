using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatus
{
    public int _gold = 2100000000;
    public int _dragon = 0;
    public int _diamond = 0;
    public int _exp = 0;
    public int _level = 1;
    public int _levelPoint = 0;
}

public class Player : Creature
{
    public Image HPUI;
    public GameObject background;
    public PlayerInfo playerInfo;
    public List<TextMeshProUGUI> uiList;

    private List<BackGroundScroll> backGroundScrolls;

    private float attackDelay = 2f;
    private float lastAttackTime;

    private Animator animator;
    public Creature enemy = null;   

    public PlayerStatus status = new PlayerStatus();

    private int nextExp;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        backGroundScrolls = background.GetComponentsInChildren<BackGroundScroll>().ToList();

    }

    private void Start()
    {
        MaxHealth = 10000;
        Damage = 100;
        currentHealth = MaxHealth;

        animator.SetBool("Move", true);
        for(int i=0; i<backGroundScrolls.Count; i++)
        {
            backGroundScrolls[i].enabled = true;
        }

        var list = DataTableMgr.GetTable<MonsterTable>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (dead) return;

        Attack();
    }

    private void Attack()
    {
        if (enemy == null) return;
        if (lastAttackTime + attackDelay > Time.time) return;

        animator.SetTrigger("Attack");
        lastAttackTime = Time.time;
    }

    private void GiveDamage()
    {
        if(enemy == null) return;
        enemy.TakeDamage(Damage);
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            animator.SetBool("Move", false);
            enemy = collision.gameObject.GetComponent<Monster>();
            for (int i = 0; i < backGroundScrolls.Count; i++)
            {
                backGroundScrolls[i].enabled = false;
            }
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if(collision.CompareTag("Enemy"))
        {
            animator.SetBool("Move", true);
            enemy = null;
            for (int i = 0; i < backGroundScrolls.Count; i++)
            {
                backGroundScrolls[i].enabled = true;
            }
        }
    }

    public void MonsterDie()
    {
        UpdateInterface();
        Invoke("StartMove", 0.5f);
    }

    private void StartMove()
    {
        animator.SetBool("Move", true);
        for (int i = 0; i < backGroundScrolls.Count; i++)
        {
            backGroundScrolls[i].enabled = true;
        }

        Debug.Log($"gold : {status._gold}, exp : {status._exp}, dgp : {status._dragon}");
    }

    override public void TakeDamage(int damage)
    {
        if (enemy.dead) return;
        base.TakeDamage(damage);
        if (dead)
        {
            animator.SetBool("Dead", true);
            StartCoroutine(DieProgress());
        }
        UpdateInterface();
    }

    public void UpdateInterface()
    {
        HPUI.fillAmount = (float)currentHealth / MaxHealth;
        HPUI.GetComponentInChildren<TextMeshProUGUI>().SetText($"{currentHealth} / {MaxHealth}");

        if (uiList.Count == 0) return;
        uiList[0].SetText($"{status._exp} / {nextExp}");
        uiList[1].SetText($"{Damage}");
        uiList[2].SetText($"{status._gold}");
        uiList[3].SetText($"{status._levelPoint}");
        uiList[4].SetText($"{status._level}");
    }

    public void RespawnPlayer()
    {
        //MonsterSpawner.Instance.SummonMonster();
        animator.SetBool("Dead", false);
        dead = false;

        currentHealth = MaxHealth;
        UpdateInterface();
    }

    IEnumerator DieProgress()
    {
        GameManager.Instance.PlayerDie();

        yield return new WaitForSeconds(3f);

        if (enemy != null) { Destroy(enemy.gameObject); }
        MonsterSpawner.Instance.SummonMonster();
        RespawnPlayer();
        StartMove();
    }

    public void StageClear()
    {
        currentHealth = MaxHealth;
        UpdateInterface();
    }

    public void GetItem(DropData itemTable)
    {
        status._exp += itemTable.Monster_EXP;
        status._gold += itemTable.Monster_Gold;
        status._dragon += itemTable.Monster_DGP;

        nextExp = playerInfo.CheckLevelUp(this);
        UpdateInterface();
    }
}