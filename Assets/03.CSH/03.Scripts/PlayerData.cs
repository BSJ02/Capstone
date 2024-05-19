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
    
    public void ResetWarriorData()
    {
        MaxHp = 100f;
        Hp = MaxHp;
        Damage = 20f;
        Armor = 100f;
        CriticalHit = 0f;
        MaxActivePoint = 100;
        activePoint = MaxActivePoint;
    }

    public void ResetArcherData()
    {
        MaxHp = 80f;
        Hp = MaxHp;
        Damage = 10f;
        Armor = 70f;
        CriticalHit = 50f;
        MaxActivePoint = 100;
        activePoint = MaxActivePoint;
    }

    public void ResetWizardData()
    {
        MaxHp = 60;
        Hp = MaxHp;
        Damage = 30;
        Armor = 60;
        CriticalHit = 0f;
        MaxActivePoint = 100;
        activePoint = MaxActivePoint;
    }
}