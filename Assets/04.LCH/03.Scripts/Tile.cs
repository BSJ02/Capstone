using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public Coord coord;

    // 타일 변수 설정
    public void SetCoord(int x, int y, bool isWall)
    {
        coord = new Coord(x, y, isWall);
    }

    // 오버 로딩 
    public void SetCoord(int x, int y)
    {
        coord = new Coord(x, y, coord.isWall);
    }

}

[System.Serializable]
public class Coord
{
    public int x;
    public int y;

    public Coord(int _x, int _y, bool _isWall)
    {
        this.x = _x;
        this.y = _y;
        this.isWall = _isWall;
    }

    [HideInInspector]
    public Tile parentNode;

    public bool isWall;

    public int G, H;

    public int F { get { return G + H; } }


}
