using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerAnimationEvent : MonoBehaviour
{
    private CardData cardData;
    private CardManager cardManager;
    private CardProcessing cardProcessing;
    private ParticleController particleController;

    private bool isFireball = false;
    private bool isTeleport = false;
    private bool isPosSwap = false;
    private bool isSummonObstacle = false;

    [HideInInspector] public Queue<GameObject> obstacleQueue = new Queue<GameObject>();
    [Header("# Wall Prefab")]
    public GameObject obstaclePrefab;
    private GameObject obstacle_Group;

    private void Awake()
    {
        cardData = FindObjectOfType<CardData>();
        cardManager = FindObjectOfType<CardManager>();
        cardProcessing = FindObjectOfType<CardProcessing>();
        particleController = FindObjectOfType<ParticleController>();
    }
    private void Start()
    {
        obstacle_Group = GameObject.Find("Particle_Group");
        if (obstacle_Group == null)
        {
            obstacle_Group = new GameObject("Particle_Group");
        }

        particleController.InitializeParticlePool(obstaclePrefab, obstacleQueue, obstacle_Group);
    }

    private void Update()
    {
        if (isTeleport)
        {
            cardProcessing.currentPlayerObj.transform.position = cardData.targetPos;
            cardData.shouldTeleport = false;
            isTeleport = false;
        }

        if (isFireball)
        {
            StartCoroutine(particleController.ProjectileEffect(particleController.fireballEffectPrefab, cardProcessing.currentPlayerObj, cardProcessing.selectedTarget));
            Monster monster = cardProcessing.selectedTarget.GetComponent<Monster>();
            monster.GetHit(cardManager.useCard.cardPower[0]);
            cardData.shouldFireball = false;
            isFireball = false;
        }

        if (isPosSwap)
        {
            Vector3 playerPos = cardData.playerPos;
            Vector3 monsterPos = cardData.targetPos;

            cardProcessing.selectedTarget.transform.position = playerPos; // Monster => PlayerPos
            cardProcessing.currentPlayerObj.transform.position = monsterPos; // Player => MonsterPos

            MapGenerator.instance.totalMap[(int)playerPos.x, (int)playerPos.y].SetCoord((int)playerPos.x, (int)playerPos.y, true);
            MapGenerator.instance.totalMap[(int)monsterPos.x, (int)monsterPos.y].SetCoord((int)monsterPos.x, (int)monsterPos.y, true);

            cardData.shouldPosSwap = false;
            isPosSwap = false;
        }

        if (isSummonObstacle)
        {
            Vector3 tilePos = cardData.targetPos;
            Vector3 goalPosition = tilePos + new Vector3(0, 0.35f, 0);

            StartCoroutine(SpawnWall(tilePos, goalPosition));

            Tile tile = MapGenerator.instance.totalMap[(int)tilePos.x, (int)tilePos.y];
            tile.SetCoord((int)tilePos.x, (int)tilePos.y, true);

            cardData.shouldSummon = false;
            isSummonObstacle = false;
        }
    }

    private void OnCardEffectAnimationEvent()
    {
        if (cardData.shouldTeleport)
        {
            isTeleport = true;
        }
        else if (cardData.shouldFireball)
        {
            isFireball = true;
        }
        else if (cardData.shouldPosSwap)
        {
            isPosSwap = true;
        }
    }

    private void OnSummonObstacle()
    {
        isSummonObstacle = true;
    }

    public IEnumerator SpawnWall(Vector3 tilePos, Vector3 goalPosition)
    {
        float elapsedTime = 0f;
        float duration = 0.2f;
        float deltaTime = Time.deltaTime;

        GameObject obsacleObj = particleController.GetAvailableParticle(obstaclePrefab, obstacleQueue);

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            obsacleObj.transform.position = Vector3.Lerp(tilePos, goalPosition, t);
            elapsedTime += deltaTime;
            yield return null;
        }
    }
}
