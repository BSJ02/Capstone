using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Scriptable Object/Cards")]
public class CardInform : ScriptableObject
{
    [Header("카드 종류")]
    [SerializeField] public List<Card> baseCards;
    [SerializeField] public List<Card> commonCards;
    [SerializeField] public List<Card> rareCards;
    [SerializeField] public List<Card> epicCards;
    [SerializeField] public List<Card> legendCards;

    [Header("카드 확률 값")]
    [Range(0, 100)] public int commonPercent = 50;
    [Range(0, 100)] public int rarePercent = 30;
    [Range(0, 100)] public int epicPercent = 15;
    [Range(0, 100)] public int legendPercent = 5;

    [Header("색상 정의")]
    public Color commonColor = Color.green;
    public Color rareColor = Color.blue;
    public Color epicColor = Color.red;
    public Color legendColor = Color.yellow;

    // 확률 값 설정
    private void OnValidate()
    {
        FixCardPercent(commonCards, commonPercent);
        FixCardPercent(rareCards, rarePercent);
        FixCardPercent(epicCards, epicPercent);
        FixCardPercent(legendCards, legendPercent);

        ApplyCardRank(baseCards, Card.CardRank.baseCards);
        ApplyCardRank(commonCards, Card.CardRank.commonCards);
        ApplyCardRank(rareCards, Card.CardRank.rareCards);
        ApplyCardRank(epicCards, Card.CardRank.epicCards);
        ApplyCardRank(legendCards, Card.CardRank.legendCards);
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
    public string cardName; // 카드 이름
    public string cardDescription;  // 카드 설명
    public int cardPower;   // 공격력, 힐량, 이동거리
    public Sprite cardSprite;   // 카드 이미지
    [Range(0, 100)] public float cardPercent; // 카드 확률
    public CardType cardType; // 카드 타입 (공격기, 회복기, 이동기)
    public CardRank cardRank; // 카드 등급

    public enum CardType
    {
        Attack, // 공격
        Heal,   // 회복
        Movement, // 이동
        Shiled   // 막기
    }

    public enum CardRank
    {
        baseCards,  // 기본
        commonCards,    // 일반
        rareCards,  // 레어
        epicCards,  // 에픽
        legendCards // 전설
    }

}