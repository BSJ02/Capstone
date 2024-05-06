using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using static Card;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.UIElements.UxmlAttributeDescription;

public class CardProcessing : MonoBehaviour
{
    [HideInInspector] public bool waitForInput = false;  // 대기 상태 여부
    [HideInInspector] public bool usingCard = false;
    [HideInInspector] public bool coroutineStop = false;
    [HideInInspector] public int TempActivePoint;

    private WeaponController weaponController;
    private BattleManager battleManager;
    private CardData cardData;
    private CardManager cardManager;

    [Header(" # Player Scripts")] public Player player;
    [Header(" # Map Scripts")] public MapGenerator mapGenerator;

    private PlayerState playerState;

    [Header(" # Player Object")] public GameObject playerObject;

    private GameObject selectedTarget = null;
    [HideInInspector] public float cardUseDistance = 0;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        cardManager = FindObjectOfType<CardManager>();
        weaponController = playerObject.GetComponent<WeaponController>();
        battleManager = FindObjectOfType<BattleManager>();
        player = playerObject.GetComponent<Player>();
        cardData = FindObjectOfType<CardData>();
    }

    private void Update()
    {
        if (usingCard)
        {
            mapGenerator.CardUseRange(playerObject.transform.position, (int)cardUseDistance);
        }
    }

    // 카드 사용 메서드
    public void UseCardAndSelectTarget(Card card, GameObject gameObject)
    {
        StartCoroutine(WaitForTargetSelection(card));
    }

    // 대상 선택을 기다리는 코루틴
    private IEnumerator WaitForTargetSelection(Card card)
    {
        battleManager.isPlayerMove = false;
        TempActivePoint = player.playerData.activePoint;
        player.playerData.activePoint = 0;
        cardUseDistance = card.cardDistance;    // 카드 거리 저장
        while (true)
        {
            waitForInput = true;    // 대기 상태로 전환
            
            // 대상 선택이 완료될 때까지 반복합니다.
            while (waitForInput)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    SelectTarget();
                }
                yield return null; // 다음 프레임까지 대기
            }
            if (coroutineStop)
            {
                coroutineStop = false;
                mapGenerator.ClearHighlightedTiles();
                yield break;
            }

            UseCard(card, selectedTarget);

            if (waitForInput)
            {
                Debug.Log("대상을 다시 선택하세요.");
                continue;
            }

            usingCard = false;
            mapGenerator.ClearHighlightedTiles();

            if (!waitForInput)
            {
                break;
            }

        }
    }

    private void SelectTarget()
    {
        selectedTarget = null;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            if (hit.collider.CompareTag("Player") || hit.collider.CompareTag("Monster") || hit.collider.CompareTag("Tile"))
            {

                selectedTarget = hit.collider.gameObject;            
                if (Vector3.Distance(player.transform.position, selectedTarget.transform.position) <= cardUseDistance)
                {
                    waitForInput = false;
                }
            }
        }
    }

    private void UseCard(Card card, GameObject selectedTarget)
    {
        // 선택된 대상에 따라 카드를 사용
        if (selectedTarget != null)
        {
            switch (card.cardRank)
            {
                case CardRank.WarriorCard:
                    WarriorCards(card);
                    break;
                case CardRank.ArcherCard:
                    ArcherCards(card);
                    break;
                case CardRank.WizardCard:
                    WizardCards(card);
                    break;
                default:
                    BaseCards(card);
                    break;
            }
        }
    }

    // Base
    private void BaseCards(Card card)
    {
        switch (card.cardName)
        {
            case "Healing Potion":
                cardData.UseHealingPotion(card, selectedTarget);
                break;
            case "Remove Ailments":
                cardData.UseRemoveAilments(card, selectedTarget);
                break;
            case "Evasion Boost":
                cardData.UseEvasionBoost(card, selectedTarget);
                break;
            case "Transmission":
                cardData.UseTransmission(card, selectedTarget);
                break;
            case "Stat Boost":
                cardData.UseStatBoost(card, selectedTarget);
                break;
            case "Rest":
                cardData.UseDivineIntervention(card, selectedTarget);
                break;
            default:
                Debug.LogError("해당 카드 타입을 처리하는 코드가 없음");
                break;
        }
    }

    // Warrior
    private void WarriorCards(Card card)
    {
        switch (card.cardName)
        {
            case "Fireball":
                cardData.UseFireball(card, selectedTarget);
                break;
            case "Lightning Strike":
                cardData.UseLightningStrike(card, selectedTarget);
                break;
            case "Excalibur's Wrath":
                cardData.UseExcalibursWrath(card, selectedTarget);
                break;
            case "Divine Intervention":
                cardData.UseDivineIntervention(card, selectedTarget);
                break;
            case "Soul Siphon":
                cardData.UseDivineIntervention(card, selectedTarget);
                break;
            default:
                Debug.LogError("해당 카드 타입을 처리하는 코드가 없음");
                break;
        }
    }

    // Archer
    private void ArcherCards(Card card)
    {
        switch (card.cardName)
        {
            // Warrior
            case "WallJump":
                cardData.WallJump(card, selectedTarget);
                break;
            case "Concealment":
                cardData.Concealment(card, selectedTarget);
                break;
            case "Agility":
                cardData.AgilityAttack(card, selectedTarget);
                break;
            case "PowerOfTurn":
                cardData.TurnCountAttack(card, selectedTarget);
                break;
            case "MarkAttack":
                cardData.MarkTargetArrow(card, selectedTarget);
                break;
            case "DoubleShot":
                cardData.DoubleTargetArrow(card, selectedTarget);
                break;
            case "PoisonAttack":
                cardData.PoisonArrow(card, selectedTarget);
                break;
            case "AimedShot":
                cardData.AimedArrow(card, selectedTarget);
                break;
            default:
                Debug.LogError("해당 카드 타입을 처리하는 코드가 없음");
                break;
        }
    }

    // Wizard
    private void WizardCards(Card card)
    {
        switch (card.cardName)
        {
            // Warrior
            case "Fireball":
                cardData.UseFireball(card, selectedTarget);
                break;
            case "Lightning Strike":
                cardData.UseLightningStrike(card, selectedTarget);
                break;
            case "Excalibur's Wrath":
                cardData.UseExcalibursWrath(card, selectedTarget);
                break;
            case "Divine Intervention":
                cardData.UseDivineIntervention(card, selectedTarget);
                break;
            case "Soul Siphon":
                cardData.UseDivineIntervention(card, selectedTarget);
                break;
            default:
                Debug.LogError("해당 카드 타입을 처리하는 코드가 없음");
                break;
        }
    }
}