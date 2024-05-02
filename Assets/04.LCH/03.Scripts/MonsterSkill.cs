using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSkill : MonoBehaviour
{
    public ParticleSystem particle;
    public GameObject skillPosition;
    
    private Vector3 particlePosition;
    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void DragonSkill()
    {
        particlePosition = skillPosition.transform.position;
        Quaternion particleRotation = skillPosition.transform.rotation; 

        ParticleSystem ps = Instantiate(particle, particlePosition, particleRotation, skillPosition.transform);
    }

   
}
