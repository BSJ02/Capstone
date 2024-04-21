using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 몬스터 담당 클래스
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
    public MonsterData monsterData; 

    public MonsterState state;
    public MonsterType monsterType;

    private Animator anim;

    protected bool isLive;
    public float critaical = 3; // CriticalDamage = MinDamage + critcal

    void Awake()
    {
        anim = GetComponent<Animator>();

        isLive = true; 
        monsterData.Hp = monsterData.MaxHp; 
    }

    void Start()
    {
        Init();
    }


    // [0] 애니메이션 초기화
    public void Init()
    {
        state = MonsterState.Idle;
        anim.SetInteger("State", (int)state);
    }

    // [1] 몬스터 공격 
    public void ReadyToAttack(Player player)
    {
        float randDamage = Random.Range(monsterData.MinDamage, monsterData.MaxDamage);
        float critcalDamage = monsterData.MinDamage + critaical;

        player.playerData.Hp -= randDamage;

        if (randDamage >= critcalDamage)
        {
            state = MonsterState.CritcalAttack;
            anim.SetInteger("State", (int)state);
            Debug.Log("플레이어 체력:" + player.playerData.Hp + $", 크리티컬 공격:{(int)randDamage}");
            return;
        }
        else
        {
            state = MonsterState.Attack;
            anim.SetInteger("State", (int)state);
            Debug.Log("플레이어 체력:" + (int)player.playerData.Hp + $", 일반 공격:{(int)randDamage}");
            return;
        }
    }


    // [2] 몬스터 피격
    public void GetHit(float damage)
    {
        if (!isLive)
            return;

        damage = FindObjectOfType<Player>().playerData.Damage;
        monsterData.Hp -= damage;

        if (monsterData.Hp <= 0)
        {
            Die();
        }

        state = MonsterState.GetHit;
        anim.SetInteger("State", (int)state);

    }

    // [2-1] 피격 시 이벤트
    public void EventToGetHit()
    {
        // 사운드 재생
        // 피격 효과 
    }


    // [3] 몬스터 사망
    public void Die()
    {
        if (isLive)
            return;

        // isWall 해제
        MapGenerator.instance.totalMap[(int)transform.position.x, (int)transform.position.z]
            .SetCoord((int)transform.position.x, (int)transform.position.z, false);

            anim.SetTrigger("Die");
    }

    // [3-1] 사망 후 이벤트
    public void EventToDie()
    {
        // 사운드 재생
        // 사망 효과
    }
}
