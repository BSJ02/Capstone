using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationEvent : MonoBehaviour
{
    private CardData cardData;
    private CardProcessing cardProcessing;

    private void Awake()
    {
        cardData = FindObjectOfType<CardData>();
        cardProcessing = FindObjectOfType<CardProcessing>();
    }

    public void OnTeleportAnimationEvent()
    {
        if (cardData.shouldTeleport)
        {
            cardProcessing.currentPlayerObj.transform.position = cardData.teloportPos;
            cardData.shouldTeleport = false; 
        }
    }
}
