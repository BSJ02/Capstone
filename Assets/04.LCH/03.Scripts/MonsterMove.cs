using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterMove : MonoBehaviour
{
    private MapGenerator mapGenerator;

    private Monster monster;

    public int detectionRange = 1;

    Tile StartNode, TargetNode, CurrentNode;
    List<Tile> OpenList = new List<Tile>();
    List<Tile> CloseList = new List<Tile>();

    Vector2Int monsterPos;
    Vector2Int playerPos;

    private void Awake()
    {
        mapGenerator = FindObjectOfType<MapGenerator>();
        monster = GetComponent<Monster>();
    }


    public void ButtonClick()
    {
        // 버튼 클릭마다 list 초기화 
        OpenList.Clear();
        CloseList.Clear();

        // 길찾기 
        SetDestination();
        List<Vector2Int> move = PathFinding();
        StartCoroutine(MoveSmoothly(move));

        // 타일 정보 초기화
        mapGenerator.ResetTotalMap();

    }


    public void SetDestination() // StartNode, TargetNode 초기화
    {
        monsterPos = new Vector2Int((int)transform.position.x, (int)transform.position.z); // 현재 몬스터의 Vector3 X,Z 값
        playerPos = new Vector2Int((int)FindObjectOfType<Player>().transform.position.x,
            (int)FindObjectOfType<Player>().transform.position.z);  // Player 컴포넌트를 가져옴

        StartNode = mapGenerator.totalMap[monsterPos.x, monsterPos.y];
        TargetNode = mapGenerator.totalMap[playerPos.x, playerPos.y];
    }

    public List<Vector2Int> PathFinding() // 길찾기
    {
        OpenList.Add(StartNode);
        
        List<Vector2Int> path = new List<Vector2Int>();

        // 기존에 찾은 path 삭제(중복 방지) 
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
            if (CurrentNode == TargetNode)
            {
                Tile currentNode = TargetNode;

                while (currentNode != null)
                {
                    path.Add(new Vector2Int(currentNode.coord.x, currentNode.coord.y));
                    currentNode = currentNode.coord.parentNode;
                }

                path.Reverse();

                /*foreach (var pos in path)
                {
                    Debug.Log("x:" + pos.x + "y:" + pos.y);
                }*/

                break;
            }

            OpenListAdd(CurrentNode.coord.x, CurrentNode.coord.y + 1);
            OpenListAdd(CurrentNode.coord.x + 1, CurrentNode.coord.y);
            OpenListAdd(CurrentNode.coord.x, CurrentNode.coord.y - 1);
            OpenListAdd(CurrentNode.coord.x - 1, CurrentNode.coord.y);
        }

        if(path.Count > 0)
        path.RemoveAt(path.Count - 1); // Player 좌표 제거, 겹침 방지

        return path;
    }

    public void OpenListAdd(int checkX, int checkY) // CurrentNode 체크
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
            mapGenerator.totalMap[checkX, checkY].coord.H = (Mathf.Abs(checkX - TargetNode.coord.x) + Mathf.Abs(checkY - TargetNode.coord.y)) * 10;
            mapGenerator.totalMap[checkX, checkY].coord.parentNode = CurrentNode;
            OpenList.Add(mapGenerator.totalMap[checkX, checkY]);
        }
    }

    public IEnumerator MoveSmoothly(List<Vector2Int> path) // 물리 이동 
    {
        monster.state = MonsterState.Moving;
        monster.gameObject.GetComponent<Animator>().SetInteger("State", (int)monster.state);

        // 최대 이동 거리
        int maxMoveDistance = monster.monsterData.MoveDistance;
        Debug.Log("이동거리 :" + maxMoveDistance + "칸");

        float moveSpeed = 1f;
        float lerpMaxTime = 0.2f; // 몬스터 이동속도 설정(작을수록 빨라짐)

        for (int i = 0; i < path.Count - 1; i++)
        {
            if (i >= maxMoveDistance)
                break;

            Vector3 monsterPosition = new Vector3(path[i].x, transform.position.y, path[i].y); // 현재 몬스터 좌표
            Vector3 nextPosition = new Vector3(path[i + 1].x, transform.position.y, path[i + 1].y); // 다음 몬스터 좌표 

            float startTime = Time.time;

            while (Time.time < startTime + lerpMaxTime)
            {
                float currentTime = (Time.time - startTime) * moveSpeed;
                float weight = currentTime / lerpMaxTime;

                // 몬스터 이동
                transform.position = Vector3.Lerp(monsterPosition, nextPosition, weight);
                // 몬스터 이동 좌표 방향 회전 
                transform.LookAt(nextPosition);
                yield return null;
            }

            // 목표값 및 회전값 보정
            transform.position = nextPosition;
            // 회전값 보정 코드 필요

        }

        // Path의 최종 좌표
        Vector2Int finalPosition = new Vector2Int((int)transform.position.x, (int)transform.position.z);

        // Player 감지
        GetSurroundingTiles(finalPosition);

        // 코루틴 종료 & 이동 종료
        yield break;
    }

    // Player 감지(3x3 타일)
    public void GetSurroundingTiles(Vector2Int monsterPos)
    {
        int distacneX = Mathf.Abs(monsterPos.x - playerPos.x);
        int distacneY = Mathf.Abs(monsterPos.y - playerPos.y);

        if(distacneX <= detectionRange && distacneY <= detectionRange)
        {
            // 플레이어 감지 O
            monster.ReadyToAttack();
            return;
        }
        else
        {
            // 플레이어 감지 X
            monster.Init();
            return;
        }

        // TurnManager 턴 바꾸기(Monster Turn -> Player Turn)

    }
}

