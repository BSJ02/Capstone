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
        bool mouseOverCard = false;

        foreach (var cardObject in CardManager.instance.handCardObject)
        {
            var cardMove = cardObject.GetComponent<CardMove>();
            if (cardMove != null && cardMove.mouseOverCard)
            {
                mouseOverCard = true;
                break;
            }
        }

        if (!mouseOverCard)
        {
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
        }

        CardManager.instance.panelObject_Group.transform.position += move;
        CardManager.instance.deckObject.transform.position += move;
        virtualCamera.transform.position += move;
    }

}