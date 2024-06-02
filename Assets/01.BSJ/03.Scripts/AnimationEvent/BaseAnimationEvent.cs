using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class BaseAnimationEvent : MonoBehaviour
{
    private CardManager cardManager;
    private CardProcessing cardProcessing;

    private bool isRemoveAilments = false;
    private bool isEvasionBoost = false;
    private bool isTransmission = false;

    private void Awake()
    {
        cardManager = FindObjectOfType<CardManager>();
        cardProcessing = FindObjectOfType<CardProcessing>();
    }


    private void Update()
    {
        if (isRemoveAilments)
        {
            GameObject particlePrefab = ParticleController.instance.dashEffectPrefab;
            GameObject playerObj = cardProcessing.currentPlayerObj;

            SoundManager.instance.PlaySoundEffect("RemoveAilments");

            ParticleController.instance.ApplyPlayerEffect(particlePrefab, playerObj);

            CharacterStatusEffect character = playerObj.GetComponent<CharacterStatusEffect>();

            character.ResetStatusEffects();

            BaseCardData.instance.shouldRemoveAilments = false;
            isRemoveAilments = false;
        }

        if (isEvasionBoost)
        {
            GameObject particlePrefab = ParticleController.instance.removeAilmentEffectPrefab;
            GameObject playerObj = cardProcessing.currentPlayerObj;
            Player player = cardProcessing.currentPlayer;
            Card useCard = cardManager.useCard;

            SoundManager.instance.PlaySoundEffect("EvasionBoost");

            ParticleController.instance.ApplyPlayerEffect(particlePrefab, playerObj, 0.5f, Quaternion.identity, 1f);

            player.playerData.activePoint += (int)useCard.cardPower[0] + cardProcessing.TempActivePoint;

            BaseCardData.instance.shouldEvasionBoost = false;
            isEvasionBoost = false;
        }

        if (isTransmission)
        {
            GameObject particlePrefab = ParticleController.instance.transmissionEffectPrefab;
            CharacterStatusEffect character = cardProcessing.selectedTarget.GetComponent<CharacterStatusEffect>();

            Vector3 particlePos = character.transform.position + new Vector3(0f, 0.5f, 0f);

            SoundManager.instance.PlaySoundEffect("Transmission");

            ParticleController.instance.ApplyTargetEffect(particlePrefab, particlePos, Quaternion.identity, 0.8f);

            foreach (Player rangeInPlayer in MapGenerator.instance.rangeInPlayers)
            {
                CharacterStatusEffect playerStatus = rangeInPlayer.GetComponent<CharacterStatusEffect>();

                playerStatus.SetActiveStatusEffects(character.ActiveStatusEffects);
            }

            foreach (Monster rangeInMonster in MapGenerator.instance.rangeInMonsters)
            {
                CharacterStatusEffect monsterStatus = rangeInMonster.GetComponent<CharacterStatusEffect>();

                monsterStatus.SetActiveStatusEffects(character.ActiveStatusEffects);
            }

            BaseCardData.instance.shouldTransmission = false;
            isTransmission = false;
        }

    }

    private void RemoveAilmentsAnimationEvent()
    {
        if (BaseCardData.instance.shouldRemoveAilments)
        {
            isRemoveAilments = true;
        }
    }

    private void EvasionBoostAnimationEvent()
    {
        if (BaseCardData.instance.shouldEvasionBoost)
        {
            isEvasionBoost = true;
        }
    }

    private void TransmissionAnimationEvent()
    {
        if (BaseCardData.instance.shouldTransmission)
        {
            isTransmission = true;
        }
    }

}