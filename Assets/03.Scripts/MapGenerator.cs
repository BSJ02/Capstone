using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public Tile[,] totalMap; // 2차원 배열 좌표
    public Tile tilePrefab;

    public int garo;
    public int sero;

    // 맵 생성
    public void CreateMap(int x, int y)
    {
        totalMap = new Tile[x, y];

        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < y; j++)
            {
                if (transform.childCount == garo * sero)
                    return;

                var tile = GameObject.Instantiate(tilePrefab, transform); // MapGenerator를 부모로 설정
                tile.transform.localPosition = new Vector3(i * 1, 0, j * 1);

                // 좌표
                tile.SetCoord(i, j, false);
                totalMap[i, j] = tile;
            }
        }
    }

    // 타일 정보 초기화(중복 방지)
    public void ResetTotalMap()
    {
        for (int i = 0; i < totalMap.GetLength(0); i++)
        {
            for (int j = 0; j < totalMap.GetLength(1); j++)
            {
                totalMap[i, j].SetCoord(i, j, false);
            }
        }
    }

}