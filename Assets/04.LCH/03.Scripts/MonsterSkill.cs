using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSkill : MonoBehaviour
{
    public ParticleSystem particle;
    public GameObject startPosition;

    private ParticleSystem skill;
    private Vector3 particlePosition;
    private Quaternion particleRotation;

    public ParticleSystem skillHitEffect;
    private Transform playerTransform;

    bool particleMoving = false;

    float maxSpeed = 3.0f; 
    float moveTime = 0.0f;
    float minDistance = 1.0f;

    private void Update()
    {
        particlePosition = startPosition.transform.position;
        particleRotation = startPosition.transform.rotation;

        MageSkill();
    }

    // ReadyTo_몬스터이름 = 애니메이션 이벤트에서 처리되는 메서드
    // 몬스터이름 + Skill = Update에서 처리해야 할 메서드

    public void ReadyTo_DragonSkil() 
    {
        skill = Instantiate(particle, particlePosition, particleRotation, startPosition.transform);
    }

    public void ReadyTo_MageSkill()
    {
        skill = Instantiate(particle, particlePosition, particleRotation, startPosition.transform);

        playerTransform = FindObjectOfType<Player>().transform;
        moveTime = 0f;
        particleMoving = true;
    }
    
    public void MageSkill()
    {
        if (skill != null && particleMoving)
        {
            // 파티클과 플레이어 사이 거리
            float distance = Vector3.Distance(skill.transform.position, playerTransform.position);

            moveTime += Time.deltaTime;
            float speed = maxSpeed * distance;

            // 3초에 걸쳐 이동
            float t = moveTime / speed;
            skill.transform.position = Vector3.Lerp(skill.transform.position, playerTransform.position, t);

            // 플레이어의 거리가 가까워지면
            if (distance <= minDistance)
            {
                particleMoving = false; // 플래그(Flag)
            }
        }
    }


    public void StopSkill()
    {
        if(skill != null)
        {
            Destroy(skill.gameObject);
        }
    }
}
