using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera;
    public float moveSpeed = 5f;
    public float edgeSize = 10f; // 화면 가장자리 감지 범위

    void Update()
    {
        if (!CardManager.instance.isCardSorting && !CardManager.instance.waitAddCard)
        {
            HandleMouseMovement();
        }
    }

    private void HandleMouseMovement()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 move = Vector3.zero;
        float time = Time.deltaTime;

        if (mousePos.x < edgeSize || mousePos.y < edgeSize)
        {
            CardManager.instance.isMainCameraMoving = true;
            move.x = -moveSpeed * time;
        }
        else if (mousePos.x > Screen.width - edgeSize || mousePos.y > Screen.height - edgeSize)
        {
            CardManager.instance.isMainCameraMoving = true;
            move.x = moveSpeed * time;
        }
        else
        {
            CardManager.instance.isMainCameraMoving = false;
        }

        if (CardManager.instance.isMainCameraMoving)
        {
            UpdateCardPositions(move);
        }

        CardManager.instance.panelObject_Group.transform.position += move;
        CardManager.instance.deckObject.transform.position += move;
        virtualCamera.transform.position += move;
    }

    private void UpdateCardPositions(Vector3 move)
    {
        foreach (var cardObject in CardManager.instance.handCardObject)
        {
            cardObject.transform.position += move;

            var cardMove = cardObject.GetComponent<CardMove>();
            if (cardMove != null)
            {
                cardMove.UpdateOriginalPosition();
            }
        }
    }
}