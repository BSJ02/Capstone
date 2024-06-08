using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Card;

[CreateAssetMenu(fileName = "New Card", menuName = "Scriptable Object/Cards")]
public class CardInform : ScriptableObject
{
    [Header(" # 카드 종류")]
    [SerializeField] public List<Card> baseCards;
    [SerializeField] public List<Card> warriorCards;
    [SerializeField] public List<Card> archerCards;
    [SerializeField] public List<Card> wizardCards;

    [Header(" # 카드 확률 값")]
    public int commonPercent = 40;
    public int rarePercent = 30;
    public int epicPercent = 20;
    public int legendPercent = 10;

    [Header(" # 등급별 색상")]
    public Color commonColor = Color.green;
    public Color rareColor = Color.blue;
    public Color epicColor = Color.red;
    public Color legendColor = Color.yellow;

    // 확률 값 설정
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

    // 리스트에 있는 카드들의 percent를 원하는 값으로 설정
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

    // 각 카드 리스트에 따라 색상을 변경하는 메서드
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
    public string cardName; // 카드 이름
    public string cardDescription;  // 카드 설명
    public string cardDescription_Power;
    public float[] cardPower;   // 공격력, 힐량
    public float cardDistance;  // 이동거리
    public Sprite cardSprite;   // 카드 이미지
    public CardTarget cardTarget;   // 카드 적용 대상
    public CardRank cardRanke;  // 카드 등급
    [HideInInspector] public float cardPercent; // 카드 확률
    [HideInInspector] public CardType cardType; // 카드 타입
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