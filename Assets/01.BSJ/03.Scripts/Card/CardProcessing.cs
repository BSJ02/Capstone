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
    [HideInInspector] public Player currentPlayer;
    /*[HideInInspector] */public GameObject currentPlayerObj;

    private PlayerState playerState;

    public GameObject selectedTarget = null;

    [HideInInspector] public float cardUseDistance = 0;
    [HideInInspector] public bool cardUseDistanceInRange = false;

    private void Update()
    {
        if (usingCard)
        {
            if (CardManager.instance.useCard.cardTarget == CardTarget.TargetPosition)
            {
                ShowTargetCardRange((int)cardUseDistance);
            }
            else
            {
                ShowPlayerCardRange((int)cardUseDistance);
            }
        }
    }

    public void ShowPlayerCardRange(int cardUseDistance)
    {
        MapGenerator.instance.selectingTarget = true;
        MapGenerator.instance.CardUseRange(currentPlayer.transform.position, (int)cardUseDistance);
    }

    public void ShowTargetCardRange(int cardUseDistance)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            GameObject target = hit.collider.gameObject;

            if (target.CompareTag("Monster") || target.layer == LayerMask.NameToLayer("Player"))
            {
                MapGenerator.instance.selectingTarget = true;
                MapGenerator.instance.CardUseRange(target.transform.position, (int)cardUseDistance);
            }
        }
        
    }

    public void UseCardAndSelectTarget(Card card, GameObject gameObject)
    {
        StartCoroutine(WaitForTargetSelection(card));
    }

    private IEnumerator WaitForTargetSelection(Card card)
    {
        BattleManager.instance.isPlayerMove = false;
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
                    if (MapGenerator.instance.highlightedTiles.Contains(selectTile))
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
            switch (card.cardType)
            {
                case CardType.WarriorCard:
                    WarriorCards(card);
                    break;
                case CardType.ArcherCard:
                    ArcherCards(card);
                    break;
                case CardType.WizardCard:
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
            case "Limit Break":
                BaseCardData.instance.UseLimitBreak(card, selectedTarget);
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
            case "Spin Attack":
                WarriorCardData.instance.UseSpinAttack(card, selectedTarget);
                break;
            case "Shield Bash":
                WarriorCardData.instance.UseShieldBash(card, selectedTarget);
                break;
            case "Desperate Strike":
                WarriorCardData.instance.UseDesperateStrike(card, selectedTarget);
                break;
            case "Dash":
                WarriorCardData.instance.UseDash(card, selectedTarget);
                break;
            case "Warrior's Roar":
                WarriorCardData.instance.UseWarriorsRoar(card, selectedTarget);
                break;
            case "Armor Crush":
                WarriorCardData.instance.UseArmorCrush(card, selectedTarget);
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
            case "Wall Jump":
                ArcherCardData.instance.UseWallJump(card, selectedTarget);
                break;
            case "Concealment":
                ArcherCardData.instance.UseConcealment(card, selectedTarget);
                break;
            case "Agility":
                ArcherCardData.instance.UseAgility(card, selectedTarget);
                break;
            case "Power Of Turn":
                ArcherCardData.instance.UsePowerOfTurn(card, selectedTarget);
                break;
            case "Mark Attack":
                ArcherCardData.instance.UseMarkAttack(card, selectedTarget);
                break;
            case "Triple Shot":
                ArcherCardData.instance.UseTripleShot(card, selectedTarget);
                break;
            case "Poison Attack":
                ArcherCardData.instance.UsePoisonAttack(card, selectedTarget);
                break;
            case "Aimed Shot":
                ArcherCardData.instance.UseAimedShot(card, selectedTarget);
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