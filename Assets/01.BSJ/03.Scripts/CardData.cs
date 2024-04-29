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
    // Sword Slash 카드 (적을 칼로 공격합니다.)
    public void UseSwordSlash(Card card, GameObject selectedTarget)
    {
        // 대상의 값을 변경
        Monster monster = selectedTarget.GetComponent<Monster>();

        if (monster != null)
        {
            // 카드 사용 애니메이션
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

    // Healing Salve 카드 (약초를 사용하여 체력을 회복합니다.)
    public void UseHealingSalve(Card card, GameObject selectedTarget)
    {
        // 대상의 값을 변경
        Player player = selectedTarget.GetComponent<Player>();

        if (player != null)
        {
            // 카드 사용 애니메이션
            player.ChargeAnim();

            player.playerData.Hp += card.cardPower[0];
            cardProcessing.cardUseDistance = card.cardPower[1];
        }
        else
        {
            cardProcessing.waitForInput = true;
        }
    }

    // Sprint 카드 (빠르게 이동하여 적의 공격을 피합니다.)
    public void UseSprint(Card card, GameObject selectedTarget)
    {
        // 대상의 값을 변경
        Player player = selectedTarget.GetComponent<Player>();
        if (player != null)
        {
            Debug.Log("Sprint 카드를 사용");

            // 카드 사용 애니메이션
            player.ChargeAnim();

            // 플레이어 추가 이동
            player.playerData.activePoint += (int)card.cardPower[0] + cardProcessing.TempActivePoint;
            cardProcessing.cardUseDistance = card.cardPower[0];
        }
        else
        {
            cardProcessing.waitForInput = true;
        }
    }

    // Basic Strike 카드 (간단한 공격을 가해 적을 공격합니다.)
    public void UseBasicStrike(Card card, GameObject selectedTarget)
    {
        // 대상의 값을 변경
        Monster monster = selectedTarget.GetComponent<Monster>();
        if (monster != null)
        {
            // 카드 사용 애니메이션
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

    // Shield Block 카드 (방패로 공격을 막아 받는 피해를 감소시킵니다.)
    public void UseShieldBlock(Card card, GameObject selectedTarget)
    {
        // 대상의 값을 변경
        Player player = selectedTarget.GetComponent<Player>();
        if (player != null)
        {
            // 카드 사용 애니메이션
            player.ChargeAnim();

            Debug.Log(card.cardName + " 카드를 사용 / " + player + " Armor: " + player.playerData.Armor);
            player.playerData.Armor += card.cardPower[0];
            cardProcessing.cardUseDistance = card.cardPower[1];
        }
        else
        {
            cardProcessing.waitForInput = true;
        }
    }

    // Common Cards --------------------------------
    // Ax Slash 카드 (적을 도끼로 공격합니다.)
    public void UseAxSlash(Card card, GameObject selectedTarget)
    {
        // 대상의 값을 변경
        Monster monster = selectedTarget.GetComponent<Monster>();
        if (monster != null)
        {
            // 카드 사용 애니메이션
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

    // Heal!! 카드 (축복을 받아 체력을 회복합니다.)
    public void UseHeal(Card card, GameObject selectedTarget)
    {
        // 대상의 값을 변경
        Player player = selectedTarget.GetComponent<Player>();
        if (player != null)
        {
            // 카드 사용 애니메이션
            player.ChargeAnim();

            Debug.Log(card.cardName + " 카드를 사용 / " + player + " Hp: " + player.playerData.Hp);
            player.playerData.Hp += card.cardPower[0];
            cardProcessing.cardUseDistance = card.cardPower[1];
        }
        else
        {
            cardProcessing.waitForInput = true;
        }
    }

    // Teleport 카드 (원하는 위치로 순간이동하여 이동합니다.)
    public void UseTeleport(Card card, GameObject selectedTarget)
    {
        // 대상의 값을 변경
        Monster monster = selectedTarget.GetComponent<Monster>();
        if (monster != null)
        {
            // 카드 사용 애니메이션
            player.ChargeAnim();

            Debug.Log("Teleport 카드를 사용");

            // 플레이어 추가 이동
            player.playerData.activePoint = cardProcessing.TempActivePoint;

        }
        else
        {
            cardProcessing.waitForInput = true;
        }
    }

    // Guardian Spirit 카드 (수호 정령을 소환하여 플레이어의 방어력을 증가시킵니다.)
    public void UseGuardianSpirit(Card card, GameObject selectedTarget)
    {
        // 대상의 값을 변경
        Monster monster = selectedTarget.GetComponent<Monster>();
        if (monster != null)
        {
            Debug.Log(card.cardName + " / TargetName: " + monster);
            monster.GetHit(card.cardPower[0]);

            // 카드 사용 애니메이션
            player.ChargeAnim();
        }
        else
        {
            cardProcessing.waitForInput = true;
        }
    }

    // Rare Cards --------------------------------
    // Holy Nova 카드 (주변 적에게 신성한 빛을 내리며 강력한 데미지를 입힙니다.)
    public void UseHolyNova(Card card, GameObject selectedTarget)
    {
        // 대상의 값을 변경
        Monster monster = selectedTarget.GetComponent<Monster>();
        if (monster != null)
        {
            Debug.Log(card.cardName + " / TargetName: " + monster);
            monster.GetHit(card.cardPower[0]);

            // 카드 사용 애니메이션
            player.ChargeAnim();
        }
        else
        {
            cardProcessing.waitForInput = true;
        }
    }

    // Fireball 카드 (화염구를 발사하여 적에게 강력한 데미지를 입힙니다.)
    public void UseFireball(Card card, GameObject selectedTarget)
    {
        // 대상의 값을 변경
        Monster monster = selectedTarget.GetComponent<Monster>();
        if (monster != null)
        {
            Debug.Log(card.cardName + " / TargetName: " + monster);
            monster.GetHit(card.cardPower[0]);

            // 카드 사용 애니메이션
            player.MacigAttack01Anim();
        }
        else
        {
            cardProcessing.waitForInput = true;
        }
    }

    // Lightning Strike (손에 번개를 모아 적에게 일격을 가해 강력한 데미지를 입힙니다.)
    public void UseLightningStrike(Card card, GameObject selectedTarget)
    {
        // 대상의 값을 변경
        Monster monster = selectedTarget.GetComponent<Monster>();
        if (monster != null)
        {
            Debug.Log(card.cardName + " / TargetName: " + monster);
            monster.GetHit(card.cardPower[0]);

            // 카드 사용 애니메이션
            player.MacigAttack02Anim();
        }
        else
        {
            cardProcessing.waitForInput = true;
        }
    }

    // Epic Cards --------------------------------
    // Excalibur's Wrath (전설적인 검의 분노로 적을 공격합니다.)
    public void UseExcalibursWrath(Card card, GameObject selectedTarget)
    {
        // 대상의 값을 변경
        Monster monster = selectedTarget.GetComponent<Monster>();
        if (monster != null)
        {
            Debug.Log(card.cardName + " / TargetName: " + monster);
            monster.GetHit(card.cardPower[0]);

            // 카드 사용 애니메이션
            player.MacigAttack03Anim();
        }
        else
        {
            cardProcessing.waitForInput = true;
        }
    }

    // Divine Intervention (신의 개입으로 플레이어를 보호하고 회복시킵니다.)
    public void UseDivineIntervention(Card card, GameObject selectedTarget)
    {
        // 대상의 값을 변경
        Monster monster = selectedTarget.GetComponent<Monster>();
        if (monster != null)
        {
            Debug.Log(card.cardName + " / TargetName: " + monster);
            monster.GetHit(card.cardPower[0]);

            // 카드 사용 애니메이션
            player.ChargeAnim();
        }
        else
        {
            cardProcessing.waitForInput = true;
        }
    }

    // Legend Cards --------------------------------
    // Soul Siphon (영혼을 흡수하여 주변 적의 생명력을 흡수하고 회복합니다.)
    public void UseSoulSiphon(Card card, GameObject selectedTarget)
    {
        // 대상의 값을 변경
        Monster monster = selectedTarget.GetComponent<Monster>();
        if (monster != null)
        {
            Debug.Log(card.cardName + " / TargetName: " + monster);
            monster.GetHit(card.cardPower[0]);

            // 카드 사용 애니메이션
            player.MacigAttack03Anim();
        }
        else
        {
            cardProcessing.waitForInput = true;
        }
    }
}
