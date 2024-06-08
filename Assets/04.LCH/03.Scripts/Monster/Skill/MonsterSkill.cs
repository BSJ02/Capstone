using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSkill : MonoBehaviour
{
    public SkillData skill;
    private Transform spawnPosition; // GameObject가 아닌 Transform으로 변경

    private void Start()
    {
        // 개별 몬스터마다 위치를 설정
        spawnPosition = transform.Find("SkillPosition");
    }

    public void UseSkill()
    {
        // 개별 몬스터의 spawnPosition을 전달
        skill.Use(spawnPosition);
    }
}