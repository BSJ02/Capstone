using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "MonsterData", menuName = "ScriptableObject/MonsterData", order = 1)]
public class MonsterData : ScriptableObject
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


    // ��� ���� �� ���� �� ���ݷ� ����
    public void IncreaseDamage(float damage)
    {
        if (MinDamage == MaxDamage)
            return;

        MinDamage += damage;

    }
}



