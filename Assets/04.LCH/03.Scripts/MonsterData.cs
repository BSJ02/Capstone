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
    public int CurrentDamage;
    public bool IsBoss = false;
    public int MoveDistance; // ���� �̵� �Ÿ�(ĭ �� = 1)
    public int DetectionRagne; // ���� ����
    [TextArea] public string Description;

  
    [Header("������ ����")]
    public float MinDamage; // �ּ� ������ 
    public float MaxDamage; // �ִ� ������


    // ��� ���� �� ���� �� ���ݷ� ����
    public void IncreaseDamage(float damageBuff)
    {
        if (MinDamage == MaxDamage)
            return;

        MinDamage += damageBuff;

    }
}



