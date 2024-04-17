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

    // ī�� Ȯ�� ��
    public int commonPercent = 50;
    public int rarePercent = 30;
    public int epicPercent = 15;
    public int legendPercent = 5;

    // ���� ����
    public Color commonColor = Color.green;
    public Color rareColor = Color.blue;
    public Color epicColor = Color.red;
    public Color legendColor = Color.yellow;

    // Ȯ�� �� ����
    private void OnValidate()
    {
        FixCardPersent(commonCards, commonPercent, commonColor);
        FixCardPersent(rareCards, rarePercent, rareColor);
        FixCardPersent(epicCards, epicPercent, epicColor);
        FixCardPersent(legendCards, legendPercent, legendColor);        
    }

    // ����Ʈ�� �ִ� ī����� percent�� ���ϴ� ������ ����
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
    public string cardName; // ī�� �̸�
    public string cardDescription_1;  // ī�� ���� 1
    public int power;   // ���ݷ�, ����, �̵��Ÿ�
    public string cardDescription_2;  // ī�� ���� 2
    public Sprite cardSprite;   // ī�� �̹���
    public float percent; // ī�� Ȯ��
    public CardType cardType; // ī�� Ÿ�� (���ݱ�, ȸ����, �̵���)
    public Color color; // ī�� ����

    public enum CardType
    {
        Attack, // ����
        Heal,   // ȸ��
        Movement // �̵�
    }
}