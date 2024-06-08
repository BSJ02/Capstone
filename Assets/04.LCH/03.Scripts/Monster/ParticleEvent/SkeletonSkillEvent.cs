using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonSkillEvent : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Player hitPlayer = other.gameObject.GetComponent<Player>();
            StartCoroutine(AttackDelay(hitPlayer));
        }
    }


    IEnumerator AttackDelay(Player player)
    {
        float damage = BattleManager.instance.selectedMonster.GetComponent<Monster>().monsterData.CurrentDamage;
        player.GetHit(damage);
        
        // 연속으로 공격되는거 방지
        yield return new WaitForSeconds(5f);
    }
}
