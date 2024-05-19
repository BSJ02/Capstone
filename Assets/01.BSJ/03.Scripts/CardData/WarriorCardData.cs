using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorCardData : MonoBehaviour
{
    public static WarriorCardData instance;

    private WeaponController weaponController;
    private ParticleController particleController;

    [Header(" # CardProcessing")]
    public CardProcessing cardProcessing;

    private PlayerState playerState;

    // Warrior variables
    [HideInInspector] public bool shouldSpinAttack;
    [HideInInspector] public bool shouldShieldBash;
    [HideInInspector] public bool shouldDesperateStrike;
    [HideInInspector] public bool shouldDash;
    [HideInInspector] public bool shouldWarriorsRoar;
    [HideInInspector] public bool shouldArmorCrush;


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
        particleController = FindObjectOfType<ParticleController>();
    }


    // Warrior Cards --------------------------------
    // Spin Attack
    public void UseSpinAttack(Card card, GameObject selectedTarget)
    {
        Player player = cardProcessing.currentPlayer;
        if (MapGenerator.instance.rangeInMonsters != null)
        {
            shouldSpinAttack = true;

            player.SpinAttackAnim(selectedTarget);

            cardProcessing.cardUseDistance = card.cardDistance;
        }
        else
        {
            cardProcessing.waitForInput = true;
        }
    }

    // Shield Bash
    public void UseShieldBash(Card card, GameObject selectedTarget)
    {
        Player player = cardProcessing.currentPlayer;
        if (MapGenerator.instance.rangeInMonsters != null)
        {
            shouldShieldBash = true;

            player.DefendAnim(selectedTarget);

            cardProcessing.cardUseDistance = card.cardDistance;
        }
        else
        {
            cardProcessing.waitForInput = true;
        }
    }

    // Desperate Strike
    public void UseDesperateStrike(Card card, GameObject selectedTarget)
    {
        Player player = cardProcessing.currentPlayer;
        if (MapGenerator.instance.rangeInMonsters != null)
        {
            shouldDesperateStrike = true;

            player.StabAnim(selectedTarget);

            cardProcessing.cardUseDistance = card.cardDistance;
        }
        else
        {
            cardProcessing.waitForInput = true;
        }
    }

    // Dash
    public void UseDash(Card card, GameObject selectedTarget)
    {
        Player player = cardProcessing.currentPlayer;

        if (MapGenerator.instance.rangeInMonsters != null)
        {
            shouldDash = true;

            player.RollFWDAnim(selectedTarget);

            cardProcessing.cardUseDistance = card.cardDistance;
        }
        else
        {
            cardProcessing.waitForInput = true;
        }
    }

    public void UseWarriorsRoar(Card card, GameObject selectedTarget)
    {
        Player player = cardProcessing.currentPlayer;

        if (MapGenerator.instance.rangeInMonsters != null)
        {
            shouldWarriorsRoar = true;

            player.VictoryAnim(selectedTarget);

            cardProcessing.cardUseDistance = card.cardDistance;
        }
        else
        {
            cardProcessing.waitForInput = true;
        }
    }

    public void UseArmorCrush(Card card, GameObject selectedTarget)
    {
        Player player = cardProcessing.currentPlayer;

        if (MapGenerator.instance.rangeInMonsters != null)
        {
            shouldArmorCrush = true;

            player.AttackTwoAnim(selectedTarget);

            cardProcessing.cardUseDistance = card.cardDistance;
        }
        else
        {
            cardProcessing.waitForInput = true;
        }
    }
}