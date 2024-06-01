using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCardData : MonoBehaviour
{
    public static BaseCardData instance;

    private WeaponController weaponController;

    [Header(" # CardProcessing")]
    public CardProcessing cardProcessing;

    private PlayerState playerState;

    [HideInInspector] public bool shouldRemoveAilments;
    [HideInInspector] public bool shouldEvasionBoost;
    [HideInInspector] public bool shouldTransmission;

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

    // Base Cards --------------------------------
    // Healing Potion
    public void UseHealingPotion(Card card, GameObject selectedTarget)
    {
        Player player = cardProcessing.currentPlayer;
        if (player != null)
        {
            SoundManager.instance.PlaySoundEffect("Healing");

            player.ChargeAnim(selectedTarget);

            ParticleController.instance.ApplyPlayerEffect(ParticleController.instance.healEffectPrefab, selectedTarget);

            if (player.playerData.Hp + card.cardPower[0] >= player.playerData.MaxHp)
            {
                player.playerData.Hp = player.playerData.MaxHp;
            }
            else
            {
                player.playerData.Hp += card.cardPower[0];
            }

        }
        else
        {
            cardProcessing.waitForInput = true;
        }
    }

    // Remove Ailments
    public void UseRemoveAilments(Card card, GameObject selectedTarget)
    {
        Player player = cardProcessing.currentPlayer;

        if (player != null)
        {
            shouldRemoveAilments = true;

            player.VictoryAnim(selectedTarget);

            cardProcessing.cardUseDistance = card.cardDistance;
        }
        else
        {
            cardProcessing.waitForInput = true;
        }
    }

    // Evasion Boost
    public void UseEvasionBoost(Card card, GameObject selectedTarget)
    {
        Player player = cardProcessing.currentPlayer;
        if (player != null)
        {
            shouldEvasionBoost = true;

            player.VictoryAnim(selectedTarget);

            cardProcessing.cardUseDistance = card.cardDistance;
        }
        else
        {
            cardProcessing.waitForInput = true;
        }
    }

    // Transmission
    public void UseTransmission(Card card, GameObject selectedTarget)
    {
        Player player = cardProcessing.currentPlayer;
        CharacterStatusEffect character = selectedTarget.GetComponent<CharacterStatusEffect>();

        if (character != null)
        {
            shouldTransmission = true;

            player.VictoryAnim(selectedTarget);

            cardProcessing.cardUseDistance = card.cardDistance;
        }
        else
        {
            cardProcessing.waitForInput = true;
        }
    }

    // Limit Break
    public void UseLimitBreak(Card card, GameObject selectedTarget)
    {
        Player player = cardProcessing.currentPlayer;
        if (player != null)
        {
            SoundManager.instance.PlaySoundEffect("RemoveAilments");

            player.ChargeAnim(selectedTarget);

            ParticleController.instance.ApplyPlayerEffect(ParticleController.instance.buffEffectPrefab, selectedTarget);

            int randNum = Random.Range(0, 3);
            switch (randNum)
            {
                case 0:
                    player.playerData.Armor += 20;

                    break;
                case 1:
                    player.playerData.MaxHp += 20;

                    break;
                case 2:
                    player.playerData.MaxActivePoint += 1;

                    break;
                default:
                    break;
            }
        }
        else
        {
            cardProcessing.waitForInput = true;
        }
    }

}