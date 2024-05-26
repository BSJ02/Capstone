using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSkill : MonoBehaviour
{
    public SkillData skill;

    public GameObject spawnPosition;

    private void Start()
    {
        skill.Initialize(spawnPosition);
    }

    public void UseSkill()
    {
        skill.Use();
    }
}
