using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera;
    public float moveSpeed = 10f;
    public float edgeSize = 10f;
    public bool setPos = false;

    void Update()
    {
        if (!CardManager.instance.isCardSorting && !CardManager.instance.waitAddCard)
        {
            HandleMouseMovement();
            UpdatePositions();
        }
    }

    private Vector3 HandleMouseMovement()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 move = Vector3.zero;
        float time = Time.deltaTime;

        if (mousePos.x < edgeSize || mousePos.y < edgeSize)
        {
            CardManager.instance.isMainCameraMoving = true;
            setPos = true;

            move.x = -moveSpeed * time;
        }
        else if (mousePos.x > Screen.width - edgeSize || mousePos.y > Screen.height - edgeSize)
        {
            CardManager.instance.isMainCameraMoving = true;
            setPos = true;

            move.x = moveSpeed * time;
        }
        else
        {
            CardManager.instance.isMainCameraMoving = false;
            setPos = false;

        }
        return move;
    }

    private void UpdateCardPositions()
    {
        foreach (var cardObject in CardManager.instance.handCardObject)
        {
            var cardMove = cardObject.GetComponent<CardMove>();
            if (cardMove != null)
            {
                cardMove.UpdateOriginalPosition();
            }
        }
    }

    public void UpdatePositions()
    {
        CardManager.instance.panelObject_Group.transform.position += HandleMouseMovement();
        CardManager.instance.deckObject.transform.position += HandleMouseMovement();

        if (CardManager.instance.isMainCameraMoving)
        {
            UpdateCardPositions();
        }

        virtualCamera.transform.position += HandleMouseMovement();
    }
}