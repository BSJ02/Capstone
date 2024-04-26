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
    

    // ī�� ���� ��ġ
    [HideInInspector] private Vector3 handCardPos = new Vector3(0, 4.42f, 0);   // ��� �ִ� ī�� ��ġ
    [HideInInspector] private Vector3 addCardPos = new Vector3(0, 10f, 0);   // �߰��� ī�� ��ġ 
    [HideInInspector] private Vector3 spawDeckPos = new Vector3(-3.6f, -3.6f, -3.6f);    // �� ��ġ

    private float handCardDistance = 0.8f;  // �տ� �ִ� ī�� ���� �Ÿ�
    private float addCardDistance = 3f; // ī�� ����â������ ī�� ���� �Ÿ�

    [HideInInspector] public List<Card> handCardList = new List<Card>(); // ����� �� �ִ� ī�� ����Ʈ
    [HideInInspector] public List<Card> addCardList = new List<Card>();   // �߰��� ī�� ����Ʈ

    // ī�带 ���� �θ� ������Ʈ
    private GameObject deckObject;

    // ������ ī�� ������Ʈ�� ���� ����Ʈ
    [HideInInspector] public List<GameObject> handCardObject;   // ����� ī�� ������Ʈ
    [HideInInspector] public List<GameObject> addCardObject;    // �߰��� ī�� ������Ʈ


    // �տ� ��� �ִ� ī�� ����
    private int handCardCount;

    [Header(" # Card Prefab")]
    [SerializeField] private GameObject handCardPrefab;

    [Header(" # Panel Prefab")]
    [SerializeField] private GameObject addCardPanelPrefab;
    [SerializeField] public GameObject useCardPanelPrefab;
    [SerializeField] public GameObject handCardPanelPrefab;


    [HideInInspector] public bool waitAddCard = false;    // ī�� ���� ����

    // ����� ī��
    [HideInInspector] public Card useCard = null;


    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

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

        useCardPanelPrefab = Instantiate(useCardPanelPrefab, new Vector3(-2, 6, -2), Quaternion.Euler(45, 45, 0));
        useCardPanelPrefab.SetActive(false);

        handCardPanelPrefab = Instantiate(handCardPanelPrefab, new Vector3(-3f, 0.35f, -3f), Quaternion.Euler(0, 45, 0));
    }


    private void Start()
    {
        cardData = FindObjectOfType<CardData>();

        // �⺻ ī�� ����
        handCardList.AddRange(cardInform.baseCards);   // �� �߰�
        CreateCard(handCardList);   // �߰��� ���� ���� Card ���� 
        StartCoroutine(CardSorting(handCardList, handCardObject, handCardPos, handCardDistance));   // ī�� ����

        handCardCount = handCardList.Count;
    }
    
    // ī�� ��� ���
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


    // ī�� ���
    public void UpdateCardList(GameObject cardObject)
    {
        int index = handCardObject.IndexOf(cardObject);

        if (index >= 0 && index < handCardList.Count)
        {
            // ������ ������Ʈ�� handCardObject ����Ʈ �ȿ� ���� ���� �ش� ī�带 �����ɴϴ�.
            useCard = handCardList[index]; // index�� �ش��ϴ� ī�带 �����ɴϴ�.

            // ����Ʈ �� �߰� �� ����
            handCardList.RemoveAt(index);
            addCardObject.Add(cardObject);
            handCardObject.RemoveAt(index);

            //cardObject.SetActive(false);
            cardObject.GetComponent<CardMove>().originalPosition = spawDeckPos;
            cardObject.transform.position = spawDeckPos;

            StartCoroutine(CardSorting(handCardList, handCardObject, handCardPos, handCardDistance));
        }
    }


    // ī�� ����
    public void ChoiceCard(GameObject cardObject)
    {
        int index = addCardObject.IndexOf(cardObject);

        if (index >= 0 && index < addCardList.Count)
        {
            Card card = addCardList[index]; // index�� �ش��ϴ� ī�带 �����ɴϴ�.

            // ����Ʈ �� �߰� �� ����
            handCardList.Add(card);
            addCardList.Clear();
            handCardObject.Add(cardObject);
            addCardObject.RemoveAt(index);

            // ���ù��� ���� ������Ʈ ��Ȱ��ȭ / ��ġ �̵�
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


    // ������ Ȯ���� ���� ī�� ����Ʈ�� ������ �� ����Ʈ�� ���� �����ϰ� ������
    public Card GetRandomCard()
    {

        waitAddCard = true;

        // ������ ī�� ����Ʈ ����
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

        // ������ ī�� ����
        return randomList[UnityEngine.Random.Range(0, randomList.Count)];
    
    }


    // ���� ī�� ����
    public void CreateRandomCard()
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
            handCardObject[i].SetActive(false);

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

            CardOrder cardOrder = cardObject[i].GetComponent<CardOrder>();
            cardOrder.SetOrder(i);
            
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
            cardObject[i].SetActive(true);
            cardObject[i].transform.position = targetPosition;  // ��ǥ ��ġ�� ��Ȯ�� �̵�
            cardObject[i].GetComponent<CardMove>().originalPosition = targetPosition;
        }
    }

}