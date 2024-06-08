using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Skills/Mage")]
public class MageSkill : SkillData
{
    public GameObject effect;
    public float speed = 5f;

    public override void Use(Transform startPosition)
    {
        Vector3 particlePosition = startPosition.position;
        Quaternion particleRotation = startPosition.rotation;

        GameObject projectile = Instantiate(effect, particlePosition, particleRotation);
        projectile.transform.SetParent(startPosition); // 생성된 파티클을 부모 객체에 붙임

        MonsterMove monsterMove = BattleManager.instance.selectedMonster.GetComponent<MonsterMove>();
        Vector2Int playerPosition = monsterMove.playerPos; // playerPos가 null로 체크될 때가 있음. 버그 추후에 수정 예정

        Vector3 target = new Vector3(playerPosition.x, 0, playerPosition.y);
        Vector3 directionToPlayer = target + Vector3.up - startPosition.position;
        directionToPlayer.Normalize();

        projectile.gameObject.GetComponent<Rigidbody>().AddForce(directionToPlayer * speed, ForceMode.VelocityChange);


    }
}
