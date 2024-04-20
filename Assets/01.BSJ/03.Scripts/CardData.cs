using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using static Card;

public class CardData : MonoBehaviour
{
    private bool waitForInput = false;  // 대기 상태 여부
    private float animationDuration = 1.0f; // 카드 애니메이션 시간


    // 카드 사용 메서드
    public void UseCardAndSelectTarget(Card card)
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
                        Debug.Log(selectedTarget.name);
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
                    UseHealingSalve(card, selectedTarget);
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


    // 카드 사라지는 애니메이션   
    private IEnumerator AnimateCardDisappearance(GameObject cardObject)
    {
        float timer = 0;
        Vector3 initialScale = cardObject.transform.localScale;

        while (timer < animationDuration)
        {
            // 시간에 따라 스케일을 줄여서 카드를 사라지게 함
            float scale = Mathf.Lerp(1.0f, 0.0f, timer / animationDuration);
            cardObject.transform.localScale = initialScale * scale;

            // 시간 업데이트
            timer += Time.deltaTime;

            yield return null;
        }

        // 카드 오브젝트 비활성화
        cardObject.SetActive(false);
    }


    // SwordSlash 카드를 사용하는 메서드
    private void UseSwordSlash(Card card, GameObject selectedTarget)
    {
        // 카드 사용 애니메이션

        // SwordSlash 카드의 처리 코드를 작성
        Debug.Log("SwordSlash 카드를 사용");

        // 대상의 값을 변경합니다. (예: 데미지를 적용)
        Monster monster = selectedTarget.GetComponent<Monster>();
        if (monster != null)
        {
            Debug.Log("Hp: " + monster.monsterData.Hp);
            monster.monsterData.Hp -= card.cardPower[0];
            Debug.Log("Hp: " + monster.monsterData.Hp);
        }
        else
        {
            Debug.LogError("monsterData 없음");
        }
    }

    private void UseHealingSalve(Card card, GameObject selectedTarget)
    {
        // 카드 사용 애니메이션

        // SwordSlash 카드의 처리 코드를 작성
        Debug.Log("SwordSlash 카드를 사용");

        // 대상의 값을 변경합니다. (예: 데미지를 적용)
        Monster monster = selectedTarget.GetComponent<Monster>();
        if (monster != null)
        {
            Debug.Log("Hp: " + monster.monsterData.Hp);
            monster.monsterData.Hp -= card.cardPower[0];
            Debug.Log("Hp: " + monster.monsterData.Hp);
        }
        else
        {
            Debug.LogError("monsterData 없음");
        }
    }
}