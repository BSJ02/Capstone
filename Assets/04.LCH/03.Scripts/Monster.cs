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

    // [0] 애니메이션 초기화
    public void Init()
    {
        state = MonsterState.Idle;
        anim.SetInteger("State", (int)state);
    }

    // [1] 몬스터 공격력 
    public void ReadyToAttack()
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
                player.GetHit(monsterData.CurrentDamage);

                state = MonsterState.CritcalAttack; // 애니메이션(파티클 + 사운드)
                anim.SetInteger("State", (int)state);

                monster_UI.GetMonsterDamage(); // UI 업데이트
                break;

            // 일반 공격 
            case AttackState.GeneralAttack:
                player.GetHit(monsterData.CurrentDamage);

                state = MonsterState.Attack; // 애니메이션(파티클 + 사운드)
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

        damage = FindObjectOfType<Player>().playerData.Damage;

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

            anim.SetTrigger("Die");
    }
}
