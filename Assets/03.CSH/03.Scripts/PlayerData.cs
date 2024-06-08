using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "ScriptableObject/PlayerData", order = 2)]
public class PlayerData : ScriptableObject
{
    public string Name;

    public float Hp;
    public float MaxHp;

    public float Damage;
    public float Armor;

    public float CriticalHit;

    public int activePoint;
    public int MaxActivePoint;
    
    public void ResetWarriorData()
    {
        Hp = MaxHp;
        Damage = 20f;
        Armor = 100f;
        CriticalHit = 0f;
        MaxActivePoint = 100;
        activePoint = MaxActivePoint;
    }

    public void ResetArcherData()
    {
        Hp = MaxHp;
        Damage = 10f;
        Armor = 70f;
        CriticalHit = 50f;
        MaxActivePoint = 100;
        activePoint = MaxActivePoint;
    }

    public void ResetWizardData()
    {
        Hp = MaxHp;
        Damage = 30;
        Armor = 60;
        CriticalHit = 0f;
        MaxActivePoint = 100;
        activePoint = MaxActivePoint;
    }
}