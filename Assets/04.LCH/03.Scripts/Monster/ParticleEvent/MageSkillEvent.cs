using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Mage 몬스터 스킬 피격 이벤트
/// Mage 몬스터는 플레이어가 스킬을 맞은 후 애니메이션 초기화(시네머신 카메라 타이밍 조절)
/// </summary>

public class MageSkillEvent : MonoBehaviour
{
    public GameObject hitEffect;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // 현재 스킬을 사용하고 있는 몬스터의 데미지 가져오기
            Monster monster = BattleManager.instance.selectedMonster.GetComponent<Monster>();

            float damage = monster.monsterData.CurrentDamage;
            other.gameObject.GetComponent<Player>().GetHit(damage);

            // 몬스터 애니메이션 초기화
            monster.Init();

            Destroy(gameObject);
            Instantiate(hitEffect, gameObject.transform.position, Quaternion.identity);

        }
    }
}
