using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Scriptable Object/Cards")]
public class CardInform : ScriptableObject
{
    [Header(" # 카드 종류")]
    [SerializeField] public List<Card> baseCards;
    [SerializeField] public List<Card> commonCards;
    [SerializeField] public List<Card> rareCards;
    [SerializeField] public List<Card> epicCards;
    [SerializeField] public List<Card> legendCards;

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
        FixCardPercent(commonCards, commonPercent);
        FixCardPercent(rareCards, rarePercent);
        FixCardPercent(epicCards, epicPercent);
        FixCardPercent(legendCards, legendPercent);

        ApplyCardRank(baseCards, Card.CardRank.BaseCards);
        ApplyCardRank(commonCards, Card.CardRank.CommonCards);
        ApplyCardRank(rareCards, Card.CardRank.RareCards);
        ApplyCardRank(epicCards, Card.CardRank.EpicCards);
        ApplyCardRank(legendCards, Card.CardRank.LegendCards);
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
    public void ApplyCardRank(List<Card> cardList, Card.CardRank rank)
    {
        foreach (Card card in cardList)
        {
            card.cardRank = rank;
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
    [Header(" # Power1, Power2, Distance")] public float[] cardPower;   // 공격력, 힐량, 이동거리
    public Sprite cardSprite;   // 카드 이미지
    public float cardPercent; // 카드 확률
    public CardRank cardRank; // 카드 등급
    public WeaponType cardWeaponType;

    public enum CardRank
    {
        BaseCards,  // 기본
        CommonCards,    // 일반
        RareCards,  // 레어
        EpicCards,  // 에픽
        LegendCards // 전설
    }

    public enum WeaponType
    {
        Sword,
        Axe,
        Bow,
        Hammer,
        Wand,
        Shield
    }
}