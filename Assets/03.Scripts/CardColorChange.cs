using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardColorChange : MonoBehaviour
{
    public Renderer[] renderers; // ī�� ������ ������ Renderer �迭

    // ������ ����
    public Color newColor = Color.red;

    void Update()
    {
        // Renderer �迭�� ����ִ��� Ȯ��
        if (renderers == null || renderers.Length == 0)
        {
            Debug.LogError("Renderer �迭�� ��� �ֽ��ϴ�.");
            return;
        }

        // ��� Renderer�� ���� ���� ����
        foreach (Renderer renderer in renderers)
        {
            if (renderer != null)
            {
                // �ش� Renderer�� ��� ����(Material)�� ������ �����մϴ�.
                foreach (Material material in renderer.materials)
                {
                    material.color = newColor;
                }
            }
        }
    }
}
