using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleController : MonoBehaviour
{
    public GameObject healEffectPrefab;
    public GameObject buffEffectPrefab;
    public GameObject fireballEffectPrefab;

    public void ApplyPlayerEffect(GameObject prefab,GameObject targetObject)
    {
        if (prefab != null && targetObject != null)
        {
            GameObject particleObject = Instantiate(prefab, targetObject.transform.position, Quaternion.identity);
            ParticleSystem particleSystem = particleObject.GetComponent<ParticleSystem>();

            if (particleSystem != null)
            {
                particleSystem.Play();
            }
        }
    }

    public void FireballEffect(GameObject targetObject)
    {
        if (healEffectPrefab != null && targetObject != null)
        {
            GameObject particleObject = Instantiate(buffEffectPrefab, targetObject.transform.position, Quaternion.identity);
            ParticleSystem particleSystem = particleObject.GetComponent<ParticleSystem>();

            if (particleSystem != null)
            {
                particleSystem.Play();
            }
        }
    }
}
