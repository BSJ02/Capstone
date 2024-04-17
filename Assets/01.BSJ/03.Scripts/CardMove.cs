using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using DG.Tweening;

public class CardMove : MonoBehaviour
{
    private Vector3 offset;
    private float distanceToCamera;

    //크기 저장
    private Vector3 originalScale;
    private Vector3 originalPosition;
    private int originalOrderInLayer;
  
    //아직 사용 안함 필요 없을 수 도 있음
    public bool dragdown = false;

    public string cardSortingLayerName = "Default"; // 변경할 Sorting Layer의 이름
    public int changeOrderInLayer; // 변경할 Order in Layer 값

    private SpriteRenderer spriteRenderer = null;


    void Start()
    {
        originalScale = this.transform.localScale;
        originalPosition = this.transform.position;
        this.spriteRenderer = this.GetComponent<SpriteRenderer>();

        GetComponent<Renderer>().sortingLayerName = cardSortingLayerName;
        changeOrderInLayer = spriteRenderer.sortingOrder;
        originalOrderInLayer = gameObject.layer;
    }

    void Update()
    {
        dragdown = false;


        //크기 뿐만아니라 카드가 위로 살짝 올라오도록 할 필요가 있어보임
        if (IsMouseOverObject(this.gameObject))
        {
            transform.DOKill();
            transform.DOScale(originalScale * 1.2f, 0.5f);
            transform.DOMove(new Vector3(originalPosition.x, originalPosition.y + 1, originalPosition.z), 0.5f);
            this.spriteRenderer.sortingOrder = 10;
            //레이어 정렬
        }
        else
        {
            transform.DOKill();
            // 마우스가 오브젝트 위에 없을 때 원래 크기로 돌아옴
            transform.DOScale(originalScale, 0.5f);
            transform.DOMove(originalPosition, 0.5f);

            this.spriteRenderer.sortingOrder = originalOrderInLayer;

            //this.gameObject.transform.localScale = originalScale;
        }

    }

    bool IsMouseOverObject(GameObject obj)
    {
        // 마우스 포인터 위치를 기준으로 Ray를 쏘아 충돌 체크
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.gameObject == obj && hit.collider.gameObject.layer == LayerMask.NameToLayer("Card"))
            {
                return true;
            }
        }
        return false;
    }
    

    //카드 패널과 충돌 처리
    void OnTriggerEnter(Collider other)
    {
        //임시
        if (other.gameObject.tag == "Panel")
        {

            Destroy(this.gameObject);
        }
    }

    void OnMouseUp()
    {
        dragdown = false;
        transform.DOKill();
        transform.DOMove(originalPosition, 1f);

        // transform.position = Vector3.Lerp(transform.position, originalPosition, 1f);

        //카드의 원래 위치값 저장
    }


    void OnMouseDown()
    {
        
        dragdown = true;
        transform.DOKill();
        //카드의 원래 위치값을 카드 정렬 코드에서 받을 필요가 있음 ( Error 마우스 광클시 위치값이 변경됨)
        //originalPosition = transform.localPosition;
        distanceToCamera = Vector3.Distance(transform.position, Camera.main.transform.position);
        offset = transform.position - GetMouseWorldPosition();
    }

  

    void OnMouseDrag()
    {
        transform.DOKill();
        transform.position = GetMouseWorldPosition() + offset;  
        
    }

    Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = distanceToCamera;
        return Camera.main.ScreenToWorldPoint(mousePoint);
    }

}
