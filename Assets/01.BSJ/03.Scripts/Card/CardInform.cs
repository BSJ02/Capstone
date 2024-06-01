using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Scriptable Object/Cards")]
public class CardInform : ScriptableObject
{
    [Header(" # 카드 종류")]
    [SerializeField] public List<Card> baseCards;
    [SerializeField] public List<Card> warriorCards;
    [SerializeField] public List<Card> archerCards;
    [SerializeField] public List<Card> wizardCards;

    [Header(" # 카드 확률 값")]
    public int basePercent = 35;
    public int commonPercent = 25;
    public int rarePercent = 20;
    public int epicPercent = 15;
    public int legendPercent = 5;

    [Header(" # 등급별 색상")]
    public Color commonColor = Color.green;
    public Color rareColor = Color.blue;
    public Color epicColor = Color.red;
    public Color legendColor = Color.yellow;

    // 확률 값 설정
    private void OnValidate()
    {
        FixCardPercent(baseCards, basePercent);
        FixCardPercent(warriorCards, commonPercent);
        FixCardPercent(archerCards, rarePercent);
        FixCardPercent(wizardCards, epicPercent);

        ApplyCardRank(baseCards, Card.CardType.BaseCard);
        ApplyCardRank(warriorCards, Card.CardType.WarriorCard);
        ApplyCardRank(archerCards, Card.CardType.ArcherCard);
        ApplyCardRank(wizardCards, Card.CardType.WizardCard);
    }

    // 리스트에 있는 카드들의 percent를 원하는 값으로 설정
    private void FixCardPercent(List<Card> cards, float percent)
    {
        foreach (Card card in cards)
        {
            card.cardPercent = percent;
        }
    }

    // 각 카드 리스트에 따라 색상을 변경하는 메서드
    public void ApplyCardRank(List<Card> cardList, Card.CardType type)
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
    //[HideInInspector] public int cardID;
    public string cardName; // 카드 이름
    public string cardDescription;  // 카드 설명
    public string cardDescription_Power;
    public float[] cardPower;   // 공격력, 힐량
    public float cardDistance;  // 이동거리
    public Sprite cardSprite;   // 카드 이미지
    public float cardPercent; // 카드 확률
    public CardType cardType; // 카드 등급
    public CardTarget cardTarget;
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
}