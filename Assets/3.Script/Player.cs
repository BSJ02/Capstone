using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum State
{
    Idle,
    Moving,
    Attack
}

public class Player : MonoBehaviour
{
    public Animator anim;
    public State state;

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
        switch (state)
        {
            case State.Idle:
                anim.SetInteger("State", 0);
                break;
            case State.Moving:
                anim.SetInteger("State", 1);
                break;
            case State.Attack:
                anim.SetInteger("State", 2);
                break;
        }
    }

}
