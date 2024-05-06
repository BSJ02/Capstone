using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class CardData : MonoBehaviour
{
    private WeaponController weaponController;
    private CardProcessing cardProcessing;
    private ParticleController particleController;

    [Header(" # Player Scripts")] public Player player;

    [Header(" # Player Object")] public GameObject playerObject;

    private PlayerState playerState;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        particleController = FindObjectOfType<ParticleController>();
        cardProcessing = FindObjectOfType<CardProcessing>();
        weaponController = playerObject.GetComponent<WeaponController>();
        player = playerObject.GetComponent<Player>();
    }

    // Base Cards --------------------------------
    // Healing Potion
    public void UseHealingPotion(Card card, GameObject selectedTarget)
    {
        Player player = selectedTarget.GetComponent<Player>();

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
            cardProcessing.cardUseDistance = card.cardPower[1];
        }
        else
        {
            cardProcessing.waitForInput = true;
        }
    }

    // Remove Ailments
    public void UseRemoveAilments(Card card, GameObject selectedTarget)
    {
        Player player = selectedTarget.GetComponent<Player>();

        if (player != null)
        {
            player.ChargeAnim(selectedTarget);

            player.playerData.Hp += card.cardPower[0];
            cardProcessing.cardUseDistance = card.cardPower[1];
        }
        else
        {
            cardProcessing.waitForInput = true;
        }
    }

    // Evasion Boost
    public void UseEvasionBoost(Card card, GameObject selectedTarget)
    {
        Player player = selectedTarget.GetComponent<Player>();
        if (player != null)
        {
            Debug.Log("Sprint ī�带 ���");

            player.ChargeAnim(selectedTarget);

            player.playerData.activePoint += (int)card.cardPower[0] + cardProcessing.TempActivePoint;
            cardProcessing.cardUseDistance = card.cardPower[0];
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
            player.StabAnim(selectedTarget);

            Debug.Log(card.cardName + " / TargetName: " + monster);
            monster.GetHit(card.cardPower[0]);
            cardProcessing.cardUseDistance = card.cardPower[1];
        }
        else
        {
            cardProcessing.waitForInput = true;
        }
    }

    // Stat Boost
    public void UseStatBoost(Card card, GameObject selectedTarget)
    {
        Debug.Log(1);
        Player player = selectedTarget.GetComponent<Player>();
        if (player != null)
        {
            player.ChargeAnim(selectedTarget);

            particleController.ApplyPlayerEffect(particleController.buffEffectPrefab, selectedTarget);

            int randNum = Random.Range(0, 3);
            switch (randNum)
            {
                case 0:
                    player.playerData.Armor += 20;
                    break;
                case 1:
                    player.playerData.MaxHp += 20;
                    break;
                case 2:
                    player.playerData.MaxActivePoint += 1;
                    break;
                default:
                    break;
            }

            Debug.Log(player.playerData.MaxHp);
            cardProcessing.cardUseDistance = card.cardDistance;
        }
        else
        {
            cardProcessing.waitForInput = true;
        }
    }

    // Rest
    public void UseRest(Card card, GameObject selectedTarget)
    {
        Player player = selectedTarget.GetComponent<Player>();
        if (player != null)
        {
            player.ChargeAnim(selectedTarget);

            player.playerData.Armor += card.cardPower[0];
            cardProcessing.cardUseDistance = card.cardPower[1];
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
            player.SpinAttackAnim(selectedTarget);

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

    // Teleport
    public void UseTeleport(Card card, GameObject selectedTarget)
    {
        // ����� ���� ����
        Tile tile = selectedTarget.GetComponent<Tile>();
        if (tile != null)
        {
            player.ChargeAnim(selectedTarget);

            Debug.Log("Teleport ī�带 ���");

            // �÷��̾� �߰� �̵�
            player.transform.position = tile.transform.position;

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

            player.ChargeAnim(selectedTarget);
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

            player.ChargeAnim(selectedTarget);
        }
        else
        {
            cardProcessing.waitForInput = true;
        }
    }

    public void WallJump(Card card, GameObject selectedTarget)
    {
        Player player = selectedTarget.GetComponent<Player>();
        
        if (player != null)
        {
            
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
            player.AttackOneAnim(selectedTarget);


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
            player.AttackOneAnim(selectedTarget);
            

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
            player.ChargeAnim(selectedTarget);
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

            


            player.ChargeAnim(selectedTarget);
        }
        else
        {
            cardProcessing.waitForInput = true;
        }

    }



    // Wizard Cards --------------------------------
    // Excalibur's Wrath
    public void UseExcalibursWrath(Card card, GameObject selectedTarget)
    {
        Monster monster = selectedTarget.GetComponent<Monster>();
        if (monster != null)
        {
            Debug.Log(card.cardName + " / TargetName: " + monster);
            monster.GetHit(card.cardPower[0]);

            player.MacigAttack03Anim(selectedTarget);
        }
        else
        {
            cardProcessing.waitForInput = true;
        }
    }

    // Divine Intervention
    public void UseDivineIntervention(Card card, GameObject selectedTarget)
    {
        Monster monster = selectedTarget.GetComponent<Monster>();
        if (monster != null)
        {
            Debug.Log(card.cardName + " / TargetName: " + monster);
            monster.GetHit(card.cardPower[0]);

            player.ChargeAnim(selectedTarget);
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
            Debug.Log(card.cardName + " / TargetName: " + monster);
            monster.GetHit(card.cardPower[0]);

            player.ChargeAnim(selectedTarget);

            cardProcessing.cardUseDistance = card.cardDistance;
        }
        else
        {
            cardProcessing.waitForInput = true;
        }
    }
}
