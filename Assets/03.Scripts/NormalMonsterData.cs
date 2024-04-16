using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "NormalMonsterData", menuName = "ScriptableObject/NormalMonsterData", order = 1)]
public class NormalMonsterData : ScriptableObject
{
    [Header("���� ����")]
    public string MonsterName; // ���� �̸�
    public int Id; // �ѹ���
    public float Hp; // ü��
    public bool IsBoss = false;
    public int MoveDistance; // ���� �̵� �Ÿ�(ĭ �� = 1)
    [TextArea] public string Description;

    // �⺻ ���� 
    [Header("���� ����")]
    public float MinDamage; // �Ϲ� ����
    public float MaxDamage; // ũ��Ƽ�� ����
}



