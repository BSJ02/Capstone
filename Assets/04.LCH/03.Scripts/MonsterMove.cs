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
    [HideInInspector]
    public Vector2Int playerPos;

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

        if (isMoving == false)
        {
            Moving();
        }

        // 현재 위치를 isWall로 유지
        MapGenerator.instance.ResetTotalMap();


        // 각 몬스터 행동 후 추가 딜레이(필요한 경우 대기 시간 추가)
        yield return new WaitForSeconds(1f);
    }

    // 몬스터 움직임 전 색상 초기화
    public void Moving()
    {
        // 초기화
        OpenList.Clear();
        CloseList.Clear();

        // 길찾기 시작
        SetDestination();
        List<Vector2Int> move = PathFinding();
        StartCoroutine(MoveSmoothly(move));
    }

    public void SetDestination()
    {
        int playerLength = BattleManager.instance.players.Count;
        float[] currentPlayerHp = new float[playerLength];
        Vector2Int[] playersPosition = new Vector2Int[playerLength];
        GameObject[] playersObj = GameObject.FindGameObjectsWithTag("Player");

        monsterPos = new Vector2Int((int)transform.position.x, (int)transform.position.z);

        // 유효한 플레이어 오브젝트를 찾고 있는지 검증
        if (playersObj.Length != playerLength)
        {
            return;
        }

        for (int i = 0; i < playerLength; i++)
        {
            Vector3 player = playersObj[i].transform.position;
            playersPosition[i] = new Vector2Int((int)player.x, (int)player.z);
        }

        // 몬스터와 플레이어의 위치가 맵의 범위를 벗어나지 않는지 검증
        int mapWidth = MapGenerator.instance.totalMap.GetLength(0);
        int mapHeight = MapGenerator.instance.totalMap.GetLength(1);

        if (monsterPos.x < 0 || monsterPos.x >= mapWidth || monsterPos.y < 0 || monsterPos.y >= mapHeight)
        {
            return;
        }

        // 플레이어 위치 검증
        foreach (var pos in playersPosition)
        {
            if (pos.x < 0 || pos.x >= mapWidth || pos.y < 0 || pos.y >= mapHeight)
            {
                return;
            }
        }

        int attackDetectionRange = monster.monsterData.DetectionRagne;
        int skillDetectionRange = monster.monsterData.SkillDetectionRange;

        List<Vector2Int> skillTiles = AttackRangeChecking(monsterPos, skillDetectionRange, true);
        List<Vector2Int> attackTiles = AttackRangeChecking(monsterPos, attackDetectionRange, false);

        bool playerDetected = false;

        // 스킬 범위 내에 플레이어가 있는지 확인
        foreach (Vector2Int tile in skillTiles)
        {
            foreach (GameObject playerObj in playersObj)
            {
                Player playerComponent = playerObj.GetComponent<Player>();
                Vector2Int playerPos = new Vector2Int((int)playerObj.transform.position.x, (int)playerObj.transform.position.z);
                if (tile == playerPos)
                {
                    playerDetected = true;
                    this.playerPos = playerPos;
                    break;
                }
            }
            if (playerDetected)
            {
                break;
            }
        }

        // 스킬 범위 내에 플레이어가 없을 경우 공격 범위 내에 플레이어가 있는지 확인
        if (!playerDetected)
        {
            foreach (Vector2Int tile in attackTiles)
            {
                foreach (GameObject playerObj in playersObj)
                {
                    Player playerComponent = playerObj.GetComponent<Player>();
                    Vector2Int playerPos = new Vector2Int((int)playerObj.transform.position.x, (int)playerObj.transform.position.z);
                    if (tile == playerPos)
                    {
                        playerDetected = true;
                        this.playerPos = playerPos;
                        break;
                    }
                }
                if (playerDetected)
                {
                    break;
                }
            }
        }

        // 플레이어를 찾지 못한 경우, 체력이 낮은 적을 추적
        if (!playerDetected)
        {
            for (int i = 0; i < playerLength; i++)
            {
                float hp = playersObj[i].GetComponent<Player>().playerData.Hp;
                currentPlayerHp[i] = hp;
            }

            if (currentPlayerHp[0] > currentPlayerHp[1])
            {
                playerPos = playersPosition[1];
            }
            else
            {
                playerPos = playersPosition[0];
            }
        }

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

        // 몬스터 최대 이동 거리(moveDistance 만큼 리스트 반환)
        int maxMoveDistance = monster.monsterData.MoveDistance;

        float moveSpeed = 1f;
        float lerpMaxTime = 0.2f;

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

        monster.Init();

        yield break;
    }

    // 플레이어 감지 및 공격 (대각선 공격 제외)
    public bool GetSurroundingTiles(Vector2Int monsterPos)
    {
        int attackDetectionRange = monster.monsterData.DetectionRagne;
        int skillDetectionRange = monster.monsterData.SkillDetectionRange;

        List<Vector2Int> skillTiles = AttackRangeChecking(monsterPos, skillDetectionRange, true);
        List<Vector2Int> attackTiles = AttackRangeChecking(monsterPos, attackDetectionRange, false);

        // 스킬 공격 범위 내에 플레이어가 존재할 경우
        foreach (Vector2Int tile in skillTiles)
        {
            foreach (GameObject playerObj in GameObject.FindGameObjectsWithTag("Player"))
            {
                Player playerComponent = playerObj.GetComponent<Player>();
                Vector2Int playerPos = new Vector2Int((int)playerObj.transform.position.x, (int)playerObj.transform.position.z);
                if (tile == playerPos && monster.monsterData.CurrentDamage >= monster.monsterData.Critical)
                {
                    isMoving = true;

                    // 공격 로직 실행 
                    monster.attack = AttackState.SkillAttack;
                    monster.ReadyToAttack();
                    monster.Attack(playerComponent);

                    // 회전값 보정 
                    transform.LookAt(playerObj.transform);
                    return true;
                }
            }
        }

        // 일반 공격 범위 내에 플레이어가 존재할 경우
        foreach (Vector2Int tile in attackTiles)
        {
            foreach (GameObject playerObj in GameObject.FindGameObjectsWithTag("Player"))
            {
                Player playerComponent = playerObj.GetComponent<Player>();
                Vector2Int playerPos = new Vector2Int((int)playerObj.transform.position.x, (int)playerObj.transform.position.z);
                if (tile == playerPos)
                {
                    isMoving = true;

                    // 공격 로직 실행 
                    monster.attack = AttackState.GeneralAttack;
                    monster.ReadyToAttack();
                    monster.Attack(playerComponent);

                    // 회전값 보정
                    transform.LookAt(playerObj.transform);
                    return true;
                }
            }
        }

        isMoving = false;
        return false;
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
