using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CardManager : MonoBehaviour
{
    [Header(" # Card Inform")] public CardInform cardInform;
    [Header(" # Player Scripts")] public Player player;
    private CardData cardData;
    

    // 카드 생성 위치
    [HideInInspector] private Vector3 handCardPos = new Vector3(0, 4.42f, 0);   // 들고 있는 카드 위치
    [HideInInspector] private Vector3 addCardPos = new Vector3(0, 10f, 0);   // 추가할 카드 위치 
    [HideInInspector] private Vector3 spawDeckPos = new Vector3(-3.6f, -3.6f, -3.6f);    // 덱 위치

    private float handCardDistance = 0.8f;  // 손에 있는 카드 간의 거리
    private float addCardDistance = 3f; // 카드 선택창에서의 카드 간의 거리

    [HideInInspector] public List<Card> handCardList = new List<Card>(); // 사용할 수 있는 카드 리스트
    [HideInInspector] public List<Card> addCardList = new List<Card>();   // 추가할 카드 리스트

    // 카드를 담을 부모 오브젝트
    private GameObject deckObject;

    // 생성한 카드 오브젝트를 담을 리스트
    [HideInInspector] public List<GameObject> handCardObject;   // 사용할 카드 오브젝트
    [HideInInspector] public List<GameObject> addCardObject;    // 추가할 카드 오브젝트


    // 손에 들고 있는 카드 개수
    private int handCardCount;

    [Header(" # Card Prefab")]
    [SerializeField] private GameObject handCardPrefab;

    [Header(" # Panel Prefab")]
    [SerializeField] private GameObject addCardPanelPrefab;
    [SerializeField] public GameObject useCardPanelPrefab;
    [SerializeField] public GameObject handCardPanelPrefab;


    [HideInInspector] public bool waitAddCard = false;    // 카드 선택 여부

    // 사용한 카드
    [HideInInspector] public Card useCard = null;


    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        // 카드를 담을 부모 오브젝트 생성
        deckObject = GameObject.Find("Cards");
        if (deckObject == null)
        {
            // 기존의 Cards 오브젝트가 없으면 새로 생성
            deckObject = new GameObject("Cards");
        }
        // 기존의 Cards 오브젝트가 있던 없던 위치를 설정
        deckObject.transform.position = spawDeckPos;
        deckObject.transform.rotation = Quaternion.Euler(0, 45, 0);

        // 초기화
        handCardObject = new List<GameObject>();
        addCardObject = new List<GameObject>();

        handCardCount = cardInform.baseCards.Count;

        addCardPanelPrefab = Instantiate(addCardPanelPrefab, new Vector3(-3, 6f, -3), Quaternion.Euler(0, 45, 0));
        addCardPanelPrefab.SetActive(false);

        useCardPanelPrefab = Instantiate(useCardPanelPrefab, new Vector3(-2, 6, -2), Quaternion.Euler(45, 45, 0));
        useCardPanelPrefab.SetActive(false);

        handCardPanelPrefab = Instantiate(handCardPanelPrefab, new Vector3(-3f, 0.35f, -3f), Quaternion.Euler(0, 45, 0));
    }


    private void Start()
    {
        cardData = FindObjectOfType<CardData>();

        // 기본 카드 생성
        handCardList.AddRange(cardInform.baseCards);   // 값 추가
        CreateCard(handCardList);   // 추가한 값을 가진 Card 생성 
        StartCoroutine(CardSorting(handCardList, handCardObject, handCardPos, handCardDistance));   // 카드 정렬

        handCardCount = handCardList.Count;
    }
    
    // 카드 사용 취소
    public void CardCancle()
    {
        if (useCard != null && cardData.usingCard)
        {
            Card card = useCard;
            GameObject cardObject = addCardObject[addCardObject.Count - 1];

            handCardList.Add(card);
            handCardObject.Add(cardObject);
            addCardList.Remove(card);
            addCardObject.Remove(cardObject);

            ApplyCardInfrom(card, cardObject);
            StartCoroutine(CardSorting(handCardList, handCardObject, handCardPos, handCardDistance));

            player.playerData.activePoint = cardData.TempActivePoint;

            cardData.usingCard = false;
            cardData.waitForInput = false;
            cardData.coroutineStop = true;
            useCard = null;
        }
    }


    // 카드 사용
    public void UpdateCardList(GameObject cardObject)
    {
        int index = handCardObject.IndexOf(cardObject);

        if (index >= 0 && index < handCardList.Count)
        {
            // 선택한 오브젝트가 handCardObject 리스트 안에 있을 때만 해당 카드를 가져옵니다.
            useCard = handCardList[index]; // index에 해당하는 카드를 가져옵니다.

            // 리스트 값 추가 및 제거
            handCardList.RemoveAt(index);
            addCardObject.Add(cardObject);
            handCardObject.RemoveAt(index);

            //cardObject.SetActive(false);
            cardObject.GetComponent<CardMove>().originalPosition = spawDeckPos;
            cardObject.transform.position = spawDeckPos;

            StartCoroutine(CardSorting(handCardList, handCardObject, handCardPos, handCardDistance));
        }
    }


    // 카드 선택
    public void ChoiceCard(GameObject cardObject)
    {
        int index = addCardObject.IndexOf(cardObject);

        if (index >= 0 && index < addCardList.Count)
        {
            Card card = addCardList[index]; // index에 해당하는 카드를 가져옵니다.

            // 리스트 값 추가 및 제거
            handCardList.Add(card);
            addCardList.Clear();
            handCardObject.Add(cardObject);
            addCardObject.RemoveAt(index);

            // 선택받지 못한 오브젝트 비활성화 / 위치 이동
            for (int i = 0; i < addCardObject.Count; i++)
            {
                addCardObject[i].GetComponent<CardMove>().originalPosition = spawDeckPos;
                addCardObject[i].transform.position = spawDeckPos;
                addCardObject[i].SetActive(false);
            }

            StartCoroutine(CardSorting(handCardList, handCardObject, handCardPos, handCardDistance));

            addCardPanelPrefab.SetActive(false);
        }
        waitAddCard = false;
    }


    // 성정한 확률에 따라 카드 리스트를 가져와 그 리스트에 값을 랜덤하게 가져옴
    public Card GetRandomCard()
    {

        waitAddCard = true;

        // 랜덤한 카드 리스트 선택
        List<Card> randomList = null;
        int randNum = UnityEngine.Random.Range(1, 101);
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
        else if (randNum <= cardInform.commonPercent)
        {
            randomList = cardInform.commonCards;
        }
        else
        {
            randomList = cardInform.baseCards;
        }

        // 랜덤한 카드 선택
        return randomList[UnityEngine.Random.Range(0, randomList.Count)];
    
    }


    // 랜덤 카드 생성
    public void CreateRandomCard()
    {
        addCardPanelPrefab.SetActive(true);

        addCardList = new List<Card>();

        HashSet<Card> dedupeCard = new HashSet<Card>();

        while (dedupeCard.Count < 3)
        {
            Card randomCard = GetRandomCard();
            dedupeCard.Add(randomCard); // 중복되면 추가되지 않음
        }

        addCardList = dedupeCard.ToList(); // 중복 없는 카드 목록 생성

        for (int i = 0; i < addCardList.Count; i++)
        {
            addCardObject[i].SetActive(true);
            ApplyCardInfrom(addCardList[i], addCardObject[i]);

            StartCoroutine(CardSorting(addCardList, addCardObject, addCardPos, addCardDistance));
        }

        for (int i = 0; i < 3; i++)
        {
            addCardObject[i].GetComponent<CardOrder>().SetOrder(20);
        }

        handCardCount = handCardList.Count;
    }


    // 카드 생성
    private void CreateCard(List<Card> cards)
    {
        int cardMaxCount = 13;

        // 활성화된 카드 생성
        for (int i = 0 ; i < cards.Count; i++)
        {
            GameObject cardObject = Instantiate(handCardPrefab, Vector3.zero, Quaternion.identity);

            handCardObject.Add(cardObject);
            handCardObject[i].SetActive(false);

            // 생성한 게임 오브젝트에 데이터 적용
            ApplyCardInfrom(cards[i], cardObject);

            cardObject.transform.SetParent(deckObject.transform, false);
        }

        // 비활성화된 카드 오브젝트 생성
        for (int i = 0; i < cardMaxCount - cards.Count; i++)
        {
            GameObject cardObject = Instantiate(handCardPrefab, Vector3.zero, Quaternion.identity);

            addCardObject.Add(cardObject);
            addCardObject[i].SetActive(false);

            cardObject.transform.SetParent(deckObject.transform, false);
        }
    }


    // 카드 정보 적용 (적용할 카드 값, 적용시킬 게임 오브젝트)
    public void ApplyCardInfrom(Card card, GameObject gameObject)
    {
        // gameObject 이름 설정
        gameObject.name = card.cardName;

        Text[] texts = gameObject.GetComponentsInChildren<Text>();
        texts[0].text = card.cardName;
        texts[1].text = card.cardDescription + "\n" + card.cardDescription_Power;

        Image cardimage = gameObject.GetComponentInChildren<Image>();

        if (cardimage != null && card.cardSprite != null)
        {
            cardimage.sprite = card.cardSprite;
        }

        gameObject.GetComponent<CardColorChanger>().ChangeCardColors(card);
    }


    // 카드 정렬 (가져올 카드 리스트, 정렬할 카드 오브젝트, 좌표 값)
    public IEnumerator CardSorting(List<Card> card, List<GameObject> cardObject, Vector3 cardPos, float cardToDistance)
    {
        float totalCardWidth = card.Count * cardToDistance;
        float startingPosX = -totalCardWidth / 2f + cardToDistance / 2f;


        float deltaTime = Time.deltaTime; // deltaTime 한 번만 계산

        for (int i = 0; i < card.Count; i++)
        {
            float elapsedTime = 0f; // 경과 시간
            float duration = 0.2f;  // 이동에 걸리는 시간

            CardOrder cardOrder = cardObject[i].GetComponent<CardOrder>();
            cardOrder.SetOrder(i);
            
            float newPosX = startingPosX + i * cardToDistance;  // 새로운 X 좌표 계산

            // deckObject를 기준으로 로컬 좌표를 계산
            Vector3 targetLocalPosition = new Vector3(newPosX, cardPos.y, cardPos.z);

            // 로컬 좌표를 월드 좌표로 변환
            Vector3 targetPosition = deckObject.transform.TransformPoint(targetLocalPosition);

            // 시작 위치부터 목표 위치까지 서서히 이동
            while (elapsedTime < duration)
            {
                float t = elapsedTime / duration;   // 보간에 사용될 비율 계산
                cardObject[i].transform.position = Vector3.Lerp(cardObject[i].transform.position, targetPosition, t);   // 오브젝트 위치 변경
                elapsedTime += deltaTime;  // 경과 시간 업데이트
                yield return null;
            }
            cardObject[i].SetActive(true);
            cardObject[i].transform.position = targetPosition;  // 목표 위치로 정확히 이동
            cardObject[i].GetComponent<CardMove>().originalPosition = targetPosition;
        }
    }

}