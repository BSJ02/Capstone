using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera;
    public float moveSpeed = 10f;
    public float edgeSize = 10f;
    [HideInInspector] public bool isMainCameraMoving = false;

    private void Update()
    {

        if (isMainCameraMoving)
        {
            HandleMouseMovement();
        }

        if (Input.GetMouseButton(0))
        {
            isMainCameraMoving = false;
        }
    }

    public void CameraMoveButton()
    {
        isMainCameraMoving = true;
    }

    private void HandleMouseMovement()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 move = Vector3.zero;
        float time = Time.deltaTime;

        if (mousePos.x < edgeSize || mousePos.y < edgeSize)
        {
            move.x = -moveSpeed * time;
        }
        else if (mousePos.x > Screen.width - edgeSize || mousePos.y > Screen.height - edgeSize)
        {
            move.x = moveSpeed * time;
        }

        CardManager.instance.panelObject_Group.transform.position += move;
        CardManager.instance.deckObject.transform.position += move;
        virtualCamera.transform.position += move;
    }

}