using UnityEngine;
using DG.Tweening;

public class PlatformMove : MonoBehaviour
{
    public float duration = 2.0f; // 애니메이션 지속 시간
    public float amplitude = 0.5f; // Y축 진동 범위

    void Start()
    {
        transform.DOMoveY(transform.position.y + amplitude, duration)
            .SetLoops(-1, LoopType.Yoyo) 
            .SetEase(Ease.InOutSine); 
    }
}
