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

    // ReadyTo_�����̸� = �ִϸ��̼� �̺�Ʈ���� ó���Ǵ� �޼���
    // �����̸� + Skill = Update���� ó���ؾ� �� �޼���

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
            // ��ƼŬ�� �÷��̾� ���� �Ÿ�
            float distance = Vector3.Distance(skill.transform.position, playerTransform.position);

            moveTime += Time.deltaTime;
            float speed = maxSpeed * distance;

            // 3�ʿ� ���� �̵�
            float t = moveTime / speed;
            skill.transform.position = Vector3.Lerp(skill.transform.position, playerTransform.position, t);

            // �÷��̾��� �Ÿ��� ���������
            if (distance <= minDistance)
            {
                particleMoving = false; // �÷���(Flag)
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
