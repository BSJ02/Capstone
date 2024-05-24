using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;

    private CardProcessing cardProcessing;
    public Camera mainCamera;
    public CinemachineVirtualCamera virtualCamera;

    public GameObject canvas;

    public float moveSpeed = 5f;
    public float edgeSize = 10f;

    [HideInInspector] public bool isMainCameraMoving = false;

    private Vector3 characterOffset;
    private bool hasTransitioned = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        cardProcessing = FindObjectOfType<CardProcessing>();

        characterOffset = new Vector3(-9f, 7.65f, -9f);
    }

    private void Update()
    {
        if (BattleManager.instance.battleState == BattleState.PlayerTurn)
        {
            CameraFollowObject();

            if (isMainCameraMoving)
            {
                if (!hasTransitioned)
                {
                    StartCoroutine(FadeController.instance.Transition());
                    hasTransitioned = true;
                }
                HandleMouseMovement();
            }
            else if (cardProcessing.currentPlayerObj != null)
            {
                if (!hasTransitioned)
                {
                    StartCoroutine(FadeController.instance.Transition());
                    hasTransitioned = true;
                }
                FollowTarget(cardProcessing.currentPlayerObj);
            }
            if (Input.GetMouseButton(0))
            {
                isMainCameraMoving = false;
                hasTransitioned = false;
            }
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

        mainCamera.orthographicSize = 2f;

        if (mousePos.x < edgeSize || mousePos.y < edgeSize)
        {
            move.x = -moveSpeed * time;
        }
        else if (mousePos.x > Screen.width - edgeSize || mousePos.y > Screen.height - edgeSize)
        {
            move.x = moveSpeed * time;
        }

        if (mainCamera.transform.position == virtualCamera.transform.position)
        {
            virtualCamera.transform.position += move;
        }
        else
        {
            virtualCamera.transform.position = mainCamera.transform.position;
        }
    }

    public void FollowTarget(GameObject target)
    {
        if (target != null)
        {
            Vector3 newPosition = target.transform.position + characterOffset;
            
            virtualCamera.transform.position = newPosition;
        }
    }

    private void CameraFollowObject()
    {
        Vector3 newDeckPosition = mainCamera.transform.position + CardManager.instance.deckOffset;
        Vector3 newPanelPosition = mainCamera.transform.position + CardManager.instance.panelOffset;

        if (newDeckPosition != Vector3.zero && newPanelPosition != Vector3.zero)
        {
            CardManager.instance.deckObject.transform.position = newDeckPosition;
            CardManager.instance.panelObject_Group.transform.position = newPanelPosition;
        }
    }
}