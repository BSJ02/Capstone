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
    // ī�� ���� ��ġ
    Vector3 handCardPos = new Vector3(0, -3, 0);
    Vector3 addCardPos = new Vector3(0, 2, 0);
    Vector3 spawPos = new Vector3(0, -10, 0);
    public Vector3 orginPos;
    float handCardDistance = 0.8f;
    float addCardDistance = 3f;

    private CardData cardData;
    private CardInform cardInform;

    [SerializeField] public List<Card> handCardList = new List<Card>(); // ����� �� �ִ� ī�� ����Ʈ
    [SerializeField] public List<Card> addCardList = new List<Card>();   // �߰��� ī�� ����Ʈ

    // ī�带 ���� �θ� ������Ʈ
    GameObject deckObject;

    // ������ ī�� ������Ʈ�� ���� ����Ʈ
    [SerializeField] List<GameObject> handCardObject;   // ����� ī�� ������Ʈ
    [SerializeField] List<GameObject> addCardObject;    // �߰��� ī�� ������Ʈ


    // �տ� ��� �ִ� ī�� ����
    private int handCardCount;

    [Header("ī�� ���� �� ��� Prefab")]
    [SerializeField] GameObject panelObject;

    [Header("ī�� Prefab")]
    [SerializeField] GameObject handCardPrefab;

    public bool press = false;
    bool isSortingInProgress = false;



    private void Start()
    {
        // ī�带 ���� �θ� ������Ʈ ����
        deckObject = new GameObject("Cards");

        // �� ������Ʈ�� ���� ���� ������Ʈ�� ã�Ƽ� ����
        cardData = FindObjectOfType<CardData>();

        // �ʱ�ȭ
        handCardObject = new List<GameObject>();
        addCardObject = new List<GameObject>();

        // cardData�� null���� Ȯ��
        if (cardData == null)
        {
            Debug.LogError("CardData    ������Ʈ�� ã�� �� ����");
        }

        handCardCount = cardData.baseCardList.Count;

        // �⺻ ī�� ����
        handCardList.AddRange(cardData.baseCardList);   // �� �߰�
        CreateCard(handCardList);   // �߰��� ���� ���� Card ���� 
        // ī�� ����
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

    // ī�� ����
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

    // ���� ī�� ����
    void CreateRandomCard()
    {
        //panelObject.SetActive(true);

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

            isSortingInProgress = true;
            StartCoroutine(CardSorting(addCardList, addCardObject, addCardPos, addCardDistance));
        }

        for (int i = 0; i < 3; i++)
        {
            addCardObject[i].GetComponent<CardOrder>().SetOrder(20);
        }

        handCardCount = handCardList.Count;
    }

    // ī�� ����
    void CreateCard(List<Card> cards)
    {
        panelObject = Instantiate(panelObject, new Vector3(0, 2.25f, 0), Quaternion.identity);
        panelObject.SetActive(false);
        panelObject.GetComponent<CardOrder>().SetOrder(19);


        int cardMaxCount = 13;

        for (int i = 0 ; i < cards.Count; i++)
        {
            // ���� ������Ʈ ����
            GameObject cardObject = Instantiate(handCardPrefab, spawPos, Quaternion.identity);

            cardObject.transform.SetParent(deckObject.transform, false);

            handCardObject.Add(cardObject);
            handCardObject[i].SetActive(true);

            // ������ ���� ������Ʈ�� ������ ����
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

    // ī�� ���� ���� (������ ī�� ��, �����ų ���� ������Ʈ)
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

    // ī�� ���� (������ ī�� ����Ʈ, ������ ī�� ������Ʈ, ��ǥ ��)
    IEnumerator CardSorting(List<Card> card, List<GameObject> cardObject, Vector3 cardPos, float cardToDistance)
    {
        float totalCardWidth = card.Count * cardToDistance;
        float startingPosX = -totalCardWidth / 2f + cardToDistance / 2f;


        for (int i = 0; i < card.Count; i++)
        {
            float elapsedTime = 0f; // ��� �ð�
            float duration = 0.15f;  // �̵��� �ɸ��� �ð�

            cardObject[i].GetComponent<CardOrder>().SetOrder(i);

            float newPosX = startingPosX + i * cardToDistance;  // ���ο� X ��ǥ ���

            Vector3 targetPosition = new Vector3(newPosX, cardPos.y, 0f);   // ��ǥ ��ġ ����

            // ���� ��ġ���� ��ǥ ��ġ���� ������ �̵�
            while (elapsedTime < duration)
            {
                float t = elapsedTime / duration;   // ������ ���� ���� ���
                cardObject[i].transform.position = Vector3.Lerp(cardObject[i].transform.position, targetPosition, t);   // ������Ʈ ��ġ ����
                elapsedTime += Time.deltaTime;  // ��� �ð� ������Ʈ
                yield return null;
            }

            cardObject[i].transform.position = targetPosition;  // ��ǥ ��ġ�� ��Ȯ�� �̵�
            orginPos = targetPosition;

            isSortingInProgress = false;
        }

    }

    // ī�� ���
    void UseCard(Card card)
    {
        int power = card.power;

        if (card.cardType == Card.CardType.Attack)
        {
            // ���� Ÿ���� ī�� ó��
            
            // ���� ü�� - power

        }
        else if (card.cardType == Card.CardType.Heal)
        {
            // ȸ�� Ÿ���� ī�� ó��

            // �÷��̾� ü�� + power
        }
        else if (card.cardType == Card.CardType.Movement)
        {
            // �̵� Ÿ���� ī�� ó��

            // �̵� �Ÿ� ���� / �̵��ϴ� �Լ� �ٽ� ȣ��
        }
        else
        {
            // �ٸ� Ÿ���� ī�� ó��
        }
    }
    
}