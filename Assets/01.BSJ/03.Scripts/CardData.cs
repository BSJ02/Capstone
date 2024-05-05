using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CardData : MonoBehaviour
{
    private WeaponController weaponController;
    private CardProcessing cardProcessing;
    private ParticleController particleController;

    private Player currentPlayer;

    private GameObject currentPlayerObj;

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

    private void Update()
    {
        if (currentPlayer != null && currentPlayerObj != null)
        {
            currentPlayer = cardProcessing.currentPlayer;
            currentPlayerObj = cardProcessing.currentPlayerObj;

            weaponController = currentPlayerObj.GetComponent<WeaponController>();
            currentPlayer = currentPlayerObj.GetComponent<Player>();
        }
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
            currentPlayer.ChargeAnim(selectedTarget);

            Debug.Log("Teleport ī�带 ���");

            // �÷��̾� �߰� �̵�
            currentPlayerObj.transform.position = tile.transform.position;

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

            currentPlayer.ChargeAnim(selectedTarget);
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

            currentPlayer.ChargeAnim(selectedTarget);
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

            currentPlayer.MacigAttack02Anim(selectedTarget);
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
    // Excalibur's Wrath
    public void UseExcalibursWrath(Card card, GameObject selectedTarget)
    {
        Monster monster = selectedTarget.GetComponent<Monster>();
        if (monster != null)
        {
            Debug.Log(card.cardName + " / TargetName: " + monster);
            monster.GetHit(card.cardPower[0]);

            currentPlayer.MacigAttack03Anim(selectedTarget);
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

            currentPlayer.ChargeAnim(selectedTarget);
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

            currentPlayer.ChargeAnim(selectedTarget);

            cardProcessing.cardUseDistance = card.cardDistance;
        }
        else
        {
            cardProcessing.waitForInput = true;
        }
    }
}
