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

    bool isMoving;

    private void Awake()
    {
        monster = GetComponent<Monster>();
        isMoving = false;
    }

    public IEnumerator StartDetection()
    {
        // 초기화
        OpenList.Clear();
        CloseList.Clear();

        // 좌표 설정 및 감지
        SetDestination();
        MapGenerator.instance.totalMap[monsterPos.x, monsterPos.y].SetCoord(monsterPos.x, monsterPos.y, true);
        GetSurroundingTiles(monsterPos);

        // 현재 위치를 isWall로 유지
        MapGenerator.instance.ResetTotalMap();

        // 각 몬스터 행동 후 추가 딜레이(필요한 경우 대기 시간 추가)
        yield return new WaitForSeconds(1f);
    }

    // 몬스터 움직임
    public void Moving()
    {
        // 초기화
        OpenList.Clear();
        CloseList.Clear();

        // 길찾기 시작
        SetDestination();
        List<Vector2Int> move = PathFinding();
        StartCoroutine(MoveSmoothly(move));

        // 타일 좌표 초기화 
        MapGenerator.instance.ResetTotalMap();
    }

    // 플레이어 및 몬스터 초기 좌표 설정
    public void SetDestination() 
    {
        monsterPos = new Vector2Int((int)transform.position.x, (int)transform.position.z);
      
        Vector3 player = FindObjectOfType<Player>().transform.position;
        playerPos = new Vector2Int((int)player.x, (int)player.z);

        MapGenerator.instance.totalMap[monsterPos.x, monsterPos.y].SetCoord(monsterPos.x, monsterPos.y, false);

        StartNode = MapGenerator.instance.totalMap[monsterPos.x, monsterPos.y];
        TargetNode = MapGenerator.instance.totalMap[playerPos.x, playerPos.y];
    }

    // 길찾기 시작
    public List<Vector2Int> PathFinding() 
    {
        OpenList.Add(StartNode); 
        
        List<Vector2Int> path = new List<Vector2Int>(); // 경로가 담길 리스트

        path.Clear();

        while (OpenList.Count > 0)
        {
            CurrentNode = OpenList[0];

            // 연산 비용 계산
            for (int i = 1; i < OpenList.Count; i++)
            {
                if (OpenList[i].coord.F <= CurrentNode.coord.F && OpenList[i].coord.H < CurrentNode.coord.H)
                {
                    CurrentNode = OpenList[i];
                }
            }

            OpenList.Remove(CurrentNode);
            CloseList.Add(CurrentNode);

            // 좌표 전부 찾음
            if (CurrentNode == TargetNode)
            {
                Tile currentNode = TargetNode;

                while (currentNode != null)
                {
                    path.Add(new Vector2Int(currentNode.coord.x, currentNode.coord.y));
                    currentNode = currentNode.coord.parentNode;
                }

                path.Reverse(); // 플레이어 기준 좌표 -> 몬스터 기준 좌표[순서 정리]

                break;
            }

            // 위, 오른쪽, 아래, 왼쪽 순서대로 OpenList 찾기
            OpenListAdd(CurrentNode.coord.x, CurrentNode.coord.y + 1);
            OpenListAdd(CurrentNode.coord.x + 1, CurrentNode.coord.y);
            OpenListAdd(CurrentNode.coord.x, CurrentNode.coord.y - 1);
            OpenListAdd(CurrentNode.coord.x - 1, CurrentNode.coord.y);
        }

        // 플레이어 겹침 방지
        if (path.Count > 0)
        path.RemoveAt(path.Count - 1); 

        // 경로 반환 
        return path;
    }

    // 노드 연산 비용 계산 
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

    // 몬스터 물리적 움직임
    public IEnumerator MoveSmoothly(List<Vector2Int> path) 
    {
        isMoving = true;

        monster.state = MonsterState.Moving;
        monster.gameObject.GetComponent<Animator>().SetInteger("State", (int)monster.state);

        // 몬스터 최대 이동 거리(moveDistance 만큼 리스트 반환)
        int maxMoveDistance = monster.monsterData.MoveDistance;

        float moveSpeed = 1f;
        float lerpMaxTime = 0.2f; 

        for (int i = 0; i < path.Count - 1; i++)
        {
            // 몬스터의 moveDistacne 값이 최대이면 종료
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

            // Position 값 보정
            transform.position = nextPosition;
        }

        // 최종 플레이어 좌표
        Vector2Int finalPosition = new Vector2Int((int)transform.position.x, (int)transform.position.z);

        // 최종 좌표 isWall 설정(몬스터 및 플레이어 겹침 방지)
        MapGenerator.instance.totalMap[finalPosition.x, finalPosition.y].SetCoord(finalPosition.x, finalPosition.y, true);

        // RandomDamage 선택
        monster.ReadyToAttack();

        // 플레이어 감지 후 공격
        GetSurroundingTiles(finalPosition);

        yield break;
    }

    // 플레이어 감지 및 공격(대각선 공격 X)
    public void GetSurroundingTiles(Vector2Int monsterPos)
    {
        // 스킬 공격(벽 뚫 O)
        int attackDetectionRange = monster.monsterData.DetectionRagne;
        int skillDetectionRange = monster.monsterData.SkillDetectionRange;

        int distanceX = Mathf.Abs(monsterPos.x - playerPos.x);
        int distanceY = Mathf.Abs(monsterPos.y - playerPos.y);


        int isWallTileX = monsterPos.x + attackDetectionRange;
        int isWallTileY = monsterPos.y = attackDetectionRange;

        // [1] 스킬 공격 범위 내에 플레이어가 존재 할 경우
        if ((monster.monsterData.CurrentDamage >= monster.monsterData.Critical) &&
            (distanceX <= skillDetectionRange && monsterPos.y == playerPos.y) ||
            (distanceY <= skillDetectionRange && monsterPos.x == playerPos.x))
        {
            List<Tile> checkisWallTiles = new List<Tile>();

            for (int i = monsterPos.x; i < isWallTileX; i++)
            {
                for (int j = monsterPos.y; j < isWallTileY; j++)
                {
                    checkisWallTiles.Add(MapGenerator.instance.totalMap[i, j]);
                }
            }

            foreach (var checkTile in checkisWallTiles)
            {
                Debug.Log("감지한 타일 이름:" + checkTile.name);
                Debug.Log("감지한 타일 개수:" + checkisWallTiles.Count);
            }


            isMoving = true;
            monster.attack = AttackState.SkillAttack;

            Player player = FindObjectOfType<Player>();
            transform.LookAt(player.transform); // 회전 값 보정
            monster.Attack(player);

            isMoving = false;
            return;
        }
        // [2] 일반 공격 범위 내에 플레이어가 존재 할 경우
        else if ((monster.monsterData.CurrentDamage <= monster.monsterData.Critical) &&
            (distanceX <= attackDetectionRange && monsterPos.y == playerPos.y) ||
            (distanceY <= attackDetectionRange && monsterPos.x == playerPos.x))
        {
            isMoving = true;
            monster.attack = AttackState.GeneralAttack;

            Player player = FindObjectOfType<Player>();
            transform.LookAt(player.transform); // 회전 값 보정
            monster.Attack(player); // 데미지 연산

            isMoving = false;
            return;
        }
        else // [3] 범위 내에 없을 경우(처음 시작 및 움직인 후)
        {
            if (isMoving == false) // 범위 내에 없고 && 움직이지 않았을 경우
            {
                Moving();
            }
            else if (isMoving == true) // 범위 내에 없고 && 움직였을 경우
            {
                monster.Init();
                isMoving = false;
                return;
            }
        }
    }
}

   

