using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Skills/Skeleton")]
public class SkeletonSkill : SkillData
{
    public GameObject effect;
    private Transform startPosition;

    public override void Initialize(GameObject obj)
    {
        startPosition = obj.transform;
    }

    public override void Use()
    {
        // startPosition을 월드 좌표로 설정
        Vector3 particlePosition = startPosition.position; 
        Quaternion particleRotation = startPosition.rotation;

        GameObject fire = Instantiate(effect, particlePosition, particleRotation);
        fire.transform.SetParent(startPosition, true); // startPosition을 부모로 설정

    }
}
