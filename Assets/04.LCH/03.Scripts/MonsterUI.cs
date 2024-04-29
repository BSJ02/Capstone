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
    public Text amor;

    float hp_Data, damage_Data, amor_Data;

    private void Start()
    {
        GetMonsterHp();
        GetMonsterAmor();
    }

    private void Update()
    {
        hp.text = hp_Data.ToString();
        damage.text = damage_Data.ToString();
        amor.text = amor_Data.ToString();
    }

    public void GetMonsterHp()
    {
        hp_Data = GetComponent<Monster>().monsterData.Hp;
    }

    public void GetMonsterDamage()
    {
        damage_Data = GetComponent<Monster>().monsterData.CurrentDamage;
    }

    public void GetMonsterAmor()
    {
        amor_Data = GetComponent<Monster>().monsterData.Amor;
    }

}
