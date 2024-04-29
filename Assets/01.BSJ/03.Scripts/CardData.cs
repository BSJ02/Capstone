using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using static Card;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.UIElements.UxmlAttributeDescription;


//public enum CardState
//{
//    SwordSlash,
//    HealingSalve,
//    Sprint,
//    BasicStrike,
//    ShieldBlock,
//    AxSlash,
//    Heal,
//    Teleport,
//    GuardianSpirit,
//    HolyNova,
//    Fireball,
//    LightningStrike,
//    ExcalibursWrath,
//    DivineIntervention,
//    SoulSiphon
//}

public class CardData : MonoBehaviour
{
    [HideInInspector] public bool waitForInput = false;  // 대기 상태 여부
    [HideInInspector] public bool waitAnim = false;
    [HideInInspector] public bool usingCard = false;
    [HideInInspector] public bool coroutineStop = false;
    [HideInInspector] public int TempActivePoint;

    private PlayerMoveTest playerMoveTest;
    private BattleManager battleManager;

    [Header(" # Player Scripts")] public Player player;
    [Header(" # Map Scripts")] public MapGenerator mapGenerator;

    private PlayerState playerState;

    [Header(" # Player Object")] public GameObject playerObject;


    private GameObject selectedTarget = null;
    private float cardUseDistance = 0;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        battleManager = FindObjectOfType<BattleManager>();
        player = playerObject.GetComponent<Player>();
    }

    private void Update()
    {
        if (usingCard)
        {
            mapGenerator.CardUseRange(playerObject.transform.position, (int)cardUseDistance);
        }
    }

    // 카드 사용 메서드
    public void UseCardAndSelectTarget(Card card, GameObject gameObject)
    {
        StartCoroutine(WaitForTargetSelection(card));
    }


    // 대상 선택을 기다리는 코루틴
    private IEnumerator WaitForTargetSelection(Card card)
    {
        battleManager.isPlayerMove = false;
        TempActivePoint = player.playerData.activePoint;
        player.playerData.activePoint = 0;
        cardUseDistance = card.cardPower[2];    // 카드 거리 저장
        while (true)
        {
            waitForInput = true;    // 대기 상태로 전환

            // 대상 선택이 완료될 때까지 반복합니다.
            while (waitForInput)
            {
                if (Input.GetMouseButtonDown(0))
                {

                    SelectTarget();
                
                }
                yield return null; // 다음 프레임까지 대기
            }
            if (coroutineStop)
            {
                coroutineStop = false;
                mapGenerator.ClearHighlightedTiles();
                yield break;
            }

            UseCard(card, selectedTarget);

            if (waitForInput)
            {
                Debug.Log("대상을 다시 선택하세요.");
                continue;
            }

            usingCard = false;
            mapGenerator.ClearHighlightedTiles();

            if (!waitForInput)
            {
                break;
            }

        }
    }

    private void SelectTarget() // 대상 설정
    {
        selectedTarget = null;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            if (hit.collider.CompareTag("Player") || hit.collider.CompareTag("Monster") || hit.collider.CompareTag("Tile"))
            {

                selectedTarget = hit.collider.gameObject;            
                if (Vector3.Distance(player.transform.position, selectedTarget.transform.position) <= cardUseDistance)
                {
                    waitForInput = false;
                }
            }
        }
    }

    private void UseCard(Card card, GameObject selectedTarget)
    {
        // 선택된 대상에 따라 카드를 사용
        if (selectedTarget != null)
        {
            waitAnim = true;
            // cardName을 사용하는 로직을 호출
            switch (card.cardName)
            {
                case "Sword Slash":
                    UseSwordSlash(card, selectedTarget);
                    break;
                case "Healing Salve":
                    UseHealingSalve(card, selectedTarget);
                    break;
                case "Sprint":
                    UseSprint(card, selectedTarget);
                    break;
                case "Basic Strike":
                    UseBasicStrike(card, selectedTarget);
                    break;
                case "Shield Block":
                    UseShieldBlock(card, selectedTarget);
                    break;
                case "Ax Slash":
                    UseAxSlash(card, selectedTarget);
                    break;
                case "Heal!!":
                    UseHeal(card, selectedTarget);
                    break;
                case "Teleport":
                    UseTeleport(card, selectedTarget);
                    break;
                case "Guardian Spirit":
                    UseGuardianSpirit(card, selectedTarget);
                    break;
                case "Holy Nova":
                    UseHolyNova(card, selectedTarget);
                    break;
                case "Fireball":
                    UseFireball(card, selectedTarget);
                    break;
                case "Lightning Strike":
                    UseLightningStrike(card, selectedTarget);
                    break;
                case "Excalibur's Wrath":
                    UseExcalibursWrath(card, selectedTarget);
                    break;
                case "Divine Intervention":
                    UseDivineIntervention(card, selectedTarget);
                    break;
                case "Soul Siphon":
                    UseSoulSiphon(card, selectedTarget);
                    break;
                default:
                    Debug.LogError("해당 카드 타입을 처리하는 코드가 없음");
                    break;
            }

        }
    }

    // Base Cards --------------------------------
    // Sword Slash 카드 (적을 칼로 공격합니다.)
    private void UseSwordSlash(Card card, GameObject selectedTarget)
    {
        // 대상의 값을 변경
        Monster monster = selectedTarget.GetComponent<Monster>();

        if (monster != null)
        {
            // 카드 사용 애니메이션
            player.AttackTwoAnim();

            Debug.Log(card.cardName + " / TargetName: " + monster);
            monster.GetHit(card.cardPower[0]);
            //cardUseDistance = card.cardPower[1];
        }
        else
        {
            waitForInput = true;
        }
    }

    // Healing Salve 카드 (약초를 사용하여 체력을 회복합니다.)
    private void UseHealingSalve(Card card, GameObject selectedTarget)
    {
        // 대상의 값을 변경
        Player player = selectedTarget.GetComponent<Player>();

        if (player != null)
        {
            // 카드 사용 애니메이션
            player.ChargeAnim();

            player.playerData.Hp += card.cardPower[0];
            cardUseDistance = card.cardPower[1];
        }
        else
        {
            waitForInput = true;
        }
        
    }

    // Sprint 카드 (빠르게 이동하여 적의 공격을 피합니다.)
    private void UseSprint(Card card, GameObject selectedTarget)
    {
        // 대상의 값을 변경
        Player player = selectedTarget.GetComponent<Player>();
        if (player != null)
        {
            Debug.Log("Sprint 카드를 사용");

            // 카드 사용 애니메이션
            player.ChargeAnim();

            // 플레이어 추가 이동
            player.playerData.activePoint += (int)card.cardPower[0] + TempActivePoint;
            cardUseDistance = card.cardPower[0];
        }
        else
        {
            waitForInput = true;
        }
    }

    // Basic Strike 카드 (간단한 공격을 가해 적을 공격합니다.)
    private void UseBasicStrike(Card card, GameObject selectedTarget)
    {
        // 대상의 값을 변경
        Monster monster = selectedTarget.GetComponent<Monster>();
        if (monster != null)
        {
            // 카드 사용 애니메이션
            player.StabAnim();

            Debug.Log(card.cardName + " / TargetName: " + monster);
            monster.GetHit(card.cardPower[0]);
            cardUseDistance = card.cardPower[1];
        }
        else
        {
            waitForInput = true;
        }
    }

    // Shield Block 카드 (방패로 공격을 막아 받는 피해를 감소시킵니다.)
    private void UseShieldBlock(Card card, GameObject selectedTarget)
    {
        // 대상의 값을 변경
        Player player = selectedTarget.GetComponent<Player>();
        if (player != null)
        {
            // 카드 사용 애니메이션
            player.ChargeAnim();

            Debug.Log(card.cardName + " 카드를 사용 / " + player + " Armor: " + player.playerData.Armor);
            player.playerData.Armor += card.cardPower[0];
            Debug.Log(player + "Armor: " + player.playerData.Armor);
        }
        else
        {
            waitForInput = true;
        }
    }

    // Common Cards --------------------------------
    // Ax Slash 카드 (적을 도끼로 공격합니다.)
    private void UseAxSlash(Card card, GameObject selectedTarget)
    {
        // 대상의 값을 변경
        Monster monster = selectedTarget.GetComponent<Monster>();
        if (monster != null)
        {
            // 카드 사용 애니메이션
            player.SpinAttackAnim();

            Debug.Log(card.cardName + " / TargetName: " + monster);
            monster.GetHit(card.cardPower[0]);
        }
        else
        {
            waitForInput = true;
        }
    }

    // Heal!! 카드 (축복을 받아 체력을 회복합니다.)
    private void UseHeal(Card card, GameObject selectedTarget)
    {
        // 대상의 값을 변경
        Player player = selectedTarget.GetComponent<Player>();
        if (player != null)
        {
            // 카드 사용 애니메이션
            player.ChargeAnim();

            Debug.Log(card.cardName + " 카드를 사용 / " + player + " Hp: " + player.playerData.Hp);
            player.playerData.Hp += card.cardPower[0];
            Debug.Log(player + "Hp: " + player.playerData.Hp);
        }
        else
        {
            waitForInput = true;
        }
    }

    // Teleport 카드 (원하는 위치로 순간이동하여 이동합니다.)
    private void UseTeleport(Card card, GameObject selectedTarget)
    {
        // 대상의 값을 변경
        Monster monster = selectedTarget.GetComponent<Monster>();
        if (monster != null)
        {
            // 카드 사용 애니메이션
            player.ChargeAnim();

            Debug.Log("Teleport 카드를 사용");

            // 플레이어 추가 이동
            player.playerData.activePoint = TempActivePoint;

        }
        else
        {
            waitForInput = true;
        }
    }

    // Guardian Spirit 카드 (수호 정령을 소환하여 플레이어의 방어력을 증가시킵니다.)
    private void UseGuardianSpirit(Card card, GameObject selectedTarget)
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
            waitForInput = true;
        }
    }

    // Rare Cards --------------------------------
    // Holy Nova 카드 (주변 적에게 신성한 빛을 내리며 강력한 데미지를 입힙니다.)
    private void UseHolyNova(Card card, GameObject selectedTarget)
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
            waitForInput = true;
        }
    }

    // Fireball 카드 (화염구를 발사하여 적에게 강력한 데미지를 입힙니다.)
    private void UseFireball(Card card, GameObject selectedTarget)
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
            waitForInput = true;
        }
    }

    // Lightning Strike (손에 번개를 모아 적에게 일격을 가해 강력한 데미지를 입힙니다.)
    private void UseLightningStrike(Card card, GameObject selectedTarget)
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
            waitForInput = true;
        }
    }

    // Epic Cards --------------------------------
    // Excalibur's Wrath (전설적인 검의 분노로 적을 공격합니다.)
    private void UseExcalibursWrath(Card card, GameObject selectedTarget)
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
            waitForInput = true;
        }
    }

    // Divine Intervention (신의 개입으로 플레이어를 보호하고 회복시킵니다.)
    private void UseDivineIntervention(Card card, GameObject selectedTarget)
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
            waitForInput = true;
        }
    }

    // Legend Cards --------------------------------
    // Soul Siphon (영혼을 흡수하여 주변 적의 생명력을 흡수하고 회복합니다.)
    private void UseSoulSiphon(Card card, GameObject selectedTarget)
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
            waitForInput = true;
        }
    }
}