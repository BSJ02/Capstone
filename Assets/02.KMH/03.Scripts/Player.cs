using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;

public enum PlayerState
{
    Idle,
    Moving,
    Attack,
    GetHit,
    Attack2,
    Stab,
    Charge,
    SpinAttack,
    MacigAttack01,
    MacigAttack02,
    MacigAttack03
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
        playerData.Hp = playerData.MaxHp;
        ResetActivePoint();
    }

    void Start()
    {
        anim = GetComponent<Animator>();
        cardData = FindObjectOfType<CardData>();

        isLive = true;
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
            case PlayerState.Attack:
                anim.SetInteger("State", 2);
                break;
            case PlayerState.GetHit:
                anim.SetInteger("State", 3);
                break;
            case PlayerState.Attack2:
                anim.SetInteger("State", 4);
                break;
            case PlayerState.Stab:
                anim.SetInteger("State", 5);
                break;
            case PlayerState.Charge:
                anim.SetInteger("State", 6);
                break;
            case PlayerState.SpinAttack:
                anim.SetInteger("State", 7);
                break;
            case PlayerState.MacigAttack01:
                anim.SetInteger("State", 8);
                break;
            case PlayerState.MacigAttack02:
                anim.SetInteger("State", 9);
                break;
            case PlayerState.MacigAttack03:
                anim.SetInteger("State", 10);
                break;
        }
    }

    public void AttackTwoAnim()
    {
        playerState = PlayerState.Attack2;
        cardData.waitAnim = false;
        StartCoroutine(ChangeStateDelayed(0));
        return;
    }

    public void StabAnim()
    {
        playerState = PlayerState.Stab;
        cardData.waitAnim = false;
        StartCoroutine(ChangeStateDelayed(0));
        return;
    }

    public void ChargeAnim()
    {
        playerState = PlayerState.Charge;
        cardData.waitAnim = false;
        StartCoroutine(ChangeStateDelayed(0));
        return;
    }

    public void SpinAttackAnim()
    {
        playerState = PlayerState.SpinAttack;
        cardData.waitAnim = false;
        StartCoroutine(ChangeStateDelayed(0));
        return;
    }

    public void MacigAttack01Anim()
    {
        playerState = PlayerState.MacigAttack01;
        cardData.waitAnim = false;
        StartCoroutine(ChangeStateDelayed(0));
        return;
    }

    public void MacigAttack02Anim()
    {
        playerState = PlayerState.MacigAttack02;
        cardData.waitAnim = false;
        StartCoroutine(ChangeStateDelayed(0));
        return;
    }

    public void MacigAttack03Anim()
    {
        playerState = PlayerState.MacigAttack03;
        cardData.waitAnim = false;
        StartCoroutine(ChangeStateDelayed(0));
        return;
    }


    // �÷��̾� ����
    public void ReadyToAttack(Monster monster)
    {
        float monsterHp = monster.monsterData.Hp;
        float randDamage = Random.Range(playerData.Damage, playerData.Damage);
        //float critcalDamage = monsterData.MinDamage + addDamage;

        monsterHp -= randDamage;

        transform.LookAt(monster.transform);
        playerState = PlayerState.Attack;
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

        anim.SetTrigger("Die");
    }
}
