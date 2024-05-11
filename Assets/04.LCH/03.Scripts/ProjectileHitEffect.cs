using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileHitEffect : MonoBehaviour
{
    public GameObject hitEffect;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Instantiate(hitEffect, other.transform.position, Quaternion.identity);
        }
    }

}
