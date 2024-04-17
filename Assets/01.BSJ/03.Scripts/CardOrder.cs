using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardOrder : MonoBehaviour
{
    [SerializeField] public Renderer backRenderer;
    [SerializeField] public Renderer imagebrderRenderer;
    [SerializeField] Renderer[] bordersRenderers;
    [SerializeField] Canvas[] canvas;
    [SerializeField] string sortingLayerName;

    public void SetOrder(int order)
    {
        int mulOrder = order * 5;
        
        backRenderer.sortingLayerName = sortingLayerName;
        backRenderer.sortingOrder = mulOrder;
        
        imagebrderRenderer.sortingLayerName = sortingLayerName;
        imagebrderRenderer.sortingOrder = mulOrder + 1;

        if (bordersRenderers != null)
        {
            foreach (var renderer in bordersRenderers)
            {
                renderer.sortingLayerName = sortingLayerName;
                renderer.sortingOrder = mulOrder + 2;
            }

        }
        
        if (canvas != null)
        {
            foreach (var renderer in canvas)
            {
                renderer.sortingLayerName = sortingLayerName;
                renderer.sortingOrder = mulOrder + 3;
            }
        }
        
    }

}
