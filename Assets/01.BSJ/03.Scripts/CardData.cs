using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class CardData : MonoBehaviour
{
    private CardInform cardInform;

    [SerializeField] public List<Card> baseCardList;
    [SerializeField] public List<Card> addCardList;

    private void Awake()
    {
        baseCardList = new List<Card>();  // �⺻���� ī�� ����Ʈ
        addCardList = new List<Card>(); // ���߿� �߰��� ī�� ����Ʈ

        cardInform = Resources.Load<CardInform>("Cards");

        baseCardList.AddRange(cardInform.baseCards);
        addCardList.AddRange(cardInform.commonCards);
        addCardList.AddRange(cardInform.rareCards);
        addCardList.AddRange(cardInform.epicCards);
        addCardList.AddRange(cardInform.legendCards);
    }

    

}