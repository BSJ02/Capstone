using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using static Card;

public class CardData : MonoBehaviour
{
    private CardManager cardManager;
    public LayerMask targetLayer; // 대상으로 인식할 레이어 마스크
    private GameObject target;  // 카드 효과 적용할 대상

    private bool waitForInput = false;  // 대기 상태 여부

    // 카드 사용 메서드
    public void UseCardToSelectTarget(Card card)
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
            // 대상에 따른 처리를 수행합니다.
            switch (card.cardName)
            {
                case "Sword Slash":
                    // SwordSlash 카드를 사용하는 로직을 호출
                    UseSwordSlash(card, selectedTarget);
                    break;
                case "Healing Salve":
                    // SwordSlash 카드를 사용하는 로직을 호출
                    UseSwordSlash(card, selectedTarget);
                    break;
                case "Sprint":
                    // SwordSlash 카드를 사용하는 로직을 호출
                    UseSwordSlash(card, selectedTarget);
                    break;
                case "Basic Strike":
                    // SwordSlash 카드를 사용하는 로직을 호출
                    UseSwordSlash(card, selectedTarget);
                    break;
                case "Shield Block":
                    // SwordSlash 카드를 사용하는 로직을 호출
                    UseSwordSlash(card, selectedTarget);
                    break;
                // 다른 카드 타입에 대한 처리를 추가
                default:
                    Debug.LogError("해당 카드 타입을 처리하는 코드가 없음");
                    break;
            }
        }
    }

    // SwordSlash 카드를 사용하는 메서드
    private void UseSwordSlash(Card card, GameObject target)
    {
        // SwordSlash 카드의 처리 코드를 작성
        Debug.Log("SwordSlash 카드를 사용");
        UseCardAnimation();

        // 대상의 값을 변경합니다. (예: 데미지를 적용)
        MonsterData monsterData = target.GetComponent<MonsterData>();
        if (monsterData != null)
        {
            monsterData.Hp -= card.cardPower[0];
        }
        else
        {
            Debug.LogError("monsterData 없음");
        }
    }

    // 카드 사용시 애니메이션
    private void UseCardAnimation()
    {

    }
}