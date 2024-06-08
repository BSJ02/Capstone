using UnityEngine;

[CreateAssetMenu(menuName = "Skills/Dragon")]
public class DragonSkill : SkillData
{
    public GameObject effect;

    public override void Use(Transform startPosition)
    {
        Vector3 particlePosition = startPosition.position;
        Quaternion particleRotation = startPosition.rotation;

        GameObject fire = Instantiate(effect, particlePosition, particleRotation);
        fire.transform.SetParent(startPosition); // 생성된 파티클을 부모 객체에 붙임
        
    }
}
