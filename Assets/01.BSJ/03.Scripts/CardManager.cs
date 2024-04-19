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
    public CardInform cardInform;
    
    // ī�� ���� ��ġ
    [HideInInspector] private Vector3 handCardPos = new Vector3(0, 5f, 0);   // ��� �ִ� ī�� ��ġ
    [HideInInspector] private Vector3 addCardPos = new Vector3(0, 10f, 0);   // �߰��� ī�� ��ġ 
    [HideInInspector] private Vector3 spawDeckPos = new Vector3(-3.6f, -3.6f, -3.6f);    // �� ��ġ
    private float handCardDistance = 0.8f;  // �տ� �ִ� ī�� ���� �Ÿ�
    private float addCardDistance = 3f; // ī�� ����â������ ī�� ���� �Ÿ�

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
    [SerializeField] private GameObject addCardPanelPrefab;

    [Header("ī�� Prefab")]
    [SerializeField] private GameObject handCardPrefab;

    [Header("ī�� ��� �г� Prefab")]
    [SerializeField] public GameObject useCardPanelPrefab;

    public bool addCard = false;

    // ����� ī��
    [HideInInspector] public Card useCard;


    private void Awake()
    {
        // ī�带 ���� �θ� ������Ʈ ����
        deckObject = GameObject.Find("Cards");
        if (deckObject == null)
        {
            // ������ Cards ������Ʈ�� ������ ���� ����
            deckObject = new GameObject("Cards");
        }
        // ������ Cards ������Ʈ�� �ִ� ���� ��ġ�� ����
        deckObject.transform.position = spawDeckPos;
        deckObject.transform.rotation = Quaternion.Euler(0, 45, 0);

        // �ʱ�ȭ
        handCardObject = new List<GameObject>();
        addCardObject = new List<GameObject>();

        handCardCount = cardInform.baseCards.Count;

        addCardPanelPrefab = Instantiate(addCardPanelPrefab, new Vector3(-3, 6f, -3), Quaternion.Euler(0, 45, 0));
        addCardPanelPrefab.SetActive(false);

        useCardPanelPrefab = Instantiate(useCardPanelPrefab, new Vector3(-2, 6, -2), Quaternion.Euler(0, 45, 0));
        useCardPanelPrefab.SetActive(false);
    }


    private void Start()
    {
        // �⺻ ī�� ����
        handCardList.AddRange(cardInform.baseCards);   // �� �߰�
        CreateCard(handCardList);   // �߰��� ���� ���� Card ���� 
        StartCoroutine(CardSorting(handCardList, handCardObject, handCardPos, handCardDistance));   // ī�� ����

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
    public void UpdateCardList(GameObject cardObject)
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
            useCard = handCardList[index]; // index�� �ش��ϴ� ī�带 �����ɴϴ�.
             
            // ����Ʈ �� �߰� �� ����
            handCardList.RemoveAt(index);
            addCardObject.Add(cardObject);
            handCardObject.RemoveAt(index);

            cardObject.SetActive(false);
            cardObject.transform.position = Vector3.zero;
        
            StartCoroutine(CardSorting(handCardList, handCardObject, handCardPos, handCardDistance));

            addCardPanelPrefab.SetActive(false);
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

            // ����Ʈ �� �߰� �� ����
            handCardList.Add(card);
            addCardList.Clear();
            handCardObject.Add(cardObject);
            addCardObject.RemoveAt(index);

            // ���ù��� ���� ������Ʈ ��Ȱ��ȭ / ��ġ �̵�
            for (int i = 0; i < addCardObject.Count; i++)
            {
                addCardObject[i].SetActive(false);
                addCardObject[i].transform.position = spawDeckPos;
            }

            StartCoroutine(CardSorting(handCardList, handCardObject, handCardPos, handCardDistance));

            addCardPanelPrefab.SetActive(false);
        }
    }


    // ������ Ȯ���� ���� ī�� ����Ʈ�� ������ �� ����Ʈ�� ���� �����ϰ� ������
    private Card GetRandomCard()
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


    // ���� ī�� ����
    private void CreateRandomCard()
    {
        addCardPanelPrefab.SetActive(true);

        addCardList = new List<Card>();

        HashSet<Card> dedupeCard = new HashSet<Card>();

        while (dedupeCard.Count < 3)
        {
            Card randomCard = GetRandomCard();
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
        int cardMaxCount = 13;

        // Ȱ��ȭ�� ī�� ����
        for (int i = 0 ; i < cards.Count; i++)
        {
            GameObject cardObject = Instantiate(handCardPrefab, Vector3.zero, Quaternion.identity);

            handCardObject.Add(cardObject);
            handCardObject[i].SetActive(true);

            // ������ ���� ������Ʈ�� ������ ����
            ApplyCardInfrom(cards[i], cardObject);

            cardObject.transform.SetParent(deckObject.transform, false);
        }

        // ��Ȱ��ȭ�� ī�� ������Ʈ ����
        for (int i = 0; i < cardMaxCount - cards.Count; i++)
        {
            GameObject cardObject = Instantiate(handCardPrefab, Vector3.zero, Quaternion.identity);

            addCardObject.Add(cardObject);
            addCardObject[i].SetActive(false);

            cardObject.transform.SetParent(deckObject.transform, false);
        }
    }


    // ī�� ���� ���� (������ ī�� ��, �����ų ���� ������Ʈ)
    public void ApplyCardInfrom(Card card, GameObject gameObject)
    {
        CardOrder cardOrder = gameObject.AddComponent<CardOrder>();

        // gameObject �̸� ����
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


    // ī�� ���� (������ ī�� ����Ʈ, ������ ī�� ������Ʈ, ��ǥ ��)
    public IEnumerator CardSorting(List<Card> card, List<GameObject> cardObject, Vector3 cardPos, float cardToDistance)
    {
        float totalCardWidth = card.Count * cardToDistance;
        float startingPosX = -totalCardWidth / 2f + cardToDistance / 2f;


        float deltaTime = Time.deltaTime; // deltaTime �� ���� ���

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
                elapsedTime += deltaTime;  // ��� �ð� ������Ʈ
                yield return null;
            }

            cardObject[i].transform.position = targetPosition;  // ��ǥ ��ġ�� ��Ȯ�� �̵�
            cardObject[i].GetComponent<CardMove>().originalPosition = targetPosition;

        }
    }

}