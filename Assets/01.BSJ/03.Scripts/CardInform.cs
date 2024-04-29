using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Scriptable Object/Cards")]
public class CardInform : ScriptableObject
{
    [Header(" # ī�� ����")]
    [SerializeField] public List<Card> baseCards;
    [SerializeField] public List<Card> commonCards;
    [SerializeField] public List<Card> rareCards;
    [SerializeField] public List<Card> epicCards;
    [SerializeField] public List<Card> legendCards;

    [Header(" # ī�� Ȯ�� ��")]
    public int basePercent = 35;
    public int commonPercent = 25;
    public int rarePercent = 20;
    public int epicPercent = 15;
    public int legendPercent = 5;

    [Header(" # ��޺� ����")]
    public Color commonColor = Color.green;
    public Color rareColor = Color.blue;
    public Color epicColor = Color.red;
    public Color legendColor = Color.yellow;

    // Ȯ�� �� ����
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
    //[HideInInspector] public int cardID;
    public string cardName; // ī�� �̸�
    public string cardDescription;  // ī�� ����
    public string cardDescription_Power;
    [Header(" # Power1, Power2, Distance")] public float[] cardPower;   // ���ݷ�, ����, �̵��Ÿ�
    public Sprite cardSprite;   // ī�� �̹���
    public float cardPercent; // ī�� Ȯ��
    public CardRank cardRank; // ī�� ���
    public WeaponType cardWeaponType;

    public enum CardRank
    {
        BaseCards,  // �⺻
        CommonCards,    // �Ϲ�
        RareCards,  // ����
        EpicCards,  // ����
        LegendCards // ����
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