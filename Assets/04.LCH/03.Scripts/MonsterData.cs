using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "MonsterData", menuName = "ScriptableObject/MonsterData", order = 1)]
public class MonsterData : ScriptableObject
{
    [Header("# Monster Info")]
    public string MonsterName;
    public int Id; 
    public float Hp; 
    public float MaxHp = 100f;
    public float Amor;
    public float CurrentDamage;
    public int MoveDistance; 
    public int DetectionRagne; 
    [TextArea] public string Description;


    [Header("# Skill")]
    public float MinDamage;
    public float MaxDamage;
    public float Critical;
    public int SkillDetectionRange;

    public void IncreaseDamage(float damage)
    {
        if (CurrentDamage >= MaxDamage)
            CurrentDamage = MaxDamage;

        CurrentDamage += damage;
    }

    public void IncreaseHp(float heal)
    {
        if (Hp >= MaxHp)
            Hp = MaxHp;

        Hp += heal;
    }

    public void IncreaseAmor(float amor)
    {
        Amor = amor;
    }
}



