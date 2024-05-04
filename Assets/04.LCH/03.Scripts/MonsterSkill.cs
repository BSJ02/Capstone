using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSkill : MonoBehaviour
{
    public GameObject skillPosition;
    public ParticleSystem particle;

    private Vector3 particlePosition;

    private ParticleSystem skill;

    public void DragonSkill()
    {
        particlePosition = skillPosition.transform.position;
        Quaternion particleRotation = skillPosition.transform.rotation; 

        skill = Instantiate(particle, particlePosition, particleRotation, skillPosition.transform);

    }

    public void StopSkill()
    {
        if(skill != null)
        {
            Destroy(skill.gameObject);
        }
    }
}
