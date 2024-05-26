using Cinemachine;
using System.Collections;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;

    private CardProcessing cardProcessing;
    public Camera mainCamera;
    public CinemachineVirtualCamera virtualCamera;

    private float moveSpeed = 7f;
    private float edgeSize = 10f;

    [HideInInspector] public bool isMainCameraMoving = false;
    private bool hasTransitioned = false;

    private Vector3 characterOffset;
    private GameObject lastTarget = null;

    [HideInInspector] public bool startGame = true;

    // Zoom
    public GameObject canvas;
    public GameObject zoomPanel;
    private float originalOrthographicSize;
    private float zoomSize = 4f;

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
        originalOrthographicSize = 6f;
    }

    private void Update()
    {
        if (BattleManager.instance.battleState == BattleState.PlayerTurn)
        {
            CameraFollowObject();

            if (isMainCameraMoving)
            {
                HandleMouseMovement();
                ZoomCamera(true);
            }
            else if (cardProcessing.currentPlayerObj != null)
            {
                FollowTarget(cardProcessing.currentPlayerObj);
                CameraFollowObject();
            }
            if (Input.GetMouseButton(0) && isMainCameraMoving)
            {
                hasTransitioned = false;
                HasTransition();

                hasTransitioned = false;
                isMainCameraMoving = false;

                ZoomCamera(false);
            }
        }
    }

    private void HasTransition()
    {
        if (!hasTransitioned)
        {
            StartCoroutine(FadeController.instance.FadeInOut());
            hasTransitioned = true;
        }
    }

    public void CameraMoveButton()
    {
        isMainCameraMoving = true;
    }

    private void HandleMouseMovement()
    {
        HasTransition();

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
        if (target != lastTarget)
        {
            hasTransitioned = false;
            HasTransition();
            lastTarget = target;
        }

        if (target != null)
        {
            Vector3 newPosition = target.transform.position + characterOffset;
            virtualCamera.transform.position = newPosition;

            if (target.CompareTag("Monster"))
            {
                ZoomCamera(true);
            }
        }
    }

    private void CameraFollowObject()
    {
        Vector3 newDeckPosition = mainCamera.transform.position + CardManager.instance.deckOffset;
        Vector3 newPanelPosition = mainCamera.transform.position + CardManager.instance.panelOffset;

        CardManager.instance.deckObject.transform.position = newDeckPosition;
        CardManager.instance.panelObject_Group.transform.position = newPanelPosition;
    }

    public void ZoomCamera(bool zoomIn)
    {
        if (zoomIn)
        {
            virtualCamera.m_Lens.OrthographicSize = zoomSize;
        }
        else
        {
            virtualCamera.m_Lens.OrthographicSize = originalOrthographicSize;
        }

        CardManager.instance.deckObject.SetActive(!zoomIn);
        CardManager.instance.panelObject_Group.SetActive(!zoomIn);
        canvas.SetActive(!zoomIn);
        zoomPanel.SetActive(zoomIn);
    }

    public IEnumerator StartCameraMoving()
    {
        Vector3 originalCameraPos = mainCamera.transform.position;

        ZoomCamera(true);

        yield return new WaitForSeconds(1f);
        Vector3 startCameraPos = virtualCamera.transform.position;

        Vector3 move = Vector3.zero;
        float time = Time.deltaTime;

        while (!CardManager.instance.isSettingCards)
        {
            if (mainCamera.transform.position != virtualCamera.transform.position)
            {
                yield return new WaitForSeconds(0.3f);
                ZoomCamera(false);
                StartCoroutine(FadeController.instance.FadeInOut());
                virtualCamera.transform.position = originalCameraPos;
                break;
            }

            move.x = moveSpeed * time;
            virtualCamera.transform.position += move;
            yield return null;
        }

        ZoomCamera(false);

        yield return new WaitForSeconds(1f);
    }
}