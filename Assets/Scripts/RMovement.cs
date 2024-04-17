using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RMovement : MonoBehaviour
{

    public GameObject[] objects;

    // 초기 크기를 저장할 배열
    private Vector3[] originalScales;

    void Start()
    { 
        // 배열 크기 초기화
        originalScales = new Vector3[objects.Length];

        // 각 오브젝트의 초기 크기 저장
        for (int i = 0; i < objects.Length; i++)
        {
            originalScales[i] = objects[i].transform.localScale;
        }
    }

    void Update()
    {
        // 마우스 이벤트는 Update()에서 처리
        for (int i = 0; i < objects.Length; i++)
        {
            if (IsMouseOverObject(objects[i]))
            {
                // 마우스가 오브젝트 위에 있을 때 크기를 2배로 변경
                objects[i].transform.localScale = originalScales[i] * 2f;
            }
            else
            {
                // 마우스가 오브젝트 위에 없을 때 원래 크기로 돌아옴
                objects[i].transform.localScale = originalScales[i];
            }
        }

        
    }


    // 오브젝트 위에 마우스가 있는지 확인하는 함수
    bool IsMouseOverObject(GameObject obj)
    {
        // 마우스 포인터 위치를 기준으로 Ray를 쏘아 충돌 체크
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {

            if (hit.collider.gameObject == obj)
            {
                return true;
            }
        }
        return false;
    }

}
