using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Skills/Dragon")]
public class DragonSkill : SkillData
{
    public GameObject effect;
    private Transform startPosition;
   
    public override void Initialize(GameObject obj)
    {
        startPosition = obj.transform;
    }

    public override void Use()
    {
        Vector3 particlePosition = startPosition.transform.position;
        Quaternion particleRotation = startPosition.transform.rotation;

        GameObject fire = Instantiate(effect, particlePosition, particleRotation);

    }
}
