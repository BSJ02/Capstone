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
        baseCardList = new List<Card>();  // 기본적인 카드 리스트
        addCardList = new List<Card>(); // 나중에 추가할 카드 리스트

        cardInform = Resources.Load<CardInform>("Cards");

        baseCardList.AddRange(cardInform.baseCards);
        addCardList.AddRange(cardInform.commonCards);
        addCardList.AddRange(cardInform.rareCards);
        addCardList.AddRange(cardInform.epicCards);
        addCardList.AddRange(cardInform.legendCards);
    }

    

}