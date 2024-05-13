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
    private bool isFlamePillar = false;
    private bool isSummonObstacle = false;

    private void Awake()
    {
        cardData = FindObjectOfType<CardData>();
        cardManager = FindObjectOfType<CardManager>();
        cardProcessing = FindObjectOfType<CardProcessing>();
        particleController = FindObjectOfType<ParticleController>();
    }
    

    private void Update()
    {
        if (isTeleport)
        {
            Vector3 playerPos = cardProcessing.currentPlayerObj.transform.position;
            Vector3 tilePos = cardData.targetPos;

            cardProcessing.currentPlayerObj.transform.position = tilePos; // Player => TilePos

            MapGenerator.instance.totalMap[(int)playerPos.x, (int)playerPos.y].SetCoord((int)playerPos.x, (int)playerPos.y, false);
            MapGenerator.instance.totalMap[(int)tilePos.x, (int)tilePos.y].SetCoord((int)tilePos.x, (int)tilePos.y, true);

            cardData.shouldTeleport = false;
            isTeleport = false;
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

        if (isFireball)
        {
            GameObject particlePrefab = particleController.fireballEffectPrefab;
            GameObject playerObj = cardProcessing.currentPlayerObj;
            GameObject targetObj = cardProcessing.selectedTarget;
            Card useCard = cardManager.useCard;

            StartCoroutine(particleController.ProjectileEffect(particlePrefab, playerObj, targetObj));
            Monster monster = targetObj.GetComponent<Monster>();
            monster.GetHit(useCard.cardPower[0]);

            cardData.shouldFireball = false;
            isFireball = false;
        }

        if (isFlamePillar)
        {
            GameObject particlePrefab_One = particleController.flamePillarEffectPrefab;
            GameObject particlePrefab_Two = particleController.flamePillarEffectPrefab;
            GameObject playerObj = cardProcessing.currentPlayerObj;
            Card useCard = cardManager.useCard;

            particleController.AreaAttack(particlePrefab_One, playerObj, 0);
            particleController.AreaAttack(particlePrefab_Two, playerObj, 90);

            foreach (Monster monster in MapGenerator.instance.rangeInMonsters)
            {
                monster.GetHit(useCard.cardPower[0]);
            }
            cardData.shouldFlamePillar = false;
            isFlamePillar = false;
        }

        if (isSummonObstacle)
        {
            Vector3 tilePos = cardData.targetPos;
            Vector3 goalPosition = tilePos + new Vector3(0, 0.35f, 0);

            StartCoroutine(particleController.elevateObject(tilePos, goalPosition));

            Tile tile = MapGenerator.instance.totalMap[(int)tilePos.x, (int)tilePos.y];
            tile.SetCoord((int)tilePos.x, (int)tilePos.y, true);

            cardData.shouldSummon = false;
            isSummonObstacle = false;
        }
    }

    private void OnChargeAnimationEvent()
    {
        if (cardData.shouldTeleport)
        {
            isTeleport = true;
        }
        else if (cardData.shouldPosSwap)
        {
            isPosSwap = true;
        }
        
    }

    private void OnFireballAnimationEvent()
    {
        if (cardData.shouldFireball)
        {
            isFireball = true;
        }
    }

    private void OnSummonObstacle()
    {
        if (cardData.shouldSummon)
        {
            isSummonObstacle = true;
        }
    }

    private void OnFlamePillar()
    {
        if (cardData.shouldFlamePillar)
        {
            isFlamePillar = true;
        }
    }


}
