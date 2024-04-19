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

    public PlayerData playerData;

    private Animator anim;
    public PlayerState playerState;

    public int activePoint = 3;

    void Start()
    {
        anim = GetComponent<Animator>();
    }


}
