using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CardManager : MonoBehaviour
{
    // 카드 생성 위치
    Vector3 handCardPos = new Vector3(0, -3, 0);
    Vector3 addCardPos = new Vector3(0, 2, 0);
    Vector3 spawPos = new Vector3(0, -10, 0);
    public Vector3 orginPos;
    float handCardDistance = 0.8f;
    float addCardDistance = 3f;

    private CardData cardData;
    private CardInform cardInform;

    [SerializeField] public List<Card> handCardList = new List<Card>(); // 사용할 수 있는 카드 리스트
    [SerializeField] public List<Card> addCardList = new List<Card>();   // 추가할 카드 리스트

    // 카드를 담을 부모 오브젝트
    GameObject deckObject;

    // 생성한 카드 오브젝트를 담을 리스트
    [SerializeField] List<GameObject> handCardObject;   // 사용할 카드 오브젝트
    [SerializeField] List<GameObject> addCardObject;    // 추가할 카드 오브젝트


    // 손에 들고 있는 카드 개수
    private int handCardCount;

    [Header("카드 얻을 때 배경 Prefab")]
    [SerializeField] GameObject panelObject;

    [Header("카드 Prefab")]
    [SerializeField] GameObject handCardPrefab;

    public bool press = false;
    bool isSortingInProgress = false;



    private void Start()
    {
        // 카드를 담을 부모 오브젝트 생성
        deckObject = new GameObject("Cards");

        // 각 컴포넌트를 가진 게임 오브젝트를 찾아서 참조
        cardData = FindObjectOfType<CardData>();

        // 초기화
        handCardObject = new List<GameObject>();
        addCardObject = new List<GameObject>();

        // cardData가 null인지 확인
        if (cardData == null)
        {
            Debug.LogError("CardData    컴포넌트를 찾을 수 없음");
        }

        handCardCount = cardData.baseCardList.Count;

        // 기본 카드 생성
        handCardList.AddRange(cardData.baseCardList);   // 값 추가
        CreateCard(handCardList);   // 추가한 값을 가진 Card 생성 
        // 카드 정렬
        isSortingInProgress = true;
        StartCoroutine(CardSorting(handCardList, handCardObject, handCardPos, handCardDistance));

        handCardCount = handCardList.Count;

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            press = true;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            isSortingInProgress = true;
            StartCoroutine(CardSorting(handCardList, handCardObject, handCardPos, handCardDistance));
        }

        if (press)
        {
            CreateRandomCard();
            press = false;
        }

        if (Input.GetKeyDown("1"))
        {
            ChoiceCard(addCardList[0], addCardObject[0]);
        }

    }

    // 카드 선택
    void ChoiceCard(Card card, GameObject cardObject)
    {
        handCardList.Add(card);
        addCardList.RemoveAt(0);
        handCardObject.Add(cardObject);
        addCardObject.RemoveAt(0);

        for (int i = 0; i <= addCardList.Count; i++)
        {
            addCardObject[i].SetActive(false);
            addCardObject[i].transform.position = spawPos;
        }

        isSortingInProgress = true;
        StartCoroutine(CardSorting(handCardList, handCardObject, handCardPos, handCardDistance));

        panelObject.SetActive(false);
    }

    // 랜덤 카드 생성
    void CreateRandomCard()
    {
        //panelObject.SetActive(true);

        addCardList = new List<Card>();

        HashSet<Card> dedupeCard = new HashSet<Card>();

        while (dedupeCard.Count < 3)
        {
            Card randomCard = cardData.GetRandomCard();
            dedupeCard.Add(randomCard); // 중복되면 추가되지 않음
        }

        addCardList = dedupeCard.ToList(); // 중복 없는 카드 목록 생성

        for (int i = 0; i < addCardList.Count; i++)
        {
            addCardObject[i].SetActive(true);
            ApplyCardInfrom(addCardList[i], addCardObject[i]);

            isSortingInProgress = true;
            StartCoroutine(CardSorting(addCardList, addCardObject, addCardPos, addCardDistance));
        }

        for (int i = 0; i < 3; i++)
        {
            addCardObject[i].GetComponent<CardOrder>().SetOrder(20);
        }

        handCardCount = handCardList.Count;
    }

    // 카드 생성
    void CreateCard(List<Card> cards)
    {
        panelObject = Instantiate(panelObject, new Vector3(0, 2.25f, 0), Quaternion.identity);
        panelObject.SetActive(false);
        panelObject.GetComponent<CardOrder>().SetOrder(19);


        int cardMaxCount = 13;

        for (int i = 0 ; i < cards.Count; i++)
        {
            // 게임 오브젝트 생성
            GameObject cardObject = Instantiate(handCardPrefab, spawPos, Quaternion.identity);

            cardObject.transform.SetParent(deckObject.transform, false);

            handCardObject.Add(cardObject);
            handCardObject[i].SetActive(true);

            // 생성한 게임 오브젝트에 데이터 적용
            ApplyCardInfrom(cards[i], cardObject);

        }

        for (int i = 0; i < cardMaxCount - cards.Count; i++)
        {
            GameObject cardObject = Instantiate(handCardPrefab, spawPos, Quaternion.identity);
            cardObject.transform.SetParent(deckObject.transform, false);

            addCardObject.Add(cardObject);
            addCardObject[i].SetActive(false);
        }
    }

    // 카드 정보 적용 (적용할 카드 값, 적용시킬 게임 오브젝트)
    public void ApplyCardInfrom(Card card, GameObject gameObject)
    {
        CardOrder cardOrder = new CardOrder();

        gameObject.name = card.cardName;

        Text[] texts = gameObject.GetComponentsInChildren<Text>();
        texts[0].text = card.cardName;
        texts[1].text = card.cardDescription_1 + " " + card.power.ToString() + card.cardDescription_2;

        Image cardimage = gameObject.GetComponentInChildren<Image>();

        if (cardimage != null && card.cardSprite != null)
        {
            cardimage.sprite = card.cardSprite;
        }

        //Renderer backRenderer = gameObject.GetComponent<Renderer>();
        //Renderer[] renderers = gameObject.GetComponentsInChildren<Renderer>();

        //if (cardInform.commonCards.Contains(card))
        //{
        //    renderers[0].material.color = Color.red;
        //    renderers[1].material.color = Color.red;
        //}
    }

    // 카드 정렬 (가져올 카드 리스트, 정렬할 카드 오브젝트, 좌표 값)
    IEnumerator CardSorting(List<Card> card, List<GameObject> cardObject, Vector3 cardPos, float cardToDistance)
    {
        float totalCardWidth = card.Count * cardToDistance;
        float startingPosX = -totalCardWidth / 2f + cardToDistance / 2f;


        for (int i = 0; i < card.Count; i++)
        {
            float elapsedTime = 0f; // 경과 시간
            float duration = 0.15f;  // 이동에 걸리는 시간

            cardObject[i].GetComponent<CardOrder>().SetOrder(i);

            float newPosX = startingPosX + i * cardToDistance;  // 새로운 X 좌표 계산

            Vector3 targetPosition = new Vector3(newPosX, cardPos.y, 0f);   // 목표 위치 지정

            // 시작 위치부터 목표 위치까지 서서히 이동
            while (elapsedTime < duration)
            {
                float t = elapsedTime / duration;   // 보간에 사용될 비율 계산
                cardObject[i].transform.position = Vector3.Lerp(cardObject[i].transform.position, targetPosition, t);   // 오브젝트 위치 변경
                elapsedTime += Time.deltaTime;  // 경과 시간 업데이트
                yield return null;
            }

            cardObject[i].transform.position = targetPosition;  // 목표 위치로 정확히 이동
            orginPos = targetPosition;

            isSortingInProgress = false;
        }

    }

    // 카드 사용
    void UseCard(Card card)
    {
        int power = card.power;

        if (card.cardType == Card.CardType.Attack)
        {
            // 공격 타입의 카드 처리
            
            // 몬스터 체력 - power

        }
        else if (card.cardType == Card.CardType.Heal)
        {
            // 회복 타입의 카드 처리

            // 플레이어 체력 + power
        }
        else if (card.cardType == Card.CardType.Movement)
        {
            // 이동 타입의 카드 처리

            // 이동 거리 설정 / 이동하는 함수 다시 호출
        }
        else
        {
            // 다른 타입의 카드 처리
        }
    }
    
}