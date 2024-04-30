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
        if (cardRenderers != null)
        {
            Color color = GetColorForCardRank(card.cardRank);
            ApplyColorToRenderers(color);
        }
    }

    private Color GetColorForCardRank(Card.CardRank rank)
    {
        switch (rank)
        {
            case Card.CardRank.WarriorCard:
                return cardInform.commonColor;
            case Card.CardRank.ArcherCard:
                return cardInform.rareColor;
            case Card.CardRank.WizardCard:
                return cardInform.epicColor;
            default:
                return Color.white;
        }
    }

    private void ApplyColorToRenderers(Color color)
    {
        foreach (Renderer renderer in cardRenderers)
        {
            if (renderer != null && renderer.material != null)
            {
                renderer.material.color = color;
            }
        }
    }
}
