using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

public class CardData : MonoBehaviour
{
    private WeaponController weaponController;
    private CardProcessing cardProcessing;
    private ParticleController particleController;

    private PlayerState playerState;


    // Wizard variables
    [HideInInspector] public bool shouldTeleport = false;
    [HideInInspector] public bool shouldPosSwap = false;
    [HideInInspector] public bool shouldFireball = false;

    [HideInInspector] public Vector3 tempPos;
    [HideInInspector] public Vector3 targetPos;
    [HideInInspector] public Vector3 playerPos;

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
        }
        else
        {
            cardProcessing.waitForInput = true;
        }
    }

    public void AgilityAttack(Card card, GameObject selectedTarget)
    {
        Monster monster = selectedTarget.GetComponent<Monster>();
        if (monster != null)
        {
            Debug.Log(card.cardName + " / TargetName: " + monster);
            monster.GetHit(card.cardPower[0]);
            //monster.monsterData.Hp -= card.cardPower[0];
            //animation


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
        if (MapGenerator.instance.rangeInMonsters != null)
        {
            foreach (Monster monster in MapGenerator.instance.rangeInMonsters)
            {
                monster.GetHit(card.cardPower[0]);
            }

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
        if (MapGenerator.instance.rangeInMonsters != null)
        {
            foreach (Monster monster in MapGenerator.instance.rangeInMonsters)
            {
                monster.GetHit(card.cardPower[0]);
            }

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
