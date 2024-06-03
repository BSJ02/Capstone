using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WizardAnimationEvent : MonoBehaviour
{
    private CardManager cardManager;
    private CardProcessing cardProcessing;

    private bool isFireball = false;
    private bool isTeleport = false;
    private bool isPosSwap = false;
    private bool isFlamePillar = false;
    private bool isSummonObstacle = false;
    private bool isLifeDrain = false;

    private void Awake()
    {
        cardManager = FindObjectOfType<CardManager>();
        cardProcessing = FindObjectOfType<CardProcessing>();
    }
    
    private void Update()
    {
        if (isTeleport)
        {
            Vector3 playerPos = cardProcessing.currentPlayerObj.transform.position;
            Vector3 tilePos = WizardCardData.instance.targetPos;

            cardProcessing.currentPlayerObj.transform.position = tilePos; // Player => TilePos

            MapGenerator.instance.totalMap[(int)playerPos.x, (int)playerPos.y].SetCoord((int)playerPos.x, (int)playerPos.z, false);
            MapGenerator.instance.totalMap[(int)tilePos.x, (int)tilePos.y].SetCoord((int)tilePos.x, (int)tilePos.z, true);

            WizardCardData.instance.shouldTeleport = false;
            isTeleport = false;
        }

        if (isPosSwap)
        {
            Vector3 playerPos = WizardCardData.instance.playerPos;
            Vector3 monsterPos = WizardCardData.instance.targetPos;

            cardProcessing.selectedTarget.transform.position = playerPos; // Monster => PlayerPos
            cardProcessing.currentPlayerObj.transform.position = monsterPos; // Player => MonsterPos

            MapGenerator.instance.totalMap[(int)playerPos.x, (int)playerPos.z].SetCoord((int)playerPos.x, (int)playerPos.z, true);
            MapGenerator.instance.totalMap[(int)monsterPos.x, (int)monsterPos.z].SetCoord((int)monsterPos.x, (int)monsterPos.z, true);

            WizardCardData.instance.shouldPosSwap = false;
            isPosSwap = false;
        }

        if (isFireball)
        {
            GameObject particlePrefab = ParticleController.instance.fireballEffectPrefab;
            GameObject playerObj = cardProcessing.currentPlayerObj;
            GameObject targetObj = cardProcessing.selectedTarget;
            Card useCard = cardManager.useCard;

            SoundManager.instance.PlaySoundEffect("Fireball");

            StartCoroutine(ParticleController.instance.ProjectileEffect(particlePrefab, playerObj, targetObj));

            Monster monster = targetObj.GetComponent<Monster>();
            monster.GetHit(useCard.cardPower[0]);

            CharacterStatusEffect characterStatusEffect = targetObj.GetComponent<CharacterStatusEffect>();
            StatusEffectManager.instance.ApplyBurnEffect(characterStatusEffect);

            WizardCardData.instance.shouldFireball = false;
            isFireball = false;
        }

        if (isFlamePillar)
        {
            SoundManager.instance.PlaySoundEffect("FlamePillar");

            foreach (Monster monster in MapGenerator.instance.rangeInMonsters)
            {
                GameObject particlePrefab = ParticleController.instance.flamePillarEffectPrefab;
                Quaternion particleRot = Quaternion.Euler(270f, 0f, 0f);
                Vector3 targetObjPos = monster.gameObject.transform.position + new Vector3(0, 0.15f, 0);
                Card useCard = cardManager.useCard;

                ParticleController.instance.ApplyTargetEffect(particlePrefab, targetObjPos, particleRot, 0.5f);

                CharacterStatusEffect characterStatusEffect = monster.GetComponent<CharacterStatusEffect>();
                StatusEffectManager.instance.ApplyBurnEffect(characterStatusEffect);

                monster.GetHit(useCard.cardPower[0]);
            }

            WizardCardData.instance.shouldFlamePillar = false;
            isFlamePillar = false;
        }

        if (isLifeDrain)
        {
            Card useCard = cardManager.useCard;
            Player player = cardProcessing.currentPlayer;
            float healAmount = 0;

            foreach (Monster monster in MapGenerator.instance.rangeInMonsters)
            {
                GameObject particlePrefab = ParticleController.instance.lifeDrainEffectPrefab;
                Quaternion particleRot = Quaternion.Euler(0f, 0f, 0f);
                Vector3 targetObjPos = monster.gameObject.transform.position + new Vector3(0, 0.15f, 0);

                ParticleController.instance.ApplyTargetEffect(particlePrefab, targetObjPos, particleRot, 0.5f);

                monster.GetHit(useCard.cardPower[0]);
                healAmount += useCard.cardPower[0] / 7f;
            }

            if (player.playerData.Hp + healAmount >= player.playerData.MaxHp)
            {
                player.playerData.Hp = player.playerData.MaxHp;
            }
            else
            {
                player.playerData.Hp += healAmount;
            }

            WizardCardData.instance.shouldLifeDrain = false;
            isLifeDrain = false;
        }

        if (isSummonObstacle)
        {
            Vector3 tilePos = WizardCardData.instance.targetPos;
            Vector3 goalPosition = tilePos + new Vector3(0, 0.35f, 0);

            StartCoroutine(ParticleController.instance.ObjectElevateEffect(tilePos, goalPosition));

            Tile tile = MapGenerator.instance.totalMap[(int)tilePos.x, (int)tilePos.z];
            tile.SetCoord((int)tilePos.x, (int)tilePos.z, true);

            WizardCardData.instance.shouldSummon = false;
            isSummonObstacle = false;
        }
    }

    private void TeleportAnimationEvent()
    {
        if (WizardCardData.instance.shouldTeleport)
        {
            isTeleport = true;
        }
        else if (WizardCardData.instance.shouldPosSwap)
        {
            isPosSwap = true;
        }
    }

    private void SummonAnimationEvent()
    {
        if (WizardCardData.instance.shouldSummon)
        {
            isSummonObstacle = true;
        }
    }

    private void FireballAnimationEvent()
    {
        if (WizardCardData.instance.shouldFireball)
        {
            isFireball = true;
        }
    }


    private void FlamePillarAnimationEvent()
    {
        if (WizardCardData.instance.shouldFlamePillar)
        {
            isFlamePillar = true;
        }
    }

    private void LifeDrainAnimationEvent()
    {
        if (WizardCardData.instance.shouldLifeDrain)
        {
            isLifeDrain = true;
        }
    }
}
