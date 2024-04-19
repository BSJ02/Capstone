using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;

public class CardMove : MonoBehaviour
{
    private CardManager cardManager;
    private Vector3 offset;
    private float distanceToCamera;
    private Vector3 originalScale;   // 기본 크기
    [HideInInspector] public Vector3 originalPosition;   // 기본 위치
    private int originalOrderInLayer;

    private string cardSortingLayerName = "Card";    // 변경할 Sorting Layer의 이름
    private SpriteRenderer spriteRenderer;

    private const float scaleFactor = 1.2f;
    private const float animationDuration = 0.1f;   // 애니메이션 속도

    private bool clickOnCard = false;   // 마우스 클릭 여부
    //private bool panelCollision = false;  // 충돌 여부

    private void Start()
    {
        cardManager = FindObjectOfType<CardManager>();

        originalScale = this.transform.localScale;   // 기본 크기 저장
        originalPosition = this.transform.position;   // 기본 위치 저장
        spriteRenderer = GetComponent<SpriteRenderer>();

        GetComponent<Renderer>().sortingLayerName = cardSortingLayerName;
        originalOrderInLayer = spriteRenderer.sortingOrder;
    }


    private void Update()
    {
        if (IsMouseOverCard(this.gameObject))
        {
            AnimateCard(scaleFactor, originalPosition + Vector3.up);
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


    private void AnimateCard(float scale, Vector3 position)
    {
        transform.DOKill();
        transform.DOScale(originalScale * scale, animationDuration);
        transform.DOMove(position, animationDuration);
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


    //카드 패널과 충돌 처리
    private void OnTriggerStay(Collider other)
    {
        if (!clickOnCard && cardManager != null && cardManager.handCardObject != null)
        {
            // handCardObject 리스트를 순회하면서 선택한 cardObject가 있는지 확인합니다.
            bool isFound = false;
            int index = -1;
            for (int i = 0; i < cardManager.handCardObject.Count; i++)
            {
                if (cardManager.handCardObject[i] == this.gameObject)
                {
                    isFound = true;
                    index = i;
                    break;
                }
            }

            if (isFound)
            {
                if (other.gameObject.CompareTag("CardPanel"))
                {
                    cardManager.UpdateCardList(this.gameObject);
                    UnityEngine.Debug.Log("충돌함");
                    cardManager.useCardPanelPrefab.SetActive(false);
                }

            }   
        }
    }

    private void OnMouseUp()
    {
        // 클릭 상태를 해제합니다.
        clickOnCard = false;

        // 카드를 기본 위치로 이동합니다.
        transform.DOKill();
        transform.DOMove(originalPosition, animationDuration);
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
        if (clickOnCard)
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
