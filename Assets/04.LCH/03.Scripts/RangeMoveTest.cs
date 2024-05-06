using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RangeMoveTest : MonoBehaviour
{
/*    private Monster monster;

    Tile StartNode, TargetNode, CurrentNode;
    List<Tile> OpenList = new List<Tile>();
    List<Tile> CloseList = new List<Tile>();

    Vector2Int monsterPos;
    Vector2Int playerPos;

    private void Awake()
    {
        monster = GetComponent<Monster>();
    }


    // �÷��̾� �� ���� �� ȣ��[���� ��]
    public void MoveStart()
    {
        // �ʱ�ȭ
        OpenList.Clear();
        CloseList.Clear();

        // �÷��̾� ���� �ϰ� ����
        PlayerDetect();
        // 1. �÷��̾ optimalRange �ȿ� ���� ��
        // 2. �÷��̾ optimalRange �ۿ� ���� ��

        // ��ã�� ����
        SetDestination();
        List<Vector2Int> move = PathFinding();
        StartCoroutine(MoveSmoothly(move));

        // Ÿ�� ��ǥ �ʱ�ȭ 
        MapGenerator.instance.ResetTotalMap();
    }

    private void PlayerDetect()
    {
        // �ڱ� �߽��� �������� optimalRange * optimalRange Ÿ���� ������ �����ͼ� ���� �÷��̾ ������  
    }

    // �÷��̾� �� ���� �ʱ� ��ǥ ����
    public void SetDestination()
    {
        monsterPos = new Vector2Int((int)transform.position.x, (int)transform.position.z);

        Vector3 player = FindObjectOfType<Player>().transform.position;
        playerPos = new Vector2Int((int)player.x, (int)player.z);

        MapGenerator.instance.totalMap[monsterPos.x, monsterPos.y].SetCoord(monsterPos.x, monsterPos.y, false);

        StartNode = MapGenerator.instance.totalMap[monsterPos.x, monsterPos.y];
        TargetNode = MapGenerator.instance.totalMap[playerPos.x, playerPos.y];
    }

    // ��ã�� ����
    public List<Vector2Int> PathFinding()
    {
        OpenList.Add(StartNode);

        List<Vector2Int> path = new List<Vector2Int>(); // ��ΰ� ��� ����Ʈ

        path.Clear();

        while (OpenList.Count > 0)
        {
            CurrentNode = OpenList[0];

            // ���� ��� ���
            for (int i = 1; i < OpenList.Count; i++)
            {
                if (OpenList[i].coord.F <= CurrentNode.coord.F && OpenList[i].coord.H < CurrentNode.coord.H)
                {
                    CurrentNode = OpenList[i];
                }
            }

            OpenList.Remove(CurrentNode);
            CloseList.Add(CurrentNode);

            // ��ǥ ���� ã��
            if (CurrentNode == TargetNode)
            {
                Tile currentNode = TargetNode;

                while (currentNode != null)
                {
                    path.Add(new Vector2Int(currentNode.coord.x, currentNode.coord.y));
                    currentNode = currentNode.coord.parentNode;
                }

                path.Reverse(); // �÷��̾� ���� ��ǥ -> ���� ���� ��ǥ[���� ����]

                break;
            }

            // ��, ������, �Ʒ�, ���� ������� OpenList ã��
            OpenListAdd(CurrentNode.coord.x, CurrentNode.coord.y + 1);
            OpenListAdd(CurrentNode.coord.x + 1, CurrentNode.coord.y);
            OpenListAdd(CurrentNode.coord.x, CurrentNode.coord.y - 1);
            OpenListAdd(CurrentNode.coord.x - 1, CurrentNode.coord.y);
        }

        // �÷��̾� ��ħ ����
        if (path.Count > 0)
            path.RemoveAt(path.Count - 1);

        // ��� ��ȯ 
        return path;
    }

    // ��� ���� ��� ��� 
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

    // ���� ������ ������
    public IEnumerator MoveSmoothly(List<Vector2Int> path)
    {
        monster.state = MonsterState.Moving;
        monster.gameObject.GetComponent<Animator>().SetInteger("State", (int)monster.state);

        // ���� �ִ� �̵� �Ÿ�(moveDistance ��ŭ ����Ʈ ��ȯ)
        *//*        int maxMoveDistance = monster.monsterData.MoveDistance;*//*
        int maxMoveDistance = monster.GetComponent<MonsterData>().MoveDistance;

        float moveSpeed = 1f;
        float lerpMaxTime = 0.2f;

        for (int i = 0; i < path.Count - 1; i++)
        {
            // ������ moveDistacne ���� �ִ��̸� ����
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

            // Position �� ����
            transform.position = nextPosition;
        }

        // ���� �÷��̾� ��ǥ
        Vector2Int finalPosition = new Vector2Int((int)transform.position.x, (int)transform.position.z);

        // ���� ��ǥ isWall ����(���� �� �÷��̾� ��ħ ����)
        MapGenerator.instance.totalMap[finalPosition.x, finalPosition.y].SetCoord(finalPosition.x, finalPosition.y, true);

        // �÷��̾� ���� �� ����
        GetSurroundingTiles(finalPosition);

        // ���� �� ����
        StartCoroutine(EscapeMonsterTurn());
        yield break;
    }


    // �÷��̾� ���� �� ����
    public void GetSurroundingTiles(Vector2Int monsterPos)
    {
        *//*int detectionRange = monster.monsterData.DetectionRagne;*//*
        int detectionRange = monster.GetComponent<MonsterData>().DetectionRagne;

        int distacneX = Mathf.Abs(monsterPos.x - playerPos.x);
        int distacneY = Mathf.Abs(monsterPos.y - playerPos.y);


        switch (monster.monsterType)
        {
            case MonsterType.Short:
                // �ٰŸ� ����
                if ((distacneX <= detectionRange && monsterPos.y == playerPos.y) || (distacneY <= detectionRange && monsterPos.x == playerPos.x) && (monster.monsterType == MonsterType.Short))
                {
                    // ���� O 
                    Player player = FindObjectOfType<Player>();
                    transform.LookAt(player.transform); // ȸ�� �� ����
                   *//* monster.ReadyToAttack(player);*//*
                    return;
                }
                else
                {
                    // ���� X
                    monster.Init();
                    return;
                }
            case MonsterType.Long:
                // ���Ÿ� ����
                if ((distacneX <= detectionRange && distacneY <= detectionRange) && (monster.monsterType == MonsterType.Short))
                {
                    // �밢�� ���� O (���Ÿ� ����)
                    if (monster.monsterType != MonsterType.Long)
                        return;

                    // ���� O
                    Player player = FindObjectOfType<Player>();
                *//*    monster.ReadyToAttack(player);*//*
                    return;
                }
                else
                {
                    // ���� X
                    monster.Init();
                    return;
                }
        }
    }

    // ���� �� ���� �� 2�� ���(�ٷ� ���� ����)
    IEnumerator EscapeMonsterTurn()
    {
        yield return new WaitForSeconds(2f);
        BattleManager.instance.turn_UI[1].gameObject.SetActive(false);
        BattleManager.instance.PlayerTurn();
    }*/
}

