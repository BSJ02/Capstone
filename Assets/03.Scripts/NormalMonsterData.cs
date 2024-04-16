using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "NormalMonsterData", menuName = "ScriptableObject/NormalMonsterData", order = 1)]
public class NormalMonsterData : ScriptableObject
{
    [Header("몬스터 정보")]
    public string MonsterName; // 몬스터 이름
    public int Id; // 넘버링
    public float Hp; // 체력
    public bool IsBoss = false;
    public int MoveDistance; // 몬스터 이동 거리(칸 당 = 1)
    [TextArea] public string Description;

    // 기본 몬스터 
    [Header("몬스터 공격")]
    public float MinDamage; // 일반 공격
    public float MaxDamage; // 크리티컬 공격
}



