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
        Debug.Log("���� ü��:" + (int)monsterHp + $"�÷��̾�{(int)randDamage} ����!");


        monster.GetHit(playerData.Damage);

        return;
    }
}
