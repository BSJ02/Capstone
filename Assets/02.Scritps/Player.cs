using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameEvent gameEvent;

    public float hp;
    public float damage;


    private void Awake()
    {
        hp = 100f;
        damage = 20f;
    }

    // �÷��̾� �� ���� 
    public void UpdateMonsterMove()
    {
        gameEvent.Raise();
    }
}
    
