using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleController : MonoBehaviour
{
    public GameObject healEffectPrefab;
    public GameObject buffEffectPrefab;
    public GameObject fireballEffectPrefab;

    private IEnumerator StopParticleAfterDelay(GameObject particleObject, float delay)
    {
        // 지정된 시간만큼 대기합니다.
        yield return new WaitForSeconds(delay);

        // 파티클을 중지합니다.
        Destroy(particleObject);
    }

    public void ApplyPlayerEffect(GameObject prefab,GameObject targetObject)
    {
        if (prefab != null && targetObject != null)
        {
            // 프리팹을 인스턴스화하여 파티클 시스템을 생성합니다.
            GameObject particleObject = Instantiate(prefab, targetObject.transform.position, Quaternion.identity);
            ParticleSystem particleSystem = particleObject.GetComponent<ParticleSystem>();

            // 파티클 시스템이 있다면 재생합니다.
            if (particleSystem != null)
            {
                particleSystem.Play();

                StartCoroutine(StopParticleAfterDelay(particleObject, particleSystem.main.duration));
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
                StartCoroutine(StopParticleAfterDelay(particleObject, particleSystem.main.duration));
            }
        }
    }
}
