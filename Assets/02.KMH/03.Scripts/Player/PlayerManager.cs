using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;

    public GameObject clickedPlayer;
    private GameObject playerChoice;

    public List<Monster> detectedMonsters = new List<Monster>();

    private void Awake()
    {
        playerChoice = GameObject.FindGameObjectWithTag("PlayerChoice");

        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        //playerChoice.SetActive(false);
    }

    void Update()
    {
        // ClickedPlayer
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            int PlayerLayerMask = 1 << LayerMask.NameToLayer("Player");

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, PlayerLayerMask))
            {

                if (hit.collider.CompareTag("Player"))
                {
                    //playerMove.playerChoice.SetActive(true);
                    
                    clickedPlayer = hit.collider.gameObject;
                    //playerMove.isActionSelect = true;
                }
            }
        }
    }

    // Clicked MoveButton
    public void OnMoveButtonClick()
    {
        // Code
        Player clickPlayer = clickedPlayer.GetComponent<Player>();

        if (clickPlayer.playerData.activePoint <= 0)
        {
            Debug.Log("No remaining ActivePoints");
            playerChoice.SetActive(false);
            SoundManager.instance.PlaySoundEffect("Impossible");
        }
        else
        {
            MapGenerator.instance.HighlightPlayerRange(clickedPlayer.transform.position, clickPlayer.playerData.activePoint);
        }

        CardManager.instance.isCardButtonClicked = false;
    }

    // Clicked AttackButton
    public void OnAttackButtonClick()
    {
        Player clickPlayer = clickedPlayer.GetComponent<Player>();

        // Code
        if (clickPlayer.isAttack == true)
        {
            Debug.Log("Already Attack");
            playerChoice.SetActive(false);
            SoundManager.instance.PlaySoundEffect("Impossible");
        }
/*        else if (detectedMonsters.Count == 0)
        {
            Debug.Log("No target to attack");
            playerChoice.SetActive(false);
            SoundManager.instance.PlaySoundEffect("Impossible");
        }*/

        else
        {
            Vector2Int finalPosition = new Vector2Int((int)clickedPlayer.transform.position.x, (int)clickedPlayer.transform.position.z);
            GetSurroundingTiles(finalPosition);
        }

        CardManager.instance.isCardButtonClicked = false;
    }

    public void OnCardButtonClick()
    {
        CardManager.instance.isCardButtonClicked = true;
    }

    // 몬스터 감지
    public void GetSurroundingTiles(Vector2Int playerPos)
    {
        Player player = clickedPlayer.GetComponent<Player>();

        detectedMonsters.Clear();

        Monster[] monsters = FindObjectsOfType<Monster>();

        foreach (Monster m in monsters)
        {
            Vector3 monsterPosition = m.transform.position;
            Vector2Int monsterPos = new Vector2Int((int)monsterPosition.x, (int)monsterPosition.z);

            //플레이어와 몬스터 거리 계산
            int distanceX = Mathf.Abs(playerPos.x - monsterPos.x);
            int distanceY = Mathf.Abs(playerPos.y - monsterPos.y);

            //몬스터가 감지 범위 안에 있으면 실행
            if (distanceX <= player.playerData.detectionRange && distanceY <= player.playerData.detectionRange)
            {
                detectedMonsters.Add(m);
                Debug.Log(m);
            }
        }
    }
}
