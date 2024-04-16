using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardColorChange : MonoBehaviour
{
    public Renderer[] renderers; // 카드 색상을 변경할 Renderer 배열

    // 변경할 색상
    public Color newColor = Color.red;

    void Update()
    {
        // Renderer 배열이 비어있는지 확인
        if (renderers == null || renderers.Length == 0)
        {
            Debug.LogError("Renderer 배열이 비어 있습니다.");
            return;
        }

        // 모든 Renderer에 색상 변경 적용
        foreach (Renderer renderer in renderers)
        {
            if (renderer != null)
            {
                // 해당 Renderer의 모든 소재(Material)에 색상을 변경합니다.
                foreach (Material material in renderer.materials)
                {
                    material.color = newColor;
                }
            }
        }
    }
}
