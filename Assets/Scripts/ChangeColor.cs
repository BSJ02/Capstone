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
        // ���콺�� ������Ʈ ���� ���� �� ����Ǵ� �Լ�
        if (GetComponent<Renderer>().material.color == Color.red) // ���� ������ �������� ��쿡�� ����
        {
            GetComponent<Renderer>().material.color = Color.blue; // �Ķ������� ���� ����
        }
    }

    void OnMouseExit()
    {
        // ���콺�� ������Ʈ�� ��� �� ����Ǵ� �Լ�
        if (GetComponent<Renderer>().material.color == Color.blue) // ���� ������ �������� ��쿡�� ����
        {
            GetComponent<Renderer>().material.color = Color.red; // ���� �������� ����
        }
    }
}

