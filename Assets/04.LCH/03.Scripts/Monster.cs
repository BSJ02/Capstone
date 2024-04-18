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
    GetHit = 3
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

    public GameObject warning;
    private Animator anim;

    protected bool isLive;
    private bool hasTarget;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        isLive = true;
        hasTarget = false;
        warning.gameObject.SetActive(false);

        monsterData.Hp = monsterData.MaxHp;

        state = MonsterState.Idle;
        anim.SetInteger("State", (int)state);

    }


    void Update()
    {
        Debug.Log("���� ���� :" + state);
    }

    // [1] �Ϲ� ���� ����
    public void ReadyToAttack()
    {
        hasTarget = true;
        warning.gameObject.SetActive(true);

        state = MonsterState.Attack;
        anim.SetInteger("State", (int)state);

    }

    // [1-1] �Ϲ� ���� ���� �̺�Ʈ ó��
    public void EventToAttack()
    {
        if (hasTarget)
        {
            Player player = FindObjectOfType<Player>();
            float playerHp = player.playerData.Hp;
            float randDamage = Random.Range(monsterData.MinDamage, monsterData.MaxDamage);

            playerHp -= randDamage;

            state = MonsterState.Idle;
            anim.SetInteger("State", (int)state);

            Debug.Log("�÷��̾� ü��:" + playerHp);

            warning.gameObject.SetActive(false);
            hasTarget = false;
        }

    }

    // [2] ���� �ǰ� ó��
    public void GetHit()
    {
        if (!isLive) // ���� ���
            return;

        float playerDamage = FindObjectOfType<Player>().playerData.Damage;
        monsterData.Hp -= playerDamage;
       
        if(monsterData.Hp <= 0)
        {
            Die();
        }
    }

    // [2] ���� �ǰ� �̺�Ʈ ó��
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

    // [3-1] ���� ��� �̺�Ʈ ó��
    public void EventToDie()
    {
        // ��ƼŬ ����
        // ���� ��� ���� ���
    }
}
