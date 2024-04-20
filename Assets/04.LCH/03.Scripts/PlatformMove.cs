using UnityEngine;
using DG.Tweening;

public class PlatformMove : MonoBehaviour
{
    public float duration = 2.0f; // �ִϸ��̼� ���� �ð�
    public float amplitude = 0.5f; // Y�� ���� ����

    void Start()
    {
        transform.DOMoveY(transform.position.y + amplitude, duration)
            .SetLoops(-1, LoopType.Yoyo) 
            .SetEase(Ease.InOutSine); 
    }
}
