using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardData : MonoBehaviour
{
    private WeaponController weaponController;
    private CardProcessing cardProcessing;

    [Header(" # Player Scripts")] public Player player;

    private PlayerState playerState;

    [Header(" # Player Object")] public GameObject playerObject;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        cardProcessing = FindObjectOfType<CardProcessing>();
        weaponController = playerObject.GetComponent<WeaponController>();
        player = playerObject.GetComponent<Player>();
    }

    // Base Cards --------------------------------
    // Sword Slash
    public void UseSwordSlash(Card card, GameObject selectedTarget)
    {
        Monster monster = selectedTarget.GetComponent<Monster>();

        if (monster != null)
        {
            player.AttackTwoAnim(selectedTarget);

            Debug.Log(card.cardName + " / TargetName: " + monster);
            monster.GetHit(card.cardPower[0]);
            cardProcessing.cardUseDistance = card.cardPower[1];
        }
        else
        {
            cardProcessing.waitForInput = true;
        }
    }

    // Healing Salve
    public void UseHealingSalve(Card card, GameObject selectedTarget)
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

    // Sprint
    public void UseSprint(Card card, GameObject selectedTarget)
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

    // Basic Strike
    public void UseBasicStrike(Card card, GameObject selectedTarget)
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

    // Shield Block
    public void UseShieldBlock(Card card, GameObject selectedTarget)
    {
        Player player = selectedTarget.GetComponent<Player>();
        if (player != null)
        {
            player.ChargeAnim(selectedTarget);

            Debug.Log(card.cardName + " ī�带 ��� / " + player + " Armor: " + player.playerData.Armor);
            player.playerData.Armor += card.cardPower[0];
            cardProcessing.cardUseDistance = card.cardPower[1];
        }
        else
        {
            cardProcessing.waitForInput = true;
        }
    }

    // Common Cards --------------------------------
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
        Monster monster = selectedTarget.GetComponent<Monster>();
        if (monster != null)
        {
            player.ChargeAnim(selectedTarget);

            Debug.Log("Teleport ī�带 ���");

            player.playerData.activePoint = cardProcessing.TempActivePoint;

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

    // Rare Cards --------------------------------
    // Holy Nova
    public void UseHolyNova(Card card, GameObject selectedTarget)
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

            player.MacigAttack01Anim(selectedTarget);
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

            player.MacigAttack02Anim(selectedTarget);
        }
        else
        {
            cardProcessing.waitForInput = true;
        }
    }

    // Epic Cards --------------------------------
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

    // Legend Cards --------------------------------
    // Soul Siphon
    public void UseSoulSiphon(Card card, GameObject selectedTarget)
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
}
