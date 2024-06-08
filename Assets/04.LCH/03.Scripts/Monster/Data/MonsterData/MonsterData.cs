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
    public int MoveDistance; 
    [TextArea] public string Description;


    [Header("# Attack")]
    public float CurrentDamage;
    public float MinDamage;
    public float MaxDamage;
    public float Critical;
    public int DetectionRagne;
    public int SkillDetectionRange;
}



