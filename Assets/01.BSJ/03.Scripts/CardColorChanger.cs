using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardColorChanger : MonoBehaviour
{
    public CardInform cardInform;

    [Header("���� �ٲ� Renderer")]
    public Renderer[] cardRenderers;

    // ����Ʈ�� �ִ� ī����� Renderer.material.Color�� �� ����
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
