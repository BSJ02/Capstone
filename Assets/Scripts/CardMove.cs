using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using DG.Tweening;

public class CardMove : MonoBehaviour
{
    private Vector3 offset;
    private float distanceToCamera;

        //ũ�� ����
    private Vector3 originalScale;
    private Vector3 originalPosition;
    private int originalLayer;   
  
    //���� ��� ���� �ʿ� ���� �� �� ����
    public bool dragdown = false;


    public string sortingLayerName = "Default"; // ������ Sorting Layer�� �̸�
    public int orderInLayer; // ������ Order in Layer ��

    private SpriteRenderer spriteRenderer = null;

    private void Awake()
    {
        originalPosition = transform.position;  
        this.spriteRenderer = this.GetComponent<SpriteRenderer>();
    }
    void Start()
    {
        GetComponent<Renderer>().sortingLayerName = sortingLayerName;
        originalScale = transform.localScale;
        orderInLayer = spriteRenderer.sortingOrder;
        originalLayer = gameObject.layer;
        //originalPosition = transform.localPosition;
    }

    void Update()
    {
        dragdown = false;


        //ũ�� �Ӹ��ƴ϶� ī�尡 ���� ��¦ �ö������ �� �ʿ䰡 �־��

        if (IsMouseOverObject(this.gameObject))
        {
            // ���콺�� ������Ʈ ���� ���� �� ũ�⸦ 2��� ����
            //this.gameObject.transform.localScale = originalScale * 2f;
            transform.DOKill();
            transform.DOScale(originalScale * 1.2f, 0.5f);
            transform.DOMove(new Vector3(originalPosition.x, originalPosition.y + 1, originalPosition.z), 0.5f);
            this.spriteRenderer.sortingOrder = 10;
            //���̾� ����
        }
        else
        {
            transform.DOKill();
            // ���콺�� ������Ʈ ���� ���� �� ���� ũ��� ���ƿ�
            transform.DOScale(originalScale, 0.5f);
            transform.DOMove(originalPosition, 0.5f);
            
            this.spriteRenderer.sortingOrder = orderInLayer;
            
            //this.gameObject.transform.localScale = originalScale;
        }



    }

    bool IsMouseOverObject(GameObject obj)
    {
        // ���콺 ������ ��ġ�� �������� Ray�� ��� �浹 üũ
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
    

    //ī�� �гΰ� �浹 ó��
    void OnTriggerEnter(Collider other)
    {
        //�ӽ�
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

        //ī���� ���� ��ġ�� ����
    }


    void OnMouseDown()
    {
        
        dragdown = true;
        transform.DOKill();
        //ī���� ���� ��ġ���� ī�� ���� �ڵ忡�� ���� �ʿ䰡 ���� ( Error ���콺 ��Ŭ�� ��ġ���� �����)
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
