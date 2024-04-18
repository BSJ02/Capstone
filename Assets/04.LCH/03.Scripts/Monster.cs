using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��ü ���� ��� Ŭ����
/// </summary>
/// 

public enum MonsterState
{
    Idle, // �÷��̾� �� 
    Moving, // �÷��̾� �� ���� �� ���� �̵�
    Attack // �̵��� ��ġ�� �����Ÿ� �ȿ� �÷��̾ ������ ����
}

public enum MonsterType
{
    Normal,
    Boss
}

public class Monster : MonoBehaviour
{
    private MapGenerator mapGenerator;
    public MonsterData monsterData; // ���͸��� �ش��ϴ� ������ �� �ֱ�

    public MonsterState state = MonsterState.Idle;
    public MonsterType monsterType;

    private Animator anim;

    protected bool isLive = true;

    void Awake()
    {
        anim = GetComponent<Animator>();
        // �ݶ��̴�(�浹 ó��)

        isLive = true;
        state = MonsterState.Idle;
    }

    public void OnEnable()
    {
        Initialize();
    }

    void Update()
    {
        /*UpdateAnimation();*/
    }

    // �ʱ�ȭ
    public void Initialize()
    {
        switch (monsterType)
        {
            case MonsterType.Normal:
                break;
            case MonsterType.Boss:

                break;
        }
    }

    /*// �ִϸ��̼�
    public void UpdateAnimation()
    {
        switch (state)
        {
            // �ִϸ��̼� loop Ȯ��
            case MonsterState.Idle:
                anim.SetInteger("Idle", 0);
                break;
            case MonsterState.Moving:
                anim.SetInteger("Moving", 1);
                break;
        }
    }*/


    /*// �Ϲ� ���� ����
    public void Attack()
    {
        if (state != MonsterState.Attack)
            return;

        // �ִϸ��̼� loop üũ ����
        anim.SetInteger("Attack", 2);

        float randDamage = Random.Range(monsterData.MinDamage, monsterData.MaxDamage);
        *//*float playerHp = FindObjectOfType<Player>().hp; // PlayerData �޾ƿ� ����*/

     /*   // �÷��̾� ���� ����
        while(playerHp > 0)
        {
            playerHp -= randDamage;
            Debug.Log("Player HP:" + playerHp);
            if (playerHp <= 0)
            {
                // �÷��̾� Die �̺�Ʈ ȣ��
                Debug.Log("Player Die!");
                break;
            }
        }*//*

        state = MonsterState.Idle;
    }*/

    // ���� �ǰ� ó��
    /*public void TakeDamage()
    {
        if (!isLive) // ���� ���
            return;

*//*        float playeDamage = FindObjectOfType<Player>().damage;*//*
        
        while(monsterData.Hp > 0)
        {
*//*            normalMonster.Hp -= playeDamage;*//*
            if(monsterData.Hp <= 0)
            {
                Die();
                break;
            }
        }
    }

    // ���� ��� ó��
    public void Die()
    {
        if (isLive) // ���� ����
            return;

            anim.SetBool("Die", true);

            // ��ƼŬ ����
            // ���� ��� ���� ���
            // Ǯ�� ������Ʈ
    }*/
}
