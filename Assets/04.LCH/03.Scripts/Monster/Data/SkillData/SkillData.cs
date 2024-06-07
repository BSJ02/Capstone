using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SkillData : ScriptableObject
{
    public string SkillName;
    [TextArea] public string Description;
    public abstract void Use(Transform startPosition);
}
