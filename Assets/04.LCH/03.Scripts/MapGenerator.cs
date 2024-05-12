using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;


public class MapGenerator : MonoBehaviour
{
    public static MapGenerator instance;

    public Tile[,] totalMap; // 2���� �迭 ��ǥ
    public Tile[] tilePrefab;

    public int garo;
    public int sero;

    [HideInInspector] public bool selectingTarget;

    [SerializeField]
    public List<Tile> highlightedTiles = new List<Tile>(); // �̵� ������ ���� Ÿ�� ����Ʈ
    public List<Monster> rangeInMonsters = new List<Monster>();

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
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

                // ��ü ��ǥ ����
                tile.SetCoord(i, j, false);

                // Ÿ�� ���� ���Ͱ� ���� ���(���� ��ħ ����)
                RaycastHit hit;
                if(Physics.Raycast(tile.transform.position, Vector3.up, out hit, 1f))
                {
                    if (hit.collider.CompareTag("Monster") || hit.collider.CompareTag("Item"))
                    {
                        tile.SetCoord(i, j, true);
                    }
                }

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

    // ī�� ��� ����
    public void CardUseRange(Vector3 playerPosition, int distance)
    {
        ClearHighlightedTiles();
        rangeInMonsters.Clear();

        int playerX = Mathf.RoundToInt(playerPosition.x);
        int playerZ = Mathf.RoundToInt(playerPosition.z);
        Tile playerTile = totalMap[playerX, playerZ];

        Queue<PathNode> queue = new Queue<PathNode>();
        HashSet<Tile> visited = new HashSet<Tile>();
        queue.Enqueue(new PathNode(playerTile, 0));
        visited.Add(playerTile);

        while (queue.Count > 0)
        {
            PathNode currentNode = queue.Dequeue();
            Tile currentTile = currentNode.tile;
            int currentDistance = currentNode.distance;

            if (currentDistance <= distance)
            {
                if (currentTile != playerTile)
                {
                    currentTile.GetComponent<Renderer>().material.color = Color.black;
                    highlightedTiles.Add(currentTile);
                }
            }
            CheckAdjacentTiles(currentTile, queue, visited, distance, currentDistance + 1);
        }
        TileOnMonster(highlightedTiles);
        selectingTarget = false;
    }

    public void TileOnMonster(List<Tile> tiles)
    {
        Monster[] monsters = FindObjectsOfType<Monster>();
        
        foreach (Monster monster in monsters)
        {
            foreach (Tile tile in tiles)
            {
                if (monster.transform.position.x == tile.transform.position.x && monster.transform.position.z == tile.transform.position.z)
                {
                    rangeInMonsters.Add(monster);
                    break;
                }
            }
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
            if (selectingTarget)
            {
                if (!visited.Contains(tile) && nextDistance <= maxDistance)
                {
                    queue.Enqueue(new PathNode(tile, nextDistance));
                    visited.Add(tile);
                }
            }
            else if (!tile.coord.isWall && !visited.Contains(tile) && nextDistance <= maxDistance)
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