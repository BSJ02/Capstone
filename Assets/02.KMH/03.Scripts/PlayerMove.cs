using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public Player player;

    private GameObject playerChoice;

    private MapGenerator mapGenerator;
    private BattleManager battleManager;
    private CardProcessing cardProcessing;
    private PlayerManager playerManager;


    Vector2Int playerPos;
    Vector2Int targetPos;

    Tile StartNode, EndNode, CurrentNode;
    List<Tile> OpenList = new List<Tile>();
    List<Tile> CloseList = new List<Tile>();

    public int detectionRange = 1;
    private List<Monster> detectedMonsters = new List<Monster>();
    private Monster clickedMonster;

/*    [SerializeField]
    private GameObject clickedPlayer;*/

    private bool isMoving = false;
    private bool isActionSelect = false;

    private void Awake()
    {
        cardProcessing = FindObjectOfType<CardProcessing>();
        battleManager = FindObjectOfType<BattleManager>();
        mapGenerator = FindObjectOfType<MapGenerator>();
        playerManager = FindObjectOfType<PlayerManager>();
        playerChoice = GameObject.FindGameObjectWithTag("PlayerChoice");
    }

    private void Start()
    {
        playerChoice.SetActive(false);
    }

    private void SetDestination(Vector2Int clickedTargetPos)
    {
        playerPos = new Vector2Int((int)playerManager.clickedPlayer.transform.position.x, (int)playerManager.clickedPlayer.transform.position.z);
        targetPos = new Vector2Int(clickedTargetPos.x, clickedTargetPos.y);

        StartNode = mapGenerator.totalMap[playerPos.x, playerPos.y];
        EndNode = mapGenerator.totalMap[targetPos.x, targetPos.y];
    }

    private void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(ray, out hit, Mathf.Infinity) && !cardProcessing.usingCard)
            {
                if (hit.collider != null)
                {
                    if (hit.collider.CompareTag("Tile"))
                    {
                        Tile clickedTile = hit.collider.GetComponent<Tile>();
                        if (clickedTile != null && mapGenerator.IsHighlightedTile(clickedTile))
                        {
                            Vector2Int targetPos = ReturnTargetPosition(clickedTile.coord);

                            OpenList.Clear();
                            CloseList.Clear();
                            SetDestination(targetPos);
                            List<Vector2Int> move = PathFinding();
                            StartCoroutine(MoveSmoothly(move));
                            mapGenerator.ResetTotalMap();
                        }
                        else
                        {
                            ResetSelection();
                        }
                    }
                    else if (hit.collider.CompareTag("Monster"))
                    {
                        Monster monster = hit.collider.GetComponent<Monster>();
                        if (monster != null)
                        {
                            List<Vector2Int> tiles = monster.GetComponent<MonsterMove>().AttackRangeChecking
                                (new Vector2Int((int)monster.transform.position.x, (int)monster.transform.position.z), monster.monsterData.SkillDetectionRange, true);

                            // Reset all tile colors to white
                            ResetTilesColor();

                            // Highlight monster's range
                            foreach (var tile in tiles)
                            {
                                Tile coord = MapGenerator.instance.totalMap[tile.x, tile.y];
                                if (coord != null)
                                {
                                    coord.SetColor(Color.red);
                                }
                            }
                        }
                    }
                    else
                    {
                        ResetSelection();
                    }
                }
            }


            if (Input.GetMouseButtonDown(0) && !isMoving)
            {
                int PlayerLayerMask = 1 << LayerMask.NameToLayer("Player");

                if (Physics.Raycast(ray, out hit, Mathf.Infinity, PlayerLayerMask))
                {

                    if (hit.collider.CompareTag("Player"))
                    {
                        mapGenerator.ClearHighlightedTiles();
                        playerManager.detectedMonsters.Clear();
                        playerChoice.SetActive(true);
                        playerManager.clickedPlayer = hit.collider.gameObject;

                        cardProcessing.currentPlayerObj = playerManager.clickedPlayer;
                        cardProcessing.currentPlayer = playerManager.clickedPlayer.GetComponent<Player>();

                        isActionSelect = true;
                    }
                }


                if (Physics.Raycast(ray, out hit, Mathf.Infinity) && battleManager.isPlayerTurn == true && isActionSelect == true)
                {
                    if (hit.collider.CompareTag("Monster") && !cardProcessing.usingCard)
                    {
                        clickedMonster = hit.collider.GetComponent<Monster>();

                        if (playerManager.detectedMonsters.Contains(clickedMonster))
                        {
                            Player clickPlayer = playerManager.clickedPlayer.GetComponent<Player>();
                            if (clickPlayer.isAttack == false)
                            {
                                clickPlayer.ReadyToAttack(clickedMonster);
                                isActionSelect = false;
                            }
                        }
                    }
                }
            }


            if (battleManager.isPlayerTurn == false)
            {
                playerChoice.SetActive(false);
            }

        }
    }

    private void ResetSelection()
    {
        mapGenerator.ClearHighlightedTiles();
        playerManager.detectedMonsters.Clear();
        //playerManager.clickedPlayer = null;
        clickedMonster = null;
        playerChoice.SetActive(false);

        // Reset all tile colors to white
        ResetTilesColor();
    }

    private void ResetTilesColor()
    {
        foreach (Tile tile in mapGenerator.totalMap)
        {
            if (tile != null)
            {
                tile.SetColor(Color.white);
            }
        }
    }


