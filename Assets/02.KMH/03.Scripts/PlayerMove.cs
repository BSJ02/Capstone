using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    //public MapGenerator mapGenerator;
    public Player player;
    public PlayerData playerData;

    Vector2Int playerPos;
    Vector2Int targetPos;
    Vector2Int monsterPos;

    Tile StartNode, EndNode, CurrentNode;
    List<Tile> OpenList = new List<Tile>();
    List<Tile> CloseList = new List<Tile>();

    public int detectionRange = 1;

    private bool isMoving = false; // 이동 중인지 여부

    public void SetDestination(Vector2Int clickedTargetPos)
    {
        playerPos = new Vector2Int((int)transform.position.x, (int)transform.position.z);
        targetPos = new Vector2Int(clickedTargetPos.x, clickedTargetPos.y);

        StartNode = MapGenerator.instance.totalMap[playerPos.x, playerPos.y];
        EndNode = MapGenerator.instance.totalMap[targetPos.x, targetPos.y];
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isMoving)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.CompareTag("Tile") && MapGenerator.instance.IsHighlightedTile(hit.collider.GetComponent<Tile>()))
                {
                    // 타일을 클릭했을 때 플레이어 이동
                    Tile clickedTile = hit.collider.GetComponent<Tile>();

                    // target의 위치 설정
                    Vector2Int targetPos = ReturnTargetPosition(clickedTile.coord); // 클릭한 좌표값 = targetPos

                    OpenList.Clear();
                    CloseList.Clear();
                    SetDestination(targetPos);
                    List<Vector2Int> move = PathFinding();
                    StartCoroutine(MoveSmoothly(move));
                    MapGenerator.instance.ResetTotalMap();
                }
            }
        }

    }

    public List<Vector2Int> PathFinding()
    {
        OpenList.Add(StartNode);

        List<Vector2Int> path = new List<Vector2Int>();

        // 기존에 찾은 path 삭제
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

            // 모든 길 찾음
            if (CurrentNode == EndNode)
            {
                Tile currentNode = EndNode;

                while (currentNode != null)
                {
                    path.Add(new Vector2Int(currentNode.coord.x, currentNode.coord.y));
                    currentNode = currentNode.coord.parentNode;
                }

                path.Reverse();

                //foreach (var pos in path) // 좌표 측정
                //{
                //    Debug.Log("X축:" + pos.x + "Y축:" + pos.y);
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



    // 클릭한 좌표 반환
    private Vector2Int ReturnTargetPosition(Coord destination)
    {
        Vector2Int clickedCoord = new Vector2Int(destination.x, destination.y);
        MapGenerator.instance.ClearHighlightedTiles();
        return clickedCoord;
    }


    public IEnumerator MoveSmoothly(List<Vector2Int> path)
    {
        isMoving = true;
        transform.gameObject.GetComponent<Collider>().enabled = false;
        player.playerState = PlayerState.Moving;

        float moveSpeed = 1f;
        float lerpMaxTime = 0.2f;

        for (int i = 0; i < path.Count - 1; i++)
        {
            Vector3 playerPos = new Vector3(path[i].x, transform.position.y, path[i].y); // X와 Y 좌표를 Mathf.Round를 사용하여 가장 가까운 정수로 반올림
            Vector3 nextPosition = new Vector3(path[i + 1].x, transform.position.y, path[i + 1].y); // 도착지점 좌표도 정수로 반올림

            float startTime = Time.time;

            while (Time.time < startTime + lerpMaxTime)
            {
                float currentTime = (Time.time - startTime) * moveSpeed;
                float weight = currentTime / lerpMaxTime;

                transform.position = Vector3.Lerp(playerPos, nextPosition, weight);
                transform.LookAt(nextPosition);
                yield return null;
            }

            transform.position = nextPosition;

            playerData.activePoint--;

            // 모든 타일을 이동했는지 확인
            if (0 >= playerData.activePoint)
                break;
        }

        isMoving = false;
        transform.gameObject.GetComponent<Collider>().enabled = true;
        player.playerState = PlayerState.Idle;

        // Path의 최종 좌표
        Vector2Int finalPosition = new Vector2Int((int)transform.position.x, (int)transform.position.z);

        // Monster 감지 실행
        GetSurroundingTiles(finalPosition);

        yield break;
    }




    public void OpenListAdd(int checkX, int checkY)
    {
        if (checkX < 0 || checkX >= MapGenerator.instance.totalMap.GetLength(0) || checkY < 0 || checkY >= MapGenerator.instance.totalMap.GetLength(1))
            return;

        if (CloseList.Contains(MapGenerator.instance.totalMap[checkX, checkY]))
            return;

        if (MapGenerator.instance.totalMap[checkX, checkY].coord.isWall)
            return;

        if (OpenList.Contains(MapGenerator.instance.totalMap[checkX, checkY]))
        {
            int newG = CurrentNode.coord.G + (Mathf.Abs(CurrentNode.coord.x - checkX) == 0 || Mathf.Abs(CurrentNode.coord.y - checkY) == 0 ? 10 : 14);
            if (newG < MapGenerator.instance.totalMap[checkX, checkY].coord.G)
            {
                MapGenerator.instance.totalMap[checkX, checkY].coord.G = newG;
                MapGenerator.instance.totalMap[checkX, checkY].coord.parentNode = CurrentNode;
            }
        }
        else
        {
            MapGenerator.instance.totalMap[checkX, checkY].coord.G = CurrentNode.coord.G + (Mathf.Abs(CurrentNode.coord.x - checkX) == 0 || Mathf.Abs(CurrentNode.coord.y - checkY) == 0 ? 10 : 14);
            MapGenerator.instance.totalMap[checkX, checkY].coord.H = (Mathf.Abs(checkX - EndNode.coord.x) + Mathf.Abs(checkY - EndNode.coord.y)) * 10;
            MapGenerator.instance.totalMap[checkX, checkY].coord.parentNode = CurrentNode;
            OpenList.Add(MapGenerator.instance.totalMap[checkX, checkY]);
        }
    }

    private void OnMouseDown()
    {
        // 플레이어가 이동 중이 아니라면 클릭 이벤트를 처리 (수정 항목)
        if (!isMoving)
        {
            // 플레이어를 클릭했을 때 이동 가능한 범위 표시
            MapGenerator.instance.HighlightPlayerRange(transform.position, playerData.activePoint);
        }
    }

    // Monster 감지(3x3 타일)
    public void GetSurroundingTiles(Vector2Int playerPos)
    {
        // 몬스터 좌표
        Vector3 monster = FindObjectOfType<Monster>().transform.position;
        monsterPos = new Vector2Int((int)monster.x, (int)monster.z);

        int distacneX = Mathf.Abs(playerPos.x - monsterPos.x);
        int distacneY = Mathf.Abs(playerPos.y - monsterPos.y);

        if (distacneX <= detectionRange && distacneY <= detectionRange)
        {
            // 몬스터 감지 O
//            player.ReadyToAttack();
            //Debug.Log("몬스터 좌표 : " + monsterPos);
            return;
        }
        else
        {
            //// 몬스터 감지 X
            //player.Init();
            //return;
        }

        // TurnManager 턴 바꾸기(Monster Turn -> Player Turn)

    }
}