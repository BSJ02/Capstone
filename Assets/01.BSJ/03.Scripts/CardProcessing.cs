using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    [HideInInspector] public bool waitForInput = false;  
    [HideInInspector] public bool usingCard = false;
    [HideInInspector] public bool coroutineStop = false;
    [HideInInspector] public int TempActivePoint;

    private WeaponController weaponController;
    private BattleManager battleManager;
    private CardManager cardManager;
    [HideInInspector] public Player currentPlayer;
    [HideInInspector] public GameObject currentPlayerObj;

    private PlayerState playerState;

    public GameObject selectedTarget = null;

    [HideInInspector] public float cardUseDistance = 0;
    [HideInInspector] public bool cardUseDistanceInRange = false;

    [HideInInspector] public bool isCardMoving = false;

    private void Start()
    {
        cardManager = FindObjectOfType<CardManager>();
        battleManager = FindObjectOfType<BattleManager>();
    }

    private void Update()
    {
        if (usingCard)
        {
            ShowCardRange((int)cardUseDistance);
        }
    }

    public void ShowCardRange(int cardUseDistance)
    {
        MapGenerator.instance.selectingTarget = true;
        MapGenerator.instance.CardUseRange(currentPlayer.transform.position, (int)cardUseDistance);
    }

    public void UseCardAndSelectTarget(Card card, GameObject gameObject)
    {
        StartCoroutine(WaitForTargetSelection(card));
    }

    private IEnumerator WaitForTargetSelection(Card card)
    {
        battleManager.isPlayerMove = false;
        //TempActivePoint = currentPlayer.playerData.activePoint;
        //currentPlayer.playerData.activePoint = 0;
        cardUseDistance = card.cardDistance;

        while (true)
        {
            waitForInput = true;
            
            if (card.cardTarget == Card.CardTarget.Player || card.cardTarget == Card.CardTarget.AreaTarget)
            {
                selectedTarget = currentPlayerObj;
                waitForInput = false;
                yield return null;
            }
            else 
            {
                while (waitForInput)
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        SelectTarget();
                    }
                    yield return null;
                }
            }

            if (coroutineStop)
            {
                coroutineStop = false;
                MapGenerator.instance.ClearHighlightedTiles();
                yield break;
            }

            UseCard(card, selectedTarget);

            if (waitForInput)
            {
                continue;
            }

            usingCard = false;
            cardUseDistance = 0;
            MapGenerator.instance.ClearHighlightedTiles();

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
            selectedTarget = hit.collider.gameObject;

            if (selectedTarget.CompareTag("Monster") || selectedTarget.layer == LayerMask.NameToLayer("Tile"))
            {
                if (selectedTarget.CompareTag("Monster"))
                {
                    Monster selectMonster = selectedTarget.GetComponent<Monster>();
                    if (MapGenerator.instance.rangeInMonsters.Contains(selectMonster))
                    {
                        waitForInput = false;
                    }
                }
                else if (selectedTarget.layer == LayerMask.NameToLayer("Tile"))
                {
                    Tile selectTile = selectedTarget.GetComponent<Tile>();
                    if (MapGenerator.instance.highlightedTiles.Contains(selectTile) && !selectTile.coord.isWall)
                    {
                        waitForInput = false;
                    }
                }
            }
            else
            {
                Debug.Log("Select again");
            }
        }
    }

    private void UseCard(Card card, GameObject selectedTarget)
    {
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
                BaseCardData.instance.UseHealingPotion(card, selectedTarget);
                break;
            case "Remove Ailments":
                BaseCardData.instance.UseRemoveAilments(card, selectedTarget);
                break;
            case "Evasion Boost":
                BaseCardData.instance.UseEvasionBoost(card, selectedTarget);
                break;
            case "Transmission":
                BaseCardData.instance.UseTransmission(card, selectedTarget);
                break;
            case "Stat Boost":
                BaseCardData.instance.UseStatBoost(card, selectedTarget);
                break;
            case "Rest":
                BaseCardData.instance.UseRest(card, selectedTarget);
                break;
            default:
                break;
        }
    }

    // Warrior
    private void WarriorCards(Card card)
    {
        switch (card.cardName)
        {
            case "Lightning Strike":
                WarriorCardData.instance.UseAxSlash(card, selectedTarget);
                break;
            default:
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
                ArcherCardData.instance.WallJump(card, selectedTarget);
                break;
            case "Concealment":
                ArcherCardData.instance.Concealment(card, selectedTarget);
                break;
            case "Agility":
                ArcherCardData.instance.AgilityAttack(card, selectedTarget);
                break;
            case "PowerOfTurn":
                ArcherCardData.instance.TurnCountAttack(card, selectedTarget);
                break;
            case "MarkAttack":
                ArcherCardData.instance.MarkTargetArrow(card, selectedTarget);
                break;
            case "DoubleShot":
                ArcherCardData.instance.DoubleTargetArrow(card, selectedTarget);
                break;
            case "PoisonAttack":
                ArcherCardData.instance.PoisonArrow(card, selectedTarget);
                break;
            case "AimedShot":
                ArcherCardData.instance.AimedArrow(card, selectedTarget);
                break;
            default:
                break;
        }
    }

    // Wizard
    private void WizardCards(Card card)
    {
        switch (card.cardName)
        {
            case "Teleport":
                WizardCardData.instance.UseTeleport(card, selectedTarget);
                break;
            case "Position Swap":
                WizardCardData.instance.UsePositionSwap(card, selectedTarget);
                break;
            case "Fireball":
                WizardCardData.instance.UseFireball(card, selectedTarget);
                break;
            case "Flame Pillar":
                WizardCardData.instance.UseFlamePillar(card, selectedTarget);
                break;
            case "Life Drain":
                WizardCardData.instance.UseLifeDrain(card, selectedTarget);
                break;
            case "Magic Shield":
                WizardCardData.instance.UseMagicShield(card, selectedTarget);
                break;
            case "Summon Obstacle":
                WizardCardData.instance.UseSummonObstacle(card, selectedTarget);
                break;
            default:
                break;
        }
    }
}