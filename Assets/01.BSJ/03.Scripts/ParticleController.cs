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
        // ������ �ð���ŭ ����մϴ�.
        yield return new WaitForSeconds(delay);

        // ��ƼŬ�� �����մϴ�.
        Destroy(particleObject);
    }

    public void ApplyPlayerEffect(GameObject prefab,GameObject targetObject)
    {
        if (prefab != null && targetObject != null)
        {
            // �������� �ν��Ͻ�ȭ�Ͽ� ��ƼŬ �ý����� �����մϴ�.
            GameObject particleObject = Instantiate(prefab, targetObject.transform.position, Quaternion.identity);
            ParticleSystem particleSystem = particleObject.GetComponent<ParticleSystem>();

            // ��ƼŬ �ý����� �ִٸ� ����մϴ�.
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
