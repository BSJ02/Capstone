using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;
using System;

public class CardMove : MonoBehaviour
{
    private CardProcessing cardProcessing;
    private CardManager cardManager;
    private BattleManager battleManager;
    private MapGenerator mapGenerator;

    private Vector3 offset;
    private float distanceToCamera;
    private Vector3 originalScale;   // 기본 크기
    [HideInInspector] public Vector3 originalPosition;   // 기본 위치

    private const float scaleFactor = 1.2f; // 카드 확대 배율
    private const float animationDuration = 0.1f;   // 애니메이션 속도

    //private bool cardMoveAble = false;

    private int index;

    private void Start()
    {
        cardManager = FindObjectOfType<CardManager>();
        cardProcessing = FindObjectOfType<CardProcessing>();
        battleManager = FindObjectOfType<BattleManager>();
        mapGenerator = FindObjectOfType<MapGenerator>();

        originalScale = this.transform.localScale;   // 기본 크기 저장
    }


    private void Update()
    {
        if (IsMouseOverCard(this.gameObject) && !cardManager.waitAddCard)
        {
            index = cardManager.handCardObject.IndexOf(gameObject) + 1;
            AnimateCard(scaleFactor, originalPosition + Vector3.up * 0.5f);
            gameObject.GetComponent<CardOrder>().SetOrder(index * 10);
        }
        else
        {
            index = cardManager.handCardObject.IndexOf(gameObject) + 1;
            if (cardManager.handCardObject.IndexOf(gameObject) >= 0)
            {
                gameObject.GetComponent<CardOrder>().SetOrder(index);
                AnimateCard(1f, originalPosition);
            }
        }
    }

    void AnimateCard(float scale, Vector3 position)
    {
        transform.DOKill();
        transform.DOMove(position, animationDuration);
        transform.DOScale(originalScale * scale, animationDuration);
    }

    // Position 이동
    private void MoveCardToPosition(Vector3 position)
    {
        transform.DOKill();
        transform.DOMove(position, animationDuration);
    }

    // Scale 변경
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
                cardManager.useCardPanelPrefab.SetActive(false);
            }
        }
    }


    // 카드 처리 과정
    private void ProcessingCard()
    {
        if (cardManager != null && cardManager.handCardObject != null)
        {
            int index = cardManager.handCardObject.IndexOf(this.gameObject);
            if (index != -1)
            {
                if (cardProcessing == null)
                {
                    cardProcessing = GetComponent<CardProcessing>();
                }
                else
                {
                    MoveCardToScale(0f);
                    cardManager.useCardPanelPrefab.SetActive(false);
                    cardManager.UpdateCardList(this.gameObject);
                    cardProcessing.UseCardAndSelectTarget(cardManager.useCard, this.gameObject);
                }
            }
        }
        
    }


    private void OnMouseUp()
    {
        transform.DOKill();

        if (battleManager.isPlayerTurn)
        {
            CardPanelCollision();
            mapGenerator.ClearHighlightedTiles();
        }
        
    }

    private void OnMouseDown()
    {
        if (IsMouseOverCard(gameObject))
        {
            offset = transform.position - GetMouseWorldPosition();
            if (cardManager.addCardObject.Contains(this.gameObject))
            {
                cardManager.ChoiceCard(this.gameObject);
            }
            else if (!cardProcessing.waitForInput && !cardManager.waitAddCard)
            {
                cardManager.useCardPanelPrefab.SetActive(true);
            }
        }
        if (cardProcessing.waitForInput)
        {
            UnityEngine.Debug.Log("cardProcessing.waitForInput" + battleManager.isPlayerTurn);
        }
        if (cardManager.waitAddCard)
        {
            UnityEngine.Debug.Log("cardManager.waitAddCard" + battleManager.isPlayerTurn);
        }
    }
  

    private void OnMouseDrag()
    {
        // 카드 클릭 & !대상 선택 & !카드 선택
        if (!cardProcessing.waitForInput && !cardManager.waitAddCard && battleManager.isPlayerTurn)
        {
            transform.DOKill();
            transform.position = GetMouseWorldPosition() + offset;

            if (cardProcessing.currentPlayer != null)
            {
                int cardUseDistance = (int)cardManager.CardDrag(this.gameObject).cardDistance;
                cardProcessing.ShowCardRange(cardUseDistance);
            }
        }
        else
        {
            return;
        }
    }


    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = distanceToCamera;
        return Camera.main.ScreenToWorldPoint(mousePoint);
    }

}
