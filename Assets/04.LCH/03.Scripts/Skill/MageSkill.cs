using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Skills/Mage")]
public class MageSkill : SkillData
{
    public GameObject effect;
    private Transform startPosition;
    public float speed = 5f;

    public override void Initialize(GameObject obj)
    {
        startPosition = obj.transform;
    }

    public override void Use()
    {
        Vector3 particlePosition = startPosition.transform.position;
        Quaternion particleRotation = startPosition.transform.rotation;

        GameObject projectile = Instantiate(effect, particlePosition, particleRotation);

        Vector2Int playerPosition = FindObjectOfType<MonsterMove>().playerPos;
        Vector3 target = new Vector3(playerPosition.x, 0, playerPosition.y);

        Vector3 directionToPlayer = (target + Vector3.up) - startPosition.position;
        directionToPlayer.Normalize();

        projectile.gameObject.GetComponent<Rigidbody>().AddForce(directionToPlayer * speed, ForceMode.VelocityChange);

    }
}
