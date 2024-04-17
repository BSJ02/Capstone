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

    // 카드 확률 값
    public int commonPercent = 50;
    public int rarePercent = 30;
    public int epicPercent = 15;
    public int legendPercent = 5;

    // 색상 정의
    public Color commonColor = Color.green;
    public Color rareColor = Color.blue;
    public Color epicColor = Color.red;
    public Color legendColor = Color.yellow;

    // 확률 값 설정
    private void OnValidate()
    {
        FixCardPersent(commonCards, commonPercent, commonColor);
        FixCardPersent(rareCards, rarePercent, rareColor);
        FixCardPersent(epicCards, epicPercent, epicColor);
        FixCardPersent(legendCards, legendPercent, legendColor);        
    }

    // 리스트에 있는 카드들의 percent를 원하는 값으로 설정
    private void FixCardPersent(List<Card> cards, float probability, Color color)
    {
        foreach (Card card in cards)
        {
            card.percent = probability;
            card.color = color;
        }
    }

}

[System.Serializable]
public class Card
{
    public string cardName; // 카드 이름
    public string cardDescription_1;  // 카드 설명 1
    public int power;   // 공격력, 힐량, 이동거리
    public string cardDescription_2;  // 카드 설명 2
    public Sprite cardSprite;   // 카드 이미지
    public float percent; // 카드 확률
    public CardType cardType; // 카드 타입 (공격기, 회복기, 이동기)
    public Color color; // 카드 색상

    public enum CardType
    {
        Attack, // 공격
        Heal,   // 회복
        Movement // 이동
    }
}