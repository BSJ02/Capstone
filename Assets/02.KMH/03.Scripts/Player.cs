using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public enum PlayerState
{
    Idle,
    Moving,
    Attack,
    GetHit
}

public class Player : MonoBehaviour
{
    public PlayerData playerData;

    private Animator anim;
    public PlayerState playerState;

    protected bool isLive;

    private void Awake()
    {
        playerData.Hp = playerData.MaxHp;
        playerData.activePoint = 4;
    }

    void Start()
    {
        anim = GetComponent<Animator>();

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
        }
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

        playerData.Hp -= damage;

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
