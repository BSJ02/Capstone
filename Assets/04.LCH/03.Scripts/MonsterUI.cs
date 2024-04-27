using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 몬스터 UI 관련 클래스
/// </summary>

public class MonsterUI : MonoBehaviour
{
    [Header("몬스터 정보")]
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
