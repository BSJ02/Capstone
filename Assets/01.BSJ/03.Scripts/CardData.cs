using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using static Card;

public class CardData : MonoBehaviour
{
    [HideInInspector] public bool waitForInput = false;  // 대기 상태 여부

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }


    // 카드 사용 메서드
    public void UseCardAndSelectTarget(Card card, GameObject gameObject)
    {
        StartCoroutine(WaitForTargetSelection(card));   // 대기 코루틴 시작
    }


    // 대상 선택을 기다리는 코루틴
    private IEnumerator WaitForTargetSelection(Card card)
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
                    if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Monster"))
                    {
                        selectedTarget = hit.collider.gameObject;
                        waitForInput = false;
                        break;
                    }

                }
            }

            yield return null; // 다음 프레임까지 대기
        }

        // 선택된 대상에 따라 카드를 사용
        if (selectedTarget != null)
        {
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
        // 카드 사용 애니메이션

        Debug.Log(card.cardName + " 카드를 사용");

        // 대상의 값을 변경
        Monster monster = selectedTarget.GetComponent<Monster>();
        if (monster != null)
        {
            Debug.Log("Hp: " + monster.monsterData.Hp);
            monster.monsterData.Hp -= card.cardPower[0];
            Debug.Log("Hp: " + monster.monsterData.Hp);
        }
    }

    // Healing Salve 카드 (약초를 사용하여 체력을 회복합니다.)
    private void UseHealingSalve(Card card, GameObject selectedTarget)
    {
        // 카드 사용 애니메이션

        Debug.Log(card.cardName + " 카드를 사용");

        // 대상의 값을 변경
        Monster monster = selectedTarget.GetComponent<Monster>();
        if (monster != null)
        {
            // 플레이어 체력 회복
        }
    }

    // Sprint 카드 (빠르게 이동하여 적의 공격을 피합니다.)
    private void UseSprint(Card card, GameObject selectedTarget)
    {
        // 카드 사용 애니메이션

        Debug.Log("Sprint 카드를 사용");

        // 대상의 값을 변경
        Monster monster = selectedTarget.GetComponent<Monster>();
        if (monster != null)
        {
            // 플레이어 추가 이동
        }
    }

    // Basic Strike 카드 (간단한 공격을 가해 적을 공격합니다.)
    private void UseBasicStrike(Card card, GameObject selectedTarget)
    {
        // 카드 사용 애니메이션

        Debug.Log("BasicStrike 카드를 사용");

        // 대상의 값을 변경
        Monster monster = selectedTarget.GetComponent<Monster>();
        if (monster != null)
        {
            Debug.Log("Hp: " + monster.monsterData.Hp);
            monster.monsterData.Hp -= card.cardPower[0];
            Debug.Log("Hp: " + monster.monsterData.Hp);
        }
    }

    // Shield Block 카드 (방패로 공격을 막아 받는 피해를 감소시킵니다.)
    private void UseShieldBlock(Card card, GameObject selectedTarget)
    {
        // 카드 사용 애니메이션

        Debug.Log("ShieldBlock 카드를 사용");

        // 대상의 값을 변경
        Monster monster = selectedTarget.GetComponent<Monster>();
        if (monster != null)
        {
            // 플레이어 방어력 증가
        }
    }

    // Common Cards --------------------------------
    // Ax Slash 카드 (적을 도끼로 공격합니다.)
    private void UseAxSlash(Card card, GameObject selectedTarget)
    {
        // 카드 사용 애니메이션

        Debug.Log("Shield Block 카드를 사용");

        // 대상의 값을 변경
        Monster monster = selectedTarget.GetComponent<Monster>();
        if (monster != null)
        {
            Debug.Log("Hp: " + monster.monsterData.Hp);
            monster.monsterData.Hp -= card.cardPower[0];
            Debug.Log("Hp: " + monster.monsterData.Hp);
        }
    }

    // Heal!! 카드 (축복을 받아 체력을 회복합니다.)
    private void UseHeal(Card card, GameObject selectedTarget)
    {
        // 카드 사용 애니메이션

        Debug.Log("Heal!! 카드를 사용");

        // 대상의 값을 변경
        Monster monster = selectedTarget.GetComponent<Monster>();
        if (monster != null)
        {
            // 플레이어 체력 회복
        }
    }

    // Teleport 카드 (원하는 위치로 순간이동하여 이동합니다.)
    private void UseTeleport(Card card, GameObject selectedTarget)
    {
        // 카드 사용 애니메이션

        Debug.Log("Teleport 카드를 사용");

        // 대상의 값을 변경
        Monster monster = selectedTarget.GetComponent<Monster>();
        if (monster != null)
        {
            // 플레이어 추가 이동
        }
    }

    // Swift Strike 카드 (빠르고 강력한 공격을 가해 적을 공격합니다.)
    private void UseSwiftStrike(Card card, GameObject selectedTarget)
    {
        // 카드 사용 애니메이션

        Debug.Log("Swift Strike 카드를 사용");

        // 대상의 값을 변경
        Monster monster = selectedTarget.GetComponent<Monster>();
        if (monster != null)
        {
            Debug.Log("Hp: " + monster.monsterData.Hp);
            monster.monsterData.Hp -= card.cardPower[0];
            Debug.Log("Hp: " + monster.monsterData.Hp);
        }
    }

    // Thunderstorm 카드 (주변에 번개를 내려 모든 적에게 데미지를 입힙니다.)
    private void UseThunderstorm(Card card, GameObject selectedTarget)
    {
        // 카드 사용 애니메이션

        Debug.Log("Thunderstorm 카드를 사용");

        // 대상의 값을 변경
        Monster monster = selectedTarget.GetComponent<Monster>();
        if (monster != null)
        {
            Debug.Log("Hp: " + monster.monsterData.Hp);
            monster.monsterData.Hp -= card.cardPower[0];
            Debug.Log("Hp: " + monster.monsterData.Hp);
        }
    }

    // Rare Cards --------------------------------
}