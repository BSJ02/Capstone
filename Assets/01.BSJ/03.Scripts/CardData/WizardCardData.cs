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
    private ParticleController particleController;

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

    // Wizard Cards --------------------------------
    // Teleport
    public void UseTeleport(Card card, GameObject selectedTarget)
    {
        Tile tile = selectedTarget.GetComponent<Tile>();
        if (tile != null)
        {
            targetPos = tile.transform.position + new Vector3(0, 0.35f, 0);

            shouldTeleport = true;
            cardProcessing.currentPlayer.ChargeAnim(selectedTarget);

            particleController.ApplyPlayerEffect(particleController.teleportEffectPrefab, cardProcessing.currentPlayerObj);
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
            playerPos = cardProcessing.currentPlayerObj.transform.position;

            shouldPosSwap = true;

            cardProcessing.currentPlayer.ChargeAnim(selectedTarget);

            particleController.ApplyPlayerEffect(particleController.teleportEffectPrefab, cardProcessing.currentPlayerObj);
            particleController.ApplyPlayerEffect(particleController.teleportEffectPrefab, selectedTarget);
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
        if (player != null)
        {
            player.ChargeAnim(selectedTarget);

            particleController.ApplyPlayerEffect(particleController.healEffectPrefab, selectedTarget);

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

    // Summon Obstacle
    public void UseSummonObstacle(Card card, GameObject selectedTarget)
    {
        Tile tile = selectedTarget.GetComponent<Tile>();
        Player player = cardProcessing.currentPlayer;
        if (tile != null)
        {
            targetPos = tile.transform.position;

            shouldSummon = true;

            player.ChargeAnim(selectedTarget);

            particleController.ApplyPlayerEffect(particleController.teleportEffectPrefab, selectedTarget, 0.35f);

            cardProcessing.cardUseDistance = card.cardDistance;
        }
        else
        {
            cardProcessing.waitForInput = true;
        }
    }
}
