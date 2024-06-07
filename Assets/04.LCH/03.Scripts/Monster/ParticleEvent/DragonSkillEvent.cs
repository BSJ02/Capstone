using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Dragon 몬스터 스킬 피격 이벤트
/// </summary>

public class DragonSkillEvent : MonoBehaviour
{
    [SerializeField] private float delay = 0.3f; // 피격 간격
    [SerializeField] private int hitCount = 6; // 총 피격 횟수

    Monster currentMonster;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            currentMonster = BattleManager.instance.selectedMonster.GetComponent<Monster>();

            Player hitPlayer = other.gameObject.GetComponent<Player>();
            StartCoroutine(DelayHitAnimation(hitPlayer));
        }
    }

    IEnumerator DelayHitAnimation(Player player)
    {
        float damage = currentMonster.monsterData.CurrentDamage; // 몬스터 데미지 가져오기

        for (int i = 0; i < hitCount; i++)
        {
            player.GetHit(damage); // 플레이어에게 데미지 입히기
            yield return new WaitForSeconds(delay); // 지연 시간
        }
    }
}
