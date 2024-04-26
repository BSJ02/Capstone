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
    MacigAttack03 = 10
}

public class Player : MonoBehaviour
{
    public PlayerData playerData;
    private CardData cardData;

    private Animator anim;
    public PlayerState playerState;

    protected bool isLive;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        cardData = FindObjectOfType<CardData>();

        playerData.Hp = playerData.MaxHp;
        ResetActivePoint();

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


    // �ִϸ��̼� ����
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
        cardData.waitAnim = false;
    }

    public void MovingAnim()
    {
        playerState = PlayerState.Idle;
        anim.SetInteger("State", (int)playerState);
        cardData.waitAnim = false;
    }

    public void AttackOneAnim()
    {
        playerState = PlayerState.Attack1;
        anim.SetInteger("State", (int)playerState);
        cardData.waitAnim = false;
    }

    public void AttackTwoAnim()
    {
        playerState = PlayerState.Attack2;
        anim.SetInteger("State", (int)playerState);
        cardData.waitAnim = false;
    }

    public void StabAnim()
    {
        playerState = PlayerState.Stab;
        anim.SetInteger("State", (int)playerState);
        cardData.waitAnim = false;
    }

    public void ChargeAnim()
    {
        playerState = PlayerState.Charge;
        anim.SetInteger("State", (int)playerState);
        cardData.waitAnim = false;
    }

    public void SpinAttackAnim()
    {
        playerState = PlayerState.SpinAttack;
        anim.SetInteger("State", (int)playerState);
        cardData.waitAnim = false;
    }

    public void MacigAttack01Anim()
    {
        playerState = PlayerState.MacigAttack01;
        anim.SetInteger("State", (int)playerState);
        cardData.waitAnim = false;
    }

    public void MacigAttack02Anim()
    {
        playerState = PlayerState.MacigAttack02;
        anim.SetInteger("State", (int)playerState);
        cardData.waitAnim = false;
    }

    public void MacigAttack03Anim()
    {
        playerState = PlayerState.MacigAttack03;
        anim.SetInteger("State", (int)playerState);
        cardData.waitAnim = false;
    }


    // �÷��̾� ����
    public void ReadyToAttack(Monster monster)
    {
        float monsterHp = monster.monsterData.Hp;
        float randDamage = Random.Range(playerData.Damage, playerData.Damage);
        //float critcalDamage = monsterData.MinDamage + addDamage;

        monsterHp -= randDamage;

        transform.LookAt(monster.transform);
        playerState = PlayerState.Attack1;
        Debug.Log("몬스터 체력:" + (int)monsterHp + $"데미지{(int)randDamage}!");


        monster.GetHit(playerData.Damage);

        StartCoroutine(ChangeStateDelayed(0));

        return;
    }

    //idle로 변경
    public IEnumerator ChangeStateDelayed(float delay)
    {
        yield return new WaitForSeconds(delay);
        playerState = PlayerState.Idle;
    }

    public void GetHit(float damage)
    {
        if (!isLive)
            return;

        int randNum = Random.Range(0, 100);
        
        if (randNum > playerData.CriticalHit)
        {
            Debug.Log("Critical!");
            playerData.Hp -= damage * (float)1.5;
        }
        else
        {
            playerData.Hp -= damage;
        }

        Debug.Log("남은 체력 : " + (int)playerData.Hp);

        if (playerData.Hp <= 0)
        {
            Die();
        }

        playerState = PlayerState.GetHit;
        StartCoroutine(ChangeStateDelayed(1));

    }

    // [3] ���� ��� ó��
    public void Die()
    {
        /*if (isLive) 
            return;*/

        // ���� ��� �� isWall ����
        MapGenerator.instance.totalMap[(int)transform.position.x, (int)transform.position.z]
            .SetCoord((int)transform.position.x, (int)transform.position.z, false);

        anim.SetTrigger("Die");
    }
}
