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
    [Range(0, 100)] public int commonPercent = 50;
    [Range(0, 100)] public int rarePercent = 30;
    [Range(0, 100)] public int epicPercent = 15;
    [Range(0, 100)] public int legendPercent = 5;

    [Header("���� ����")]
    public Color commonColor = Color.green;
    public Color rareColor = Color.blue;
    public Color epicColor = Color.red;
    public Color legendColor = Color.yellow;

    // Ȯ�� �� ����
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

    // ����Ʈ�� �ִ� ī����� percent�� ���ϴ� ������ ����
    private void FixCardPercent(List<Card> cards, float percent)
    {
        foreach (Card card in cards)
        {
            card.cardPercent = percent;
        }
    }

    // �� ī�� ����Ʈ�� ���� ������ �����ϴ� �޼���
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
    public string cardName; // ī�� �̸�
    public string cardDescription;  // ī�� ����
    public int cardPower;   // ���ݷ�, ����, �̵��Ÿ�
    public Sprite cardSprite;   // ī�� �̹���
    [Range(0, 100)] public float cardPercent; // ī�� Ȯ��
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