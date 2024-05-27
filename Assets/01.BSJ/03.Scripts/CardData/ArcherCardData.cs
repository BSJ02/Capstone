using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherCardData : MonoBehaviour
{
    public static ArcherCardData instance;

    private WeaponController weaponController;
    private ParticleController particleController;

    [Header(" # CardProcessing")]
    public CardProcessing cardProcessing;

    [HideInInspector] public Vector3 tempPos;
    [HideInInspector] public Vector3 targetPos;
    [HideInInspector] public Vector3 playerPos;

    [HideInInspector] public bool shouldWallJump = false;
    [HideInInspector] public bool shouldConcealment = false;
    [HideInInspector] public bool shouldAgility = false;
    [HideInInspector] public bool shouldPowerOfTurn = false;
    [HideInInspector] public bool shouldMarkAttack = false;
    [HideInInspector] public bool shouldTripleShot = false;
    [HideInInspector] public bool shouldPoisonAttack = false;
    [HideInInspector] public bool shouldAimedShot = false;
    
    private PlayerState playerState;

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

    // Archer Cards --------------------------------
    public void UseWallJump(Card card, GameObject selectedTarget)
    {
        Tile tile = selectedTarget.GetComponent<Tile>();
        if (tile != null && tile.coord.isWall)
        {
            shouldWallJump = true;

            cardProcessing.currentPlayer.ChargeAnim(selectedTarget);

            // 전사 카드 응용
            particleController.ApplyPlayerEffect(particleController.teleportEffectPrefab, cardProcessing.currentPlayerObj);
        }
        else
        {
            cardProcessing.waitForInput = true;
        }
    }

    public void UseConcealment(Card card, GameObject selectedTarget)
    {
        Monster monster = selectedTarget.GetComponent<Monster>();
        if (monster != null)
        {
            Debug.Log(card.cardName + " / TargetName: " + monster);
            monster.GetHit(card.cardPower[0]);

            //animation

        }
        else
        {
            cardProcessing.waitForInput = true;
        }
    }

    public void UseAgility(Card card, GameObject selectedTarget)
    {
        Monster monster = selectedTarget.GetComponent<Monster>();
        if (monster != null)
        {
            shouldAgility = true;

            cardProcessing.currentPlayer.MacigAttack02Anim(selectedTarget);

            cardProcessing.cardUseDistance = card.cardDistance;
        }
        else
        {
            cardProcessing.waitForInput = true;
        }
    }

    public void UsePowerOfTurn(Card card, GameObject selectedTarget)
    {
        Monster monster = selectedTarget.GetComponent<Monster>();
        Player player = selectedTarget.GetComponent<Player>();
        if (monster != null)
        {
            Debug.Log(card.cardName + " / TargetName: " + monster);
            monster.GetHit(card.cardPower[0]);
            //monster.monsterData.Hp -= card.cardPower[0];
            //animation
            monster.monsterData.Hp -= card.cardPower[0] + card.cardDistance;
            particleController.ApplyPlayerEffect(particleController.healEffectPrefab, selectedTarget);
            cardProcessing.currentPlayer.AttackOneAnim(selectedTarget);
        }
        else
        {
            cardProcessing.waitForInput = true;
        }

    }

    public void UseMarkAttack(Card card, GameObject selectedTarget)
    {
        Monster monster = selectedTarget.GetComponent<Monster>();
        if (monster != null)
        {
            Debug.Log(card.cardName + " / TargetName: " + monster);
            monster.GetHit(card.cardPower[0]);
            //monster.monsterData.Hp -= ArcherData.ActivePoint;
            //monster.monsterData.Hp -= card.cardPower[0];

            //animation
            cardProcessing.currentPlayer.AttackOneAnim(selectedTarget);

        }
        else
        {
            cardProcessing.waitForInput = true;
        }

    }

    public void UseTripleShot(Card card, GameObject selectedTarget)
    {
        Monster monster = selectedTarget.GetComponent<Monster>();
        if (monster != null)
        {
            Debug.Log(card.cardName + " / TargetName: " + monster);
            monster.GetHit(card.cardPower[0]);
            cardProcessing.currentPlayer.AttackOneAnim(selectedTarget);
            monster.monsterData.Hp -= card.cardPower[0];
        }
        else
        {
            cardProcessing.waitForInput = true;
        }

    }

    public void UsePoisonAttack(Card card, GameObject selectedTarget)
    {
        Monster monster = selectedTarget.GetComponent<Monster>();
        if (monster != null)
        {
            Debug.Log(card.cardName + " / TargetName: " + monster);
            monster.GetHit(card.cardPower[0]);

            monster.monsterData.Hp -= card.cardPower[0];

        }
        else
        {
            cardProcessing.waitForInput = true;
        }

    }

    public void UseAimedShot(Card card, GameObject selectedTarget)
    {
        Monster monster = selectedTarget.GetComponent<Monster>();
        if (monster != null)
        {
            particleController.ApplyPlayerEffect(particleController.healEffectPrefab, selectedTarget);
            Debug.Log(card.cardName + " / TargetName: " + monster);
            monster.GetHit(card.cardPower[0]);
            monster.monsterData.Hp -= card.cardPower[2];
            //animation
            //player.AttackTwoAnim(selectedTarget);


        }
        else
        {
            cardProcessing.waitForInput = true;
        }

    }
}
