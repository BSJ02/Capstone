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
    Short,
    Long
}

public class Monster : MonoBehaviour
{
    public MonsterData monsterData;
    private MonsterUI monster_UI;

    public MonsterState state;
    public MonsterType monsterType;

    private Animator anim;

    private bool isLive;
    
    [SerializeField] public float critaical = 3; // CriticalDamage = MinDamage + critcal

    void Awake()
    {
        anim = GetComponent<Animator>();
        monster_UI = GetComponent<MonsterUI>();

        isLive = true; 

        monsterData.Hp = monsterData.MaxHp;
        monsterData.CurrentDamage = 0;
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
        monsterData.CurrentDamage = Mathf.RoundToInt(randDamage);
        float critcalDamage = monsterData.MinDamage + critaical;

        if (randDamage >= critcalDamage)
        {
            // 크리티컬 공격
            player.GetHit(randDamage);

            state = MonsterState.CritcalAttack; // 애니메이션
            anim.SetInteger("State", (int)state);

            monster_UI.GetMonsterDamage(); // UI 업데이트
            Debug.Log("플레이어 체력:" + player.playerData.Hp + $", 크리티컬 공격:{(int)randDamage}");
            return;
        }
        else
        {
            // 일반 공격
            player.GetHit(randDamage);

            state = MonsterState.Attack; // 애니메이션
            anim.SetInteger("State", (int)state);

            monster_UI.GetMonsterDamage(); // UI 업데이트
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

        float finalDamage = damage - monsterData.Amor;
        monsterData.Hp -= Mathf.FloorToInt(finalDamage);
        monster_UI.GetMonsterHp();

        Debug.Log("몬스터 피격 당한 데미지:" + (int)finalDamage);

        // 몬스터 사망
        if (monsterData.Hp <= 0)
        {
            isLive = false;
            Die();
        }

        state = MonsterState.GetHit;
        anim.SetInteger("State", (int)state);
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


    // 후처리(사운드 및 이펙트)

    // 공격 시 이벤트
    public void EventToAttack()
    {
        // 공격 사운드 재생
        // 공격 효과
    }

    // 피격 시 이벤트
    public void EventToGetHit()
    {
        // ShowFloatingText();
        // 피격 사운드 재생
        // 피격 효과 
    }

    // 사망 후 이벤트
    public void EventToDie()
    {
        // 사망 사운드 재생
        // 사망 효과
    }
}
