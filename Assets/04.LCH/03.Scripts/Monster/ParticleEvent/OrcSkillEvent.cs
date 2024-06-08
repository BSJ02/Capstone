using System.Collections;
using System.Collections.Generic;
using Tiny;
using UnityEngine;

/// <summary>
/// Orc 몬스터 스킬 피격 이벤트(콜라이더X, 애니메이션 이벤트 사용)
/// </summary>

public class OrcSkillEvent : MonoBehaviour
{
    [SerializeField] private float delay = 1f; // 피격 간격
    [SerializeField] private int hitCount = 3; // 총 피격 횟수

    Monster monster;
    Trail trail;

    private void Awake()
    {
        monster = GetComponent<Monster>();
        trail = GetComponent<Trail>();
    }

    // 추후 수정 예정
    /*private void OnTriggerEnter(Collider[] others)
    {
        foreach (Collider other in others)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                *//*Player player = other.gameObject.GetComponent<Player>();
                DelayHitAnimation(player);*//*
            }
        }
    }

    IEnumerator DelayHitAnimation(Player player)
    {
        monster = GetComponent<Monster>();
        float damage = monster.monsterData.CurrentDamage; // 몬스터 데미지 가져오기

        for (int i = 0; i < hitCount; i++)
        {
            player.GetHit(damage); // 플레이어에게 데미지 입히기
            yield return new WaitForSeconds(delay); // 지연 시간
        }

        trail.enabled = false;
        monster.GetComponent<Collider>().enabled = false;

        // 몬스터 상태 초기화
        monster.Init();
    }*/


}
