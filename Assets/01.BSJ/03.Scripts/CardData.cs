using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CardData : MonoBehaviour
{
    private WeaponController weaponController;
    private CardProcessing cardProcessing;
    private ParticleController particleController;

    private PlayerState playerState;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        particleController = FindObjectOfType<ParticleController>();
        cardProcessing = FindObjectOfType<CardProcessing>();   
    }

    // Base Cards --------------------------------
    // Healing Potion
    public void UseHealingPotion(Card card, GameObject selectedTarget)
    {
        if (cardProcessing.currentPlayer != null)
        {
            cardProcessing.currentPlayer.ChargeAnim(selectedTarget);

            particleController.ApplyPlayerEffect(particleController.healEffectPrefab, selectedTarget);
            if (cardProcessing.currentPlayer.playerData.Hp + card.cardPower[0] >= cardProcessing.currentPlayer.playerData.MaxHp)
            {
                cardProcessing.currentPlayer.playerData.Hp = cardProcessing.currentPlayer.playerData.MaxHp;
            }
            else
            {
                cardProcessing.currentPlayer.playerData.Hp += card.cardPower[0];
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
        if (cardProcessing.currentPlayer != null)
        {
            cardProcessing.currentPlayer.ChargeAnim(selectedTarget);

            cardProcessing.currentPlayer.playerData.Hp += card.cardPower[0];
        }
        else
        {
            cardProcessing.waitForInput = true;
        }
    }

    // Evasion Boost
    public void UseEvasionBoost(Card card, GameObject selectedTarget)
    {
        if (cardProcessing.currentPlayer != null)
        {
            cardProcessing.currentPlayer.ChargeAnim(selectedTarget);

            cardProcessing.currentPlayer.playerData.activePoint += (int)card.cardPower[0] + cardProcessing.TempActivePoint;
        }
        else
        {
            cardProcessing.waitForInput = true;
        }
    }

    // Transmission
    public void UseTransmission(Card card, GameObject selectedTarget)
    {
        Monster monster = selectedTarget.GetComponent<Monster>();
        if (monster != null)
        {
            cardProcessing.currentPlayer.ChargeAnim(selectedTarget);

            monster.GetHit(card.cardPower[0]);
        }
        else
        {
            cardProcessing.waitForInput = true;
        }
    }

    // Stat Boost
    public void UseStatBoost(Card card, GameObject selectedTarget)
    {
        if (cardProcessing.currentPlayer != null)
        {
            cardProcessing.currentPlayer.ChargeAnim(selectedTarget);

            particleController.ApplyPlayerEffect(particleController.buffEffectPrefab, selectedTarget);

            int randNum = Random.Range(0, 3);
            switch (randNum)
            {
                case 0:
                    cardProcessing.currentPlayer.playerData.Armor += 20;

                    break;
                case 1:
                    cardProcessing.currentPlayer.playerData.MaxHp += 20;

                    break;
                case 2:
                    cardProcessing.currentPlayer.playerData.MaxActivePoint += 1;

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

    // Rest
    public void UseRest(Card card, GameObject selectedTarget)
    {
        if (cardProcessing.currentPlayer != null)
        {
            cardProcessing.currentPlayer.ChargeAnim(selectedTarget);

            particleController.ApplyPlayerEffect(particleController.healEffectPrefab, selectedTarget);

            if (cardProcessing.currentPlayer.playerData.Hp + card.cardPower[0] >= cardProcessing.currentPlayer.playerData.MaxHp)
            {
                cardProcessing.currentPlayer.playerData.Hp = cardProcessing.currentPlayer.playerData.MaxHp;
            }
            else
            {
                cardProcessing.currentPlayer.playerData.Hp += card.cardPower[0];
            }
        }
        else
        {
            cardProcessing.waitForInput = true;
        }
    }

    // Warrior Cards --------------------------------
    // Ax Slash
    public void UseAxSlash(Card card, GameObject selectedTarget)
    {
        Monster monster = selectedTarget.GetComponent<Monster>();
        if (monster != null)
        {

            Debug.Log(card.cardName + " / TargetName: " + monster);
            monster.GetHit(card.cardPower[0]);
            cardProcessing.cardUseDistance = card.cardPower[1];
        }
        else
        {
            cardProcessing.waitForInput = true;
        }
    }

    // Heal!!
    public void UseHeal(Card card, GameObject selectedTarget)
    {
        Player player = selectedTarget.GetComponent<Player>();
        if (player != null)
        {
            player.ChargeAnim(selectedTarget);

            Debug.Log(card.cardName + " ī�带 ��� / " + player + " Hp: " + player.playerData.Hp);
            player.playerData.Hp += card.cardPower[0];
            cardProcessing.cardUseDistance = card.cardPower[1];
        }
        else
        {
            cardProcessing.waitForInput = true;
        }
    }

    // Guardian Spirit
    public void UseGuardianSpirit(Card card, GameObject selectedTarget)
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

    // Archer Cards --------------------------------
    // Holy Nova
    public void UseHolyNova(Card card, GameObject selectedTarget)
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

    // Lightning Strike
    public void UseLightningStrike(Card card, GameObject selectedTarget)
    {
        Monster monster = selectedTarget.GetComponent<Monster>();
        if (monster != null)
        {
            Debug.Log(card.cardName + " / TargetName: " + monster);
            monster.GetHit(card.cardPower[0]);

            cardProcessing.currentPlayer.MacigAttack02Anim(selectedTarget);
        }
        else
        {
            cardProcessing.waitForInput = true;
        }
    }
    

    public void UsePoisonArrow(Card card, GameObject selectedTarget)
    {
        Monster monster = selectedTarget.GetComponent<Monster>(); 
        if(monster!= null)
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

    public void TargetArrow(Card card, GameObject selectedTarget)
    {
        Monster monster = selectedTarget.GetComponent<Monster>();
        if (monster != null)
        {
            Debug.Log(card.cardName + " / TargetName: " + monster);

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

        }
        else
        {
            cardProcessing.waitForInput = true;
        }

    }

    // Wizard Cards --------------------------------
    // Teleport
    public bool shouldTeleport = false;
    public Vector3 teloportPos;

    public void UseTeleport(Card card, GameObject selectedTarget)
    {
        Tile tile = selectedTarget.GetComponent<Tile>();

        if (tile != null)
        {
            shouldTeleport = true;
            cardProcessing.currentPlayer.ChargeAnim(selectedTarget);

            teloportPos = tile.transform.position;
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
            
        }
        else
        {
            cardProcessing.waitForInput = true;
        }
    }

    // Fireball
    public bool shouldFireball = false;
    public void UseFireball(Card card, GameObject selectedTarget)
    {
        Monster monster = selectedTarget.GetComponent<Monster>();
        if (monster != null)
        {
            shouldFireball = true;
            cardProcessing.currentPlayer.ChargeAnim(selectedTarget);
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
        Monster monster = selectedTarget.GetComponent<Monster>();
        if (monster != null)
        {
            Debug.Log(card.cardName + " / TargetName: " + monster);
            monster.GetHit(card.cardPower[0]);

            cardProcessing.currentPlayer.ChargeAnim(selectedTarget);

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
        Monster monster = selectedTarget.GetComponent<Monster>();
        if (monster != null)
        {
            
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
        if (cardProcessing.currentPlayer != null)
        {
            
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
        if (tile != null)
        {

            cardProcessing.currentPlayer.ChargeAnim(selectedTarget);

            cardProcessing.cardUseDistance = card.cardDistance;
        }
        else
        {
            cardProcessing.waitForInput = true;
        }
    }
}
