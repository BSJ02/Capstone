using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ParticleController : MonoBehaviour
{
    public static ParticleController instance;

    // Group
    private GameObject obstacle_Group;
    private GameObject particle_Group;

    [Header("# Wall Prefab")]
    public GameObject obstaclePrefab;
    private Queue<GameObject> obstacleQueue = new Queue<GameObject>();

    [Header("# Base Particle Prefabs")]
    public GameObject healEffectPrefab;
    public GameObject buffEffectPrefab;
    public GameObject removeAilmentEffectPrefab;
    public GameObject transmissionEffectPrefab;

    [Header("# Warrior Particle Prefabs")]
    public GameObject spinAttackEffectPrefab;
    public GameObject shieldBashEffectPrefab;
    public GameObject desperateStrikeEffectPrefab;
    public GameObject dashEffectPrefab;
    public GameObject WarriorsRoarEffectPrefab;
    public GameObject ArmorCrushEffectPrefab;

    /*[Header("# Archer Particle Prefabs")]*/


    [Header("# Wizard Particle Prefabs")]
    public GameObject teleportEffectPrefab;
    public GameObject fireballEffectPrefab;
    public GameObject flamePillarEffectPrefab;
    public GameObject lifeDrainEffectPrefab;
    public GameObject magicShieldEffectPrefab;

    // Base Queue Pool
    private Queue<GameObject> healEffectPool = new Queue<GameObject>();
    private Queue<GameObject> buffEffectPool = new Queue<GameObject>();
    private Queue<GameObject> removeAilmentEffectPool = new Queue<GameObject>();
    private Queue<GameObject> transmissionEffectPool = new Queue<GameObject>();

    // Warrior Queue Pool
    private Queue<GameObject> spinAttackEffectPool = new Queue<GameObject>();
    private Queue<GameObject> shieldBashEffectPool = new Queue<GameObject>();
    private Queue<GameObject> desperateStrikeEffectPool = new Queue<GameObject>();
    private Queue<GameObject> dashEffectPool = new Queue<GameObject>();
    private Queue<GameObject> WarriorsRoarEffectPool = new Queue<GameObject>();
    private Queue<GameObject> ArmorCrushEffectPool = new Queue<GameObject>();

    // Archer Queue Pool


    // Wizard Queue Pool
    private Queue<GameObject> fireballEffectPool = new Queue<GameObject>();
    private Queue<GameObject> teleportEffectPool = new Queue<GameObject>();
    private Queue<GameObject> flamePillarEffectPool = new Queue<GameObject>();
    private Queue<GameObject> lifeDrainEffectPool = new Queue<GameObject>();
    private Queue<GameObject> magicShieldEffectPool = new Queue<GameObject>();

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(instance);
        }
    }

    private void Start()
    {
        obstacle_Group = GameObject.Find("Particle_Group");
        if (obstacle_Group == null)
        {
            obstacle_Group = new GameObject("Particle_Group");
        }

        InitializeParticlePool(obstaclePrefab, obstacle_Group);

        particle_Group = GameObject.Find("Particle_Group");
        if (particle_Group == null)
        {
            particle_Group = new GameObject("Particle_Group");
        }

        InitializeParticlePools();
    }

    private void InitializeParticlePools()
    {
        // Base
        InitializeParticlePool(healEffectPrefab, particle_Group);
        InitializeParticlePool(buffEffectPrefab, particle_Group);
        InitializeParticlePool(removeAilmentEffectPrefab, particle_Group);
        InitializeParticlePool(transmissionEffectPrefab, particle_Group);

        // Warrior
        InitializeParticlePool(spinAttackEffectPrefab, particle_Group);
        InitializeParticlePool(shieldBashEffectPrefab, particle_Group);
        InitializeParticlePool(desperateStrikeEffectPrefab, particle_Group);
        InitializeParticlePool(dashEffectPrefab, particle_Group);
        InitializeParticlePool(WarriorsRoarEffectPrefab, particle_Group);
        InitializeParticlePool(ArmorCrushEffectPrefab, particle_Group);

        // Archer


        // Wizard
        InitializeParticlePool(fireballEffectPrefab, particle_Group);
        InitializeParticlePool(teleportEffectPrefab, particle_Group);
        InitializeParticlePool(flamePillarEffectPrefab, particle_Group);
        InitializeParticlePool(lifeDrainEffectPrefab, particle_Group);
        InitializeParticlePool(magicShieldEffectPrefab, particle_Group);
    }

    public void InitializeParticlePool(GameObject prefab, GameObject parentObj)
    {
        const int initialPoolSize = 1;
        for (int i = 0; i < initialPoolSize; i++)
        {
            GameObject particleObject = Instantiate(prefab, transform.position, Quaternion.identity);
            particleObject.transform.SetParent(parentObj.transform, false);
            particleObject.SetActive(false);
            GetAppropriatePool(prefab).Enqueue(particleObject);
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
        Queue<GameObject> appropriatePool = GetAppropriatePool_Base(prefab);

        if (appropriatePool != null)
        {
            return appropriatePool;
        }
        appropriatePool = GetAppropriatePool_Warrior(prefab);
        if (appropriatePool != null)
        {
            return appropriatePool;
        }
        appropriatePool = GetAppropriatePool_Archer(prefab);
        if (appropriatePool != null)
        {
            return appropriatePool;
        }
        appropriatePool = GetAppropriatePool_Wizard(prefab);
        if (appropriatePool != null)
        {
            return appropriatePool;
        }

        Debug.LogError("?particle prefab?");
        return null;
    }

    private Queue<GameObject> GetAppropriatePool_Base(GameObject prefab)
    {
        if (prefab == obstaclePrefab)
        {
            return obstacleQueue;
        }
        else if (prefab == healEffectPrefab)
        {
            return healEffectPool;
        }
        else if (prefab == buffEffectPrefab)
        {
            return buffEffectPool;
        }
        else if (prefab == removeAilmentEffectPrefab)
        {
            return removeAilmentEffectPool;
        }
        else if (prefab == transmissionEffectPrefab)
        {
            return transmissionEffectPool;
        }
        else
        {
            return null;
        }
    }

    // Warrior
    private Queue<GameObject> GetAppropriatePool_Warrior(GameObject prefab)
    {
        if (prefab == spinAttackEffectPrefab)
        {
            return spinAttackEffectPool;
        }
        else if (prefab == shieldBashEffectPrefab)
        {
            return shieldBashEffectPool;
        }
        else if (prefab == desperateStrikeEffectPrefab)
        {
            return desperateStrikeEffectPool;
        }
        else if (prefab == dashEffectPrefab)
        {
            return dashEffectPool;
        }
        else if (prefab == WarriorsRoarEffectPrefab)
        {
            return WarriorsRoarEffectPool;
        }
        else if (prefab == ArmorCrushEffectPrefab)
        {
            return ArmorCrushEffectPool;
        }
        else
        {
            return null;
        }
    }

    // Archer
    private Queue<GameObject> GetAppropriatePool_Archer(GameObject prefab)
    {
        if (prefab == fireballEffectPrefab)
        {
            return fireballEffectPool;
        }
        else
        {
            return null;
        }
    }

    // Wizard
    private Queue<GameObject> GetAppropriatePool_Wizard(GameObject prefab)
    {
        if (prefab == fireballEffectPrefab)
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
        else if (prefab == lifeDrainEffectPrefab)
        {
            return lifeDrainEffectPool;
        }
        else if (prefab == magicShieldEffectPrefab)
        {
            return magicShieldEffectPool;
        }
        else
        {
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

    public void ApplyPlayerEffect(GameObject prefab, GameObject playerObject, float height, Quaternion rotation, float scale)
    {
        GameObject particleObject = GetAvailableParticle(prefab, GetAppropriatePool(prefab));
        particleObject.transform.position = new Vector3(playerObject.transform.position.x, height, playerObject.transform.position.z);
        particleObject.transform.rotation = rotation;
        particleObject.transform.localScale = new Vector3(scale, scale, scale);

        ParticleSystem particleSystem = particleObject.GetComponent<ParticleSystem>();

        if (particleSystem != null)
        {
            particleSystem.Play();
            StartCoroutine(ReturnParticleToPool(particleObject, GetAppropriatePool(prefab), particleSystem.main.duration));
        }
    }

    public void ApplyTargetEffect(GameObject prefab, Vector3 targetObjPos, Quaternion rotation, float plusTime)
    {
        GameObject particleObject = GetAvailableParticle(prefab, GetAppropriatePool(prefab));
        particleObject.transform.position = targetObjPos;
        particleObject.transform.rotation = rotation;

        ParticleSystem particleSystem = particleObject.GetComponent<ParticleSystem>();
        if (particleSystem != null)
        {
            particleSystem.Play();
            StartCoroutine(ReturnParticleToPool(particleObject, GetAppropriatePool(prefab), particleSystem.main.duration + plusTime));
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

    public IEnumerator ObjectElevateEffect(Vector3 tilePos, Vector3 goalPosition)
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

    public IEnumerator PlayerMoveEffect(GameObject playerObj, Vector3 goalPosition)
    {
        Vector3 startPos = playerObj.transform.position;

        float elapsedTime = 0f;
        float duration = 0.2f;
        float deltaTime = Time.deltaTime;

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            playerObj.transform.position = Vector3.Lerp(startPos, goalPosition, t);
            elapsedTime += deltaTime;
            yield return null;
        }
    }
}
