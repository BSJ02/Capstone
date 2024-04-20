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
    public MonsterData monsterData; // 몬스터마다 해당하는 데이터 값 넣기

    public MonsterState state;
    public MonsterType monsterType;

    private Animator anim;
    private SpriteRenderer warning;

    protected bool isLive;
    public float addDamage = 3;

    void Awake()
    {
        anim = GetComponent<Animator>();
        warning = GetComponentInChildren<SpriteRenderer>();

        isLive = true; // 오브젝트 활성화 시 
        monsterData.Hp = monsterData.MaxHp; // 오브젝트 활성화 시 

    }

    void Start()
    {
        Init();
    }


    // [0] 몬스터 초기화
    public void Init()
    {
        state = MonsterState.Idle;
        anim.SetInteger("State", (int)state);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            /*GetHit();*/
            Die();
        }
        
    }

    // [1] 일반 몬스터 공격
    public void ReadyToAttack()
    {
        Player player = FindObjectOfType<Player>();

        float playerHp = player.playerData.Hp;
        float randDamage = Random.Range(monsterData.MinDamage, monsterData.MaxDamage);
        float critcalDamage = monsterData.MinDamage + addDamage;

        playerHp -= randDamage;

        if(randDamage >= critcalDamage)
        {
            state = MonsterState.CritcalAttack;
            anim.SetInteger("State", (int)state);
            Debug.Log("플레이어 체력:" + playerHp + $"몬스터{(int)randDamage} 치명타 공격!");
            return;
        }
        else
        {
            state = MonsterState.Attack;
            anim.SetInteger("State", (int)state);
            Debug.Log("플레이어 체력:" + (int)playerHp + $"몬스터{(int)randDamage} 공격!");
            return;
        }
    }


    // [2] 몬스터 피격 처리
    public void GetHit()
    {
        if (!isLive) 
            return;

        float playerDamage = FindObjectOfType<Player>().playerData.Damage;
        monsterData.Hp -= playerDamage;

        Debug.Log("몬스터 체력" + (int)monsterData.Hp);

        if (monsterData.Hp <= 0)
        {
            Die();
        }

        state = MonsterState.GetHit;
        anim.SetInteger("State", (int)state);

    }

    // [2] 몬스터 피격 후 이벤트 처리
    public void EventToGetHit()
    {
        // GetHit 사운드 재생
        // 몬스터 피격 시 Color 변경 
        
    }


    // [3] 몬스터 사망 처리
    public void Die()
    {
        /*if (isLive) 
            return;*/

        // 몬스터 사망 시 isWall 해제
        MapGenerator.instatnce.totalMap[(int)transform.position.x, (int)transform.position.z]
            .SetCoord((int)transform.position.x, (int)transform.position.z, false);

            anim.SetTrigger("Die");
    }

    // [3-1] 몬스터 사망 후 이벤트 처리
    public void EventToDie()
    {
        // 파티클 생성
        // 몬스터 사망 사운드 재생
    }
}
