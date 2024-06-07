using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;

public enum PlayerState
{
    Idle = 0,
    Moving = 1,
    Attack1 = 2,
    GetHit = 3,
    Attack2 = 4,
    Stab = 5,
    Charge = 6,
    SpinAttack = 7,
    MacigAttack01 = 8,
    MacigAttack02 = 9,
    MacigAttack03 = 10,
    Defend = 11,
    RollFWD = 12,
    Victory = 13
}

public class Player : MonoBehaviour
{
    private GameObject playerChoice;

    public PlayerData playerData;
    private CardProcessing cardProcessing;

    private Animator anim;
    public PlayerState playerState;

    public bool isAttack;
    protected bool isLive;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        cardProcessing = FindObjectOfType<CardProcessing>();
        playerChoice = GameObject.FindGameObjectWithTag("PlayerChoice");

        /*playerData.ResetWarriorData();*/
        ResetActivePoint();

        playerData.Hp = playerData.MaxHp;
        isLive = true;

    }

    void Start()
    {
        IdleAnim();
    }

    private void Update()
    {
        UpdateAnimation();

        //test
        if(Input.GetKeyDown(KeyCode.D))
        {
            GetHit(1);
        }
    }

    // ActivePoint 초기화
    public void ResetActivePoint()
    {
        playerData.activePoint = playerData.MaxActivePoint;
    }


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
            case PlayerState.Attack1:
                anim.SetInteger("State", 2);
                break;
            case PlayerState.GetHit:
                anim.SetInteger("State", 3);
                break;
        }
    }

    public void IdleAnim()
    {
        playerState = PlayerState.Idle;
        anim.SetInteger("State", (int)playerState);
    }

    public void MovingAnim()
    {
        playerState = PlayerState.Idle;
        anim.SetInteger("State", (int)playerState);
    }

    public void AttackOneAnim(GameObject monster)
    {
        transform.LookAt(monster.transform);
        playerState = PlayerState.Attack1;
        anim.SetInteger("State", (int)playerState);

    }

    public void AttackTwoAnim(GameObject monster)
    {
        transform.LookAt(monster.transform);
        playerState = PlayerState.Attack2;
        anim.SetInteger("State", (int)playerState);

    }

    public void StabAnim(GameObject monster)
    {
        transform.LookAt(monster.transform);
        playerState = PlayerState.Stab;
        anim.SetInteger("State", (int)playerState);

    }

    public void ChargeAnim(GameObject monster)
    {
        transform.LookAt(monster.transform);
        playerState = PlayerState.Charge;
        anim.SetInteger("State", (int)playerState);

    }

    public void SpinAttackAnim(GameObject monster)
    {
        transform.LookAt(monster.transform);
        playerState = PlayerState.SpinAttack;
        anim.SetInteger("State", (int)playerState);

    }

    public void MacigAttack01Anim(GameObject monster)
    {
        transform.LookAt(monster.transform);
        playerState = PlayerState.MacigAttack01;
        anim.SetInteger("State", (int)playerState);

    }

    public void MacigAttack02Anim(GameObject monster)
    {
        transform.LookAt(monster.transform);
        playerState = PlayerState.MacigAttack02;
        anim.SetInteger("State", (int)playerState);

    }

    public void MacigAttack03Anim(GameObject monster)
    {
        transform.LookAt(monster.transform);
        playerState = PlayerState.MacigAttack03;
        anim.SetInteger("State", (int)playerState);
    }

    public void DefendAnim(GameObject monster)
    {
        transform.LookAt(monster.transform);
        playerState = PlayerState.Defend;
        anim.SetInteger("State", (int)playerState);
    }

    public void RollFWDAnim(GameObject monster)
    {
        transform.LookAt(monster.transform);
        playerState = PlayerState.RollFWD;
        anim.SetInteger("State", (int)playerState);
    }

    public void VictoryAnim(GameObject monster)
    {
        transform.LookAt(monster.transform);
        playerState = PlayerState.Victory;
        anim.SetInteger("State", (int)playerState);
    }


    public void ReadyToAttack(Monster monster)
    {
        SoundManager.instance.PlaySoundEffect("AttackPlayer");
        SoundManager.instance.PlaySoundEffect("AttackSword");

        float monsterHp = monster.monsterData.Hp;
        float randDamage = Random.Range(playerData.Damage, playerData.Damage);
        //float critcalDamage = monsterData.MinDamage + addDamage;

        monsterHp -= randDamage;

        transform.LookAt(monster.transform);
        playerState = PlayerState.Attack1;
        anim.SetInteger("State", (int)playerState);

        Debug.Log("몬스터 체력:" + (int)monsterHp + $"데미지{(int)randDamage}!");


        monster.GetHit(playerData.Damage);

        playerChoice.SetActive(false);
        isAttack = true;

        return;
    }

    // 애니메이션 초기화
    public void Init()
    {
        playerState = PlayerState.Idle;
        anim.SetInteger("State", (int)playerState);
    }

    public void GetHit(float damage)
    {
        SoundManager.instance.PlaySoundEffect("Hit");

        if (!isLive)
            return;

        playerData.Hp -= damage;

        if (playerData.Hp <= 0)
        {
            Die();
        }

        Debug.Log($"{playerData.Name}의 체력:" + (int)playerData.Hp);

        playerState = PlayerState.GetHit;
        anim.SetInteger("State", (int)playerState);

    }

    public void Die()
    {
        /*if (isLive) 
            return;*/
        SoundManager.instance.PlaySoundEffect("Death");

        anim.SetTrigger("Die");
        Destroy(gameObject, 3.5f);
        BattleManager.instance.players.Remove(gameObject);
    }

}
