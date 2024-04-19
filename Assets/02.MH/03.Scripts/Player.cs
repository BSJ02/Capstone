using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public enum PlayerState
    {
        Idle,
        Moving,
        Attack
    }

    Animator anim;
    public PlayerState playerState;

    public int activePoint = 3;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
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

}
