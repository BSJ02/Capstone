using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardColorChanger : MonoBehaviour
{
    public CardInform cardInform;

    [Header("색상 바꿀 Renderer")]
    public Renderer[] cardRenderers;

    // 리스트에 있는 카드들의 Renderer.material.Color의 색 변경
    public void ChangeCardColors(Card card)
    {
        Color color;
        switch (card.cardRank)
        {
            case Card.CardRank.commonCards:
                color = cardInform.commonColor;
                break;
            case Card.CardRank.rareCards:
                color = cardInform.rareColor;
                break;
            case Card.CardRank.epicCards:
                color = cardInform.epicColor;
                break;
            case Card.CardRank.legendCards:
                color = cardInform.legendColor;
                break;
            default:
                color = Color.white;
                break;
        }

        // 모든 렌더러의 색상 변경
        foreach (Renderer renderer in cardRenderers)
        {
            renderer.material.color = color;
        }

    }
}
