using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "BossMonsterData", menuName = "ScriptableObject/BossMonsterData", order = 2)]
public class BossMonsterData : ScriptableObject
{
    [Header("���� ���� ����")]
    public string BossName; // ���� ���� �̸�
    public int Id; // �ѹ���
    public float Hp; // ü��
    public bool IsBoss = true;
    public int MoveDistance; // �̵� �Ÿ�(ĭ �� = 1)
    [TextArea] public string Description;

    // ���� ����
    [Header("���� ���� ��ų")]
    public SkillData[] Skill;
}
