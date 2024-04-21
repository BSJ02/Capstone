using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterMove : MonoBehaviour
{
    private Monster monster;

    public int detectionRange = 1;

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
        // ��ư Ŭ������ list �ʱ�ȭ 
        OpenList.Clear();
        CloseList.Clear();

        // ��ã�� 
        SetDestination();
        List<Vector2Int> move = PathFinding();
        StartCoroutine(MoveSmoothly(move));

        // Ÿ�� ���� �ʱ�ȭ
        MapGenerator.instatnce.ResetTotalMap();

    }


    public void SetDestination() // StartNode, TargetNode �ʱ�ȭ
    {
        
        // ���� ��ǥ
        monsterPos = new Vector2Int((int)transform.position.x, (int)transform.position.z);

        // �÷��̾� ��ǥ
        Vector3 player = FindObjectOfType<Player>().transform.position;
        playerPos = new Vector2Int((int)player.x, (int)player.z);

        // �̵� �� 
        MapGenerator.instatnce.totalMap[monsterPos.x, monsterPos.y].SetCoord(monsterPos.x, monsterPos.y, false); 

        StartNode = MapGenerator.instatnce.totalMap[monsterPos.x, monsterPos.y];
        TargetNode = MapGenerator.instatnce.totalMap[playerPos.x, playerPos.y];
    }

    public List<Vector2Int> PathFinding() // ��ã��
    {
        OpenList.Add(StartNode);
        
        List<Vector2Int> path = new List<Vector2Int>();

        // ������ ã�� path ����(�ߺ� ����) 
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

            // ���?�� ã��
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

        if(path.Count > 0)
        path.RemoveAt(path.Count - 1); // Player ��ǥ ����, ��ħ ����

        return path;
    }

    public void OpenListAdd(int checkX, int checkY) // CurrentNode üũ
    {
        if (checkX < 0 || checkX >= MapGenerator.instatnce.totalMap.GetLength(0) || checkY < 0 || checkY >= MapGenerator.instatnce.totalMap.GetLength(1))
            return;

        if (CloseList.Contains(MapGenerator.instatnce.totalMap[checkX, checkY]))
            return;

        if (MapGenerator.instatnce.totalMap[checkX, checkY].coord.isWall)
            return;


        if (OpenList.Contains(MapGenerator.instatnce.totalMap[checkX, checkY]))
        {
            int newG = CurrentNode.coord.G + (Mathf.Abs(CurrentNode.coord.x - checkX) == 0 || Mathf.Abs(CurrentNode.coord.y - checkY) == 0 ? 10 : 14);
            if (newG < MapGenerator.instatnce.totalMap[checkX, checkY].coord.G)
            {
                MapGenerator.instatnce.totalMap[checkX, checkY].coord.G = newG;
                MapGenerator.instatnce.totalMap[checkX, checkY].coord.parentNode = CurrentNode;
            }
        }
        else
        {
            MapGenerator.instatnce.totalMap[checkX, checkY].coord.G = CurrentNode.coord.G + (Mathf.Abs(CurrentNode.coord.x - checkX) == 0 || Mathf.Abs(CurrentNode.coord.y - checkY) == 0 ? 10 : 14);
            MapGenerator.instatnce.totalMap[checkX, checkY].coord.H = (Mathf.Abs(checkX - TargetNode.coord.x) + Mathf.Abs(checkY - TargetNode.coord.y)) * 10;
            MapGenerator.instatnce.totalMap[checkX, checkY].coord.parentNode = CurrentNode;
            OpenList.Add(MapGenerator.instatnce.totalMap[checkX, checkY]);
        }
    }

    public IEnumerator MoveSmoothly(List<Vector2Int> path) // ���� �̵� 
    {
        monster.state = MonsterState.Moving;
        monster.gameObject.GetComponent<Animator>().SetInteger("State", (int)monster.state);

        // �ִ� �̵� �Ÿ�
        int maxMoveDistance = monster.monsterData.MoveDistance;
        Debug.Log("�̵��Ÿ� :" + maxMoveDistance + "ĭ");

        float moveSpeed = 1f;
        float lerpMaxTime = 0.2f; // ���� �̵��ӵ� ����(�������� ������)

        for (int i = 0; i < path.Count - 1; i++)
        {
            if (i >= maxMoveDistance)
                break;

            Vector3 monsterPosition = new Vector3(path[i].x, transform.position.y, path[i].y); // ���� ���� ��ǥ
            Vector3 nextPosition = new Vector3(path[i + 1].x, transform.position.y, path[i + 1].y); // ���� ���� ��ǥ 

            float startTime = Time.time;

            while (Time.time < startTime + lerpMaxTime)
            {
                float currentTime = (Time.time - startTime) * moveSpeed;
                float weight = currentTime / lerpMaxTime;

                // ���� �̵�
                transform.position = Vector3.Lerp(monsterPosition, nextPosition, weight);
                // ���� �̵� ��ǥ ���� ȸ�� 
                transform.LookAt(nextPosition);
                yield return null;
            }

            // ��ǥ�� �� ȸ���� ����
            transform.position = nextPosition;
            // ȸ���� ���� �ڵ� �ʿ�

        }

        // Path�� ���� ��ǥ
        Vector2Int finalPosition = new Vector2Int((int)transform.position.x, (int)transform.position.z);

        // ���� ��ħ ����
        MapGenerator.instatnce.totalMap[finalPosition.x, finalPosition.y].SetCoord(finalPosition.x, finalPosition.y, true); // ���� Ÿ�� isWal��

        // Player ����
        GetSurroundingTiles(finalPosition);

        StartCoroutine(EscapeMonsterTurn());

        // �ڷ�ƾ ���� & �̵� ����
        yield break;
    }

    // Player ����(3x3 Ÿ��)
    public void GetSurroundingTiles(Vector2Int monsterPos)
    {
        int distacneX = Mathf.Abs(monsterPos.x - playerPos.x);
        int distacneY = Mathf.Abs(monsterPos.y - playerPos.y);

        if(distacneX <= detectionRange && distacneY <= detectionRange)
        {
            // �÷��̾� ���� O
            monster.ReadyToAttack();
            return;
        }
        else
        {
            // �÷��̾� ���� X
            monster.Init();
            return;
        }
    }

    // Player �� �ѱ�
    IEnumerator EscapeMonsterTurn()
    {
        yield return new WaitForSeconds(2f);
        BattleManager.instance.ui[1].gameObject.SetActive(false);
        BattleManager.instance.PlayerTurn();
    }
}

