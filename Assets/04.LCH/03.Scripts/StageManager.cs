using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public List<GameObject> monsters = new List<GameObject>();

    private int currentMonsterIndex = -1;
    private float delay = 1.5f;

    public void StartMonsterSequence()
    {
        StartCoroutine(NextMonster());
    }


    // 몬스터 행동 시작
    IEnumerator NextMonster()
    {
        // 잠시 대기
        yield return new WaitForSeconds(delay);

        if (currentMonsterIndex < monsters.Count - 1)
        {
            currentMonsterIndex++;
            monsters[currentMonsterIndex].GetComponent<MonsterMove>().ButtonClick();

            // 인덱스 초기화
            if (currentMonsterIndex == monsters.Count - 1)
            {
                monsters[currentMonsterIndex].GetComponent<MonsterData>().IncreaseDamage(1);
                currentMonsterIndex = -1;
            }
        }
    }
}

