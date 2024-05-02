using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "MonsterData", menuName = "ScriptableObject/MonsterData", order = 1)]
public class MonsterData : ScriptableObject
{
    [Header("���� ����")]
    public string MonsterName; // ���� �̸�
    public int Id; // �ѹ���
    public float Hp; // ü��
    public float MaxHp = 100f;
    public float Amor;
    public float CurrentDamage;
    public bool IsBoss = false;
    public int MoveDistance; // ���� �̵� �Ÿ�(ĭ �� = 1)
    public int DetectionRagne; // ���� ����
    [TextArea] public string Description;

  
    [Header("��ų ����")]
    public float MinDamage; // �ּ� ������ 
    public float MaxDamage; // �ִ� ������
    public float Critical; // ��ų Ȯ��
    public int SkillDetectionRange;



    // ���� ����
    // ���ݷ� ����
    public void IncreaseDamage(float damage)
    {
        if (CurrentDamage >= MaxDamage)
            CurrentDamage = MaxDamage;

        CurrentDamage += damage;
    }

    // ü�� ����
    public void IncreaseHp(float heal)
    {
        if (Hp >= MaxHp)
            Hp = MaxHp;

        Hp += heal;
    }

    // ���� ����
    public void IncreaseAmor(float amor)
    {
        Amor = amor;
    }
}



