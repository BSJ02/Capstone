using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public MapGenerator mapGenerator;
    public Player player;
    private BattleManager battleManager;
    private CardProcessing cardProcessing;

    Vector2Int playerPos;
    Vector2Int targetPos;
    Vector2Int monsterPos;

    Tile StartNode, EndNode, CurrentNode;
    List<Tile> OpenList = new List<Tile>();
    List<Tile> CloseList = new List<Tile>();

    public int detectionRange = 1;
    private List<Monster> detectedMonsters = new List<Monster>();
    private Monster clickedMonster;

    private GameObject clickedPlayer;

    private bool isMoving = false;

    private void Awake()
    {
        cardProcessing = FindObjectOfType<CardProcessing>();
    }

    private void SetDestination(Vector2Int clickedTargetPos)
    {
        playerPos = new Vector2Int((int)clickedPlayer.transform.position.x, (int)clickedPlayer.transform.position.z);
        targetPos = new Vector2Int(clickedTargetPos.x, clickedTargetPos.y);

        StartNode = mapGenerator.totalMap[playerPos.x, playerPos.y];
        EndNode = mapGenerator.totalMap[targetPos.x, targetPos.y];
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            //int TileLayerMask = 1 << LayerMask.NameToLayer("Tile");

            if (Physics.Raycast(ray, out hit, Mathf.Infinity/*, TileLayerMask*/) && !cardProcessing.usingCard)
            {
                if (hit.collider.CompareTag("Tile") && mapGenerator.IsHighlightedTile(hit.collider.GetComponent<Tile>()))
                {

                    Tile clickedTile = hit.collider.GetComponent<Tile>();

                    Vector2Int targetPos = ReturnTargetPosition(clickedTile.coord);

                    OpenList.Clear();
                    CloseList.Clear();
                    SetDestination(targetPos);
                    List<Vector2Int> move = PathFinding();
                    StartCoroutine(MoveSmoothly(move));
                    mapGenerator.ResetTotalMap();
                }
            }
        }

        if (Input.GetMouseButtonDown(0) && !isMoving /*&& !cardData.usingCard*/)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            int PlayerLayerMask = 1 << LayerMask.NameToLayer("Player");

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, PlayerLayerMask))
            {
                if (hit.collider.CompareTag("Player"))
                {
                    clickedPlayer = hit.collider.gameObject;
                    Player clickplayer = clickedPlayer.GetComponent<Player>();

                    cardProcessing.currentPlayerObj = clickedPlayer;
                    cardProcessing.currentPlayer = clickplayer;

                    mapGenerator.HighlightPlayerRange(clickedPlayer.transform.position, clickplayer.playerData.activePoint);
                }
            }


            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                if (hit.collider.CompareTag("Monster") && !cardProcessing.usingCard)
                {
                    clickedMonster = hit.collider.GetComponent<Monster>();

                    if (detectedMonsters.Contains(clickedMonster))
                    {
                        player.ReadyToAttack(clickedMonster);
                    }
                }
            }
        }

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
        clickedPlayer.layer = LayerMask.NameToLayer("Ignore Raycast");
        Player clickplayer = clickedPlayer.GetComponent<Player>();
        clickplayer.playerState = PlayerState.Moving;

        float moveSpeed = 1f;
        float lerpMaxTime = 0.2f;

        for (int i = 0; i < path.Count - 1; i++)
        {
            Vector3 playerPos = new Vector3(path[i].x, clickedPlayer.transform.position.y, path[i].y); // X�� Y ��ǥ�� Mathf.Round�� ����Ͽ� ���� ����� ������ �ݿø�
            Vector3 nextPosition = new Vector3(path[i + 1].x, clickedPlayer.transform.position.y, path[i + 1].y); // �������� ��ǥ�� ������ �ݿø�

            float startTime = Time.time;

            while (Time.time < startTime + lerpMaxTime)
            {
                float currentTime = (Time.time - startTime) * moveSpeed;
                float weight = currentTime / lerpMaxTime;

                clickedPlayer.transform.position = Vector3.Lerp(playerPos, nextPosition, weight);
                clickedPlayer.transform.LookAt(nextPosition);
                yield return null;
            }

            clickedPlayer.transform.position = nextPosition;


            clickplayer.playerData.activePoint--;

            if (0 >= clickplayer.playerData.activePoint)
                break;
        }

        isMoving = false;
        clickedPlayer.layer = LayerMask.NameToLayer("Player");
        clickplayer.playerState = PlayerState.Idle;

        Vector2Int finalPosition = new Vector2Int((int)clickedPlayer.transform.position.x, (int)clickedPlayer.transform.position.z);

        GetSurroundingTiles(finalPosition);
        clickedPlayer = null;

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

    // 몬스터 감지
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
    }
}