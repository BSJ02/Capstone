using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterMove : MonoBehaviour
{
    private Monster monster;


    Tile StartNode, TargetNode, CurrentNode;
    List<Tile> OpenList = new List<Tile>();
    List<Tile> CloseList = new List<Tile>();

    Vector2Int monsterPos;
    Vector2Int playerPos;

    private void Awake()
    {
        monster = GetComponent<Monster>();
    }


    public void MoveStart()
    {
        // 리스트 초기화 
        OpenList.Clear();
        CloseList.Clear();

        // 길찾기 
        SetDestination();
        List<Vector2Int> move = PathFinding();
        StartCoroutine(MoveSmoothly(move));

        // 타일 좌표 초기화
        MapGenerator.instance.ResetTotalMap();

    }


    public void SetDestination() 
    {
        monsterPos = new Vector2Int((int)transform.position.x, (int)transform.position.z);
      
        Vector3 player = FindObjectOfType<Player>().transform.position;
        playerPos = new Vector2Int((int)player.x, (int)player.z);

        MapGenerator.instance.totalMap[monsterPos.x, monsterPos.y].SetCoord(monsterPos.x, monsterPos.y, false); 

        StartNode = MapGenerator.instance.totalMap[monsterPos.x, monsterPos.y];
        TargetNode = MapGenerator.instance.totalMap[playerPos.x, playerPos.y];
    }

    public List<Vector2Int> PathFinding() 
    {
        OpenList.Add(StartNode);
        
        List<Vector2Int> path = new List<Vector2Int>();

        // 리스트 중복 방지
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

            // 모든 길을 다 찾음
            if (CurrentNode == TargetNode)
            {
                Tile currentNode = TargetNode;

                while (currentNode != null)
                {
                    path.Add(new Vector2Int(currentNode.coord.x, currentNode.coord.y));
                    currentNode = currentNode.coord.parentNode;
                }

                path.Reverse();

                break;
            }

            OpenListAdd(CurrentNode.coord.x, CurrentNode.coord.y + 1);
            OpenListAdd(CurrentNode.coord.x + 1, CurrentNode.coord.y);
            OpenListAdd(CurrentNode.coord.x, CurrentNode.coord.y - 1);
            OpenListAdd(CurrentNode.coord.x - 1, CurrentNode.coord.y);
        }

        // 플레이어 좌표 삭제(겹침 방지)
        if(path.Count > 0)
        path.RemoveAt(path.Count - 1); 

        return path;
    }


    // 현재 노드 체크
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
            MapGenerator.instance.totalMap[checkX, checkY].coord.H = (Mathf.Abs(checkX - TargetNode.coord.x) + Mathf.Abs(checkY - TargetNode.coord.y)) * 10;
            MapGenerator.instance.totalMap[checkX, checkY].coord.parentNode = CurrentNode;
            OpenList.Add(MapGenerator.instance.totalMap[checkX, checkY]);
        }
    }


    // 몬스터 이동
    public IEnumerator MoveSmoothly(List<Vector2Int> path) 
    {
        monster.state = MonsterState.Moving;
        monster.gameObject.GetComponent<Animator>().SetInteger("State", (int)monster.state);

        // 몬스터 이동거리
        int maxMoveDistance = monster.monsterData.MoveDistance;

        float moveSpeed = 1f;
        float lerpMaxTime = 0.2f; 

        for (int i = 0; i < path.Count - 1; i++)
        {
            // 몬스터 이동거리에 맞게 설정
            if (i >= maxMoveDistance)
                break;

            Vector3 monsterPosition = new Vector3(path[i].x, transform.position.y, path[i].y); 
            Vector3 nextPosition = new Vector3(path[i + 1].x, transform.position.y, path[i + 1].y);

            float startTime = Time.time;

            while (Time.time < startTime + lerpMaxTime)
            {
                float currentTime = (Time.time - startTime) * moveSpeed;
                float weight = currentTime / lerpMaxTime;

                transform.position = Vector3.Lerp(monsterPosition, nextPosition, weight);
                transform.LookAt(nextPosition);
                yield return null;
            }

            // 몬스터 위치 값 보정 
            transform.position = nextPosition;

        }

        // 몬스터 최종 위치
        Vector2Int finalPosition = new Vector2Int((int)transform.position.x, (int)transform.position.z);

        // 최종 위치에 isWall 체크(겹침 방지)
        MapGenerator.instance.totalMap[finalPosition.x, finalPosition.y].SetCoord(finalPosition.x, finalPosition.y, true); 

        // 최종 위치 기준으로 플레이어 감지
        GetSurroundingTiles(finalPosition);

        // 몬스터 턴 종료
        StartCoroutine(EscapeMonsterTurn());
        yield break;
    }

    // 몬스터 주변 타일 감지
    public void GetSurroundingTiles(Vector2Int monsterPos)
    {
        int detectionRange = monster.monsterData.DetectionRagne;

        int distacneX = Mathf.Abs(monsterPos.x - playerPos.x);
        int distacneY = Mathf.Abs(monsterPos.y - playerPos.y);

        if(distacneX <= detectionRange && distacneY <= detectionRange)
        {
            // 감지 O
            Player player = FindObjectOfType<Player>();
            monster.ReadyToAttack(player);
            return;
        }
        else
        {
            // 감지 X
            monster.Init();
            return;
        }
    }

    // Player 턴 넘김
    IEnumerator EscapeMonsterTurn()
    {
        yield return new WaitForSeconds(2f);
        BattleManager.instance.ui[1].gameObject.SetActive(false);
        BattleManager.instance.PlayerTurn();
    }
}

