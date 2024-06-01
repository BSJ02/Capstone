using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Scriptable Object/Cards")]
public class CardInform : ScriptableObject
{
    [Header(" # ī�� ����")]
    [SerializeField] public List<Card> baseCards;
    [SerializeField] public List<Card> warriorCards;
    [SerializeField] public List<Card> archerCards;
    [SerializeField] public List<Card> wizardCards;

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
        FixCardPercent(warriorCards, commonPercent);
        FixCardPercent(archerCards, rarePercent);
        FixCardPercent(wizardCards, epicPercent);

        ApplyCardRank(baseCards, Card.CardType.BaseCard);
        ApplyCardRank(warriorCards, Card.CardType.WarriorCard);
        ApplyCardRank(archerCards, Card.CardType.ArcherCard);
        ApplyCardRank(wizardCards, Card.CardType.WizardCard);
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
    public string cardName; // ī�� �̸�
    public string cardDescription;  // ī�� ����
    public string cardDescription_Power;
    public float[] cardPower;   // ���ݷ�, ����
    public float cardDistance;  // �̵��Ÿ�
    public Sprite cardSprite;   // ī�� �̹���
    public float cardPercent; // ī�� Ȯ��
    public CardType cardType; // ī�� ���
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