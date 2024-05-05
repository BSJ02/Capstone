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
    [HideInInspector] public Player currentPlayer;
    [HideInInspector] public GameObject currentPlayerObj;

    [Header(" # Map Scripts")] public MapGenerator mapGenerator;

    private PlayerState playerState;


    private GameObject selectedTarget = null;
    [HideInInspector] public float cardUseDistance = 0;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        cardData = FindObjectOfType<CardData>();
        cardManager = FindObjectOfType<CardManager>();
        battleManager = FindObjectOfType<BattleManager>();
    }

    private void Update()
    {
        if (usingCard)
        {
            mapGenerator.CardUseRange(currentPlayer.transform.position, (int)cardUseDistance);
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
        TempActivePoint = currentPlayer.playerData.activePoint;
        currentPlayer.playerData.activePoint = 0;
        cardUseDistance = card.cardDistance;    // 카드 거리 저장

        while (true)
        {
            waitForInput = true;    // 대기 상태로 전환
            
            if (card.cardTarget == Card.CardTarget.Player)
            {
                selectedTarget = currentPlayerObj;
                waitForInput = false;
                yield return null; // 다음 프레임까지 대기
            }
            else
            {
                // 대상 선택이 완료될 때까지 반복합니다.
                while (waitForInput)
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        SelectTarget();
                    }
                    yield return null; // 다음 프레임까지 대기
                }
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
                if (Vector3.Distance(currentPlayerObj.transform.position, selectedTarget.transform.position) <= cardUseDistance)
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
                cardData.UseRest(card, selectedTarget);
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
            case "Fireball":
                cardData.UseFireball(card, selectedTarget);
                break;
            case "Lightning Strike":
                cardData.UseLightningStrike(card, selectedTarget);
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
            case "Teleport":
                cardData.UseTeleport(card, selectedTarget);
                break;
            case "Position Swap":
                cardData.UsePositionSwap(card, selectedTarget);
                break;
            case "Fireball":
                cardData.UseFireball(card, selectedTarget);
                break;
            case "Flame Pillar":
                cardData.UseFlamePillar(card, selectedTarget);
                break;
            case "Life Drain":
                cardData.UseLifeDrain(card, selectedTarget);
                break;
            case "Magic Shield":
                cardData.UseMagicShield(card, selectedTarget);
                break;
            case "Summon Obstacle":
                cardData.UseSummonObstacle(card, selectedTarget);
                break;
            default:
                Debug.LogError("해당 카드 타입을 처리하는 코드가 없음");
                break;
        }
    }
}