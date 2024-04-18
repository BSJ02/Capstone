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
    private Vector3 originalScale;  // �⺻ ũ��
    public Vector3 originalPosition;   // �⺻ ��ġ
    private int originalOrderInLayer;

    public string cardSortingLayerName = "Card";    // ������ Sorting Layer�� �̸�
    private SpriteRenderer spriteRenderer;

    private const float scaleFactor = 1.2f;
    private const float animationDuration = 0.1f;

    private bool clickOnCard = false;

    void Start()
    {
        cardManager = FindObjectOfType<CardManager>();

        originalScale = this.transform.localScale;   // �⺻ ũ�� ����
        originalPosition = this.transform.position;  // �⺻ ��ġ ����
        spriteRenderer = GetComponent<SpriteRenderer>();

        GetComponent<Renderer>().sortingLayerName = cardSortingLayerName;
        originalOrderInLayer = spriteRenderer.sortingOrder;
    }


    void Update()
    {
        if (IsMouseOverCard(this.gameObject))
        {
            AnimateCard(scaleFactor, originalPosition + Vector3.up);
            spriteRenderer.sortingOrder = 10;
        }
        else
        {
            if (clickOnCard == false)
            {
                AnimateCard(1f, originalPosition);
                spriteRenderer.sortingOrder = originalOrderInLayer;
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
        transform.DOKill();
        transform.DOScale(originalScale * scale, animationDuration);
        transform.DOMove(position, animationDuration);
    }


    private bool IsMouseOverCard(GameObject obj)
    {
        // ���콺 ������ ��ġ�� �������� Ray�� ��� �浹 üũ
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


    //ī�� �гΰ� �浹 ó��
    private void OnTriggerEnter(Collider other)
    {
        // addCardObject ����Ʈ�� ��ȸ�ϸ鼭 ������ cardObject�� �ִ��� Ȯ���մϴ�.
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
            if (other.gameObject.CompareTag("Panel"))
            {
                cardManager.UseToCard(this.gameObject);
            }
        }
        
    }

    private void OnMouseUp()
    {
        clickOnCard = false;
        //UnityEngine.Debug.Log(clickOnCard);
        transform.DOKill();
        transform.DOMove(originalPosition, animationDuration);  
    }


    private void OnMouseDown()
    {
        if (IsMouseOverCard(gameObject))
        {
            clickOnCard = true;
            //UnityEngine.Debug.Log(clickOnCard);
            offset = transform.position - GetMouseWorldPosition();
            cardManager.ChoiceCard(this.gameObject);
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
