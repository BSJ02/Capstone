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

    private void Awake()
    {
        cardData = FindObjectOfType<CardData>();
        cardManager = FindObjectOfType<CardManager>();
        cardProcessing = FindObjectOfType<CardProcessing>();
        particleController = FindObjectOfType<ParticleController>();
    }

    private void Update()
    {
        if (isFireball)
        {
            StartCoroutine(particleController.ProjectileEffect(particleController.fireballEffectPrefab, cardProcessing.currentPlayerObj, cardProcessing.selectedTarget));
            Monster monster = cardProcessing.selectedTarget.GetComponent<Monster>();
            monster.GetHit(cardManager.useCard.cardPower[0]);
            cardData.shouldFireball = false;
            isFireball = false;
        }
    }

    public void OnTeleportAnimationEvent()
    {
        if (cardData.shouldTeleport)
        {
            cardProcessing.currentPlayer.transform.position = cardData.teloportPos;
            cardData.shouldTeleport = false; 
        }
    }

    public void OnFireballAnimationEvent()
    {
        if (cardData.shouldFireball)
        {
            isFireball = true;
        }
    }
    
}
