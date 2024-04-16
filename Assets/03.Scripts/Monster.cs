using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum State
{
    Idle, // 플레이어 턴 
    Moving, // 플레이어 턴 종료 후 몬스터 이동
    Attack // 이동한 위치의 사정거리 안에 플레이어가 있으면 공격
}

public enum MonsterType
{
    Normal,
    Boss
}

/// <summary>
/// 전체 몬스터 담당 클래스
/// </summary>
public class Monster : MonoBehaviour
{
    private MapGenerator mapGenerator;
    public NormalMonsterData normalMonster; // 몬스터마다 해당하는 데이터 값 넣기

    public State state = State.Idle;
    public MonsterType monsterType;

    private Animator anim;

    protected bool isLive = true;

    void Awake()
    {
        anim = GetComponent<Animator>();
        // 콜라이더(충돌 처리)

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

    // 초기화
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

    // 애니메이션
    public void UpdateAnimation()
    {
        switch (state)
        {
            // 애니메이션 loop 확인
            case State.Idle:
                anim.SetInteger("Idle", 0);
                break;
            case State.Moving:
                anim.SetInteger("Moving", 1);
                break;
        }
    }


    // 일반 몬스터 공격
    public void Attack()
    {
        if (state != State.Attack)
            return;

        // 애니메이션 loop 체크 해제
        anim.SetInteger("Attack", 2);

        float randDamage = Random.Range(normalMonster.MinDamage, normalMonster.MaxDamage);
        float playerHp = FindObjectOfType<Player>().hp; // PlayerData 받아올 예정

        // 플레이어 생존 여부
        while(playerHp > 0)
        {
            playerHp -= randDamage;
            Debug.Log("Player HP:" + playerHp);
            if (playerHp <= 0)
            {
                // 플레이어 Die 이벤트 호출
                Debug.Log("Player Die!");
                break;
            }
        }

        state = State.Idle;
    }

    // 몬스터 피격 처리
    public void TakeDamage()
    {
        if (!isLive) // 몬스터 사망
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

    // 몬스터 사망 처리
    public void Die()
    {
        if (isLive) // 몬스터 생존
            return;

            anim.SetBool("Die", true);

            // 파티클 생성
            // 몬스터 사망 사운드 재생
            // 풀링 오브젝트
    }
}
