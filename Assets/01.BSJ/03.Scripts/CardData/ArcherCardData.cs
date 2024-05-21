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
    // Holy Nova
    public void UseLightningStrike(Card card, GameObject selectedTarget)
    {
        Monster monster = selectedTarget.GetComponent<Monster>();
        if (monster != null)
        {
            Debug.Log(card.cardName + " / TargetName: " + monster);
            monster.GetHit(card.cardPower[0]);

            cardProcessing.currentPlayer.ChargeAnim(selectedTarget);
        }
        else
        {
            cardProcessing.waitForInput = true;
        }
    }

    public void WallJump(Card card, GameObject selectedTarget)
    {
        Monster monster = selectedTarget.GetComponent<Monster>();

        if (monster != null)
        {
            Debug.Log(card.cardName + " / TargetName: " + monster);
            monster.GetHit(card.cardPower[0]);

        }
        else
        {
            cardProcessing.waitForInput = true;
        }
    }


    public void UsePoisonArrow(Card card, GameObject selectedTarget)
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

    public void Concealment(Card card, GameObject selectedTarget)
    {
        Player player = selectedTarget.GetComponent<Player>();

        if (player != null)
        {
            /* 
             * Concealment transparency 
             material.color.a = 0.5f 
              

            scanRendererMesh use
            become transparency 0.5
       
             */
            particleController.ApplyPlayerEffect(particleController.healEffectPrefab, selectedTarget);
        }
        else
        {
            cardProcessing.waitForInput = true;
        }
    }

    public void AgilityAttack(Card card, GameObject selectedTarget)
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

    public void TurnCountAttack(Card card, GameObject selectedTarget)
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

    public void MarkTargetArrow(Card card, GameObject selectedTarget)
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

    public void DoubleTargetArrow(Card card, GameObject selectedTarget)
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

    public void PoisonArrow(Card card, GameObject selectedTarget)
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

    public void AimedArrow(Card card, GameObject selectedTarget)
    {
        Monster monster = selectedTarget.GetComponent<Monster>();
        if (monster != null)
        {
            Debug.Log(card.cardName + " / TargetName: " + monster);
            monster.GetHit(card.cardPower[0]);
            monster.monsterData.Hp += card.cardPower[3];

        }
        else
        {
            cardProcessing.waitForInput = true;
        }

    }
}
