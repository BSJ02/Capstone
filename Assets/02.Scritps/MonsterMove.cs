using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterMove : MonoBehaviour
{
    private MapGenerator mapGenerator;

    private Monster monster;

    Tile StartNode, TargetNode, CurrentNode;
    List<Tile> OpenList = new List<Tile>();
    List<Tile> CloseList = new List<Tile>();

    Vector2Int monsterPos;
    Vector2Int playerPos;

    private void Awake()
    {
        mapGenerator = FindObjectOfType<MapGenerator>();
        monster = GetComponent<Monster>();

        monster.state = State.Idle;
    }


    // TurnManager���� ȣ��(Player Turn -> Enemy Turn)
    public void ButtonClick()
    {
        // ��ư Ŭ������ list �ʱ�ȭ 
        OpenList.Clear();
        CloseList.Clear();

        // ��ã�� 
        SetDestination();
        List<Vector2Int> move = PathFinding();
        StartCoroutine(MoveSmoothly(move));



        // Ÿ�� ���� �ʱ�ȭ
        mapGenerator.ResetTotalMap();

    }


    public void SetDestination() // StartNode, TargetNode �ʱ�ȭ
    {
        monsterPos = new Vector2Int((int)transform.position.x, (int)transform.position.z); // ���� ������ Vector3 X,Z ��
        playerPos = new Vector2Int((int)FindObjectOfType<Player>().transform.position.x,
            (int)FindObjectOfType<Player>().transform.position.z);  // Player ������Ʈ�� ������

        StartNode = mapGenerator.totalMap[monsterPos.x, monsterPos.y];
        TargetNode = mapGenerator.totalMap[playerPos.x, playerPos.y];
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

            // ��� �� ã��
            if (CurrentNode == TargetNode)
            {
                Tile currentNode = TargetNode;

                while (currentNode != null)
                {
                    path.Add(new Vector2Int(currentNode.coord.x, currentNode.coord.y));
                    currentNode = currentNode.coord.parentNode;
                }

                path.Reverse();

                foreach (var position in path)
                {
                    /*Debug.Log($"X:{position.x}, Y:{position.y}");*/
                }

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

    public IEnumerator MoveSmoothly(List<Vector2Int> path) // ���� �̵� 
    {
        monster.state = State.Moving;

        float moveSpeed = 1f;
        float lerpMaxTime = 0.2f; // ���� �̵��ӵ� ����(�������� ������)

        for (int i = 0; i < path.Count - 1; i++)
        {
            Vector3 monsterPosition = new Vector3(path[i].x, transform.position.y, path[i].y); // ���� ���� ��ǥ
            Vector3 nextPosition = new Vector3(path[i + 1].x, transform.position.y, path[i + 1].y); // ���� ���� ��ǥ 

            float startTime = Time.time;

            while (Time.time < startTime + lerpMaxTime)
            {
                float currentTime = (Time.time - startTime) * moveSpeed;
                float weight = currentTime / lerpMaxTime;

                // ���� �̵�
                transform.position = Vector3.Lerp(monsterPosition, nextPosition, weight);
                // ���� ȸ��
                transform.LookAt(nextPosition);
                yield return null;
            }

            // ��ǥ�� ����
            transform.position = nextPosition;
        }

        // Path�� ���� ��ǥ
        Vector2Int finalPosition = new Vector2Int(path[path.Count - 1].x, path[path.Count - 1].y);
        
        // Player ����
        GetSurroundingTiles(finalPosition);

        // �ڷ�ƾ ���� & �̵� ����
        yield break;
    }

    // Player ����(3x3 Ÿ��)
    public void GetSurroundingTiles(Vector2Int monsterPos)
    {
        // ���� ���� ��ǥ ����
        List<Tile> surroundingTiles = new List<Tile>();

        for (int xOffset = -1; xOffset <= 1; xOffset++)
        {
            for (int yOffset = -1; yOffset <= 1; yOffset++)
            {
                int x = monsterPos.x + xOffset;
                int y = monsterPos.y + yOffset;

                if (x >= 0 && x < mapGenerator.totalMap.GetLength(0) && y >= 0 && y < mapGenerator.totalMap.GetLength(1))
                {
                    surroundingTiles.Add(mapGenerator.totalMap[x, y]);
                }
            }
        }

        // Player Ȯ��
        foreach (Tile tile in surroundingTiles)
        {
            RaycastHit hit;
            if(Physics.Raycast(new Vector3(tile.coord.x, 0, tile.coord.y), Vector3.up, out hit, 2f))
            {
                if (hit.collider.gameObject.CompareTag("Player"))
                {
                    // �÷��̾ ������
                    monster.state = State.Attack;
                    monster.Attack();
                }
                else
                {
                    // �÷��̾ ������ 
                    monster.state = State.Idle;
                    // TurnManager �� �ٲٱ�(Monster Turn -> Player Turn)
                }
            }
        }
    }

}
