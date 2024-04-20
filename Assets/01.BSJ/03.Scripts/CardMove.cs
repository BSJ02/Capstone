using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;

public class CardMove : MonoBehaviour
{
    private CardData cardData;
    private CardManager cardManager;
    private Vector3 offset;
    private float distanceToCamera;
    private Vector3 originalScale;   // 기본 크기
    [HideInInspector] public Vector3 originalPosition;   // 기본 위치
    private int originalOrderInLayer;

    private string cardSortingLayerName = "Card";    // 변경할 Sorting Layer의 이름
    private SpriteRenderer spriteRenderer;

    private const float scaleFactor = 1.2f; // 카드 확대 배율
    private const float animationDuration = 0.1f;   // 애니메이션 속도

    private bool clickOnCard = false;   // 마우스 클릭 여부
    //private bool panelCollision = false;  // 충돌 여부

    private void Start()
    {
        cardManager = FindObjectOfType<CardManager>();
        cardData = FindObjectOfType<CardData>();

        originalScale = this.transform.localScale;   // 기본 크기 저장

        spriteRenderer = GetComponent<SpriteRenderer>();
        GetComponent<Renderer>().sortingLayerName = cardSortingLayerName;
        originalOrderInLayer = spriteRenderer.sortingOrder;
    }


    private void Update()
    {
        if (IsMouseOverCard(this.gameObject))
        {
            AnimateCard(scaleFactor, originalPosition + Vector3.up * 0.5f);
            spriteRenderer.sortingOrder = 100;
            //gameObject.GetComponent<CardOrder>().SetOrder(100);
        }
        else
        {
            if (!clickOnCard)
            {

                AnimateCard(1f, originalPosition);
                spriteRenderer.sortingOrder = originalOrderInLayer;
                //gameObject.GetComponent<CardOrder>().ResetOrder();
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
            if (hit.collider.gameObject == obj && hit.collider.gameObject.layer == LayerMask.NameToLayer("Card"))
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
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.gameObject.CompareTag("CardPanel"))
            {
                ProcessingCard();
            }
        }
        cardManager.useCardPanelPrefab.SetActive(false);
    }


    // 카드 처리 과정
    private void ProcessingCard()
    {
        if (cardManager != null && cardManager.handCardObject != null)
        {
            int index = cardManager.handCardObject.IndexOf(this.gameObject);
            if (index != -1)
            {
                if (cardData == null)
                {
                    cardData = GetComponent<CardData>();
                }
                else
                {
                    MoveCardToScale(0f);
                    cardManager.UpdateCardList(this.gameObject);
                    cardData.UseCardAndSelectTarget(cardManager.useCard, this.gameObject);
                }
            }
        }
    }


    private void OnMouseUp()
    {
        clickOnCard = false;    // 클릭 상태 해제

        transform.DOKill();
        CardPanelCollision();
    }

    private void OnMouseDown()
    {
        if (IsMouseOverCard(gameObject))
        {
            clickOnCard = true;
            offset = transform.position - GetMouseWorldPosition();
            if (cardManager.addCardObject.Contains(this.gameObject))
            {
                cardManager.ChoiceCard(this.gameObject);
            }
            else
            {
                cardManager.useCardPanelPrefab.SetActive(true);
            }
        }
    }
  

    private void OnMouseDrag()
    {
        // 카드 클릭 & 대상 선택 & 카드 선택
        if (clickOnCard && !cardData.waitForInput && !cardManager.waitAddCard)  
        {
            transform.DOKill();
            transform.position = GetMouseWorldPosition() + offset;
        }
    }


    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = distanceToCamera;
        return Camera.main.ScreenToWorldPoint(mousePoint);
    }

}
