using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "ScriptableObject/PlayerData", order = 2)]
public class PlayerData : ScriptableObject
{
    public float Hp;
    public float MaxHp;

    public float Damage;
    public float Armor;

    public float CriticalHit;

    public int activePoint;
    public int MaxActivePoint;
    
}