using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Card;

[CreateAssetMenu(fileName = "New Card", menuName = "Scriptable Object/Cards")]
public class CardInform : ScriptableObject
{
    [Header(" # ī�� ����")]
    [SerializeField] public List<Card> baseCards;
    [SerializeField] public List<Card> warriorCards;
    [SerializeField] public List<Card> archerCards;
    [SerializeField] public List<Card> wizardCards;

    [Header(" # ī�� Ȯ�� ��")]
    public int commonPercent = 40;
    public int rarePercent = 30;
    public int epicPercent = 20;
    public int legendPercent = 10;

    [Header(" # ��޺� ����")]
    public Color commonColor = Color.green;
    public Color rareColor = Color.blue;
    public Color epicColor = Color.red;
    public Color legendColor = Color.yellow;

    // Ȯ�� �� ����
    private void OnValidate()
    {
        FixCardPercent(baseCards);
        FixCardPercent(warriorCards);
        FixCardPercent(archerCards);
        FixCardPercent(wizardCards);

        ApplyCardColor(baseCards, Card.CardType.BaseCard);
        ApplyCardColor(warriorCards, Card.CardType.WarriorCard);
        ApplyCardColor(archerCards, Card.CardType.ArcherCard);
        ApplyCardColor(wizardCards, Card.CardType.WizardCard);
    }

    // ����Ʈ�� �ִ� ī����� percent�� ���ϴ� ������ ����
    private void FixCardPercent(List<Card> cards)
    {
        foreach (Card card in cards)
        {
            CardRank cardRank = card.cardRanke;

            if (cardRank == CardRank.Common)
            {
                card.cardPercent = commonPercent;
            }
            else if (cardRank == CardRank.rare)
            {
                card.cardPercent = rarePercent;
            }
            else if (cardRank == CardRank.Epic)
            {
                card.cardPercent = epicPercent;
            }
            else if (cardRank == CardRank.Legend)
            {
                card.cardPercent = legendPercent;
            }
        }
    }

    // �� ī�� ����Ʈ�� ���� ������ �����ϴ� �޼���
    public void ApplyCardColor(List<Card> cardList, Card.CardType type)
    {
        foreach (Card card in cardList)
        {
            card.cardType = type;
        }
    }
}

[System.Serializable]
public class Card
{
    public string cardName; // ī�� �̸�
    public string cardDescription;  // ī�� ����
    public string cardDescription_Power;
    public float[] cardPower;   // ���ݷ�, ����
    public float cardDistance;  // �̵��Ÿ�
    public Sprite cardSprite;   // ī�� �̹���
    public CardTarget cardTarget;   // ī�� ���� ���
    public CardRank cardRanke;  // ī�� ���
    [HideInInspector] public float cardPercent; // ī�� Ȯ��
    [HideInInspector] public CardType cardType; // ī�� Ÿ��
    [HideInInspector] public bool isCardMoveEnabled = false;

    public enum CardType
    {
        BaseCard,
        WarriorCard, 
        ArcherCard,
        WizardCard
    }

    public enum CardTarget
    {
        Player,
        SingleTarget,
        AreaTarget,
        TargetPosition
    }

    public enum CardRank
    {
        Common,
        rare,
        Epic,
        Legend
    }
}