using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ���� UI ���� Ŭ����
/// </summary>

public class MonsterUI : MonoBehaviour
{
    [Header("���� ����")]
    public Text hp;
    public Text damage;

    float hp_Data, damage_Data;

    private void Start()
    {
        hp_Data = GetComponent<Monster>().monsterData.Hp;
    }

    private void Update()
    {
        hp.text = hp_Data.ToString();
        damage.text = damage_Data.ToString();
    }

    public void GetMonsterHp()
    {
        hp_Data = GetComponent<Monster>().monsterData.Hp;
    }

    public void GetMonsterDamage()
    {
        damage_Data = GetComponent<Monster>().monsterData.CurrentDamage;
    }

}
