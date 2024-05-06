using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleController : MonoBehaviour
{
    public GameObject healEffectPrefab;
    public GameObject buffEffectPrefab;
    public GameObject fireballEffectPrefab;

    float moveTime;

    public void ApplyPlayerEffect(GameObject prefab, GameObject playerObject)
    {
        if (prefab != null && playerObject != null)
        {
            GameObject particleObject = Instantiate(prefab, playerObject.transform.position, Quaternion.identity);
            ParticleSystem particleSystem = particleObject.GetComponent<ParticleSystem>();

            if (particleSystem != null)
            {
                particleSystem.Play();
            }
        }
    }

    public void ProjectileEffect(GameObject prefab, GameObject playerObject, GameObject targetObject)
    {
        if (healEffectPrefab != null && playerObject != null)
        {
            GameObject particleObject = Instantiate(prefab, playerObject.transform.position, Quaternion.identity);
            ParticleSystem particleSystem = particleObject.GetComponent<ParticleSystem>();

            if (particleSystem != null)
            {
                particleSystem.Play();
            }

            moveTime += Time.deltaTime;
            float t = moveTime / 0.5f;
            particleObject.transform.position = Vector3.Lerp(playerObject.transform.position, targetObject.transform.position, t);
        }
    }
}
