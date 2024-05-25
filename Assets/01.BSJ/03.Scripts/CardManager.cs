using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class CardManager : MonoBehaviour
{
    public static CardManager instance;

    [Header(" # Card Inform")] public CardInform cardInform;
    
    private CardProcessing cardProcessing;

    private Vector3 handCardPos = new Vector3(0, 4.3f, 0);
    private Vector3 addCardPos = new Vector3(0, 10f, 0);
    private Vector3 spawDeckPos = new Vector3(-4f, -3.3f, -4f);

    [HideInInspector] public Vector3 deckOffset = new Vector3(2f, -11.3f, 2f);
    [HideInInspector] public Vector3 panelOffset = new Vector3(6f, -8f, 6f);

    private float handCardDistance = 0.9f;
    private float addCardDistance = 3f;

    [HideInInspector] public List<Card> handCardList = new List<Card>();
    [HideInInspector] public List<Card> addCardList = new List<Card>();

    [HideInInspector] public GameObject deckObject;
    [HideInInspector] public GameObject panelObject_Group;

    [HideInInspector] public List<GameObject> handCardObject;
    [HideInInspector] public List<GameObject> addCardObject;

    [HideInInspector] public int handCardCount;

    [Header(" # Card Prefab")]
    [SerializeField] private GameObject cardPrefab;

    [Header(" # Panel Prefab")]
    public GameObject addCardPanelPrefab;
    public GameObject useCardPanelPrefab;
    public GameObject handCardPanelPrefab;

    [HideInInspector] public bool waitAddCard = false;
    [HideInInspector] public bool isCardSorting = false;

    [HideInInspector] public Card useCard = null;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        cardProcessing = FindObjectOfType<CardProcessing>();

        deckObject = GameObject.Find("Deck");
        if (deckObject == null)
        {
            deckObject = new GameObject("Deck");
        }
        deckObject.transform.position = spawDeckPos;
        deckObject.transform.rotation = Quaternion.Euler(0, 45, 0);
        Debug.Log(deckObject.transform.position);

        panelObject_Group = GameObject.Find("PanelObject_Group");
        if (panelObject_Group == null)
        {
            panelObject_Group = new GameObject("PanelObject_Group");
        }
        panelObject_Group.transform.position = Vector3.zero;

        CreatePanelPrefab(addCardPanelPrefab, new Vector3(-3f, 6f, -3f), Quaternion.Euler(0, 45, 0), false);
        CreatePanelPrefab(useCardPanelPrefab, new Vector3(-2f, 6f, -2f), Quaternion.Euler(45, 45, 0), false);
        CreatePanelPrefab(handCardPanelPrefab, new Vector3(-5f, 1.8f, -5f), Quaternion.Euler(0, 45, 0), true);

        handCardObject = new List<GameObject>();
        addCardObject = new List<GameObject>();

        handCardList.AddRange(cardInform.baseCards);
        CreateCard(handCardList);
        StartCoroutine(CardSorting(handCardList, handCardObject, handCardPos, handCardDistance));

        handCardCount = handCardList.Count;
    }

    public GameObject FindPanelGroupChildObject(string childObjectName)
    {
        Transform childTransform = panelObject_Group.transform.Find(childObjectName);
        GameObject childGameObject = childTransform.gameObject;
        return childGameObject;
    }

    private void CreatePanelPrefab(GameObject prefab, Vector3 worldPos, Quaternion quaternion, bool setActive)
    {
        prefab = Instantiate(prefab, Vector3.zero, Quaternion.identity);
        prefab.transform.SetParent(panelObject_Group.transform, false);
        prefab.transform.position = worldPos;
        prefab.transform.rotation = quaternion;
        prefab.SetActive(setActive);
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

            //cardProcessing.currentPlayer.playerData.activePoint = cardProcessing.TempActivePoint;

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

            FindPanelGroupChildObject("Add Card Panel(Clone)").SetActive(false);
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
        FindPanelGroupChildObject("Add Card Panel(Clone)").SetActive(true);

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
            GameObject cardObject = Instantiate(cardPrefab, Vector3.zero, Quaternion.identity);

            handCardObject.Add(cardObject);
            handCardObject[i].SetActive(false);

            ApplyCardInfrom(cards[i], cardObject);

            cardObject.transform.SetParent(deckObject.transform, false);
        }

        for (int i = 0; i < cardMaxCount - cards.Count; i++)
        {
            GameObject cardObject = Instantiate(cardPrefab, Vector3.zero, Quaternion.identity);

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

        UnityEngine.UI.Image cardimage = gameObject.GetComponentInChildren<UnityEngine.UI.Image>();

        if (cardimage != null && card.cardSprite != null)
        {
            cardimage.sprite = card.cardSprite;
        }

        gameObject.GetComponent<CardColorChanger>().ChangeCardColors(card);
    }


    public IEnumerator CardSorting(List<Card> card, List<GameObject> cardObject, Vector3 cardPos, float cardToDistance)
    {
        isCardSorting = true;

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
            cardObject[i].GetComponent<CardMove>().cardOffset = deckObject.transform.position - targetPosition;
            
            Vector3 newCardPos = deckObject.transform.position - cardObject[i].GetComponent<CardMove>().cardOffset;
            
            cardObject[i].transform.position = newCardPos;  
            cardObject[i].GetComponent<CardMove>().originalPosition = newCardPos;
        }

        isCardSorting = false;
    }

}