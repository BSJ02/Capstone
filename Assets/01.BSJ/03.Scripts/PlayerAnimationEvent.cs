using System.Collections;
using System.Collections.Generic;
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
    }

    public void OnTeleportAnimationEvent()
    {
        if (cardData.shouldTeleport)
        {
            isTeleport = true;
        }
    }

    public void OnFireballAnimationEvent()
    {
        if (cardData.shouldFireball)
        {
            isFireball = true;
        }
    }

    public void OnPositionSwapAnimationEvent()
    {
        if (cardData.shouldPosSwap)
        {
            isPosSwap = true;
        }
    }
}
