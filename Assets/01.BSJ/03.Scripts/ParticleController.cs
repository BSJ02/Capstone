using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ParticleController : MonoBehaviour
{
    private GameObject obstacle_Group;
    private GameObject particle_Group;


    [Header("# Wall Prefab")]
    public GameObject obstaclePrefab;

    private Queue<GameObject> obstacleQueue = new Queue<GameObject>();

    [Header("# Particle Prefabs")]
    public GameObject healEffectPrefab;
    public GameObject buffEffectPrefab;
    public GameObject fireballEffectPrefab;
    public GameObject teleportEffectPrefab;
    public GameObject flamePillarEffectPrefab;

    private Queue<GameObject> healEffectPool = new Queue<GameObject>();
    private Queue<GameObject> buffEffectPool = new Queue<GameObject>();
    private Queue<GameObject> fireballEffectPool = new Queue<GameObject>();
    private Queue<GameObject> teleportEffectPool = new Queue<GameObject>();
    private Queue<GameObject> flamePillarEffectPool = new Queue<GameObject>();

    private void Start()
    {
        obstacle_Group = GameObject.Find("Particle_Group");
        if (obstacle_Group == null)
        {
            obstacle_Group = new GameObject("Particle_Group");
        }
        InitializeParticlePool(obstaclePrefab, obstacleQueue, obstacle_Group);


        particle_Group = GameObject.Find("Particle_Group");
        if (particle_Group == null)
        {
            particle_Group = new GameObject("Particle_Group");
        }
        InitializeParticlePool(healEffectPrefab, healEffectPool, particle_Group);
        InitializeParticlePool(buffEffectPrefab, buffEffectPool, particle_Group);
        InitializeParticlePool(fireballEffectPrefab, fireballEffectPool, particle_Group);
        InitializeParticlePool(teleportEffectPrefab, teleportEffectPool, particle_Group);
        InitializeParticlePool(flamePillarEffectPrefab, flamePillarEffectPool, particle_Group);

    }

    public void InitializeParticlePool(GameObject prefab, Queue<GameObject> pool, GameObject parentObj)
    {
        const int initialPoolSize = 2;
        for (int i = 0; i < initialPoolSize; i++)
        {
            GameObject particleObject = Instantiate(prefab, transform.position, Quaternion.identity);
            particleObject.transform.SetParent(parentObj.transform, false);
            particleObject.SetActive(false);
            pool.Enqueue(particleObject);
        }
    }

    public GameObject GetAvailableParticle(GameObject prefab, Queue<GameObject> pool)
    {
        if (pool.Count > 0)
        {
            GameObject particleObject = pool.Dequeue();
            particleObject.SetActive(true);
            return particleObject;
        }
        else
        {
            GameObject newParticleObject = Instantiate(prefab, transform.position, Quaternion.identity);
            newParticleObject.transform.SetParent(particle_Group.transform, false);
            return newParticleObject;
        }
    }

    public void ReturnToPool(GameObject particleObject, Queue<GameObject> pool)
    {
        particleObject.SetActive(false);
        pool.Enqueue(particleObject);
    }

    public IEnumerator ReturnParticleToPool(GameObject particleObject, Queue<GameObject> pool, float duration)
    {
        yield return new WaitForSeconds(duration);
        ReturnToPool(particleObject, pool);
    }

    private Queue<GameObject> GetAppropriatePool(GameObject prefab)
    {
        if (prefab == healEffectPrefab)
        {
            return healEffectPool;
        }
        else if (prefab == buffEffectPrefab)
        {
            return buffEffectPool;
        }
        else if (prefab == fireballEffectPrefab)
        {
            return fireballEffectPool;
        }
        else if (prefab == teleportEffectPrefab)
        {
            return teleportEffectPool;
        }
        else if (prefab == flamePillarEffectPrefab)
        {
            return flamePillarEffectPool;
        }
        else
        {
            Debug.LogError("?particle prefab?");
            return null;
        }
    }

    public void ApplyPlayerEffect(GameObject prefab, GameObject playerObject)
    {
        if (prefab != null && playerObject != null)
        {
            GameObject particleObject = GetAvailableParticle(prefab, GetAppropriatePool(prefab));
            particleObject.transform.position = playerObject.transform.position;

            ParticleSystem particleSystem = particleObject.GetComponent<ParticleSystem>();
            if (particleSystem != null)
            {
                particleSystem.Play();
                StartCoroutine(ReturnParticleToPool(particleObject, GetAppropriatePool(prefab), particleSystem.main.duration));
            }
        }
    }

    public void ApplyPlayerEffect(GameObject prefab, GameObject playerObject, float height)
    {
        GameObject particleObject = GetAvailableParticle(prefab, GetAppropriatePool(prefab));
        particleObject.transform.position = playerObject.transform.position + new Vector3(0, height, 0);

        ParticleSystem particleSystem = particleObject.GetComponent<ParticleSystem>();
        if (particleSystem != null)
        {
            particleSystem.Play();
            StartCoroutine(ReturnParticleToPool(particleObject, GetAppropriatePool(prefab), particleSystem.main.duration));
        }
    }

    public IEnumerator ProjectileEffect(GameObject prefab, GameObject playerObject, GameObject targetObject)
    {

        Vector3 startPosition = playerObject.transform.position + new Vector3(0, 1, 0);
        Vector3 targetPosition = targetObject.transform.position;

        GameObject particleObject = GetAvailableParticle(prefab, GetAppropriatePool(prefab));
        particleObject.transform.position = playerObject.transform.position;

        ParticleSystem particleSystem = particleObject.GetComponent<ParticleSystem>();

        float distance = Vector3.Distance(startPosition, targetPosition);
        particleSystem.Stop();

        var main = particleSystem.main;
        main.duration = distance / 10f;

        if (particleSystem != null)
        {
            particleSystem.Play();
            StartCoroutine(ReturnParticleToPool(particleObject, GetAppropriatePool(prefab), particleSystem.main.duration + 0.5f));
        }

        float duration = 0.5f;

        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;

            particleObject.transform.position = Vector3.Lerp(startPosition, targetPosition, t);

            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    public IEnumerator elevateObject(Vector3 tilePos, Vector3 goalPosition)
    {
        float elapsedTime = 0f;
        float duration = 0.2f;
        float deltaTime = Time.deltaTime;

        GameObject obsacleObj = GetAvailableParticle(obstaclePrefab, obstacleQueue);

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            obsacleObj.transform.position = Vector3.Lerp(tilePos, goalPosition, t);
            elapsedTime += deltaTime;
            yield return null;
        }
    }

    public void AreaAttack(GameObject prefab, GameObject playerObj, float rotationY)
    {
        GameObject particleObject = GetAvailableParticle(prefab, GetAppropriatePool(prefab));
        particleObject.transform.position = playerObj.transform.position;
        particleObject.transform.rotation = new Quaternion(0, rotationY, 0, 0);

        ParticleSystem particleSystem = particleObject.GetComponent<ParticleSystem>();
        if (particleSystem != null)
        {
            particleSystem.Play();
            StartCoroutine(ReturnParticleToPool(particleObject, GetAppropriatePool(prefab), particleSystem.main.duration));
        }
    }
}
