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
    private CardInform cardInform;
    private CardManager cardManager;

    // 카드 생성 위치
    [HideInInspector] public Vector3 handCardPos = new Vector3(0, 5f, 0);
    [HideInInspector] public Vector3 addCardPos = new Vector3(0, 11f, 0);
    [HideInInspector] public Vector3 spawDeckPos = new Vector3(-3.6f, -5f, -3.6f);
    private float handCardDistance = 0.8f;
    private float addCardDistance = 3f;

    private CardData cardData;

    [SerializeField] public List<Card> handCardList = new List<Card>(); // 사용할 수 있는 카드 리스트
    [SerializeField] public List<Card> addCardList = new List<Card>();   // 추가할 카드 리스트

    // 카드를 담을 부모 오브젝트
    private GameObject deckObject;

    // 생성한 카드 오브젝트를 담을 리스트
    [SerializeField] public List<GameObject> handCardObject;   // 사용할 카드 오브젝트
    [SerializeField] public List<GameObject> addCardObject;    // 추가할 카드 오브젝트


    // 손에 들고 있는 카드 개수
    private int handCardCount;

    [Header("카드 얻을 때 배경 Prefab")]
    [SerializeField] private GameObject panelObject;

    [Header("카드 Prefab")]
    [SerializeField] private GameObject handCardPrefab;

    public bool addCard = false;


    private void Awake()
    {
        // 카드를 담을 부모 오브젝트 생성
        deckObject = new GameObject("Cards");
        deckObject.transform.position = spawDeckPos;
        deckObject.transform.rotation = Quaternion.Euler(0, 45, 0);

        // 각 컴포넌트를 가진 게임 오브젝트를 찾아서 참조
        cardData = FindObjectOfType<CardData>();
        cardManager = FindObjectOfType<CardManager>();

        // 초기화
        handCardObject = new List<GameObject>();
        addCardObject = new List<GameObject>();

        handCardCount = cardData.baseCardList.Count;
    }


    private void Start()
    {
        // 기본 카드 생성
        handCardList.AddRange(cardData.baseCardList);   // 값 추가
        CreateCard(handCardList);   // 추가한 값을 가진 Card 생성 
        
        // 카드 정렬
        StartCoroutine(CardSorting(handCardList, handCardObject, handCardPos, handCardDistance));

        handCardCount = handCardList.Count;
    }
    

    private void Update()
    {
        if (addCard)
        {
            CreateRandomCard();
            addCard = false;
        }
    }


    // 카드 사용
    public void UseToCard(GameObject cardObject)
    {
        // handCardObject 리스트를 순회하면서 선택한 cardObject가 있는지 확인합니다.
        bool isFound = false;
        int index = -1;
        for (int i = 0; i < handCardObject.Count; i++)
        {
            if (handCardObject[i] == cardObject)
            {
                isFound = true;
                index = i;
                break;
            }
        }

        if (isFound)
        {
            // 선택한 오브젝트가 handCardObject 리스트 안에 있을 때만 해당 카드를 가져옵니다.
            Card card = handCardList[index]; // index에 해당하는 카드를 가져옵니다.

            addCardList.Add(card);
            handCardList.RemoveAt(index); // index에 해당하는 카드를 리스트에서 제거합니다.
            addCardObject.Add(cardObject);
            handCardObject.RemoveAt(index); // index에 해당하는 오브젝트를 리스트에서 제거합니다.


            gameObject.SetActive(false);
            gameObject.transform.position = spawDeckPos;
        
            StartCoroutine(CardSorting(handCardList, handCardObject, handCardPos, handCardDistance));

            panelObject.SetActive(false);
        }
    }


    // 카드 선택
    public void ChoiceCard(GameObject cardObject)
    {
        // addCardObject 리스트를 순회하면서 선택한 cardObject가 있는지 확인합니다.
        bool isFound = false;
        int index = -1;
        for (int i = 0; i < addCardObject.Count; i++)
        {
            if (addCardObject[i] == cardObject)
            {
                isFound = true;
                index = i;
                break;
            }
        }

        if (isFound)
        {
            // 선택한 오브젝트가 addCardObject 리스트 안에 있을 때만 해당 카드를 가져옵니다.
            Card card = addCardList[index]; // index에 해당하는 카드를 가져옵니다.

            handCardList.Add(card);
            addCardList.RemoveAt(index); // index에 해당하는 카드를 리스트에서 제거합니다.
            handCardObject.Add(cardObject);
            addCardObject.RemoveAt(index); // index에 해당하는 오브젝트를 리스트에서 제거합니다.

            // 선택받지 못한 오브젝트 비활성화 / 위치 이동
            for (int i = 0; i < addCardObject.Count; i++)
            {
                addCardObject[i].SetActive(false);
                addCardObject[i].transform.position = spawDeckPos;
            }

            StartCoroutine(CardSorting(handCardList, handCardObject, handCardPos, handCardDistance));

            panelObject.SetActive(false);
        }
    }


    // 랜덤 카드 생성
    private void CreateRandomCard()
    {
        panelObject.SetActive(true);

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
            
            StartCoroutine(CardSorting(addCardList, addCardObject, addCardPos, addCardDistance));
        }

        // Order In Layer 값 변경 (왜 안돼!!!!)
        for (int i = 0; i < 3; i++)
        {
            addCardObject[i].GetComponent<CardOrder>().SetOrder(20);
        }

        handCardCount = handCardList.Count;
    }


    // 카드 생성
    private void CreateCard(List<Card> cards)
    {
        panelObject = Instantiate(panelObject, new Vector3(-3, 5.5f, -3), Quaternion.Euler(0, 45, 0));
        panelObject.SetActive(false);
        panelObject.GetComponent<CardOrder>().SetOrder(19);


        int cardMaxCount = 13;

        // 활성화된 카드 생성
        for (int i = 0 ; i < cards.Count; i++)
        {
            GameObject cardObject = Instantiate(handCardPrefab, Vector3.zero, Quaternion.identity);

            cardObject.transform.SetParent(deckObject.transform, false);

            handCardObject.Add(cardObject);
            handCardObject[i].SetActive(true);

            // 생성한 게임 오브젝트에 데이터 적용
            ApplyCardInfrom(cards[i], cardObject);
            
        }

        // 비활성화된 카드 오브젝트 생성
        for (int i = 0; i < cardMaxCount - cards.Count; i++)
        {
            GameObject cardObject = Instantiate(handCardPrefab, Vector3.zero, Quaternion.identity);
            cardObject.transform.SetParent(deckObject.transform, false);

            addCardObject.Add(cardObject);
            addCardObject[i].SetActive(false);
        }
    }


    // 카드 정보 적용 (적용할 카드 값, 적용시킬 게임 오브젝트)
    public void ApplyCardInfrom(Card card, GameObject gameObject)
    {
        CardOrder cardOrder = gameObject.AddComponent<CardOrder>();

        gameObject.name = card.cardName;

        string cardEffect;
        if (card.cardType == Card.CardType.Attack)
        {
            cardEffect = "(데미지: ";
        }
        else if (card.cardType == Card.CardType.Heal)
        {
            cardEffect = "(회복량: ";
        }
        else if (card.cardType == Card.CardType.Movement)
        {
            cardEffect = "(이동 칸: ";
        }
        else
        {
            cardEffect = "(피해 감소: -";
        }

        Text[] texts = gameObject.GetComponentsInChildren<Text>();
        texts[0].text = card.cardName;
        texts[1].text = $"{ card.cardDescription}\n{cardEffect}{card.cardPower})";

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

        // 부모 오브젝트를 기준으로 좌표를 변환
        Vector3 parentPosition = deckObject.transform.position;

        for (int i = 0; i < card.Count; i++)
        {
            float elapsedTime = 0f; // 경과 시간
            float duration = 0.2f;  // 이동에 걸리는 시간

            cardObject[i].GetComponent<CardOrder>().SetOrder(i);

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
                elapsedTime += Time.deltaTime;  // 경과 시간 업데이트
                yield return null;
            }

            cardObject[i].transform.position = targetPosition;  // 목표 위치로 정확히 이동
            cardObject[i].GetComponent<CardMove>().originalPosition = targetPosition;

        }
    }

}