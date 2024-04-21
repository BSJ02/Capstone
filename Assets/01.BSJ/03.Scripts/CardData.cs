using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using static Card;

public class CardData : MonoBehaviour
{
    [HideInInspector] public bool waitForInput = false;  // 대기 상태 여부
    [HideInInspector] public bool waitAnim = false;

    private PlayerAnimationEvents playerAnimationEvents;
    private PlayerMove playerMove;

    [Header("Animation 적용 할 캐릭터")]
    public GameObject playerObject;


    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        playerAnimationEvents = playerObject.GetComponent<PlayerAnimationEvents>();
    }


    private Coroutine currentCoroutine; // 현재 실행 중인 코루틴

    // 카드 사용 메서드
    public void UseCardAndSelectTarget(Card card, GameObject gameObject)
    {
        StartCoroutine(WaitForTargetSelection(card));
    }


    // 대상 선택을 기다리는 코루틴
    private IEnumerator WaitForTargetSelection(Card card)
    {
        while (true)
        {
            waitForInput = true;    // 대기 상태로 전환

            GameObject selectedTarget = null;   // 선택된 대상을 저장할 변수

            // 대상 선택이 완료될 때까지 반복합니다.
            while (waitForInput)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;

                    if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                    {
                        if (hit.collider.CompareTag("Player") || hit.collider.CompareTag("Monster") || hit.collider.CompareTag("Tile"))
                        {
                            selectedTarget = hit.collider.gameObject;
                            waitForInput = false;
                        }
                    }
                }
                yield return null; // 다음 프레임까지 대기
            }
            UseCard(card, selectedTarget);
            if (waitForInput)
            {
                Debug.Log("대상을 다시 선택하세요.");
                continue;
            }
            else
            {
                break;
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
                    UseShieldBlock(card, selectedTarget);
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
            Debug.Log(card.cardName + " 카드를 사용 / " + monster + " Hp: " + monster.monsterData.Hp);

            monster.GetHit(card.cardPower[0]);

            Debug.Log(monster + " Hp: " + monster.monsterData.Hp);

            // 카드 사용 애니메이션
            playerAnimationEvents.SlashAnim();
            waitAnim = false;
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
            Debug.Log(card.cardName + " 카드를 사용 / " + player + " Hp: " + player.playerData.Hp);
            player.playerData.Hp += card.cardPower[0];
            Debug.Log(player +  "Hp: " + player.playerData.Hp);

            // 카드 사용 애니메이션
            playerAnimationEvents.ChargeAnim();
            waitAnim = false;
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
        Monster monster = selectedTarget.GetComponent<Monster>();

        if (monster != null)
        {
            Debug.Log("Sprint 카드를 사용");

            // 플레이어 추가 이동
            //playerMove.isMoving = true;

            // 카드 사용 애니메이션
            waitAnim = false;
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
            Debug.Log(card.cardName + " 카드를 사용 / " + monster + " Hp: " + monster.monsterData.Hp);
            monster.monsterData.Hp -= card.cardPower[0];
            Debug.Log(monster + " Hp: " + monster.monsterData.Hp);

            // 카드 사용 애니메이션
            playerAnimationEvents.StabAnim();
            waitAnim = false;
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
            Debug.Log(card.cardName + " 카드를 사용 / " + player + " Armor: " + player.playerData.Armor);
            player.playerData.Armor += card.cardPower[0];
            Debug.Log(player + "Armor: " + player.playerData.Armor);

            // 카드 사용 애니메이션
            playerAnimationEvents.DefendAnim();
            waitAnim = false;
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
            Debug.Log(card.cardName + " 카드를 사용 / " + monster + " Hp: " + monster.monsterData.Hp);
            monster.monsterData.Hp -= card.cardPower[0];
            Debug.Log(monster + " Hp: " + monster.monsterData.Hp);

            // 카드 사용 애니메이션
            playerAnimationEvents.SlashAnim();
            waitAnim = false;
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
            Debug.Log(card.cardName + " 카드를 사용 / " + player + " Hp: " + player.playerData.Hp);
            player.playerData.Hp += card.cardPower[0];
            Debug.Log(player + "Hp: " + player.playerData.Hp);

            // 카드 사용 애니메이션
            playerAnimationEvents.ChargeAnim();
            waitAnim = false;
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
            Debug.Log("Teleport 카드를 사용");

            // 플레이어 추가 이동

            // 카드 사용 애니메이션
            waitAnim = false;
        }
        else
        {
            waitForInput = true;
        }
    }

    // Swift Strike 카드 (빠르고 강력한 공격을 가해 적을 공격합니다.)
    private void UseSwiftStrike(Card card, GameObject selectedTarget)
    {
        // 대상의 값을 변경
        Monster monster = selectedTarget.GetComponent<Monster>();
        if (monster != null)
        {
            Debug.Log("Swift Strike 카드를 사용");

            Debug.Log(card.cardName + " 카드를 사용 / " + monster + " Hp: " + monster.monsterData.Hp);
            monster.monsterData.Hp -= card.cardPower[0];
            Debug.Log(monster + " Hp: " + monster.monsterData.Hp);

            // 카드 사용 애니메이션
            playerAnimationEvents.StabAnim();
            waitAnim = false;
        }
        else
        {
            waitForInput = true;
        }
    }

    // Thunderstorm 카드 (주변에 번개를 내려 모든 적에게 데미지를 입힙니다.)
    private void UseThunderstorm(Card card, GameObject selectedTarget)
    {
        // 대상의 값을 변경
        Monster monster = selectedTarget.GetComponent<Monster>();
        if (monster != null)
        {
            Debug.Log("Thunderstorm 카드를 사용");

            Debug.Log(card.cardName + " 카드를 사용 / " + monster + " Hp: " + monster.monsterData.Hp);
            monster.monsterData.Hp -= card.cardPower[0];
            Debug.Log(monster + " Hp: " + monster.monsterData.Hp);

            // 카드 사용 애니메이션
            playerAnimationEvents.ChargeAnim();
            waitAnim = false;
        }
        else
        {
            waitForInput = true;
        }
    }

    // Rare Cards --------------------------------
}