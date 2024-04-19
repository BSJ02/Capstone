using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeColor : MonoBehaviour
{
    public Tile tile;
    public Color[] color; // 0 = red(FF0000) / 1 = blue(0030FF) / 2 = 

    void Start()
    {

    }

    void Update()
    {

        if(tile.coord.isWall == true)
        {
            GetComponent<Renderer>().material.color = Color.gray;
        }

    }


    void OnMouseEnter()
    {
        // 마우스가 오브젝트 위에 있을 때 실행되는 함수
        if (GetComponent<Renderer>().material.color == Color.red) // 현재 색상이 빨간색인 경우에만 실행
        {
            GetComponent<Renderer>().material.color = Color.blue; // 파란색으로 색상 변경
        }
    }

    void OnMouseExit()
    {
        // 마우스가 오브젝트를 벗어날 때 실행되는 함수
        if (GetComponent<Renderer>().material.color == Color.blue) // 현재 색상이 빨간색인 경우에만 실행
        {
            GetComponent<Renderer>().material.color = Color.red; // 원래 색상으로 복원
        }
    }
}

