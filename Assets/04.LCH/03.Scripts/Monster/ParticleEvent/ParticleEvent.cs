using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleEvent : MonoBehaviour
{
    Monster monster;

    public GameObject attack_effect;
    public GameObject getHit_effect;
    public GameObject die_effect;


    public void Attack_Event()
    {
        if (monster.state != MonsterState.Attack)
            return;
    }

    public void GetHit_Event()
    {
        if (monster.state != MonsterState.Attack)
            return;
    }

    public void Die_Event()
    {
        if (monster.state != MonsterState.Attack)
            return;
    }

    



}
