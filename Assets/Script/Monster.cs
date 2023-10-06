using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline.Actions;
using UnityEngine;
using UnityEngine.UI;

public class Monster : Creature
{
    public float speed = 0.1f;

    public Image HPUI;
    private Animator animator;
    private Rigidbody rb;
    private BoxCollider bc;
    private Player player = null;

    private float attackDelay = 3f;
    private float lastAttackTime;
    private bool move = false;

    //monster info
    public string ID;
    public string Name;
    public int Masin_Armor;
    public int Masin_Shield;
    public int Divine;
    public int Type;
    public int Drop_ID;


    // Start is called before the first frame update
    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        bc = GetComponent<BoxCollider>();
    }
    private void Start()
    {
        int mainStage = GameManager.Instance.gameInfo.mainStageCurr;
        int subStage = GameManager.Instance.gameInfo.subStageCurr;
        int currentStage = ((mainStage - 1) * 9) + subStage;

        var info = DataTableMgr.GetTable<MonsterTable>().GetMonsterData(currentStage - 1);

        MaxHealth = info.Monster_Hp;
        Damage = info.Monster_Atk;
        ID = info.Monster_ID;
        Name = info.Monster_Name;
        Masin_Armor = info.Masin_Armor;
        Masin_Shield = info.Masin_Shield;
        Divine = info.Monster_Divine;
        Type = info.Monster_Type;
        Drop_ID = info.Drop_ID;

        currentHealth = MaxHealth;
        Debug.Log($"KF{mainStage} - {subStage}, {ID}, {Name}, {MaxHealth}, {Damage}");

        Invoke("StartMove", 0.5f);
    }

    private void FixedUpdate()
    {
        if (move) MovePosition();
    }

    // Update is called once per frame  
    private void Update()
    {
        if(dead) return;

        Attack();
    }

    private void StartMove()
    {
        move = true;
        animator.SetBool("Move", true);
    }

    private void Attack()
    {
        if (player == null) return;
        if (lastAttackTime + attackDelay > Time.time) return;
        if (player.dead) return;

        animator.SetTrigger("Attack");
        lastAttackTime = Time.time;
    }

    private void GiveDamage()
    {
        if (player == null) return;
        player.TakeDamage(Damage);
    }

    private void Destroy()
    {
        GameManager.Instance.MonsterDie(Drop_ID);
        MonsterSpawner.Instance.SummonMonster();
        player.MonsterDie();
        Destroy(gameObject);
    }

    override public void TakeDamage(int damage)
    {
        base.TakeDamage(damage);

        HPUI.fillAmount = (float)currentHealth / MaxHealth;
        if (dead)
        {
            animator.SetBool("Death", true);
        }
    }

    private void MovePosition()
    {
        Vector3 pos = rb.position + Vector3.left * speed;
        rb.MovePosition(pos);
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            player = collision.gameObject.GetComponent<Player>();
            animator.SetBool("Move", false);
            move = false;
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            player = null;
        }
    }
}
