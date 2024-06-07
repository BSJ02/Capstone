using UnityEngine;

[CreateAssetMenu(menuName = "Skills/Skeleton")]
public class SkeletonSkill : SkillData
{
    public GameObject effect;

    public override void Use(Transform startPosition)
    {
        Vector3 particlePosition = startPosition.position;
        Quaternion particleRotation = startPosition.rotation;

        GameObject blood = Instantiate(effect, particlePosition, particleRotation);
        blood.transform.SetParent(startPosition); // 생성된 파티클을 부모 객체에 붙임  
    }
}

