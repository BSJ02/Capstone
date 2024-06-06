using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MonsterMove : MonoBehaviour
{
    public Monster monster { get; private set; }

    Tile StartNode, TargetNode, CurrentNode;
    List<Tile> OpenList = new List<Tile>();
    List<Tile> CloseList = new List<Tile>();

    Vector2Int monsterPos;
    public Vector2Int playerPos;

    Player player;
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

        // 초기 감지 시작
        (isMoving, player) = IsPlayerInRange();

        // 1. 감지 O
        if(isMoving == true)
        {
            // 플레이어 및 몬스터 위치 세팅
            SetDestination();

            // 공격 설정
            SetAttackState(isMoving, player);

            // 현재 위치를 isWall로 유지
            MapGenerator.instance.totalMap[monsterPos.x, monsterPos.y].SetCoord(monsterPos.x, monsterPos.y, true);
            MapGenerator.instance.ResetTotalMap();

            yield break;
        }
        // 2. 감지 X 
        else if(isMoving == false)
        {
            // 몬스터 플레이어 방향으로 움직이기
            Moving();

            // 현재 위치의 타일을 바뀐 isWall로 유지
            MapGenerator.instance.ResetTotalMap();

            yield break;
        }
    }

    // 몬스터 움직임
    public void Moving()
    {
        // 가장 가까운 플레이어 찾기
        FindClosestPlayer(monsterPos);

        // 플레이어 및 몬스터 위치 세팅
        SetDestination();

        // 현재 위치 isWall 해제
        MapGenerator.instance.totalMap[monsterPos.x, monsterPos.y].SetCoord(monsterPos.x, monsterPos.y, false);

        // 최소비용 노드 찾기
        List<Vector2Int> move = PathFinding();

        // 최단 거리로 이동
        StartCoroutine(MoveSmoothly(move));
        
    }


    // 스킬 범위 및 공격 범위 감지
    public (bool, Player) IsPlayerInRange()
    {
        // 기본 값 세팅
        GameObject[] playersObj = GameObject.FindGameObjectsWithTag("Player");
        monsterPos = new Vector2Int((int)transform.position.x, (int)transform.position.z);

        int attackDetectionRange = monster.monsterData.DetectionRagne;
        int skillDetectionRange = monster.monsterData.SkillDetectionRange;

        Player detectedPlayer = null;

        // 현재 타일에서 스킬 범위 확인
        List<Vector2Int> skillTiles = AttackRangeChecking(monsterPos, skillDetectionRange, true);
        foreach (Vector2Int tile in skillTiles)
        {
            foreach (GameObject playerObj in playersObj)
            {
                player = playerObj.GetComponent<Player>();
                Vector2Int playerPos = new Vector2Int((int)playerObj.transform.position.x, (int)playerObj.transform.position.z);

                if (tile == playerPos)
                {
                    this.playerPos = playerPos;
                    monster.attack = AttackState.SkillAttack;

                    Debug.Log("스킬 공격 범위 감지!");
                    detectedPlayer = player;
                    return (true, detectedPlayer);
                }
            }
        }

        // 현재 타일에서 일반 공격 범위 확인
        List<Vector2Int> attackTiles = AttackRangeChecking(monsterPos, attackDetectionRange, false);
        foreach (Vector2Int tile in attackTiles)
        {
            foreach (GameObject playerObj in playersObj)
            {
                player = playerObj.GetComponent<Player>();
                Vector2Int playerPos = new Vector2Int((int)playerObj.transform.position.x, (int)playerObj.transform.position.z);

                if (tile == playerPos)
                {
                    this.playerPos = playerPos;
                    monster.attack = AttackState.GeneralAttack;

                    Debug.Log("일반 공격 범위 감지!");
                    detectedPlayer = player;
                    return (true, detectedPlayer);
                }
            }
        }

        // 범위 내에 없음 
        Debug.Log("감지 실패!");
        return (false, detectedPlayer);
    }

    public Player FindClosestPlayer(Vector2Int monsterPos)
    {
        // 기본 값 세팅
        GameObject[] playersObj = GameObject.FindGameObjectsWithTag("Player");
        Player closestPlayer = null;
        float closestDistance = float.MaxValue;

        foreach (GameObject playerObj in playersObj)
        {
            Player target = playerObj.GetComponent<Player>();
            Vector2Int targetPos = new Vector2Int((int)playerObj.transform.position.x, (int)playerObj.transform.position.z);
            float distance = Vector2Int.Distance(monsterPos, targetPos);

            // 가장 가까운 플레이어인 경우
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestPlayer = target;
                this.playerPos = targetPos;
                player = target; // 수정된 부분: player 변수에 할당
            }
        }

        if (closestPlayer != null)
        {
            Debug.Log("가장 가까운 적은:" + closestPlayer.name);
        }
        else
        {
            Debug.LogError("가장 가까운 적을 찾을 수 없습니다.");
        }

        return closestPlayer;
    }


    // Ghost Code 
    /*// 플레이어 체력 비교 
    public Player PlayerHealthPriority()
    {
        GameObject[] playersObj = GameObject.FindGameObjectsWithTag("Player");
        int playerLength = playersObj.Length;

        // 플레이어가 모든 공격 범위 내에 존재 X, 체력이 낮은 적 감지
        Player player1 = playersObj.Length >= 1 ? playersObj[0].GetComponent<Player>() : null; // 첫 번째 플레이어
        Player player2 = playersObj.Length >= 2 ? playersObj[1].GetComponent<Player>() : null; // 두 번째 플레이어

        Player target = null;

        if (player1 != null && player2 != null)
        {
            target = player1.playerData.Hp > player2.playerData.Hp ? player2 : player1;
        }

        monster.attack = AttackState.None;
        return target;
    }*/


    // 플레이어 및 몬스터 좌표 설정
    public void SetDestination()
    {
        // 몬스터 포지션 설정
        monsterPos = new Vector2Int((int)transform.position.x, (int)transform.position.z);
        playerPos = new Vector2Int(playerPos.x, playerPos.y);

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

    // 몬스터 물리적 움직임 계산
    public IEnumerator MoveSmoothly(List<Vector2Int> path)
    {
        // 몬스터 이동 거리
        int maxMoveDistance = monster.monsterData.MoveDistance;

        float moveSpeed = 1f;
        float lerpMaxTime = 0.2f;
        
        // 애니메이션 및 상태 변경
        monster.state = MonsterState.Moving;
        monster.GetComponent<Animator>().SetInteger("State", (int)monster.state);

        for (int i = 0; i < path.Count - 1; i++)
        {
            // 몬스터의 moveDistacne 값이 최대이면 종료
            if (i >= maxMoveDistance)
                break;

            Vector3 monsterPosition = new Vector3(path[i].x, transform.position.y, path[i].y);
            Vector3 nextPosition = new Vector3(path[i + 1].x, transform.position.y, path[i + 1].y);

            float startTime = Time.time;

            // 프레임 계산
            while (Time.time < startTime + lerpMaxTime)
            {
                float currentTime = (Time.time - startTime) * moveSpeed;
                float weight = currentTime / lerpMaxTime;

                transform.position = Vector3.Lerp(monsterPosition, nextPosition, weight);
                transform.LookAt(nextPosition);
                yield return null;
            }

            // 포지션 값 보정
            transform.position = nextPosition;
        }

        // 최종 플레이어 좌표
        Vector2Int finalPosition = new Vector2Int((int)transform.position.x, (int)transform.position.z);

        monster.Init();

        // 최종 좌표 isWall = true 설정
        MapGenerator.instance.totalMap[finalPosition.x, finalPosition.y].SetCoord(finalPosition.x, finalPosition.y, true);

        // 도착한 위치에서 감지
        (isMoving, player) = IsPlayerInRange();

        // 1. 감지O
        if (isMoving == true)
        {
            SetAttackState(isMoving, player);

        }
        // 2. 감지X 
        else if (isMoving == false)
        {
            yield break; 
        }

        yield break;
    }

    // 몬스터 공격 설정 
    public bool SetAttackState(bool isMove, Player player) 
    {
        switch (monster.attack)
        {
            // 일반 공격
            case AttackState.GeneralAttack:
                transform.LookAt(player.transform);

                monster.GetRandomDamage();
                monster.Attack(player);
                break;

            // 스킬 공격
            case AttackState.SkillAttack:
                transform.LookAt(player.transform);

                monster.GetRandomDamage();
                monster.Attack(player);
                break;
        }

        return isMoving;
    }

    // 범위 내의 타일 가져오기 (스킬 및 공격 범위)
    public List<Vector2Int> AttackRangeChecking(Vector2Int center, int range, bool isSkill)
    {
        List<Vector2Int> tilesWithinRange = new List<Vector2Int>();
        int mapWidth = MapGenerator.instance.totalMap.GetLength(0);
        int mapHeight = MapGenerator.instance.totalMap.GetLength(1);

        for (int x = center.x - range; x <= center.x + range; x++)
        {
            for (int y = center.y - range; y <= center.y + range; y++)
            {
                if (x < 0 || x >= mapWidth || y < 0 || y >= mapHeight)
                {
                    continue; // 배열의 범위를 벗어나면 무시
                }

                // 대각선 방향의 타일은 제외하고, 타일이 실제로 범위 내에 있는지 확인
                if ((Mathf.Abs(x - center.x) == 0 || Mathf.Abs(y - center.y) == 0) && Mathf.Abs(x - center.x) <= range && Mathf.Abs(y - center.y) <= range)
                {
                    Vector2Int tileCoord = new Vector2Int(x, y);
                    Tile tile = MapGenerator.instance.totalMap[x, y];

                    // 이미 색상이 설정된 타일은 건너뜀
                    if (tile != null && tile.GetColor() != Color.clear)
                    {
                        continue;
                    }

                    tilesWithinRange.Add(tileCoord);

                }
            }
        }

        return tilesWithinRange;
    }

}