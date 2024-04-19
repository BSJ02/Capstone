using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 전체 몬스터 담당 클래스
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
    public MonsterData monsterData; // 몬스터마다 해당하는 데이터 값 넣기

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
        Debug.Log("현재 상태 :" + state);
    }

    // [1] 일반 몬스터 공격
    public void ReadyToAttack()
    {
        hasTarget = true;
        warning.gameObject.SetActive(true);

        state = MonsterState.Attack;
        anim.SetInteger("State", (int)state);

    }

    // [1-1] 일반 몬스터 공격 이벤트 처리
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

            Debug.Log("플레이어 체력:" + playerHp);

            warning.gameObject.SetActive(false);
            hasTarget = false;
        }

    }

    // [2] 몬스터 피격 처리
    public void GetHit()
    {
        if (!isLive) // 몬스터 사망
            return;

        float playerDamage = FindObjectOfType<Player>().playerData.Damage;
        monsterData.Hp -= playerDamage;
       
        if(monsterData.Hp <= 0)
        {
            Die();
        }
    }

    // [2] 몬스터 피격 이벤트 처리
    public void EventToGetHit()
    {
        // GetHit 사운드 재생
        // 몬스터 피격 시 Color 변경 
        
    }


    // [3] 몬스터 사망 처리
    public void Die()
    {
        if (isLive) 
            return;

            anim.SetBool("Die", true);
    }

    // [3-1] 몬스터 사망 이벤트 처리
    public void EventToDie()
    {
        // 파티클 생성
        // 몬스터 사망 사운드 재생
    }
}
