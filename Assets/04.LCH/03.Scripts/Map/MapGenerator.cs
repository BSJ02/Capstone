using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public static MapGenerator instance;

    public Tile[,] totalMap;
    public Tile[,] EdgeTile;

    public Tile[] tilePrefab;

    public int garo;
    public int sero;

    [HideInInspector] public bool selectingTarget;

    public List<Tile> highlightedTiles = new List<Tile>();
    public List<Tile[,]> EdgeTiles = new List<Tile[,]>();

    public List<Monster> rangeInMonsters = new List<Monster>();
    public List<Player> rangeInPlayers = new List<Player>();

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


    #region Attribute Methods
    // 실시간 맵 생성
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

                var tile = GameObject.Instantiate(tilePrefab[randValue], transform); 
                tile.transform.localPosition = new Vector3(i * 1, 0, j * 1);

                tile.SetCoord(i, j, false);

                RaycastHit hit;
                if(Physics.Raycast(tile.transform.position, Vector3.up, out hit, 1f)) 
                {
                    if (hit.collider.CompareTag("Monster") || hit.collider.CompareTag("Item")) // isWall 체크
                    {
                        tile.SetCoord(i, j, true);
                    }
                }

                totalMap[i, j] = tile;
            }
        }

        int amount = 0;
        for (int k = 0; k <= EdgeTiles.Count; k++)
        {
            amount += 1;
        }
    }

    // 실시간 맵 삭제
    public void DeleteMap()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(transform.GetChild(i).gameObject);
        }
    }
    #endregion


    #region Main Methods
    // 모서리에 있는 타일의 좌표를 저장(미완성)
    // 몬스터 이동 알고리즘에 쓰일 메소드
    private void GetEdgeTiles(int x, int y)
    {
        // 좌
        if (x == 0 && y >= 0)
        {
            Tile[,] EdgeTile = new Tile[x, y];
            EdgeTiles.Add(EdgeTile);

            return;
        }

        // 우
        if (x == garo - 1 && y >= 0)
        {
            Tile[,] EdgeTile = new Tile[x, y];
            EdgeTiles.Add(EdgeTile);

            return;
        }

        // 상
        if (x >= 0 && y == sero - 1)
        {
            Tile[,] EdgeTile = new Tile[x, y];
            EdgeTiles.Add(EdgeTile);

            return;
        }

        // 하
        if (x >= 0 && y == 0)
        {
            Tile[,] EdgeTile = new Tile[x, y];
            EdgeTiles.Add(EdgeTile);

            return;
        }
    }

    // 맵 좌표 초기화
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

    // 이동 범위 
    public void HighlightPlayerRange(Vector3 playerPosition, int maxDistance)
    {
        ClearHighlightedTiles();

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

            if (currentDistance <= maxDistance)
            {
                if (!currentTile.coord.isWall && currentTile != playerTile)
                {
                    currentTile.GetComponent<Renderer>().material.color = Color.red;
                    highlightedTiles.Add(currentTile);
                }
            }

            CheckAdjacentTiles(currentTile, queue, visited, maxDistance, currentDistance + 1);
        }
    }

    // 카드 범위
    public void CardUseRange(Vector3 objectPosition, int cardDistance)
    {
        ClearHighlightedTiles();
        rangeInMonsters.Clear();
        rangeInPlayers.Clear();

        int objectX = Mathf.RoundToInt(objectPosition.x);
        int objectZ = Mathf.RoundToInt(objectPosition.z);
        Tile objectTile = totalMap[objectX, objectZ];

        Queue<PathNode> queue = new Queue<PathNode>();
        HashSet<Tile> visited = new HashSet<Tile>();
        queue.Enqueue(new PathNode(objectTile, 0));

        while (queue.Count > 0)
        {
            PathNode currentNode = queue.Dequeue();
            Tile currentTile = currentNode.tile;
            int currentDistance = currentNode.distance;

            if (currentDistance <= cardDistance)
            {
                if (currentTile != objectTile)
                {
                    currentTile.GetComponent<Renderer>().material.color = Color.black;
                    highlightedTiles.Add(currentTile);
                }
                else if (currentTile.coord.isWall)
                {
                    currentTile.GetComponent<Renderer>().material.color = Color.gray;
                }
            }
            CheckAdjacentTiles(currentTile, queue, visited, cardDistance, currentDistance + 1);
        }
        highlightedTiles.Add(objectTile);
        TileOnObject(highlightedTiles);
        selectingTarget = false;
    }

    // 타일 색 변경
    public bool IsHighlightedTile(Tile tile)
    {
        return highlightedTiles.Contains(tile);
    }

    // 타일 색 초기화
    public void ClearHighlightedTiles()
    {
        foreach (Tile tile in highlightedTiles)
        {
            tile.GetComponent<Renderer>().material.color = Color.white;
        }
        highlightedTiles.Clear();
    }

    public void TileOnObject(List<Tile> tiles)
    {
        Monster[] monsters = FindObjectsOfType<Monster>();
        Player[] players = FindObjectsOfType<Player>();

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

        foreach (Player player in players)
        {
            foreach (Tile tile in tiles)
            {
                if (player.transform.position.x == tile.transform.position.x && player.transform.position.z == tile.transform.position.z)
                {
                    rangeInPlayers.Add(player);
                    break;
                }
            }
        }
    }

    private void CheckAdjacentTiles(Tile currentTile, Queue<PathNode> queue, HashSet<Tile> visited, int maxDistance, int nextDistance)
    {
        int x = currentTile.coord.x;
        int y = currentTile.coord.y;

        TryEnqueue(totalMap, x - 1, y, queue, visited, maxDistance, nextDistance);
        TryEnqueue(totalMap, x + 1, y, queue, visited, maxDistance, nextDistance);
        TryEnqueue(totalMap, x, y - 1, queue, visited, maxDistance, nextDistance);
        TryEnqueue(totalMap, x, y + 1, queue, visited, maxDistance, nextDistance);
    }

    private void TryEnqueue(Tile[,] map, int x, int y, Queue<PathNode> queue, HashSet<Tile> visited, int maxDistance, int nextDistance)
    {
        if (x >= 0 && x < map.GetLength(0) && y >= 0 && y < map.GetLength(1))
        {
            Tile tile = map[x, y];

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
    #endregion

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
}