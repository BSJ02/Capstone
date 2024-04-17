using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "MonsterData", menuName = "ScriptableObject/MonsterData", order = 1)]
public class MonsterData : ScriptableObject
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


    // 모든 몬스터 턴 종료 후 공격력 증가
    public void IncreaseDamage(float damage)
    {
        if (MinDamage == MaxDamage)
            return;

        MinDamage += damage;

    }
}



