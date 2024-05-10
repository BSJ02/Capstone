using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillTest : MonoBehaviour
{
    public SkillData skill;

    public GameObject spawnPosition;

    private void Start()
    {
        skill.Initialize(spawnPosition);
    }


    public void MageOfSkill()
    {
        skill.Use();
    }
}
