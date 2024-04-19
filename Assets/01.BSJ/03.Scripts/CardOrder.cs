using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardOrder : MonoBehaviour
{
    [SerializeField] private Renderer cardRenderer;
    [SerializeField] private Renderer backRenderer;
    [SerializeField] private Renderer imagebrderRenderer;
    [SerializeField] private Renderer[] bordersRenderers;
    [SerializeField] private Canvas[] canvas;
    [SerializeField] private string sortingLayerName;

    private int originalOrder;

    public int OriginalOrder
    {
        get
        {
            return originalOrder;
        }
        set
        {
            originalOrder = value;
        }
    }


    public void SetOrder(int order)
    {
        int mulOrder = (order + 1) * 5;

        OriginalOrder = mulOrder;

        // CardRenderer ó��
        if (cardRenderer != null)
        {
            cardRenderer.sortingLayerName = sortingLayerName;
            cardRenderer.sortingOrder = mulOrder;
        }

        // BackRenderer ó��
        if (backRenderer != null)
        {
            backRenderer.sortingLayerName = sortingLayerName;
            backRenderer.sortingOrder = mulOrder + 1;
        }

        // ImageBorderRenderer ó��
        if (imagebrderRenderer != null)
        {
            imagebrderRenderer.sortingLayerName = sortingLayerName;
            imagebrderRenderer.sortingOrder = mulOrder + 2;
        }

        // BordersRenderers �迭 ó��
        if (bordersRenderers != null)
        {
            for (int i = 0; i < bordersRenderers.Length; i++)
            {
                if (bordersRenderers[i] != null)
                {
                    bordersRenderers[i].sortingLayerName = sortingLayerName;
                    bordersRenderers[i].sortingOrder = mulOrder + 3;
                }
            }
        }

        // Canvas �迭 ó��
        if (canvas != null)
        {
            for (int i = 0; i < canvas.Length; i++)
            {
                if (canvas[i] != null)
                {
                    canvas[i].sortingLayerName = sortingLayerName;
                    canvas[i].sortingOrder = mulOrder + 4;
                }
            }
        }
    }


    // ������ Order ������ ���ư��� �޼���
    public void ResetOrder()
    {
        if (cardRenderer != null)
        {
            cardRenderer.sortingOrder = originalOrder;
        }

        if (backRenderer != null)
        {
            backRenderer.sortingOrder = originalOrder + 1;
        }

        if (imagebrderRenderer != null)
        {
            imagebrderRenderer.sortingOrder = originalOrder + 2;
        }

        if (bordersRenderers != null)
        {
            foreach (var renderer in bordersRenderers)
            {
                if (renderer != null)
                {
                    renderer.sortingOrder = originalOrder + 3;
                }
            }
        }

        if (canvas != null)
        {
            foreach (var renderer in canvas)
            {
                if (renderer != null)
                {
                    renderer.sortingOrder = originalOrder + 4;
                }
            }
        }
    }
}
