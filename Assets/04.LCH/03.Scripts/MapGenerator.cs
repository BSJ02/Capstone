using System.Collections.Generic;
using UnityEngine;


public class MapGenerator : MonoBehaviour
{
    public Tile[,] totalMap; // 2���� �迭 ��ǥ
    public Tile[] tilePrefab;

    public int garo;
    public int sero;

    private List<Tile> highlightedTiles = new List<Tile>(); // �̵� ������ ���� Ÿ�� ����Ʈ

    // �������� ���� �� ��ũ��Ʈ ȣ��� ���� ���� 
    private void Start()
    {
        CreateMap(garo, sero);
    }

    // �� ����
    public void CreateMap(int x, int y)
    {
        totalMap = new Tile[x, y];

        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < y; j++)
            {
                if (transform.childCount >= garo * sero)
                    return;

                int randValue = Mathf.FloorToInt(Random.Range(0, tilePrefab.Length));

                var tile = GameObject.Instantiate(tilePrefab[randValue], transform); // MapGenerator�� �θ�� ����
                tile.transform.localPosition = new Vector3(i * 1, 0, j * 1);

                // ��ǥ
                tile.SetCoord(i, j, false);
                totalMap[i, j] = tile;


            }
        }
    }


    // Ÿ�� ���� �ʱ�ȭ(�ߺ� ����)
    public void ResetTotalMap()
    {
        for (int i = 0; i < totalMap.GetLength(0); i++)
        {
            for (int j = 0; j < totalMap.GetLength(1); j++)
            {
                totalMap[i, j].SetCoord(i, j);
            }
        }
    }

    // �÷��̾� �̵� ������ ���� ǥ��
    public void HighlightPlayerRange(Vector3 playerPosition, int maxDistance)
    {
        // �ʱ�ȭ
        ClearHighlightedTiles();

        // �÷��̾� ��ġ�� Ÿ���� ã��
        int playerX = Mathf.RoundToInt(playerPosition.x);
        int playerZ = Mathf.RoundToInt(playerPosition.z);
        Tile playerTile = totalMap[playerX, playerZ];

        // BFS�� �̿��Ͽ� �÷��̾� �̵� ������ ���� ã��
        Queue<PathNode> queue = new Queue<PathNode>();
        HashSet<Tile> visited = new HashSet<Tile>();
        queue.Enqueue(new PathNode(playerTile, 0));
        visited.Add(playerTile);

        while (queue.Count > 0)
        {
            PathNode currentNode = queue.Dequeue();
            Tile currentTile = currentNode.tile;
            int currentDistance = currentNode.distance;

            // ���� Ÿ���� �̵� ������ ���� ���� �ִ� ��쿡�� ó��
            if (currentDistance <= maxDistance)
            {
                // �̵� ������ ���� ���� �ִ� Ÿ���̸鼭 isWall�� false�� ��쿡�� ������ �����ϰ� ť�� �߰�
                if (!currentTile.coord.isWall && currentTile != playerTile)
                {
                    currentTile.GetComponent<Renderer>().material.color = Color.red;
                    highlightedTiles.Add(currentTile);
                }
            }

            // �����¿� �̵� ������ Ÿ�� Ȯ��
            CheckAdjacentTiles(currentTile, queue, visited, maxDistance, currentDistance + 1);
        }
    }

    // �����¿� �̵� ������ Ÿ�� Ȯ�� �� ť�� �߰�
    private void CheckAdjacentTiles(Tile currentTile, Queue<PathNode> queue, HashSet<Tile> visited, int maxDistance, int nextDistance)
    {
        int x = currentTile.coord.x;
        int y = currentTile.coord.y;

        // �����¿� Ÿ�� Ȯ��
        TryEnqueue(totalMap, x - 1, y, queue, visited, maxDistance, nextDistance);
        TryEnqueue(totalMap, x + 1, y, queue, visited, maxDistance, nextDistance);
        TryEnqueue(totalMap, x, y - 1, queue, visited, maxDistance, nextDistance);
        TryEnqueue(totalMap, x, y + 1, queue, visited, maxDistance, nextDistance);
    }

    // Ÿ���� ť�� �߰��ϴ� �޼��� ����
    private void TryEnqueue(Tile[,] map, int x, int y, Queue<PathNode> queue, HashSet<Tile> visited, int maxDistance, int nextDistance)
    {
        if (x >= 0 && x < map.GetLength(0) && y >= 0 && y < map.GetLength(1))
        {
            Tile tile = map[x, y];
            // ���� �ƴϰ�, �̵� ������ ������ �ʰ����� �ʴ� ��쿡�� ť�� �߰�
            if (!tile.coord.isWall && !visited.Contains(tile) && nextDistance <= maxDistance)
            {
                queue.Enqueue(new PathNode(tile, nextDistance));
                visited.Add(tile);
            }
        }
    }
    // �̵� ��θ� ��Ÿ���� ��� Ŭ����
    private class PathNode
    {
        public Tile tile;
        public int distance;

        public PathNode(Tile tile, int distance)
        {
            this.tile = tile;
            this.distance = distance;
        }
    }

    // �̵� ������ ���� Ÿ�� �ʱ�ȭ
    public void ClearHighlightedTiles()
    {
        foreach (Tile tile in highlightedTiles)
        {
            tile.GetComponent<Renderer>().material.color = Color.white;
        }
        highlightedTiles.Clear();
    }

    // Ŭ���� Ÿ���� �̵� ������ ���� ���� �ִ��� Ȯ��
    public bool IsHighlightedTile(Tile tile)
    {
        return highlightedTiles.Contains(tile);
    }

}