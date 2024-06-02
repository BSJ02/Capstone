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


    public void SetOrder(int order)
    {
        int mulOrder = (order + 1) * 5;

        // CardRenderer 贸府
        if (cardRenderer != null)
        {
            cardRenderer.sortingLayerName = sortingLayerName;
            cardRenderer.sortingOrder = mulOrder;
        }

        // BackRenderer 贸府
        if (backRenderer != null)
        {
            backRenderer.sortingLayerName = sortingLayerName;
            backRenderer.sortingOrder = mulOrder + 1;
        }

        // ImageBorderRenderer 贸府
        if (imagebrderRenderer != null)
        {
            imagebrderRenderer.sortingLayerName = sortingLayerName;
            imagebrderRenderer.sortingOrder = mulOrder + 2;
        }

        // BordersRenderers 硅凯 贸府
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

        // Canvas 硅凯 贸府
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

}
