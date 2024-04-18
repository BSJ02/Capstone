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

    // ī�� ���� ��ġ
    [HideInInspector] public Vector3 handCardPos = new Vector3(0, 5f, 0);
    [HideInInspector] public Vector3 addCardPos = new Vector3(0, 11f, 0);
    [HideInInspector] public Vector3 spawDeckPos = new Vector3(-3.6f, -5f, -3.6f);
    private float handCardDistance = 0.8f;
    private float addCardDistance = 3f;

    private CardData cardData;

    [SerializeField] public List<Card> handCardList = new List<Card>(); // ����� �� �ִ� ī�� ����Ʈ
    [SerializeField] public List<Card> addCardList = new List<Card>();   // �߰��� ī�� ����Ʈ

    // ī�带 ���� �θ� ������Ʈ
    private GameObject deckObject;

    // ������ ī�� ������Ʈ�� ���� ����Ʈ
    [SerializeField] public List<GameObject> handCardObject;   // ����� ī�� ������Ʈ
    [SerializeField] public List<GameObject> addCardObject;    // �߰��� ī�� ������Ʈ


    // �տ� ��� �ִ� ī�� ����
    private int handCardCount;

    [Header("ī�� ���� �� ��� Prefab")]
    [SerializeField] private GameObject panelObject;

    [Header("ī�� Prefab")]
    [SerializeField] private GameObject handCardPrefab;

    public bool addCard = false;


    private void Awake()
    {
        // ī�带 ���� �θ� ������Ʈ ����
        deckObject = new GameObject("Cards");
        deckObject.transform.position = spawDeckPos;
        deckObject.transform.rotation = Quaternion.Euler(0, 45, 0);

        // �� ������Ʈ�� ���� ���� ������Ʈ�� ã�Ƽ� ����
        cardData = FindObjectOfType<CardData>();
        cardManager = FindObjectOfType<CardManager>();

        // �ʱ�ȭ
        handCardObject = new List<GameObject>();
        addCardObject = new List<GameObject>();

        handCardCount = cardData.baseCardList.Count;
    }


    private void Start()
    {
        // �⺻ ī�� ����
        handCardList.AddRange(cardData.baseCardList);   // �� �߰�
        CreateCard(handCardList);   // �߰��� ���� ���� Card ���� 
        
        // ī�� ����
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


    // ī�� ���
    public void UseToCard(GameObject cardObject)
    {
        // handCardObject ����Ʈ�� ��ȸ�ϸ鼭 ������ cardObject�� �ִ��� Ȯ���մϴ�.
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
            // ������ ������Ʈ�� handCardObject ����Ʈ �ȿ� ���� ���� �ش� ī�带 �����ɴϴ�.
            Card card = handCardList[index]; // index�� �ش��ϴ� ī�带 �����ɴϴ�.

            addCardList.Add(card);
            handCardList.RemoveAt(index); // index�� �ش��ϴ� ī�带 ����Ʈ���� �����մϴ�.
            addCardObject.Add(cardObject);
            handCardObject.RemoveAt(index); // index�� �ش��ϴ� ������Ʈ�� ����Ʈ���� �����մϴ�.


            gameObject.SetActive(false);
            gameObject.transform.position = spawDeckPos;
        
            StartCoroutine(CardSorting(handCardList, handCardObject, handCardPos, handCardDistance));

            panelObject.SetActive(false);
        }
    }


    // ī�� ����
    public void ChoiceCard(GameObject cardObject)
    {
        // addCardObject ����Ʈ�� ��ȸ�ϸ鼭 ������ cardObject�� �ִ��� Ȯ���մϴ�.
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
            // ������ ������Ʈ�� addCardObject ����Ʈ �ȿ� ���� ���� �ش� ī�带 �����ɴϴ�.
            Card card = addCardList[index]; // index�� �ش��ϴ� ī�带 �����ɴϴ�.

            handCardList.Add(card);
            addCardList.RemoveAt(index); // index�� �ش��ϴ� ī�带 ����Ʈ���� �����մϴ�.
            handCardObject.Add(cardObject);
            addCardObject.RemoveAt(index); // index�� �ش��ϴ� ������Ʈ�� ����Ʈ���� �����մϴ�.

            // ���ù��� ���� ������Ʈ ��Ȱ��ȭ / ��ġ �̵�
            for (int i = 0; i < addCardObject.Count; i++)
            {
                addCardObject[i].SetActive(false);
                addCardObject[i].transform.position = spawDeckPos;
            }

            StartCoroutine(CardSorting(handCardList, handCardObject, handCardPos, handCardDistance));

            panelObject.SetActive(false);
        }
    }


    // ���� ī�� ����
    private void CreateRandomCard()
    {
        panelObject.SetActive(true);

        addCardList = new List<Card>();

        HashSet<Card> dedupeCard = new HashSet<Card>();

        while (dedupeCard.Count < 3)
        {
            Card randomCard = cardData.GetRandomCard();
            dedupeCard.Add(randomCard); // �ߺ��Ǹ� �߰����� ����
        }

        addCardList = dedupeCard.ToList(); // �ߺ� ���� ī�� ��� ����

        for (int i = 0; i < addCardList.Count; i++)
        {
            addCardObject[i].SetActive(true);
            ApplyCardInfrom(addCardList[i], addCardObject[i]);
            
            StartCoroutine(CardSorting(addCardList, addCardObject, addCardPos, addCardDistance));
        }

        // Order In Layer �� ���� (�� �ȵ�!!!!)
        for (int i = 0; i < 3; i++)
        {
            addCardObject[i].GetComponent<CardOrder>().SetOrder(20);
        }

        handCardCount = handCardList.Count;
    }


    // ī�� ����
    private void CreateCard(List<Card> cards)
    {
        panelObject = Instantiate(panelObject, new Vector3(-3, 5.5f, -3), Quaternion.Euler(0, 45, 0));
        panelObject.SetActive(false);
        panelObject.GetComponent<CardOrder>().SetOrder(19);


        int cardMaxCount = 13;

        // Ȱ��ȭ�� ī�� ����
        for (int i = 0 ; i < cards.Count; i++)
        {
            GameObject cardObject = Instantiate(handCardPrefab, Vector3.zero, Quaternion.identity);

            cardObject.transform.SetParent(deckObject.transform, false);

            handCardObject.Add(cardObject);
            handCardObject[i].SetActive(true);

            // ������ ���� ������Ʈ�� ������ ����
            ApplyCardInfrom(cards[i], cardObject);
            
        }

        // ��Ȱ��ȭ�� ī�� ������Ʈ ����
        for (int i = 0; i < cardMaxCount - cards.Count; i++)
        {
            GameObject cardObject = Instantiate(handCardPrefab, Vector3.zero, Quaternion.identity);
            cardObject.transform.SetParent(deckObject.transform, false);

            addCardObject.Add(cardObject);
            addCardObject[i].SetActive(false);
        }
    }


    // ī�� ���� ���� (������ ī�� ��, �����ų ���� ������Ʈ)
    public void ApplyCardInfrom(Card card, GameObject gameObject)
    {
        CardOrder cardOrder = gameObject.AddComponent<CardOrder>();

        gameObject.name = card.cardName;

        string cardEffect;
        if (card.cardType == Card.CardType.Attack)
        {
            cardEffect = "(������: ";
        }
        else if (card.cardType == Card.CardType.Heal)
        {
            cardEffect = "(ȸ����: ";
        }
        else if (card.cardType == Card.CardType.Movement)
        {
            cardEffect = "(�̵� ĭ: ";
        }
        else
        {
            cardEffect = "(���� ����: -";
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


    // ī�� ���� (������ ī�� ����Ʈ, ������ ī�� ������Ʈ, ��ǥ ��)
    public IEnumerator CardSorting(List<Card> card, List<GameObject> cardObject, Vector3 cardPos, float cardToDistance)
    {
        float totalCardWidth = card.Count * cardToDistance;
        float startingPosX = -totalCardWidth / 2f + cardToDistance / 2f;

        // �θ� ������Ʈ�� �������� ��ǥ�� ��ȯ
        Vector3 parentPosition = deckObject.transform.position;

        for (int i = 0; i < card.Count; i++)
        {
            float elapsedTime = 0f; // ��� �ð�
            float duration = 0.2f;  // �̵��� �ɸ��� �ð�

            cardObject[i].GetComponent<CardOrder>().SetOrder(i);

            float newPosX = startingPosX + i * cardToDistance;  // ���ο� X ��ǥ ���

            // deckObject�� �������� ���� ��ǥ�� ���
            Vector3 targetLocalPosition = new Vector3(newPosX, cardPos.y, cardPos.z);

            // ���� ��ǥ�� ���� ��ǥ�� ��ȯ
            Vector3 targetPosition = deckObject.transform.TransformPoint(targetLocalPosition);

            // ���� ��ġ���� ��ǥ ��ġ���� ������ �̵�
            while (elapsedTime < duration)
            {
                float t = elapsedTime / duration;   // ������ ���� ���� ���
                cardObject[i].transform.position = Vector3.Lerp(cardObject[i].transform.position, targetPosition, t);   // ������Ʈ ��ġ ����
                elapsedTime += Time.deltaTime;  // ��� �ð� ������Ʈ
                yield return null;
            }

            cardObject[i].transform.position = targetPosition;  // ��ǥ ��ġ�� ��Ȯ�� �̵�
            cardObject[i].GetComponent<CardMove>().originalPosition = targetPosition;

        }
    }

}