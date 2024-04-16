using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "BossMonsterData", menuName = "ScriptableObject/BossMonsterData", order = 2)]
public class BossMonsterData : ScriptableObject
{
    [Header("보스 몬스터 정보")]
    public string BossName; // 보스 몬스터 이름
    public int Id; // 넘버링
    public float Hp; // 체력
    public bool IsBoss = true;
    public int MoveDistance; // 이동 거리(칸 당 = 1)
    [TextArea] public string Description;

    // 보스 몬스터
    [Header("보스 몬스터 스킬")]
    public SkillData[] Skill;
}
