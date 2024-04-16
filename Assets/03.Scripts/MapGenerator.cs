using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public Tile[,] totalMap; // 2���� �迭 ��ǥ
    public Tile tilePrefab;

    public int garo;
    public int sero;

    // �� ����
    public void CreateMap(int x, int y)
    {
        totalMap = new Tile[x, y];

        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < y; j++)
            {
                if (transform.childCount == garo * sero)
                    return;

                var tile = GameObject.Instantiate(tilePrefab, transform); // MapGenerator�� �θ�� ����
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
                totalMap[i, j].SetCoord(i, j, false);
            }
        }
    }

}