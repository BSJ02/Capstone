using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public MapGenerator mapGenerator;
    public Player player;

    Vector2Int playerPos;
    Vector2Int targetPos;

    Tile StartNode, EndNode, CurrentNode;
    List<Tile> OpenList = new List<Tile>();
    List<Tile> CloseList = new List<Tile>();

    private bool isMoving = false; // 이동 중인지 여부

    public void SetDestination(Vector2Int clickedTargetPos)
    {
        playerPos = new Vector2Int((int)transform.position.x, (int)transform.position.z);
        targetPos = new Vector2Int(clickedTargetPos.x, clickedTargetPos.y);

        StartNode = mapGenerator.totalMap[playerPos.x, playerPos.y];
        EndNode = mapGenerator.totalMap[targetPos.x, targetPos.y];
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isMoving)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.CompareTag("Tile") && mapGenerator.IsHighlightedTile(hit.collider.GetComponent<Tile>()))
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
                    mapGenerator.ResetTotalMap();
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

                foreach (var pos in path) // 좌표 측정
                {
                    Debug.Log("X축:" + pos.x + "Y축:" + pos.y);
                }

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
        mapGenerator.ClearHighlightedTiles();
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

            player.activePoint--;

            // 모든 타일을 이동했는지 확인
            if (0 >= player.activePoint)
                break;
        }

        isMoving = false;
        transform.gameObject.GetComponent<Collider>().enabled = true;
        player.playerState = PlayerState.Idle;

        yield break;
    }




    public void OpenListAdd(int checkX, int checkY)
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

    private void OnMouseDown()
    {
        // 플레이어가 이동 중이 아니라면 클릭 이벤트를 처리 (수정 항목)
        if (!isMoving)
        {
            // 플레이어를 클릭했을 때 이동 가능한 범위 표시
            mapGenerator.HighlightPlayerRange(transform.position, player.activePoint);
        }
    }

    // 몬스터 감지(3x3 타일)
    public void GetSurroundingTiles(Vector2Int playerPos)
    {
        // 플레이어 기준 좌표 생성
        List<Tile> surroundingTiles = new List<Tile>();

        for (int xOffset = -1; xOffset <= 1; xOffset++)
        {
            for (int yOffset = -1; yOffset <= 1; yOffset++)
            {
                int x = playerPos.x + xOffset;
                int y = playerPos.y + yOffset;

                if (x >= 0 && x < mapGenerator.totalMap.GetLength(0) && y >= 0 && y < mapGenerator.totalMap.GetLength(1))
                {
                    surroundingTiles.Add(mapGenerator.totalMap[x, y]);
                }
            }
        }

        // Monster 확인
        foreach (Tile tile in surroundingTiles)
        {
            RaycastHit hit;
            if (Physics.Raycast(new Vector3(tile.coord.x, 0, tile.coord.y), Vector3.up, out hit, 2f))
            {
                if (hit.collider.gameObject.CompareTag("Monster"))
                {
                    // 몬스터가 있으면
                    //playerState.ReadyToAttack();
                    return;
                }
                // TurnManager 턴 바꾸기(Player Turn -> Monster Turn)
            }


        }
    }
}