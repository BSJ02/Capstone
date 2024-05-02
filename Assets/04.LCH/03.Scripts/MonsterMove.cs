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


    // 플레이어 턴 종료 후 호출[몬스터 턴]
    public void MoveStart()
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


    /*// 플레이어 감지 및 공격(기존 감지 방식 - 삭제 예정)
    public void GetSurroundingTiles(Vector2Int monsterPos)
    {
        int detectionRange = monster.monsterData.DetectionRagne;

        int distacneX = Mathf.Abs(monsterPos.x - playerPos.x);
        int distacneY = Mathf.Abs(monsterPos.y - playerPos.y);


        switch (monster.monsterType)
        {
            case MonsterType.Short:
                // 근거리 몬스터
                if ((distacneX <= detectionRange && monsterPos.y == playerPos.y) || (distacneY <= detectionRange && monsterPos.x == playerPos.x) && (monster.monsterType == MonsterType.Short))
                {
                    // 감지 O 
                    Player player = FindObjectOfType<Player>();
                    transform.LookAt(player.transform); // 회전 값 보정
                    monster.Attack(player);

                    StartCoroutine(EscapeMonsterTurn());
                    return;
                }
                else
                {
                    // 감지 X
                    monster.Init();
                 
                    StartCoroutine(EscapeMonsterTurn());
                    return;
                }

            case MonsterType.Long:
                // 원거리 몬스터(대각선 감지 O)
                if ((distacneX <= detectionRange && distacneY <= detectionRange) && (monster.monsterType == MonsterType.Long))
                {
                    // 감지 O
                    Player player = FindObjectOfType<Player>();
                    monster.Attack(player);

                    StartCoroutine(EscapeMonsterTurn());
                    return;
                }
                else
                {
                    // 감지 X
                    monster.Init();

                    StartCoroutine(EscapeMonsterTurn());
                    return;
                }
        }
    }*/

    // 플레이어 감지 및 공격
    public void GetSurroundingTiles(Vector2Int monsterPos)
    {
        int detectionRange = monster.monsterData.DetectionRagne;
        int skillDetectionRange = monster.monsterData.SkillDetectionRange;

        int distacneX = Mathf.Abs(monsterPos.x - playerPos.x);
        int distacneY = Mathf.Abs(monsterPos.y - playerPos.y);

        // 일반 공격
        if ((monster.monsterData.CurrentDamage < monster.monsterData.Critical) &&
            (distacneX <= detectionRange && monsterPos.y == playerPos.y) ||
            (distacneY <= detectionRange && monsterPos.x == playerPos.x))
        {
            monster.attack = AttackState.GeneralAttack;

            Player player = FindObjectOfType<Player>();
            transform.LookAt(player.transform); // 회전 값 보정
            monster.Attack(player); // 데미지 연산

            Debug.Log("General!");
            StartCoroutine(EscapeMonsterTurn());
            return;
        }
        // 스킬 공격
        else if ((monster.monsterData.CurrentDamage >= monster.monsterData.Critical) &&
            (distacneX <= skillDetectionRange && monsterPos.y == playerPos.y) ||
            (distacneY <= skillDetectionRange && monsterPos.x == playerPos.x))
        {
            monster.attack = AttackState.Skill;

            Player player = FindObjectOfType<Player>();
            transform.LookAt(player.transform); // 회전 값 보정
            monster.Attack(player);

            Debug.Log("Skill!");
            StartCoroutine(EscapeMonsterTurn());
            return;
        }
        // 감지 X
        else
        {
            monster.Init();
            StartCoroutine(EscapeMonsterTurn());
            return;
        }
    }


    // 몬스터 턴 종료 후 대기(바로 턴 넘어가기 방지)
    IEnumerator EscapeMonsterTurn()
    {
        while (monster.state != MonsterState.Idle)
            yield return null;

        yield return new WaitForSeconds(3f);
        BattleManager.instance.turn_UI[1].gameObject.SetActive(false);
        BattleManager.instance.PlayerTurn();
    }
}

   

