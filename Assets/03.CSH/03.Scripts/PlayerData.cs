using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "ScriptableObject/PlayerData", order = 2)]
public class PlayerData : ScriptableObject
{
    public float Hp;
    public float MaxHp = 100f;

    public float Damage = 20f;
    public float Armor = 100f;

    public float CriticalHit = 0f;

    public int activePoint;
    public int MaxActivePoint = 4;
    
    public void ResetPlayerData()
    {
        MaxHp = 100f;
        Hp = MaxHp;
        Damage = 20f;
        Armor = 100f;
        CriticalHit = 0f;
        MaxActivePoint = 100;
        activePoint = MaxActivePoint;
    }
}