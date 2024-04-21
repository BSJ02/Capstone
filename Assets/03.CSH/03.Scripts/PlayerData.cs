using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "ScriptableObject/PlayerData", order = 2)]
public class PlayerData : ScriptableObject
{
    public float Hp = 100f;
    public float MaxHp = 100f;
    public float Damage = 20f;
    public float Armor = 100f;
    public float CriticalHit = 0f;
}
