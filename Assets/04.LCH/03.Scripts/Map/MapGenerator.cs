using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.Experimental.GraphView.GraphView;


public class MapGenerator : MonoBehaviour
{
    public static MapGenerator instance;

    public Tile[,] totalMap; // 2차원 배열 좌표
    public Tile[] tilePrefab;

    public int garo;
    public int sero;

    [HideInInspector] public bool selectingTarget;

    [SerializeField]
    public List<Tile> highlightedTiles = new List<Tile>(); // 이동 가능한 범위 타일 리스트

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

    // 맵 생성
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

                var tile = GameObject.Instantiate(tilePrefab[randValue], transform); // MapGenerator를 부모로 설정
                tile.transform.localPosition = new Vector3(i * 1, 0, j * 1);

                // 전체 좌표 설정
                tile.SetCoord(i, j, false);

                // 타일 위에 몬스터가 있을 경우(몬스터 겹침 방지)
                RaycastHit hit;
                if(Physics.Raycast(tile.transform.position, Vector3.up, out hit, 1f))
                {
                    if (hit.collider.CompareTag("Monster") || hit.collider.CompareTag("Item")) // || hit.collider.CompareTag("Player") <= Ghost Code(필요 시 추가 예정)
                    {
                        tile.SetCoord(i, j, true);
                    }
                }

                totalMap[i, j] = tile;
            }
        }
    }

    public void DeleteMap()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(transform.GetChild(i).gameObject);
        }
    }
    

    // 타일 정보 초기화(중복 방지)
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

    // 플레이어 이동 가능한 범위 표시
    public void HighlightPlayerRange(Vector3 playerPosition, int maxDistance)
    {
        // 초기화
        ClearHighlightedTiles();

        // 플레이어 위치의 타일을 찾음
        int playerX = Mathf.RoundToInt(playerPosition.x);
        int playerZ = Mathf.RoundToInt(playerPosition.z);
        Tile playerTile = totalMap[playerX, playerZ];

        // BFS를 이용하여 플레이어 이동 가능한 범위 찾기
        Queue<PathNode> queue = new Queue<PathNode>();
        HashSet<Tile> visited = new HashSet<Tile>();
        queue.Enqueue(new PathNode(playerTile, 0));
        visited.Add(playerTile);

        while (queue.Count > 0)
        {
            PathNode currentNode = queue.Dequeue();
            Tile currentTile = currentNode.tile;
            int currentDistance = currentNode.distance;

            // 현재 타일이 이동 가능한 범위 내에 있는 경우에만 처리
            if (currentDistance <= maxDistance)
            {
                // 이동 가능한 범위 내에 있는 타일이면서 isWall이 false인 경우에만 색상을 변경하고 큐에 추가
                if (!currentTile.coord.isWall && currentTile != playerTile)
                {
                    currentTile.GetComponent<Renderer>().material.color = Color.red;
                    highlightedTiles.Add(currentTile);
                }
            }

            // 상하좌우 이동 가능한 타일 확인
            CheckAdjacentTiles(currentTile, queue, visited, maxDistance, currentDistance + 1);
        }
    }

    // 카드 사용 범위
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

    // 상하좌우 이동 가능한 타일 확인 및 큐에 추가
    private void CheckAdjacentTiles(Tile currentTile, Queue<PathNode> queue, HashSet<Tile> visited, int maxDistance, int nextDistance)
    {
        int x = currentTile.coord.x;
        int y = currentTile.coord.y;

        // 상하좌우 타일 확인
        TryEnqueue(totalMap, x - 1, y, queue, visited, maxDistance, nextDistance);
        TryEnqueue(totalMap, x + 1, y, queue, visited, maxDistance, nextDistance);
        TryEnqueue(totalMap, x, y - 1, queue, visited, maxDistance, nextDistance);
        TryEnqueue(totalMap, x, y + 1, queue, visited, maxDistance, nextDistance);
    }

    // 타일을 큐에 추가하는 메서드 수정
    private void TryEnqueue(Tile[,] map, int x, int y, Queue<PathNode> queue, HashSet<Tile> visited, int maxDistance, int nextDistance)
    {
        if (x >= 0 && x < map.GetLength(0) && y >= 0 && y < map.GetLength(1))
        {
            Tile tile = map[x, y];
            // 벽이 아니고, 이동 가능한 범위를 초과하지 않는 경우에만 큐에 추가
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
    // 이동 경로를 나타내는 노드 클래스
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

    // 이동 가능한 범위 타일 초기화
    public void ClearHighlightedTiles()
    {
        foreach (Tile tile in highlightedTiles)
        {
            tile.GetComponent<Renderer>().material.color = Color.white;
        }
        highlightedTiles.Clear();
    }

    // 클릭한 타일이 이동 가능한 범위 내에 있는지 확인
    public bool IsHighlightedTile(Tile tile)
    {
        return highlightedTiles.Contains(tile);
    }

}