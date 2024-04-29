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
    // Sword Slash ī�� (���� Į�� �����մϴ�.)
    public void UseSwordSlash(Card card, GameObject selectedTarget)
    {
        // ����� ���� ����
        Monster monster = selectedTarget.GetComponent<Monster>();

        weaponController.ChangeToSword();

        if (monster != null)
        {
            // ī�� ��� �ִϸ��̼�
            player.AttackTwoAnim();

            Debug.Log(card.cardName + " / TargetName: " + monster);
            monster.GetHit(card.cardPower[0]);
            cardProcessing.cardUseDistance = card.cardPower[1];
        }
        else
        {
            cardProcessing.waitForInput = true;
        }
    }

    // Healing Salve ī�� (���ʸ� ����Ͽ� ü���� ȸ���մϴ�.)
    public void UseHealingSalve(Card card, GameObject selectedTarget)
    {
        // ����� ���� ����
        Player player = selectedTarget.GetComponent<Player>();

        if (player != null)
        {
            // ī�� ��� �ִϸ��̼�
            player.ChargeAnim();

            player.playerData.Hp += card.cardPower[0];
            cardProcessing.cardUseDistance = card.cardPower[1];
        }
        else
        {
            cardProcessing.waitForInput = true;
        }

    }

    // Sprint ī�� (������ �̵��Ͽ� ���� ������ ���մϴ�.)
    public void UseSprint(Card card, GameObject selectedTarget)
    {
        // ����� ���� ����
        Player player = selectedTarget.GetComponent<Player>();
        if (player != null)
        {
            Debug.Log("Sprint ī�带 ���");

            // ī�� ��� �ִϸ��̼�
            player.ChargeAnim();

            // �÷��̾� �߰� �̵�
            player.playerData.activePoint += (int)card.cardPower[0] + cardProcessing.TempActivePoint;
            cardProcessing.cardUseDistance = card.cardPower[0];
        }
        else
        {
            cardProcessing.waitForInput = true;
        }
    }

    // Basic Strike ī�� (������ ������ ���� ���� �����մϴ�.)
    public void UseBasicStrike(Card card, GameObject selectedTarget)
    {
        // ����� ���� ����
        Monster monster = selectedTarget.GetComponent<Monster>();
        if (monster != null)
        {
            // ī�� ��� �ִϸ��̼�
            player.StabAnim();

            Debug.Log(card.cardName + " / TargetName: " + monster);
            monster.GetHit(card.cardPower[0]);
            cardProcessing.cardUseDistance = card.cardPower[1];
        }
        else
        {
            cardProcessing.waitForInput = true;
        }
    }

    // Shield Block ī�� (���з� ������ ���� �޴� ���ظ� ���ҽ�ŵ�ϴ�.)
    public void UseShieldBlock(Card card, GameObject selectedTarget)
    {
        // ����� ���� ����
        Player player = selectedTarget.GetComponent<Player>();
        if (player != null)
        {
            // ī�� ��� �ִϸ��̼�
            player.ChargeAnim();

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
    // Ax Slash ī�� (���� ������ �����մϴ�.)
    public void UseAxSlash(Card card, GameObject selectedTarget)
    {
        // ����� ���� ����
        Monster monster = selectedTarget.GetComponent<Monster>();
        if (monster != null)
        {
            // ī�� ��� �ִϸ��̼�
            player.SpinAttackAnim();

            Debug.Log(card.cardName + " / TargetName: " + monster);
            monster.GetHit(card.cardPower[0]);
            cardProcessing.cardUseDistance = card.cardPower[1];
        }
        else
        {
            cardProcessing.waitForInput = true;
        }
    }

    // Heal!! ī�� (�ູ�� �޾� ü���� ȸ���մϴ�.)
    public void UseHeal(Card card, GameObject selectedTarget)
    {
        // ����� ���� ����
        Player player = selectedTarget.GetComponent<Player>();
        if (player != null)
        {
            // ī�� ��� �ִϸ��̼�
            player.ChargeAnim();

            Debug.Log(card.cardName + " ī�带 ��� / " + player + " Hp: " + player.playerData.Hp);
            player.playerData.Hp += card.cardPower[0];
            cardProcessing.cardUseDistance = card.cardPower[1];
        }
        else
        {
            cardProcessing.waitForInput = true;
        }
    }

    // Teleport ī�� (���ϴ� ��ġ�� �����̵��Ͽ� �̵��մϴ�.)
    public void UseTeleport(Card card, GameObject selectedTarget)
    {
        // ����� ���� ����
        Monster monster = selectedTarget.GetComponent<Monster>();
        if (monster != null)
        {
            // ī�� ��� �ִϸ��̼�
            player.ChargeAnim();

            Debug.Log("Teleport ī�带 ���");

            // �÷��̾� �߰� �̵�
            player.playerData.activePoint = cardProcessing.TempActivePoint;

        }
        else
        {
            cardProcessing.waitForInput = true;
        }
    }

    // Guardian Spirit ī�� (��ȣ ������ ��ȯ�Ͽ� �÷��̾��� ������ ������ŵ�ϴ�.)
    public void UseGuardianSpirit(Card card, GameObject selectedTarget)
    {
        // ����� ���� ����
        Monster monster = selectedTarget.GetComponent<Monster>();
        if (monster != null)
        {
            Debug.Log(card.cardName + " / TargetName: " + monster);
            monster.GetHit(card.cardPower[0]);

            // ī�� ��� �ִϸ��̼�
            player.ChargeAnim();
        }
        else
        {
            cardProcessing.waitForInput = true;
        }
    }

    // Rare Cards --------------------------------
    // Holy Nova ī�� (�ֺ� ������ �ż��� ���� ������ ������ �������� �����ϴ�.)
    public void UseHolyNova(Card card, GameObject selectedTarget)
    {
        // ����� ���� ����
        Monster monster = selectedTarget.GetComponent<Monster>();
        if (monster != null)
        {
            Debug.Log(card.cardName + " / TargetName: " + monster);
            monster.GetHit(card.cardPower[0]);

            // ī�� ��� �ִϸ��̼�
            player.ChargeAnim();
        }
        else
        {
            cardProcessing.waitForInput = true;
        }
    }

    // Fireball ī�� (ȭ������ �߻��Ͽ� ������ ������ �������� �����ϴ�.)
    public void UseFireball(Card card, GameObject selectedTarget)
    {
        // ����� ���� ����
        Monster monster = selectedTarget.GetComponent<Monster>();
        if (monster != null)
        {
            Debug.Log(card.cardName + " / TargetName: " + monster);
            monster.GetHit(card.cardPower[0]);

            // ī�� ��� �ִϸ��̼�
            player.MacigAttack01Anim();
        }
        else
        {
            cardProcessing.waitForInput = true;
        }
    }

    // Lightning Strike (�տ� ������ ��� ������ �ϰ��� ���� ������ �������� �����ϴ�.)
    public void UseLightningStrike(Card card, GameObject selectedTarget)
    {
        // ����� ���� ����
        Monster monster = selectedTarget.GetComponent<Monster>();
        if (monster != null)
        {
            Debug.Log(card.cardName + " / TargetName: " + monster);
            monster.GetHit(card.cardPower[0]);

            // ī�� ��� �ִϸ��̼�
            player.MacigAttack02Anim();
        }
        else
        {
            cardProcessing.waitForInput = true;
        }
    }

    // Epic Cards --------------------------------
    // Excalibur's Wrath (�������� ���� �г�� ���� �����մϴ�.)
    public void UseExcalibursWrath(Card card, GameObject selectedTarget)
    {
        // ����� ���� ����
        Monster monster = selectedTarget.GetComponent<Monster>();
        if (monster != null)
        {
            Debug.Log(card.cardName + " / TargetName: " + monster);
            monster.GetHit(card.cardPower[0]);

            // ī�� ��� �ִϸ��̼�
            player.MacigAttack03Anim();
        }
        else
        {
            cardProcessing.waitForInput = true;
        }
    }

    // Divine Intervention (���� �������� �÷��̾ ��ȣ�ϰ� ȸ����ŵ�ϴ�.)
    public void UseDivineIntervention(Card card, GameObject selectedTarget)
    {
        // ����� ���� ����
        Monster monster = selectedTarget.GetComponent<Monster>();
        if (monster != null)
        {
            Debug.Log(card.cardName + " / TargetName: " + monster);
            monster.GetHit(card.cardPower[0]);

            // ī�� ��� �ִϸ��̼�
            player.ChargeAnim();
        }
        else
        {
            cardProcessing.waitForInput = true;
        }
    }

    // Legend Cards --------------------------------
    // Soul Siphon (��ȥ�� ����Ͽ� �ֺ� ���� ������� ����ϰ� ȸ���մϴ�.)
    public void UseSoulSiphon(Card card, GameObject selectedTarget)
    {
        // ����� ���� ����
        Monster monster = selectedTarget.GetComponent<Monster>();
        if (monster != null)
        {
            Debug.Log(card.cardName + " / TargetName: " + monster);
            monster.GetHit(card.cardPower[0]);

            // ī�� ��� �ִϸ��̼�
            player.MacigAttack03Anim();
        }
        else
        {
            cardProcessing.waitForInput = true;
        }
    }
}
