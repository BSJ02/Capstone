using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��ü ���� ��� Ŭ����
/// </summary>
/// 

public enum MonsterState
{
    Idle = 0, 
    Moving = 1, 
    Attack = 2,
    GetHit = 3,
    CritcalAttack = 4 
}

public enum MonsterType
{
    Normal,
    Boss
}

public class Monster : MonoBehaviour
{
    public MonsterData monsterData; // ���͸��� �ش��ϴ� ������ �� �ֱ�

    public MonsterState state;
    public MonsterType monsterType;

    private Animator anim;
    private SpriteRenderer warning;

    protected bool isLive;
    public float critcalDamage = 15;

    void Awake()
    {
        anim = GetComponent<Animator>();
        warning = GetComponentInChildren<SpriteRenderer>();

        isLive = true; // ������Ʈ Ȱ��ȭ �� 
        monsterData.Hp = monsterData.MaxHp; // ������Ʈ Ȱ��ȭ �� 

    }

    void Start()
    {
        Init();
    }


    // [0] ���� �ʱ�ȭ
    public void Init()
    {
        state = MonsterState.Idle;
        anim.SetInteger("State", (int)state);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GetHit();
        }
    }

    // [1] �Ϲ� ���� ����
    public void ReadyToAttack()
    {
        warning.enabled = true;
        warning.transform.position = transform.position;
        warning.transform.rotation = Quaternion.identity;

        Player player = FindObjectOfType<Player>();
        float playerHp = player.playerData.Hp;
        float randDamage = Random.Range(monsterData.MinDamage, monsterData.MaxDamage);

        playerHp -= randDamage;

        if(randDamage >= critcalDamage)
        {
            state = MonsterState.CritcalAttack;
            anim.SetInteger("State", (int)state);
            Debug.Log("�÷��̾� ü��:" + playerHp + $"����{(int)randDamage} ġ��Ÿ ����!");
            return;
        }
        else
        {
            state = MonsterState.Attack;
            anim.SetInteger("State", (int)state);
            Debug.Log("�÷��̾� ü��:" + (int)playerHp + $"����{(int)randDamage} ����!");
            return;
        }
    }


    // [2] ���� �ǰ� ó��
    public void GetHit()
    {
        if (!isLive) 
            return;

        float playerDamage = FindObjectOfType<Player>().playerData.Damage;
        monsterData.Hp -= playerDamage;

        Debug.Log("���� ü��" + (int)monsterData.Hp);

        if (monsterData.Hp <= 0)
        {
            Die();
        }

        state = MonsterState.GetHit;
        anim.SetInteger("State", (int)state);

    }

    // [2] ���� �ǰ� �� �̺�Ʈ ó��
    public void EventToGetHit()
    {
        // GetHit ���� ���
        // ���� �ǰ� �� Color ���� 
        
    }


    // [3] ���� ��� ó��
    public void Die()
    {
        if (isLive) 
            return;

            anim.SetBool("Die", true);
    }

    // [3-1] ���� ��� �� �̺�Ʈ ó��
    public void EventToDie()
    {
        // ��ƼŬ ����
        // ���� ��� ���� ���
    }
}
