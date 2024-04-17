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
        baseCardList = new List<Card>();  // 기본적인 카드 리스트
        cardList = new List<Card>(); // 나중에 추가할 카드 리스트

        cardInform = Resources.Load<CardInform>("Cards");

        // 기본적인 카드 추가
        baseCardList.AddRange(cardInform.baseCards);

        // 나머지 카드
        cardList.AddRange(cardInform.commonCards);
        cardList.AddRange(cardInform.rareCards);
        cardList.AddRange(cardInform.epicCards);
        cardList.AddRange(cardInform.legendCards);

    }

    // 성정한 확률에 따라 카드 리스트를 가져와 그 리스트에 값을 랜덤하게 가져옴
    public Card GetRandomCard()
    {
        // 랜덤한 카드 리스트 선택
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
        
        // 랜덤한 카드 선택
        return randomList[Random.Range(0, randomList.Count)];
    }

}