using DG.Tweening;
using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatus
{
    public int _gold = 0;
    public int _dragon = 0;
    public int _diamond = 0;
    public int _exp = 0;
    public int _level = 1;
    public int _levelPoint = 0;
    public int _MAP = 0;
    public int _GOD = 0;
    public float attackDelay = 2f;
}

public class Item
{
    public GachaData data;
    public int quantity;
    public int enhance;

    public Item(GachaData data, int quantity, int enhance)
    {
        this.data = data;
        this.quantity = quantity;
        this.enhance = enhance;
    }
}


public class Player : Creature
{
    public Image HPUI;
    public GameObject background;
    public PlayerInfo playerInfo;
    public List<TextMeshProUGUI> uiList;
    public List<Text> uiList2;

    private List<BackGroundScroll> backGroundScrolls;

    private float lastAttackTime;

    private Animator animator;
    public Creature enemy = null;   

    public PlayerStatus status = new PlayerStatus();
    public Dictionary<GachaData, Item> itemList = new Dictionary<GachaData, Item>();

    private int nextExp;

    private void Awake()
    {
        
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
        backGroundScrolls = background.GetComponentsInChildren<BackGroundScroll>().ToList();
        GachaManager.Instance.SetPlayer(this);

        var table = DataTableMgr.GetTable<GachaTable>();

        foreach (var data in table.m_WeaponList)
        {
            itemList.Add(data, new Item(data, 0, 0));
        }
        foreach (var data in table.m_ArmorList)
        {
            itemList.Add(data, new Item(data, 0, 0));
        }

        MaxHealth = 10;
        Damage = 2;
        currentHealth = MaxHealth;

        animator.SetBool("Move", true);
        for(int i=0; i<backGroundScrolls.Count; i++)
        {
            backGroundScrolls[i].enabled = true;
        }

        var list = DataTableMgr.GetTable<MonsterTable>();

        //UpdateInterface();
    }

    // Update is called once per frame
    private void Update()
    {
        if (dead) return;

        Attack();

        if(Input.GetKeyDown(KeyCode.PageUp))
        {
            status._gold += 10000;
            UpdateInterface();
        }
    }

    private void Attack()
    {
        if (enemy == null) return;
        if (lastAttackTime + status.attackDelay > Time.time) return;

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

    public void AbsoluteTakeDamage(int damage)
    {
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

        nextExp = playerInfo.CheckLevelUp(this);

        uiList[0].SetText($"EXP : {status._exp} / {nextExp}");
        uiList[1].SetText($"{status._level}");
        uiList[2].SetText($"{status._gold} G");

        uiList2[0].text = $"체력 : {currentHealth}/{MaxHealth}";
        uiList2[1].text = $"공격력 : {Damage}";
        uiList2[2].text = $"마력 : {status._MAP}";
        uiList2[3].text = $"신력 : {status._GOD}";
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

    public void PlayerKill()
    {
        AbsoluteTakeDamage(MaxHealth);
    }
}