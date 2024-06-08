using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 몬스터 기본 클래스
/// </summary>

public enum MonsterState
{
    Idle = 0, 
    Moving = 1, 
    Attack = 2,
    GetHit = 3,
    Skill = 4 
}


public enum AttackState
{
    GeneralAttack,
    SkillAttack
}

public class Monster : MonoBehaviour
{
    public MonsterData monsterData;
    private MonsterUI monster_UI;

    public MonsterState state;
    public AttackState attack;

    private Animator anim;

    private bool isLive;

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

    // [0] 애니메이션 초기화 및 이벤트
    public void Init()
    {
        state = MonsterState.Idle;
        anim.SetInteger("State", (int)state);

        attack = AttackState.GeneralAttack;
    }

    // [1] 몬스터 공격력 
    public void GetRandomDamage()
    {
        float randDamage = Random.Range(monsterData.MinDamage, monsterData.MaxDamage);
        monsterData.CurrentDamage = Mathf.RoundToInt(randDamage); 
    }

    //[1-2] 몬스터 실제 공격
    public void Attack(Player player)
    {
        switch (attack)
        {
            // 스킬 공격
            case AttackState.SkillAttack:
                // 플레이어 GetHit 처리는 각 몬스터 콜라이더 이벤트에서 처리 

                // 상태 및 애니메이션 제어
                state = MonsterState.Skill; 
                anim.SetInteger("State", (int)state); 

                monster_UI.GetMonsterDamage(); // UI 업데이트
                break;

            // 일반 공격 
            case AttackState.GeneralAttack:
                player.GetHit(monsterData.CurrentDamage);
                 
                // 상태 및 애니메이션 제어
                state = MonsterState.Attack;
                anim.SetInteger("State", (int)state); 

                monster_UI.GetMonsterDamage(); // UI 업데이트
                break;
        }
    }

    // [2] 몬스터 피격
    public void GetHit(float damage)
    {
        if (!isLive)
            return;

        float finalDamage = damage - monsterData.Amor;
        monsterData.Hp -= Mathf.FloorToInt(finalDamage);
        monster_UI.GetMonsterHp();

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

        // 리스트에서 제거
        BattleManager.instance.monsters.Remove(gameObject);

        anim.SetTrigger("Die");

        Destroy(gameObject, 4f);
    }

    public void HitPlayerEvent()
    {
        
    }
}
