using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class CardData : MonoBehaviour
{
    private CardInform cardInform;

    [SerializeField] public List<Card> baseCardList;
    [SerializeField] public List<Card> cardList;

    private void Awake()
    {
        baseCardList = new List<Card>();  // �⺻���� ī�� ����Ʈ
        cardList = new List<Card>(); // ���߿� �߰��� ī�� ����Ʈ

        cardInform = Resources.Load<CardInform>("Cards");

        // �⺻���� ī�� �߰�
        baseCardList.AddRange(cardInform.baseCards);

        // ������ ī��
        cardList.AddRange(cardInform.commonCards);
        cardList.AddRange(cardInform.rareCards);
        cardList.AddRange(cardInform.epicCards);
        cardList.AddRange(cardInform.legendCards);

    }

    // ������ Ȯ���� ���� ī�� ����Ʈ�� ������ �� ����Ʈ�� ���� �����ϰ� ������
    public Card GetRandomCard()
    {
        // ������ ī�� ����Ʈ ����
        List<Card> randomList = null;
        int randNum = Random.Range(1, 101);
        if (randNum <= cardInform.legendPercent)
        {
            randomList = cardInform.legendCards;
        }
        else if (randNum <= cardInform.epicPercent)
        {
            randomList = cardInform.epicCards;
        }
        else if (randNum <= cardInform.rarePercent)
        {
            randomList = cardInform.rareCards;
        }
        else
        {
            randomList = cardInform.commonCards;
        }
        
        // ������ ī�� ����
        return randomList[Random.Range(0, randomList.Count)];
    }

}