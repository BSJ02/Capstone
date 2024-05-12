using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSkill : MonoBehaviour
{
    private Monster monster;

    public ParticleSystem particle; // 스킬 이펙트
    public GameObject startPosition; // 스킬 생성 위치

    private ParticleSystem skill; // 스킬 이펙트 및 생성 위치 등 정보를 담은 변수

    public ParticleSystem skillHitEffect; // 플레이어 피격 시 생성될 타격 이펙트

    bool particleCreated = false;

    float maxSpeed = 3.0f; 
    float moveTime = 0.0f;
    float minDistance = 1.0f;

    private void Awake()
    {
        monster = GetComponent<Monster>();
        int numberOfMonster = monster.monsterData.Id;
    }

    // 몬스터 스킬 물리 연산 처리
    private void Update()
    {
        /*if (monster.attack == AttackState.SkillAttack)
        {
            switch (monster.monsterData.Id)
            {
                case 0: break; // 어둠 박쥐
                case 1: break; // 불꽃 드래곤
                case 2: MageSkill(); break;
                case 3: break; // 오크 족장
                case 4: break; // 언데드 스켈레톤
                case 5: break; // 가시 거북
            }
        }*/
    }


    // 1. ReadyTo_몬스터이름 = 애니메이션 이벤트에서 처리되는 메서드
    // 2. 몬스터이름 + Skill = Update에서 처리해야 할 메서드

    public void ReadyTo_DragonSkil() 
    {
        Vector3 particlePosition = startPosition.transform.position;
        Quaternion particleRotation = startPosition.transform.rotation;

        skill = Instantiate(particle, particlePosition, particleRotation, startPosition.transform);
    }

    public void MageSkill()
    {
        if (!particleCreated)
        {
            moveTime = 0f;

            Vector3 particlePosition = startPosition.transform.position;
            Quaternion particleRotation = startPosition.transform.rotation;

            skill = Instantiate(particle, particlePosition, particleRotation, startPosition.transform);

            particleCreated = true;
        }

        Transform playerTransform = FindObjectOfType<Player>().transform;
        float distance = Vector3.Distance(skill.transform.position, playerTransform.position);

        // 파티클 시간에 따른 속도 설정
        moveTime += Time.deltaTime;
        float speed = maxSpeed * distance;
        float t = moveTime / speed;

        // 파티클 자연스럽게 이동
        skill.transform.position = Vector3.Lerp(skill.transform.position, playerTransform.position, t);

        // 플레이어의 거리가 가까워지면
        if (distance <= minDistance)
        {
            speed = 0;
            // hitEffect Particle 재생 및 StopSkill
        }

    }

    public void StopSkill()
    {
        if(skill != null)
        {
            Destroy(skill.gameObject);
            particleCreated = false;
        }
    }
}
