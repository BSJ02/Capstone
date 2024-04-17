using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Scriptable Object/Cards")]
public class CardInform : ScriptableObject
{
    [Header("ī�� ����")]
    [SerializeField] public List<Card> baseCards;
    [SerializeField] public List<Card> commonCards;
    [SerializeField] public List<Card> rareCards;
    [SerializeField] public List<Card> epicCards;
    [SerializeField] public List<Card> legendCards;

    [Header("ī�� Ȯ�� ��")]
    public int commonPercent = 50;
    public int rarePercent = 30;
    public int epicPercent = 15;
    public int legendPercent = 5;

    [Header("���� ����")]
    public Color commonColor = Color.green;
    public Color rareColor = Color.blue;
    public Color epicColor = Color.red;
    public Color legendColor = Color.yellow;

    // Ȯ�� �� ����
    private void OnValidate()
    {
        FixCardPersent(commonCards, commonPercent);
        FixCardPersent(rareCards, rarePercent);
        FixCardPersent(epicCards, epicPercent);
        FixCardPersent(legendCards, legendPercent);

        ApplyCardRank(baseCards);
        ApplyCardRank(commonCards);
        ApplyCardRank(rareCards);
        ApplyCardRank(epicCards);
        ApplyCardRank(legendCards);
    }

    // ����Ʈ�� �ִ� ī����� percent�� ���ϴ� ������ ����
    private void FixCardPersent(List<Card> cards, float percent)
    {
        foreach (Card card in cards)
        {
            card.cardPercent = percent;
        }
    }

    // �� ī�� ����Ʈ�� ���� ������ �����ϴ� �޼���
    public void ApplyCardRank(List<Card> cardList)
    {
        foreach (Card card in cardList)
        {
            if (cardList == commonCards)
            {
                card.cardRank = Card.CardRank.commonCards;
            }
            else if (cardList == rareCards)
            {
                card.cardRank = Card.CardRank.rareCards;
            }
            else if (cardList == epicCards)
            {
                card.cardRank = Card.CardRank.epicCards;
            }
            else if (cardList == legendCards)
            {
                card.cardRank = Card.CardRank.legendCards;
            }
            else
            {
                card.cardRank = Card.CardRank.baseCards;
            }
        }
    }
}

[System.Serializable]
public class Card
{
    public string cardName; // ī�� �̸�
    public string cardDescription;  // ī�� ����
    public int cardPower;   // ���ݷ�, ����, �̵��Ÿ�
    public Sprite cardSprite;   // ī�� �̹���
    public float cardPercent; // ī�� Ȯ��
    public CardType cardType; // ī�� Ÿ�� (���ݱ�, ȸ����, �̵���)
    public CardRank cardRank; // ī�� ���

    public enum CardType
    {
        Attack, // ����
        Heal,   // ȸ��
        Movement, // �̵�
        Shiled   // ����
    }

    public enum CardRank
    {
        baseCards,  // �⺻
        commonCards,    // �Ϲ�
        rareCards,  // ����
        epicCards,  // ����
        legendCards // ����
    }

}