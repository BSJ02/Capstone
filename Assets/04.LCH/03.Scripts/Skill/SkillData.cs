using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SkillData : ScriptableObject
{
    public string SkillName;
    public abstract void Initialize(GameObject obj);
    public abstract void Use();
}
