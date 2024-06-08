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

    private bool isMainCameraMoving = false;
    private bool hasTransitioned = false;

    private Vector3 characterOffset;
    private GameObject lastTarget = null;

    [HideInInspector] public bool startGame = true;

    // Zoom
    public GameObject panel;
    public GameObject zoomPanel;
    private float originalOrthographicSize;
    private float zoomSize;

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

        characterOffset = new Vector3(-11, 11f, -11);
        originalOrthographicSize = 6f;
        zoomSize = 4f;
        virtualCamera.m_Lens.OrthographicSize = originalOrthographicSize;
    }

    private void Update()
    {
        if (BattleManager.instance.battleState == BattleState.PlayerTurn)
        {
            CameraFollowObject();

            if (!isMainCameraMoving && !CardManager.instance.isCardSorting && !CardManager.instance.waitAddCard
                    && (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D)))
            {
                cardProcessing.currentPlayerObj = null;
                HandleMovement();
                CameraFollowObject();
            }
            else if (cardProcessing.currentPlayerObj != null)
            {
                FollowTarget(cardProcessing.currentPlayerObj);
                CameraFollowObject();
            }
            else
            {
                cardProcessing.currentPlayerObj = null;
            }
        }
        else if (BattleManager.instance.battleState == BattleState.MonsterTurn)
        {
            
            FollowTarget(BattleManager.instance.selectedMonster);
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

    private void HandleMovement()
    {
        Vector3 move = Vector3.zero;
        float time = Time.deltaTime;

        if (Input.GetKey(KeyCode.A))
        {
            move.x = -moveSpeed * time * 0.5f;
            move.z = moveSpeed * time * 0.5f;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            move.x = -moveSpeed * time;
            move.z = -moveSpeed * time;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            move.x = moveSpeed * time * 0.5f;
            move.z = -moveSpeed * time * 0.5f;
        }
        else if (Input.GetKey(KeyCode.W))
        {
            move.x = moveSpeed * time;
            move.z = moveSpeed * time;
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
        panel.SetActive(!zoomIn);
        zoomPanel.SetActive(zoomIn);
    }

    public IEnumerator StartCameraMoving()
    {
        isMainCameraMoving = true;
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
                isMainCameraMoving = false;
                break;
            }

            move.x = moveSpeed * time;
            virtualCamera.transform.position += move;
            yield return null;
        }

        ZoomCamera(false);
        yield return null;
    }
}