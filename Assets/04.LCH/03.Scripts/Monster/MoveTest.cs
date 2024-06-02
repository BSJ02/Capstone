using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTest : MonoBehaviour
{
    private Monster monster;

    // ��� ����
    Tile StartNode, TargetNode, CurrentNode;
    List<Tile> OpenList = new List<Tile>();
    List<Tile> CloseList = new List<Tile>();

    // ���� ��ǥ
    public Vector2Int monsterPos { get; private set; }
    public Vector2Int playerPos { get; private set; }

    // �ൿ ó��
    bool isMoving;     // 1. ���� �� ��ų�� ����� ��� isMoving = true 
                       // 2. �̵� �� ���� ���� ���� �÷��̾ �������� ���� ��� isMoving = true

    private void Awake()
    {
        monster = GetComponent<Monster>();
        isMoving = false; // �ൿ �� �ʱ�ȭ
    }

    // ���� �ʱ� ���� O 
    public IEnumerator StartDetection()
    {
        // �ʱ�ȭ(���� ����Ʈ ����)
        OpenList.Clear();
        CloseList.Clear();

        // ��ǥ ���� �� ����
        SetDestination();
        MapGenerator.instance.totalMap[monsterPos.x, monsterPos.y].SetCoord(monsterPos.x, monsterPos.y, true);
        GetSurroundingTiles(monsterPos);

        if (isMoving == false)
        {
            Moving();
        }

        // ���� ��ġ�� isWall�� ����
        MapGenerator.instance.ResetTotalMap();

        // �� ���� �ൿ �� �߰� ������(�ʿ��� ��� ��� �ð� �߰�)
        yield return new WaitForSeconds(1f);
    }

    // ���� �ʱ� ���� X
    public void Moving()
    {
        // �ʱ�ȭ
        OpenList.Clear();
        CloseList.Clear();

        // ��ã�� ����
        SetDestination();
        List<Vector2Int> move = PathFinding();
        StartCoroutine(MoveSmoothly(move));
    }

    // �ʱ� ��� ����
    public void SetDestination()
    {
        int playerLength = BattleManager.instance.players.Count;
        float[] currentPlayerHp = new float[playerLength];
        Vector2Int[] playersPosition = new Vector2Int[playerLength];
        GameObject[] playersObj = GameObject.FindGameObjectsWithTag("Player");

        monsterPos = new Vector2Int((int)transform.position.x, (int)transform.position.z);

        // ��ȿ�� �÷��̾� ������Ʈ�� ã�� �ִ��� ����
        if (playersObj.Length != playerLength)
        {
            return;
        }

        for (int i = 0; i < playerLength; i++)
        {
            Vector3 player = playersObj[i].transform.position;
            playersPosition[i] = new Vector2Int((int)player.x, (int)player.z);
        }

        // ���Ϳ� �÷��̾��� ��ġ�� ���� ������ ����� �ʴ��� ����
        int mapWidth = MapGenerator.instance.totalMap.GetLength(0);
        int mapHeight = MapGenerator.instance.totalMap.GetLength(1);

        if (monsterPos.x < 0 || monsterPos.x >= mapWidth || monsterPos.y < 0 || monsterPos.y >= mapHeight)
        {
            return;
        }

        // �÷��̾� ��ġ ����
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

        // ��ų ���� ���� �÷��̾ �ִ��� Ȯ��
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

        // ��ų ���� ���� �÷��̾ ���� ��� ���� ���� ���� �÷��̾ �ִ��� Ȯ��
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

        // �÷��̾ ã�� ���� ���, ü���� ���� ���� ����
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

    // �ִܰŸ� ��� 
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

    // ������ ������
    public IEnumerator MoveSmoothly(List<Vector2Int> path)
    {
        // ���� �ִ� �̵� �Ÿ�(moveDistance ��ŭ ����Ʈ ��ȯ)
        int maxMoveDistance = monster.monsterData.MoveDistance;

        float moveSpeed = 1f;
        float lerpMaxTime = 0.2f;

        monster.state = MonsterState.Moving;
        monster.GetComponent<Animator>().SetInteger("State", (int)monster.state);

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

        // RandomDamage ����
        monster.ReadyToAttack();

        // �÷��̾� ���� �� ����
        GetSurroundingTiles(finalPosition);

        monster.Init();

        yield break;
    }

    // �÷��̾� ���� �� ���� (�밢�� ���� ����)
    public bool GetSurroundingTiles(Vector2Int monsterPos)
    {
        int attackDetectionRange = monster.monsterData.DetectionRagne;
        int skillDetectionRange = monster.monsterData.SkillDetectionRange;

        List<Vector2Int> skillTiles = AttackRangeChecking(monsterPos, skillDetectionRange, true);
        List<Vector2Int> attackTiles = AttackRangeChecking(monsterPos, attackDetectionRange, false);

        // ��ų ���� ���� ���� �÷��̾ ������ ���
        foreach (Vector2Int tile in skillTiles)
        {
            foreach (GameObject playerObj in GameObject.FindGameObjectsWithTag("Player"))
            {
                Player playerComponent = playerObj.GetComponent<Player>();
                Vector2Int playerPos = new Vector2Int((int)playerObj.transform.position.x, (int)playerObj.transform.position.z);
                if (tile == playerPos && monster.monsterData.CurrentDamage >= monster.monsterData.Critical)
                {
                    isMoving = true;

                    // ���� ���� ���� 
                    monster.attack = AttackState.SkillAttack;
                    monster.ReadyToAttack();
                    monster.Attack(playerComponent);

                    // ȸ���� ���� 
                    transform.LookAt(playerObj.transform);
                    return true;
                }
            }
        }

        // �Ϲ� ���� ���� ���� �÷��̾ ������ ���
        foreach (Vector2Int tile in attackTiles)
        {
            foreach (GameObject playerObj in GameObject.FindGameObjectsWithTag("Player"))
            {
                Player playerComponent = playerObj.GetComponent<Player>();
                Vector2Int playerPos = new Vector2Int((int)playerObj.transform.position.x, (int)playerObj.transform.position.z);
                if (tile == playerPos)
                {
                    isMoving = true;

                    // ���� ���� ���� 
                    monster.attack = AttackState.GeneralAttack;
                    monster.ReadyToAttack();
                    monster.Attack(playerComponent);

                    // ȸ���� ����
                    transform.LookAt(playerObj.transform);
                    return true;
                }
            }
        }

        isMoving = false;
        return false;
    }

    // ���� ���� Ÿ�� �������� (��ų �� ���� ����)
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
                    continue; // �迭�� ������ ����� ����
                }

                // �밢�� ������ Ÿ���� �����ϰ�, Ÿ���� ������ ���� ���� �ִ��� Ȯ��
                if ((Mathf.Abs(x - center.x) == 0 || Mathf.Abs(y - center.y) == 0) && Mathf.Abs(x - center.x) <= range && Mathf.Abs(y - center.y) <= range)
                {
                    Vector2Int tileCoord = new Vector2Int(x, y);
                    Tile tile = MapGenerator.instance.totalMap[x, y];

                    // �̹� ������ ������ Ÿ���� �ǳʶ�
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

