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
    
    private CardProcessing cardProcessing;
    
    [HideInInspector] private Vector3 handCardPos = new Vector3(0, 4.3f, 0);
    [HideInInspector] private Vector3 addCardPos = new Vector3(0, 10f, 0);
    [HideInInspector] private Vector3 spawDeckPos = new Vector3(-3.6f, -3.6f, -3.6f);

    private float handCardDistance = 0.8f;
    private float addCardDistance = 3f;

    [HideInInspector] public List<Card> handCardList = new List<Card>();
    [HideInInspector] public List<Card> addCardList = new List<Card>(); 

    private GameObject deckObject;

    [HideInInspector] public List<GameObject> handCardObject;
    [HideInInspector] public List<GameObject> addCardObject;

    public int handCardCount;

    [Header(" # Card Prefab")]
    [SerializeField] private GameObject handCardPrefab;

    [Header(" # Panel Prefab")]
    [SerializeField] private GameObject addCardPanelPrefab;
    [SerializeField] public GameObject useCardPanelPrefab;
    [SerializeField] public GameObject handCardPanelPrefab;

    [HideInInspector] public bool waitAddCard = false;

    [HideInInspector] public Card useCard = null;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        deckObject = GameObject.Find("Cards");
        if (deckObject == null)
        {
            deckObject = new GameObject("Cards");
        }
        deckObject.transform.position = spawDeckPos;
        deckObject.transform.rotation = Quaternion.Euler(0, 45, 0);

        handCardObject = new List<GameObject>();
        addCardObject = new List<GameObject>();

        handCardCount = cardInform.baseCards.Count;

        addCardPanelPrefab = Instantiate(addCardPanelPrefab, new Vector3(-3, 6f, -3), Quaternion.Euler(0, 45, 0));
        addCardPanelPrefab.SetActive(false);

        useCardPanelPrefab = Instantiate(useCardPanelPrefab, new Vector3(-2, 6, -2), Quaternion.Euler(45, 45, 0));
        useCardPanelPrefab.SetActive(false);

        handCardPanelPrefab = Instantiate(handCardPanelPrefab, new Vector3(-3f, 0.28f, -3f), Quaternion.Euler(0, 45, 0));
    }


    private void Start()
    {
        cardProcessing = FindObjectOfType<CardProcessing>();

        handCardList.AddRange(cardInform.baseCards);  
        CreateCard(handCardList);  
        StartCoroutine(CardSorting(handCardList, handCardObject, handCardPos, handCardDistance)); 

        handCardCount = handCardList.Count;
    }
    
    public void CardCancle()
    {
        if (useCard != null && cardProcessing.usingCard)
        {
            Card card = useCard;
            GameObject cardObject = addCardObject[addCardObject.Count - 1];

            handCardList.Add(card);
            handCardObject.Add(cardObject);
            addCardList.Remove(card);
            addCardObject.Remove(cardObject);

            ApplyCardInfrom(card, cardObject);
            StartCoroutine(CardSorting(handCardList, handCardObject, handCardPos, handCardDistance));

            cardProcessing.currentPlayer.playerData.activePoint = cardProcessing.TempActivePoint;

            cardProcessing.usingCard = false;
            cardProcessing.waitForInput = false;
            cardProcessing.coroutineStop = true;
            useCard = null;
        }
    }

    public void CardGetTest()
    {
        addCardObject[0].SetActive(true);
        Card card = cardInform.wizardCards[6]; // <- change

        cardProcessing.currentPlayer.playerData.activePoint = cardProcessing.currentPlayer.playerData.MaxActivePoint;

        ApplyCardInfrom(card, addCardObject[0]);

        for (int i = 0; i < addCardList.Count && i < addCardObject.Count; i++)
        {
            addCardObject[i].GetComponent<CardOrder>().SetOrder(20);
        }

        handCardList.Add(card);
        addCardList.Clear();
        handCardObject.Add(addCardObject[0]);
        addCardObject.RemoveAt(0);

        for (int i = 0; i < addCardObject.Count; i++)
        {
            addCardObject[i].GetComponent<CardMove>().originalPosition = spawDeckPos;
            addCardObject[i].transform.position = spawDeckPos;
            addCardObject[i].SetActive(false);
        }

        StartCoroutine(CardSorting(handCardList, handCardObject, handCardPos, handCardDistance));

        addCardPanelPrefab.SetActive(false);
        waitAddCard = false;

        handCardCount = handCardList.Count;
    }

    public void UpdateCardList(GameObject cardObject)
    {
        int index = handCardObject.IndexOf(cardObject);

        if (index >= 0 && index < handCardList.Count)
        {
            useCard = handCardList[index]; 

            handCardList.RemoveAt(index);
            addCardObject.Add(cardObject);
            handCardObject.RemoveAt(index);

            //cardObject.SetActive(false);
            cardObject.GetComponent<CardMove>().originalPosition = spawDeckPos;
            cardObject.transform.position = spawDeckPos;

            StartCoroutine(CardSorting(handCardList, handCardObject, handCardPos, handCardDistance));
        }
    }

    public Card CardDrag(GameObject cardObject)
    {
        int index = handCardObject.IndexOf(cardObject);

        if (index >= 0 && index < handCardList.Count)
        {
            Card dragCard = handCardList[index];
            return dragCard;
        }
        else
        {
            return null;
        }
    }

    public void ChoiceCard(GameObject cardObject)
    {
        int index = addCardObject.IndexOf(cardObject);

        if (index >= 0 && index < addCardList.Count)
        {
            Card card = addCardList[index]; 

            handCardList.Add(card);
            addCardList.Clear();
            handCardObject.Add(cardObject);
            addCardObject.RemoveAt(index);

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


    public Card GetRandomCard()
    {

        waitAddCard = true;

        List<Card> randomList = null;
        int randNum = UnityEngine.Random.Range(1, 101);
        if (randNum <= cardInform.legendPercent)
        {
            randomList = cardInform.warriorCards;
        }
        else if (randNum <= cardInform.epicPercent)
        {
            randomList = cardInform.wizardCards;
        }
        else if (randNum <= cardInform.rarePercent)
        {
            randomList = cardInform.archerCards;
        }
        else
        {
            randomList = cardInform.baseCards;
        }

        return randomList[UnityEngine.Random.Range(0, randomList.Count)];
    
    }


    public void CreateRandomCard()
    {
        addCardPanelPrefab.SetActive(true);

        addCardList = new List<Card>();

        HashSet<Card> dedupeCard = new HashSet<Card>();

        while (dedupeCard.Count < 3)
        {
            Card randomCard = GetRandomCard();
            dedupeCard.Add(randomCard);
        }

        addCardList = dedupeCard.ToList(); 

        for (int i = 0; i < addCardList.Count && i < addCardObject.Count; i++)
        {
            addCardObject[i].SetActive(true);
            ApplyCardInfrom(addCardList[i], addCardObject[i]);

            StartCoroutine(CardSorting(addCardList, addCardObject, addCardPos, addCardDistance));
        }

        for (int i = 0; i < addCardList.Count && i < addCardObject.Count; i++)
        {
            addCardObject[i].GetComponent<CardOrder>().SetOrder(20);
        }

        handCardCount = handCardList.Count;
    }


    private void CreateCard(List<Card> cards)
    {
        int cardMaxCount = 13;

        for (int i = 0 ; i < cards.Count; i++)
        {
            GameObject cardObject = Instantiate(handCardPrefab, Vector3.zero, Quaternion.identity);

            handCardObject.Add(cardObject);
            handCardObject[i].SetActive(false);

            ApplyCardInfrom(cards[i], cardObject);

            cardObject.transform.SetParent(deckObject.transform, false);
        }

        for (int i = 0; i < cardMaxCount - cards.Count; i++)
        {
            GameObject cardObject = Instantiate(handCardPrefab, Vector3.zero, Quaternion.identity);

            addCardObject.Add(cardObject);
            addCardObject[i].SetActive(false);

            cardObject.transform.SetParent(deckObject.transform, false);
        }
    }


    public void ApplyCardInfrom(Card card, GameObject gameObject)
    {
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


    public IEnumerator CardSorting(List<Card> card, List<GameObject> cardObject, Vector3 cardPos, float cardToDistance)
    {
        float totalCardWidth = card.Count * cardToDistance;
        float startingPosX = -totalCardWidth / 2f + cardToDistance / 2f;

        float deltaTime = Time.deltaTime; 

        for (int i = 0; i < card.Count; i++)
        {
            float elapsedTime = 0f;
            float duration = 0.2f;  

            CardOrder cardOrder = cardObject[i].GetComponent<CardOrder>();
            cardOrder.SetOrder(i);
            
            float newPosX = startingPosX + i * cardToDistance;

            Vector3 targetLocalPosition = new Vector3(newPosX, cardPos.y, cardPos.z);

            Vector3 targetPosition = deckObject.transform.TransformPoint(targetLocalPosition);

            while (elapsedTime < duration)
            {
                float t = elapsedTime / duration; 
                cardObject[i].transform.position = Vector3.Lerp(cardObject[i].transform.position, targetPosition, t);  
                elapsedTime += deltaTime;  
                yield return null;
            }
            cardObject[i].SetActive(true);
            cardObject[i].transform.position = targetPosition;  
            cardObject[i].GetComponent<CardMove>().originalPosition = targetPosition;
        }
    }

}