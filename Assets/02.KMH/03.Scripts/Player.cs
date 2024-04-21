using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState
{
    Idle,
    Moving,
    Attack
}

public class Player : MonoBehaviour
{

    public MonsterData monsterData;
    public PlayerData playerData;

    private Animator anim;
    public PlayerState playerState;

    public int activePoint = 3;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        UpdateAnimation();
    }

    // 애니메이션 세팅
    public void UpdateAnimation()
    {
        switch (playerState)
        {
            case PlayerState.Idle:
                anim.SetInteger("State", 0);
                break;
            case PlayerState.Moving:
                anim.SetInteger("State", 1);
                break;
            case PlayerState.Attack:
                anim.SetInteger("State", 2);
                break;
        }
    }

    // 플레이어 공격
    public void ReadyToAttack()
    {
        Monster monster = FindObjectOfType<Monster>();

        float monsterHp = monster.monsterData.Hp;
        float randDamage = Random.Range(playerData.Damage, playerData.Damage);
        //float critcalDamage = monsterData.MinDamage + addDamage;

        monsterHp -= randDamage;

        transform.LookAt(monster.transform);
        playerState = PlayerState.Attack;
        Debug.Log("몬스터 체력:" + (int)monsterHp + $"플레이어{(int)randDamage} 공격!");
        return;
    }
}
