using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "MonsterData", menuName = "ScriptableObject/MonsterData", order = 1)]
public class MonsterData : ScriptableObject
{
    [Header("# Monster Info")]
    public string MonsterName;
    public int Id;
    public int Hp; 
    public int MaxHp = 100;
    public int Amor;
    public int MoveDistance; 
    [TextArea] public string Description;


    [Header("# Attack")]
    public int CurrentDamage;
    public int MinDamage;
    public int MaxDamage;
    public int Critical;
    public int DetectionRagne;
    public int SkillDetectionRange;
}



