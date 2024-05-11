using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSkill : MonoBehaviour
{
    private Monster monster;

    public ParticleSystem particle; // ��ų ����Ʈ
    public GameObject startPosition; // ��ų ���� ��ġ

    private ParticleSystem skill; // ��ų ����Ʈ �� ���� ��ġ �� ������ ���� ����

    public ParticleSystem skillHitEffect; // �÷��̾� �ǰ� �� ������ Ÿ�� ����Ʈ

    bool particleCreated = false;

    float maxSpeed = 3.0f; 
    float moveTime = 0.0f;
    float minDistance = 1.0f;

    private void Awake()
    {
        monster = GetComponent<Monster>();
        int numberOfMonster = monster.monsterData.Id;
    }

    // ���� ��ų ���� ���� ó��
    private void Update()
    {
        /*if (monster.attack == AttackState.SkillAttack)
        {
            switch (monster.monsterData.Id)
            {
                case 0: break; // ��� ����
                case 1: break; // �Ҳ� �巡��
                case 2: MageSkill(); break;
                case 3: break; // ��ũ ����
                case 4: break; // �𵥵� ���̷���
                case 5: break; // ���� �ź�
            }
        }*/
    }


    // 1. ReadyTo_�����̸� = �ִϸ��̼� �̺�Ʈ���� ó���Ǵ� �޼���
    // 2. �����̸� + Skill = Update���� ó���ؾ� �� �޼���

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

        // ��ƼŬ �ð��� ���� �ӵ� ����
        moveTime += Time.deltaTime;
        float speed = maxSpeed * distance;
        float t = moveTime / speed;

        // ��ƼŬ �ڿ������� �̵�
        skill.transform.position = Vector3.Lerp(skill.transform.position, playerTransform.position, t);

        // �÷��̾��� �Ÿ��� ���������
        if (distance <= minDistance)
        {
            speed = 0;
            // hitEffect Particle ��� �� StopSkill
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