/*    // Clicked MoveButton
    public void OnMoveButtonClick()
    {
        // Code
        Player clickPlayer = playerManager.clickedPlayer.GetComponent<Player>();

        if (clickPlayer.playerData.activePoint <= 0)
        {
            Debug.Log("No remaining ActivePoints");
            playerChoice.SetActive(false);
        }
        else
        {
            mapGenerator.HighlightPlayerRange(playerManager.clickedPlayer.transform.position, clickPlayer.playerData.activePoint);
        }
    }

    // Clicked AttackButton
    public void OnAttackButtonClick()
    {
        Player clickPlayer = playerManager.clickedPlayer.GetComponent<Player>();

        // Code
        if (clickPlayer.isAttack == true)
        {
            Debug.Log("Already Attack");
            playerChoice.SetActive(false);
        }
        else
        {
            Vector2Int finalPosition = new Vector2Int((int)playerManager.clickedPlayer.transform.position.x, (int)playerManager.clickedPlayer.transform.position.z);
            GetSurroundingTiles(finalPosition);
        }
    }*/

    public void OnCardButtonClick()
    {
        cardProcessing.usingCard = true;
        playerChoice.SetActive(false);
    }

    private List<Vector2Int> PathFinding()
    {
        OpenList.Add(StartNode);

        List<Vector2Int> path = new List<Vector2Int>();


        path.Clear();

        while (OpenList.Count > 0)
        {
            CurrentNode = OpenList[0];

            for (int i = 1; i < OpenList.Count; i++)
            {
                if (OpenList[i].coord.F <= CurrentNode.coord.F && OpenList[i].coord.H < CurrentNode.coord.H)
                {
                    CurrentNode = OpenList[i];
                }
            }

            OpenList.Remove(CurrentNode);
            CloseList.Add(CurrentNode);

            if (CurrentNode == EndNode)
            {
                Tile currentNode = EndNode;

                while (currentNode != null)
                {
                    path.Add(new Vector2Int(currentNode.coord.x, currentNode.coord.y));
                    currentNode = currentNode.coord.parentNode;
                }

                path.Reverse();

                //foreach (var pos in path)
                //{
                //    Debug.Log("X��:" + pos.x + "Y��:" + pos.y);
                //}

                break;
            }

            OpenListAdd(CurrentNode.coord.x, CurrentNode.coord.y + 1);
            OpenListAdd(CurrentNode.coord.x + 1, CurrentNode.coord.y);
            OpenListAdd(CurrentNode.coord.x, CurrentNode.coord.y - 1);
            OpenListAdd(CurrentNode.coord.x - 1, CurrentNode.coord.y);
        }

        return path;
    }

    private Vector2Int ReturnTargetPosition(Coord destination)
    {
        Vector2Int clickedCoord = new Vector2Int(destination.x, destination.y);
        mapGenerator.ClearHighlightedTiles();
        return clickedCoord;
    }

    private IEnumerator MoveSmoothly(List<Vector2Int> path)
    {
        isMoving = true;
        for (int i = 0; i < 2; i++)
        {
            battleManager.players[i].layer = LayerMask.NameToLayer("Ignore Raycast");
        }
        Player clickPlayer = playerManager.clickedPlayer.GetComponent<Player>();
        clickPlayer.playerState = PlayerState.Moving;
        playerChoice.SetActive(false);

        float moveSpeed = 1f;
        float lerpMaxTime = 0.2f;

        for (int i = 0; i < path.Count - 1; i++)
        {
            Vector3 playerPos = new Vector3(path[i].x, playerManager.clickedPlayer.transform.position.y, path[i].y);
            Vector3 nextPosition = new Vector3(path[i + 1].x, playerManager.clickedPlayer.transform.position.y, path[i + 1].y);

            float startTime = Time.time;

            while (Time.time < startTime + lerpMaxTime)
            {
                float currentTime = (Time.time - startTime) * moveSpeed;
                float weight = currentTime / lerpMaxTime;

                playerManager.clickedPlayer.transform.position = Vector3.Lerp(playerPos, nextPosition, weight);
                playerManager.clickedPlayer.transform.LookAt(nextPosition);
                yield return null;
            }

            playerManager.clickedPlayer.transform.position = nextPosition;


            clickPlayer.playerData.activePoint--;

            if (0 >= clickPlayer.playerData.activePoint)
                break;
        }

        isMoving = false;
        isActionSelect = false;
        for (int i = 0; i < 2; i++)
        {
            battleManager.players[i].layer = LayerMask.NameToLayer("Player");
        }
        clickPlayer.playerState = PlayerState.Idle;


        playerManager.clickedPlayer = null;

        yield break;
    }

    private void OpenListAdd(int checkX, int checkY)
    {
        if (checkX < 0 || checkX >= mapGenerator.totalMap.GetLength(0) || checkY < 0 || checkY >= mapGenerator.totalMap.GetLength(1))
            return;

        if (CloseList.Contains(mapGenerator.totalMap[checkX, checkY]))
            return;

        if (mapGenerator.totalMap[checkX, checkY].coord.isWall)
            return;

        if (OpenList.Contains(mapGenerator.totalMap[checkX, checkY]))
        {
            int newG = CurrentNode.coord.G + (Mathf.Abs(CurrentNode.coord.x - checkX) == 0 || Mathf.Abs(CurrentNode.coord.y - checkY) == 0 ? 10 : 14);
            if (newG < mapGenerator.totalMap[checkX, checkY].coord.G)
            {
                mapGenerator.totalMap[checkX, checkY].coord.G = newG;
                mapGenerator.totalMap[checkX, checkY].coord.parentNode = CurrentNode;
            }
        }
        else
        {
            mapGenerator.totalMap[checkX, checkY].coord.G = CurrentNode.coord.G + (Mathf.Abs(CurrentNode.coord.x - checkX) == 0 || Mathf.Abs(CurrentNode.coord.y - checkY) == 0 ? 10 : 14);
            mapGenerator.totalMap[checkX, checkY].coord.H = (Mathf.Abs(checkX - EndNode.coord.x) + Mathf.Abs(checkY - EndNode.coord.y)) * 10;
            mapGenerator.totalMap[checkX, checkY].coord.parentNode = CurrentNode;
            OpenList.Add(mapGenerator.totalMap[checkX, checkY]);
        }
    }

    //private void OnMouseDown()
    //{
    //    if (!isMoving)
    //    {
    //        mapGenerator.HighlightPlayerRange(transform.position, player.playerData.activePoint);
    //    }
    //}

/*    // 몬스터 감지
    public void GetSurroundingTiles(Vector2Int playerPos)
    {
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
            if (distanceX <= detectionRange && distanceY <= detectionRange)
            {
                detectedMonsters.Add(m);
                Debug.Log(m);
            }
        }
    }*/
}