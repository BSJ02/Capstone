using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;
using System;
using static Card;
using UnityEngine.UIElements;

public class CardMove : MonoBehaviour
{
    private CardProcessing cardProcessing;

    private Vector3 offset;
    private float distanceToCamera;

    private Vector3 originalScale;   // 기본 크기
    [HideInInspector] public Vector3 originalPosition;   // 기본 위치

    private const float scaleFactor = 1.2f; // 카드 확대 배율
    private const float animationDuration = 0.1f;   // 애니메이션 속도

    public Vector3 cardOffset;

    private void Start()
    {
        DOTween.Init();

        cardProcessing = FindObjectOfType<CardProcessing>();

        originalScale = this.transform.localScale;   // 기본 크기 저장
    }

    private void Update()
    {
        if (IsMouseOverCard(gameObject) && !CardManager.instance.waitAddCard)
        {
            int index = CardManager.instance.handCardObject.IndexOf(gameObject) + 1;
            MoveCardToPosAndScale(originalPosition + Vector3.up * 0.5f, scaleFactor);
            gameObject.GetComponent<CardOrder>().SetOrder(index * 10);
        }
        else
        {
            int index = CardManager.instance.handCardObject.IndexOf(gameObject) + 1;
            if (index > 0)
            {
                gameObject.GetComponent<CardOrder>().SetOrder(index);
                MoveCardToPosAndScale(originalPosition, 1f);
            }
        }
    }

    private void MoveCardToPosAndScale(Vector3 position, float scale)
    {
        transform.DOKill();
        transform.DOMove(originalPosition, animationDuration);
        transform.DOScale(originalScale * scale, animationDuration);
    }

    private void MoveCardToPosition(Vector3 position)
    {
        transform.DOKill();
        transform.DOMove(originalPosition, animationDuration);
    }

    private void MoveCardToScale(float scale)
    {
        transform.DOKill();
        transform.DOScale(originalScale * scale, animationDuration);
    }

    private bool IsMouseOverCard(GameObject obj)
    {
        // 마우스 포인터 위치를 기준으로 Ray를 쏘아 충돌 체크
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.gameObject == obj && hit.collider.CompareTag("Card"))
            {
                return true;
            }
        }
        return false;
    }

    // 카드패널 충돌 확인
    private void CardPanelCollision()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit) && !cardProcessing.waitForInput)
        {
            if (hit.collider.gameObject.CompareTag("CardPanel"))
            {
                cardProcessing.usingCard = true;
                ProcessingCard();
            }
            else
            {
                CardManager.instance.FindPanelGroupChildObject("Use Card Panel(Clone)").SetActive(false);
            }
        }
    }

    // 카드 처리 과정
    private void ProcessingCard()
    {
        if (CardManager.instance != null && CardManager.instance.handCardObject != null)
        {
            int index = CardManager.instance.handCardObject.IndexOf(this.gameObject);
            if (index != -1)
            {
                if (cardProcessing == null)
                {
                    cardProcessing = GetComponent<CardProcessing>();
                }
                else
                {
                    MoveCardToScale(0f);
                    CardManager.instance.FindPanelGroupChildObject("Use Card Panel(Clone)").SetActive(false);
                    CardManager.instance.UpdateCardList(this.gameObject);
                    cardProcessing.UseCardAndSelectTarget(CardManager.instance.useCard, this.gameObject);
                }
            }
        }
        
    }

    private void OnMouseUp()
    {
        if (BattleManager.instance.isPlayerTurn)
        {
            CardPanelCollision();
            MapGenerator.instance.ClearHighlightedTiles();
        }
    }

    private void OnMouseDown()
    {
        int index = CardManager.instance.handCardObject.IndexOf(this.gameObject);
        Card card = null;

        if (IsMouseOverCard(gameObject) && index >= 0 && index < CardManager.instance.handCardList.Count)
        {
            card = CardManager.instance.handCardList[index];
        }
        else
        {
            card = null;
        }

        if (cardProcessing.currentPlayerObj != null && card != null)
        {
            GameObject playerObj = cardProcessing.currentPlayerObj;
            ComparePlayerTypeWithCardType(playerObj, card);
        }

        if (IsMouseOverCard(gameObject))
        {
            offset = transform.position - GetMouseWorldPosition();
            if (CardManager.instance.addCardObject.Contains(this.gameObject))
            {
                CardManager.instance.ChoiceCard(this.gameObject);
            }
            else if (!cardProcessing.waitForInput && !CardManager.instance.waitAddCard && card.isCardMoveEnabled)
            {
                CardManager.instance.FindPanelGroupChildObject("Use Card Panel(Clone)").SetActive(true);
            }
        }
    }

    private void OnMouseDrag()
    {
        int index = CardManager.instance.handCardObject.IndexOf(this.gameObject);
        Card card;

        if (index >= 0 && index < CardManager.instance.handCardList.Count)
        {
            card = CardManager.instance.handCardList[index]; ;
        }
        else
        {
            card = null;
        }

        if (cardProcessing.currentPlayerObj != null && card != null)
        {
            GameObject playerObj = cardProcessing.currentPlayerObj;
            ComparePlayerTypeWithCardType(playerObj, card);
        }

        if (!cardProcessing.waitForInput && !CardManager.instance.waitAddCard && card.isCardMoveEnabled)
        {
            transform.DOKill();
            transform.position = GetMouseWorldPosition() + offset;

            if (cardProcessing.currentPlayer != null)
            {
                int cardUseDistance = (int)CardManager.instance.CardDrag(this.gameObject).cardDistance;
                cardProcessing.ShowCardRange(cardUseDistance);
            }
        }
    }

    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = distanceToCamera;
        return Camera.main.ScreenToWorldPoint(mousePoint);
    }

    public void ComparePlayerTypeWithCardType(GameObject player, Card card)
    {
        if (card.cardType == CardType.BaseCard)
        {
            card.isCardMoveEnabled = true;
        }
        else if ((player == GameObject.Find("Warrior(HP)") || player == GameObject.Find("Warrior(ATK)")) && card.cardType == CardType.WarriorCard)
        {
            card.isCardMoveEnabled = true;
        }
        else if (player == GameObject.Find("Archer") && card.cardType == CardType.ArcherCard)
        {
            card.isCardMoveEnabled = true;
        }
        else if (player == GameObject.Find("Wizard") && card.cardType == CardType.WizardCard)
        {
            card.isCardMoveEnabled = true;
        }
        else
        {
            card.isCardMoveEnabled = false;
        }
        
    }
}
