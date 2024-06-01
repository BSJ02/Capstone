using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UIElements;

public class WarriorAnimationEvent : MonoBehaviour
{
    private CardManager cardManager;
    private CardProcessing cardProcessing;

    private bool isSpinAttack = false;
    private bool isShieldBash = false;
    private bool isDesperateStrike = false;
    private bool isDash = false;
    private bool isWarriorsRoar = false;
    private bool isArmorCrush = false;

    private void Awake()
    {
        cardManager = FindObjectOfType<CardManager>();
        cardProcessing = FindObjectOfType<CardProcessing>();
    }


    private void Update()
    {
        if (isSpinAttack)
        {
            GameObject particlePrefab = ParticleController.instance.spinAttackEffectPrefab;
            GameObject playerObj = cardProcessing.currentPlayerObj;
            Card useCard = cardManager.useCard;

            Quaternion rotation = Quaternion.Euler(90, 0, 0);
            float height = 0.85f;

            SoundManager.instance.PlaySoundEffect("SpinAttack");

            ParticleController.instance.ApplyPlayerEffect(particlePrefab, playerObj, height, rotation, 1.3f);

            foreach (Monster monster in MapGenerator.instance.rangeInMonsters)
            {
                monster.GetHit(useCard.cardPower[0]);
            }

            WarriorCardData.instance.shouldSpinAttack = false;
            isSpinAttack = false;
        }

        if (isShieldBash)
        {
            GameObject particlePrefab = ParticleController.instance.shieldBashEffectPrefab;
            Monster monster = cardProcessing.selectedTarget.GetComponent<Monster>();

            float damage = cardProcessing.currentPlayer.playerData.Armor;

            Vector3 targetObjPos = monster.transform.position + new Vector3(0, 0.5f, 0);

            SoundManager.instance.PlaySoundEffect("ShieldBash");

            ParticleController.instance.ApplyTargetEffect(particlePrefab, targetObjPos, Quaternion.identity, 0.5f);

            monster.GetHit(damage);
            Debug.Log(damage);

            WarriorCardData.instance.shouldShieldBash = false;
            isShieldBash = false;
        }

        if (isDesperateStrike)
        {
            GameObject particlePrefab = ParticleController.instance.desperateStrikeEffectPrefab;
            Player player = cardProcessing.currentPlayer;
            Monster monster = cardProcessing.selectedTarget.GetComponent<Monster>();

            Vector3 targetPos = monster.transform.position + new Vector3(0, 0.5f, 0);

            float damage = (player.playerData.MaxHp - player.playerData.Hp + 1) / player.playerData.MaxHp * 200;

            SoundManager.instance.PlaySoundEffect("DesperateStrike");

            ParticleController.instance.ApplyTargetEffect(particlePrefab, targetPos, Quaternion.identity, 0f);

            monster.GetHit(damage);

            WarriorCardData.instance.shouldDesperateStrike = false;
            isDesperateStrike = false;
        }

        if (isDash)
        {
            GameObject particlePrefab = ParticleController.instance.dashEffectPrefab;
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

            StartCoroutine(ParticleController.instance.PlayerMoveEffect(playerObj, goalPos));

            ParticleController.instance.ApplyTargetEffect(particlePrefab, targetPos, Quaternion.identity, 0.2f);

            SoundManager.instance.PlaySoundEffect("Dash");

            monster.GetHit(useCard.cardPower[0]);
        
            WarriorCardData.instance.shouldDash = false;
            isDash = false;
        }

        if (isWarriorsRoar)
        {
            GameObject particlePrefab_Player = ParticleController.instance.WarriorsRoarEffectPrefab;
            GameObject particlePrefab_OtherPlayer = ParticleController.instance.healEffectPrefab;
            Player player = cardProcessing.currentPlayerObj.GetComponent<Player>();

            float height = 1.2f;
            float scale = 0.45f;

            SoundManager.instance.PlaySoundEffect("WarriorsRoar");

            ParticleController.instance.ApplyPlayerEffect(particlePrefab_Player, player.gameObject, height, Quaternion.identity, scale);

            player.playerData.Hp -= player.playerData.MaxHp * 0.1f;


            foreach (Player otherPlayer in MapGenerator.instance.rangeInPlayers)
            {
                float healAmount = otherPlayer.playerData.Hp * 0.5f;
                Vector3 targetPos = otherPlayer.transform.position + new Vector3(0, 0.5f, 0);

                ParticleController.instance.ApplyTargetEffect(particlePrefab_OtherPlayer, targetPos, Quaternion.identity, 0.2f);

                if (player.playerData.Hp + healAmount >= player.playerData.MaxHp)
                {
                    player.playerData.Hp = player.playerData.MaxHp;
                }
                else
                {
                    player.playerData.Hp += healAmount;
                }
            }

            WarriorCardData.instance.shouldWarriorsRoar = false;
            isWarriorsRoar = false;
        }

        if (isArmorCrush)
        {
            GameObject particlePrefab = ParticleController.instance.ArmorCrushEffectPrefab;
            Monster monster = cardProcessing.selectedTarget.GetComponent<Monster>();
            Card useCard = cardManager.useCard;
            Vector3 targetPos = monster.transform.position + new Vector3(0, 0.5f, 0);

            ParticleController.instance.ApplyTargetEffect(particlePrefab, targetPos, Quaternion.identity, 0f);

            monster.GetHit(useCard.cardPower[0]);
            monster.monsterData.Amor *= 9 / 10;

            WarriorCardData.instance.shouldArmorCrush = false;
            isArmorCrush = false;
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

    private void WarriorsRoarAnimationEvent()
    {
        if (WarriorCardData.instance.shouldWarriorsRoar)
        {
            isWarriorsRoar = true;
        }
    }

    private void ArmorCrushAnimationEvent()
    {
        if (WarriorCardData.instance.shouldArmorCrush)
        {
            isArmorCrush = true;
        }
    }

}
