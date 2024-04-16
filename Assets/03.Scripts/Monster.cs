using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum State
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

/// <summary>
/// ��ü ���� ��� Ŭ����
/// </summary>
public class Monster : MonoBehaviour
{
    private MapGenerator mapGenerator;
    public NormalMonsterData normalMonster; // ���͸��� �ش��ϴ� ������ �� �ֱ�

    public State state = State.Idle;
    public MonsterType monsterType;

    private Animator anim;

    protected bool isLive = true;

    void Awake()
    {
        anim = GetComponent<Animator>();
        // �ݶ��̴�(�浹 ó��)

        isLive = true;
        state = State.Idle;
    }

    public void OnEnable()
    {
        Initialize();
    }

    void Update()
    {
        UpdateAnimation();
    }

    // �ʱ�ȭ
    public void Initialize()
    {
        switch (monsterType)
        {
            case MonsterType.Normal:
                NormalMonsterData normalMonster = new NormalMonsterData();
                break;
            case MonsterType.Boss:
                BossMonsterData bossMonster = new BossMonsterData();
                break;
        }
    }

    // �ִϸ��̼�
    public void UpdateAnimation()
    {
        switch (state)
        {
            // �ִϸ��̼� loop Ȯ��
            case State.Idle:
                anim.SetInteger("Idle", 0);
                break;
            case State.Moving:
                anim.SetInteger("Moving", 1);
                break;
        }
    }


    // �Ϲ� ���� ����
    public void Attack()
    {
        if (state != State.Attack)
            return;

        // �ִϸ��̼� loop üũ ����
        anim.SetInteger("Attack", 2);

        float randDamage = Random.Range(normalMonster.MinDamage, normalMonster.MaxDamage);
        float playerHp = FindObjectOfType<Player>().hp; // PlayerData �޾ƿ� ����

        // �÷��̾� ���� ����
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
        }

        state = State.Idle;
    }

    // ���� �ǰ� ó��
    public void TakeDamage()
    {
        if (!isLive) // ���� ���
            return;

        float playeDamage = FindObjectOfType<Player>().damage;
        
        while(normalMonster.Hp > 0)
        {
            normalMonster.Hp -= playeDamage;
            if(normalMonster.Hp <= 0)
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
    }
}
