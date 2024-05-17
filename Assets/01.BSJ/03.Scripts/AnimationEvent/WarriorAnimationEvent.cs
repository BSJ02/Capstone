using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class WarriorAnimationEvent : MonoBehaviour
{
    private CardManager cardManager;
    private CardProcessing cardProcessing;
    private ParticleController particleController;

    private bool isSpinAttack = false;
    private bool isShieldBash = false;
    private bool isDesperateStrike = false;
    private bool isDash = false;

    private void Awake()
    {
        cardManager = FindObjectOfType<CardManager>();
        cardProcessing = FindObjectOfType<CardProcessing>();
        particleController = FindObjectOfType<ParticleController>();
    }


    private void Update()
    {
        if (isSpinAttack)
        {
            GameObject particlePrefab = particleController.spinAttackEffectPrefab;
            GameObject playerObj = cardProcessing.currentPlayerObj;
            Card useCard = cardManager.useCard;

            Quaternion rotation = Quaternion.Euler(90, 0, 0);
            float height = 0.5f;

            particleController.ApplyPlayerEffect(particlePrefab, playerObj, height, rotation);

            foreach (Monster monster in MapGenerator.instance.rangeInMonsters)
            {
                monster.GetHit(useCard.cardPower[0]);
            }

            WarriorCardData.instance.shouldSpinAttack = false;
            isSpinAttack = false;
        }

        if (isShieldBash)
        {
            GameObject particlePrefab = particleController.shieldBashEffectPrefab;
            Monster monster = cardProcessing.selectedTarget.GetComponent<Monster>();

            float damage = cardProcessing.currentPlayer.playerData.Armor;

            Vector3 targetObjPos = monster.transform.position + new Vector3(0, 0.5f, 0);

            particleController.ApplyTargetEffect(particlePrefab, targetObjPos, Quaternion.identity, 0.5f);

            monster.GetHit(damage);
            Debug.Log(damage);

            WarriorCardData.instance.shouldShieldBash = false;
            isShieldBash = false;
        }

        if (isDesperateStrike)
        {
            GameObject particlePrefab = particleController.desperateStrikeEffectPrefab;
            Player player = cardProcessing.currentPlayer;
            Monster monster = cardProcessing.selectedTarget.GetComponent<Monster>();

            Vector3 targetPos = monster.transform.position + new Vector3(0, 0.5f, 0);

            float damage = (player.playerData.MaxHp - player.playerData.Hp + 1) / player.playerData.MaxHp * 200;

            particleController.ApplyTargetEffect(particlePrefab, targetPos, Quaternion.identity, 0f);

            monster.GetHit(damage);

            WarriorCardData.instance.shouldDesperateStrike = false;
            isDesperateStrike = false;
        }

        if (isDash)
        {
            GameObject particlePrefab = particleController.dashEffectPrefab;
            GameObject playerObj = cardProcessing.currentPlayerObj;
            Monster monster = cardProcessing.selectedTarget.GetComponent<Monster>();
            Card useCard = cardManager.useCard;

            float distance = 1;

            Vector3 opponentDirection = monster.transform.forward;
            Vector3 goalPos = cardProcessing.selectedTarget.transform.position - opponentDirection * distance;
            Vector3 targetPos = monster.transform.position;

            Tile tile = MapGenerator.instance.totalMap[(int)goalPos.x, (int)goalPos.z];
            
            while (tile.coord.isWall == true)
            {
                distance++;
                goalPos = cardProcessing.selectedTarget.transform.position - opponentDirection * distance;
                tile = MapGenerator.instance.totalMap[(int)goalPos.x, (int)goalPos.z];
            }

            StartCoroutine(particleController.PlayerMoveEffect(playerObj, goalPos));

            particleController.ApplyTargetEffect(particlePrefab, targetPos, Quaternion.identity, 0.2f);

            monster.GetHit(useCard.cardPower[0]);
        
            WarriorCardData.instance.shouldDash = false;
            isDash = false;
        }
    }

    private void SpinAttackAnimationEvent()
    {
        if (WarriorCardData.instance.shouldSpinAttack)
        {
            isSpinAttack = true;
        }
    }

    private void ShieldBashAnimationEvent()
    {
        if (WarriorCardData.instance.shouldShieldBash)
        {
            isShieldBash = true;
        }
    }

    private void DesperateStrikeAnimationEvent()
    {
        if (WarriorCardData.instance.shouldDesperateStrike)
        {
            isDesperateStrike = true;
        }
    }

    private void DashAnimationEvent()
    {
        if (WarriorCardData.instance.shouldDash)
        {
            isDash = true;
        }
    }

}
