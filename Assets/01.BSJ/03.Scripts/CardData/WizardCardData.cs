using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class WizardCardData : MonoBehaviour
{
    public static WizardCardData instance;

    private WeaponController weaponController;

    [Header(" # CardProcessing")]
    public CardProcessing cardProcessing;

    private PlayerState playerState;

    [HideInInspector] public Vector3 tempPos;
    [HideInInspector] public Vector3 targetPos;
    [HideInInspector] public Vector3 playerPos;

    // Wizard variables
    [HideInInspector] public bool shouldTeleport = false;
    [HideInInspector] public bool shouldPosSwap = false;
    [HideInInspector] public bool shouldFireball = false;
    [HideInInspector] public bool shouldSummon = false;
    [HideInInspector] public bool shouldFlamePillar = false;
    [HideInInspector] public bool shouldLifeDrain = false;
    [HideInInspector] public bool usingMagicShield = false;

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

    // Wizard Cards --------------------------------
    // Teleport
    public void UseTeleport(Card card, GameObject selectedTarget)
    {
        Tile tile = selectedTarget.GetComponent<Tile>();
        if (tile != null && !tile.coord.isWall)
        {
            targetPos = tile.transform.position + new Vector3(0, 0.35f, 0);

            SoundManager.instance.PlaySoundEffect("Teleport");

            shouldTeleport = true;
            cardProcessing.currentPlayer.ChargeAnim(selectedTarget);

            ParticleController.instance.ApplyPlayerEffect(ParticleController.instance.teleportEffectPrefab, cardProcessing.currentPlayerObj);
        }
        else
        {
            cardProcessing.waitForInput = true;
        }
    }

    // Position Swap
    public void UsePositionSwap(Card card, GameObject selectedTarget)
    {
        Monster monster = selectedTarget.GetComponent<Monster>();
        if (monster != null)
        {
            targetPos = monster.transform.position;

            SoundManager.instance.PlaySoundEffect("Teleport");

            playerPos = cardProcessing.currentPlayerObj.transform.position;

            shouldPosSwap = true;

            cardProcessing.currentPlayer.ChargeAnim(selectedTarget);

            ParticleController.instance.ApplyPlayerEffect(ParticleController.instance.teleportEffectPrefab, cardProcessing.currentPlayerObj);
            ParticleController.instance.ApplyPlayerEffect(ParticleController.instance.teleportEffectPrefab, selectedTarget);
        }
        else
        {
            cardProcessing.waitForInput = true;
        }
    }

    // Fireball
    public void UseFireball(Card card, GameObject selectedTarget)
    {
        Monster monster = selectedTarget.GetComponent<Monster>();
        if (monster != null)
        {
            shouldFireball = true;
            cardProcessing.currentPlayer.MacigAttack02Anim(selectedTarget);
            cardProcessing.cardUseDistance = card.cardDistance;
        }
        else
        {
            cardProcessing.waitForInput = true;
        }
    }

    // Flame Pillar
    public void UseFlamePillar(Card card, GameObject selectedTarget)
    {
        Player player = cardProcessing.currentPlayer;
        if (MapGenerator.instance.rangeInMonsters != null)
        {
            shouldFlamePillar = true;
            player.MacigAttack03Anim(selectedTarget);
            cardProcessing.cardUseDistance = card.cardDistance;
        }
        else
        {
            cardProcessing.waitForInput = true;
        }
    }

    // Life Drain
    public void UseLifeDrain(Card card, GameObject selectedTarget)
    {
        Player player = cardProcessing.currentPlayer;
        if (MapGenerator.instance.rangeInMonsters != null)
        {
            SoundManager.instance.PlaySoundEffect("LifeDrain");

            shouldLifeDrain = true;
            player.MacigAttack03Anim(selectedTarget);
            cardProcessing.cardUseDistance = card.cardDistance;
        }
        else
        {
            cardProcessing.waitForInput = true;
        }
    }

    // Magic Shield
    public void UseMagicShield(Card card, GameObject selectedTarget)
    {
        Player player = cardProcessing.currentPlayer;
        GameObject playerObj = cardProcessing.currentPlayerObj;
        if (player != null)
        {
            GameObject particlePrefab = ParticleController.instance.magicShieldEffectPrefab;

            player.ChargeAnim(selectedTarget);

            ParticleController.instance.ApplyPlayerEffect(particlePrefab, playerObj);

            usingMagicShield = true;
        }
        else
        {
            cardProcessing.waitForInput = true;
        }
    }

    // Summon Obstacle
    public void UseSummonObstacle(Card card, GameObject selectedTarget)
    {
        Tile tile = selectedTarget.GetComponent<Tile>();
        Player player = cardProcessing.currentPlayer;
        if (tile != null && !tile.coord.isWall)
        {
            shouldSummon = true;

            targetPos = tile.transform.position;

            player.ChargeAnim(selectedTarget);

            GameObject particlePrefab = ParticleController.instance.teleportEffectPrefab;
            SoundManager.instance.PlaySoundEffect("SummonObstacle");

            ParticleController.instance.ApplyPlayerEffect(particlePrefab, selectedTarget, 0.35f, Quaternion.identity , 1);

            cardProcessing.cardUseDistance = card.cardDistance;
        }
        else
        {
            cardProcessing.waitForInput = true;
        }
    }
}
