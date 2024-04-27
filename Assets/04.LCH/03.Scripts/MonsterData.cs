using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "MonsterData", menuName = "ScriptableObject/MonsterData", order = 1)]
public class MonsterData : ScriptableObject
{
    [Header("몬스터 정보")]
    public string MonsterName; // 몬스터 이름
    public int Id; // 넘버링
    public float Hp; // 체력
    public float MaxHp = 100f;
    public int CurrentDamage;
    public bool IsBoss = false;
    public int MoveDistance; // 몬스터 이동 거리(칸 당 = 1)
    public int DetectionRagne; // 감지 범위
    [TextArea] public string Description;

  
    [Header("데미지 설정")]
    public float MinDamage; // 최소 데미지 
    public float MaxDamage; // 최대 데미지


    // 모든 몬스터 턴 종료 후 공격력 증가
    public void IncreaseDamage(float damageBuff)
    {
        if (MinDamage == MaxDamage)
            return;

        MinDamage += damageBuff;

    }
}



