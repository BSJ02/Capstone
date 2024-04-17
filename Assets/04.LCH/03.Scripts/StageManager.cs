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


    // ���� �ൿ ����
    IEnumerator NextMonster()
    {
        // ��� ���
        yield return new WaitForSeconds(delay);

        if (currentMonsterIndex < monsters.Count - 1)
        {
            currentMonsterIndex++;
            monsters[currentMonsterIndex].GetComponent<MonsterMove>().ButtonClick();

            // �ε��� �ʱ�ȭ
            if (currentMonsterIndex == monsters.Count - 1)
            {
                monsters[currentMonsterIndex].GetComponent<MonsterData>().IncreaseDamage(1);
                currentMonsterIndex = -1;
            }
        }
    }
}

