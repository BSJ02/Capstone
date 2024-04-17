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

        // ��� �������� ���� ����
        foreach (Renderer renderer in cardRenderers)
        {
            renderer.material.color = color;
        }

    }
}
